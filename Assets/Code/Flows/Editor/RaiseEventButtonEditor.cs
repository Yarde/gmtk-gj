using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Yarde.EventDispatcher;

namespace Yarde.Code.Flows.Editor
{
    [CustomEditor(typeof(RaiseEventButton))]
    public class RaiseEventButtonEditor : UnityEditor.Editor
    {
        private struct Event : IComparable<Event>
        {
            public Type type;
            public ConstructorInfo constructor;
            public ParameterInfo[] parameters;
            public string[] parametersNames;
            public string toString;

            public Event(Type type, ConstructorInfo constructor)
            {
                this.type = type;
                this.constructor = constructor;
                parameters = null;
                parametersNames = null;

                var str = new StringBuilder();
                if (type != null)
                {
                    str.Append(type.FullName);
                }
                str.Append('(');
                if (constructor != null)
                {
                    parameters = constructor.GetParameters();
                    for (int i = 0; i < parameters.Length - 1; ++i)
                    {
                        str.Append($"{parameters[i].ParameterType}, ");
                    }
                    if (parameters.Length > 0)
                        str.Append(parameters[parameters.Length - 1].ParameterType);

                    parametersNames = new string[parameters.Length];
                    for (int i = 0; i < parameters.Length; ++i)
                    {
                        parametersNames[i] = char.ToUpper(parameters[i].Name[0]) + parameters[i].Name.Substring(1);
                        parametersNames[i] = Regex.Replace(parametersNames[i], "([a-z](?=[A-Z]|[0-9])|[A-Z](?=[A-Z][a-z]|[0-9])|[0-9](?=[^0-9]))", "$1 ");
                    }
                }
                str.Append(')');

                toString = str.ToString();
            }

            public override string ToString()
            {
                return toString;
            }

            public int CompareTo(Event other)
            {
                return String.Compare(ToString(), other.ToString(), StringComparison.Ordinal);
            }
        }

        private static GUIStyle ErrorStyle;

        private static Type eventBaseType = typeof(IEvent);

        private static bool IsInitialized;
        private static Event[] AllEvents;
        private static string[] AllEventNames;

        private RaiseEventButton _target;

        private void OnEnable()
        {
            _target = target as RaiseEventButton;

            Initialize();

            if (ErrorStyle == null)
            {
                ErrorStyle = new GUIStyle();
                ErrorStyle.normal.textColor = Color.red;
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUI.BeginChangeCheck();
            int selectedEventIdx = DrawEventTypesPopup(_target.EventName);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_target, "Event Selected");
                _target.SetEvent(AllEvents[selectedEventIdx].type);
            }

            DrawEvent(AllEvents[selectedEventIdx]);

            if (GUILayout.Button("Select event"))
            {
                DrawEventTypesWindow();
            }
        }


        private void DrawEventTypesWindow()
        {
            SearchableSelectWindow.ShowWindow(AllEventNames, 400, (index) => SelectEvent(AllEvents[index]));
        }


        private void SelectEvent(Event toSelect) 
        {
            Undo.RecordObject(_target, "Event Selected");
            _target.SetEvent(toSelect.type);
            DrawEvent(toSelect);
        }

        private int DrawEventTypesPopup(string selectedEventName)
        {
            int selectedIndex = Array.FindIndex(AllEventNames, evtName => evtName.StartsWith(selectedEventName));
            if (selectedIndex == -1)
            {
                Debug.LogWarning($"Event <{selectedEventName}> no longer exists.");
                selectedIndex = 0;
            }

            GUI.enabled = false;
            selectedIndex = EditorGUILayout.Popup("Event Type", selectedIndex, AllEventNames);
            GUI.enabled = true;
            if (selectedIndex < 0 || selectedIndex >= AllEventNames.Length)
                selectedIndex = 0;

            return selectedIndex;
        }

        private void DrawEvent(Event evt)
        {
            if (evt.type == null)
            {
                return;
            }
            
            if (evt.constructor == null)
            {
                EditorGUILayout.LabelField("No suitable constructors found.", ErrorStyle);
            }
        }

        private static ConstructorInfo[] GetValidConstructors(Type type)
        {
            return type?.GetConstructors().Where(constructor => IsValidConstructor(constructor)).ToArray();
        }

        private static bool IsValidConstructor(ConstructorInfo constructor)
        {
            return Array.FindIndex(constructor.GetParameters(), param => !IsValidParameter(param)) == -1;
        }

        private static bool IsValidParameter(ParameterInfo parameter)
        {
            Type type = parameter.ParameterType;
            return type.Equals(typeof(bool)) || type.Equals(typeof(int)) || type.Equals(typeof(float)) || type.Equals(typeof(string));
        }

        private static Type[] GetInheritedTypes(Type baseType)
        {
            var types = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                types.AddRange(assembly.GetTypes().Where(type => type.IsClass && !type.IsAbstract && !type.IsInterface && baseType.IsAssignableFrom(type)));
            }
            return types.ToArray();
        }

        private static void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            var events = new List<Event>();
            foreach (var type in GetInheritedTypes(eventBaseType))
            {
                var constructors = GetValidConstructors(type);
                if (constructors.Length == 0)
                {
                    events.Add(new Event(type, null));
                }
                else
                {
                    foreach (var constructor in constructors)
                    {
                        events.Add(new Event(type, constructor));
                    }
                }
            }
            events.Sort();
            events.Insert(0, new Event(null, null));

            AllEvents = events.ToArray();
            AllEventNames = AllEvents.Select(evt => evt.ToString()).ToArray();

            IsInitialized = true;
        }
    }
}
