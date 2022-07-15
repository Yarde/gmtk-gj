using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Yarde.Code.Flows.Editor
{
    public class SearchableSelectWindow : EditorWindow
    {
        private (string, string, int)[] _optionsWithIndexes;
        private Action<int> _onSelected;
        private int _minOptionWidth;

        private Vector2 _scrollPos;
        private string _searchString = "";

        public static void ShowWindow(string [] options, int minOptionWidth, Action<int> onSelected)
        {
            var window = GetWindow<SearchableSelectWindow>(true, "Select", true);
            window.minSize = new Vector2(minOptionWidth + 20, 0);
            window._optionsWithIndexes = options.Select((option, index) => (option, option.ToLower(), index)).OrderBy(x => x.option).ToArray();
            window._onSelected = onSelected;
            window._minOptionWidth = minOptionWidth;
        }

        private void OnGUI()
        {
            GUILayout.Label("Search");

            EditorGUILayout.BeginHorizontal();
            _searchString = EditorGUILayout.TextField(_searchString).ToLower();     
            if (GUILayout.Button("Clear search", GUILayout.ExpandWidth(false)))
            {
                GUIUtility.keyboardControl = 0;
                _searchString = "";
            }
            EditorGUILayout.EndHorizontal();

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        
            foreach((var option, var optionLowerCase, var index) in _optionsWithIndexes)
            {
                if(optionLowerCase.Contains(_searchString))
                {
                    if (GUILayout.Button(option, "toggle", GUILayout.MinWidth(_minOptionWidth)))
                    {                 
                        _onSelected.Invoke(index);
                        Close();
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void OnLostFocus()
        {
            Close();
        }
    }
}
