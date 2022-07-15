using System;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Yarde.EventDispatcher;
using Yarde.Utils.Logger;

namespace Yarde.Code.Flows
{
    public class RaiseEventButton : MonoBehaviour
    {
        [SerializeField] [HideInInspector] private string eventName = string.Empty;
        [Inject] [UsedImplicitly] private IDispatcher _dispatcher;

#if UNITY_EDITOR
        public string EventName => eventName;

        public void SetEvent(Type eventType)
        {
            if (eventType == null)
            {
                eventName = string.Empty;
            }
            else
            {
                eventName = eventType.FullName;
            }
        }
#endif

        private Button _button;

        protected void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClick);
        }
        
        private Type FetchEventType(string evName)
        {
            Type eventType = null;
            if (!string.IsNullOrEmpty(evName))
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                int assemblyIndex = 0;
                while (eventType == null && assemblyIndex < assemblies.Length)
                    eventType = assemblies[assemblyIndex++].GetType(evName);
            }

            return eventType;
        }
        
        private IEvent CreateEvent(Type type)
        {
            IEvent evt = null;

            if (type != null)
            {
                try
                {
                    evt = (IEvent)Activator.CreateInstance(type);
                }
                catch (Exception e)
                {
                    this.LogError($"{e.Message}");
                }
            }

            return evt;
        }

        private void OnClick()
        {
            if (_dispatcher == null)
            {
                this.LogError("Dispatcher missing ensure it is injected properly!");
                return;
            }
            
            Type type = FetchEventType(eventName);
            IEvent ev = CreateEvent(type);
            _dispatcher.RaiseEvent(ev);
        }
    }
}
