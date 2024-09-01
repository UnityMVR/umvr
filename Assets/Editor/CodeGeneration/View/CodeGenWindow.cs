using System.Collections.Generic;
using System.IO;
using System.Linq;
using pindwin.umvr.Editor.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.Window
{
    public class CodeGenWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset _codeGenWindowUXML;
        
        private List<CodeGenWindowSession> _existingSessions;
        private CodeGenWindowSession _activeSession;
        private VisualElement _root;
        private VisualElement _createSessionGroup;
        private VisualElement _workOnSessionGroup;
        private VisualElement _currentSessionGroup;
        private DropdownField _workOnSessionDropdown;

        [MenuItem("Tools/UMVR/Generator")]
        public static void OpenWindow()
        {
            CodeGenWindow codeGenWindow = GetWindow<CodeGenWindow>();
            codeGenWindow.titleContent = new GUIContent("UMVR Generator");
        }

        public void CreateGUI()
        {
            AssetDatabase.Refresh();
            DeletionDetector.Deleted += RefreshAfterOneFrame;
            _root = rootVisualElement;

            VisualElement labelFromUXML = new VisualElement();
            _codeGenWindowUXML.CloneTree(labelFromUXML);
            labelFromUXML.style.flexGrow = new StyleFloat(1);
            _root.Add(labelFromUXML);

            _workOnSessionGroup = _root.Q("SelectSessionGroup");
            _workOnSessionDropdown = _workOnSessionGroup.Q<DropdownField>();
            _workOnSessionDropdown.RegisterValueChangedCallback(OnDropdownValueChanged);
        
            _createSessionGroup = _root.Q("CreateSessionGroup");
            _createSessionGroup.Q<Button>().clickable.clicked += CreateSession;

            _currentSessionGroup = _root.Q("ActiveSessionGroup");
        
            RefreshExistingSessions();
            RefreshWindow();
        }

        private void RefreshExistingSessions()
        {
            _existingSessions = new List<CodeGenWindowSession>();
            string[] assets = AssetDatabase.FindAssets($"t: {nameof(CodeGenWindowSession)}");
            
            if (assets == null)
            {
                _activeSession = null;
                return;
            }

            bool activeSessionExists = false;
            foreach (string guid in assets)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                CodeGenWindowSession s = AssetDatabase.LoadAssetAtPath<CodeGenWindowSession>(path);
                if (s != null)
                {
                    _existingSessions.Add(s);
                    if (s == _activeSession)
                    {
                        activeSessionExists = true;
                    }
                }
            }

            if (activeSessionExists == false)
            {
                _activeSession = null;
            }

            if (_existingSessions.Count == 1)
            {
                _activeSession = _existingSessions[0];
            }
        }

        private void CreateSession()
        {
            CodeGenWindowSession s = CreateInstance<CodeGenWindowSession>();
            string path = EditorUtility.SaveFilePanel("Create Generator Session", Application.dataPath, "GeneratorSession", "asset");
            path = path.Remove(0, Application.dataPath.Length);
            if (path.StartsWith(Path.DirectorySeparatorChar) || path.StartsWith('/'))
            {
                path = path.Substring(1);
            }
            path = Path.Combine("Assets", path);
            AssetDatabase.CreateAsset(s, path);
            AssetDatabase.SaveAssets();
            RefreshAfterOneFrame();
        }

        private void RefreshWindow()
        {
            _createSessionGroup.style.display = (_existingSessions.Count == 0).ToDisplayStyle();
            _workOnSessionGroup.style.display = (_existingSessions.Count > 0).ToDisplayStyle();
            RefreshWorkOnSessionGroup();
            RefreshActiveSession(); 
        
            _root.MarkDirtyRepaint();
        }

        private void OnDropdownValueChanged(ChangeEvent<string> changeEvent)
        {
            if (changeEvent.previousValue == changeEvent.newValue)
            {
                return;
            }
            
            _activeSession = _existingSessions.FirstOrDefault(s => s.name == changeEvent.newValue);
            
            RefreshWorkOnSessionGroup();
            RefreshActiveSession();
        }

        private void RefreshWorkOnSessionGroup()
        {
            _workOnSessionDropdown.choices.Clear();
            foreach (CodeGenWindowSession session in _existingSessions)
            {
                _workOnSessionDropdown.choices.Add(session.name);
            }

            if (_workOnSessionDropdown.index < 0)
            {
                _workOnSessionDropdown.index = 0;
            }
        
            _workOnSessionGroup.MarkDirtyRepaint();
        }

        private void RefreshActiveSession()
        {
            _currentSessionGroup.Clear();
            
            if (_activeSession != null)
            {
                _activeSession.Refresh();
                _currentSessionGroup.Add(new InspectorElement(_activeSession));
            }
            
            _currentSessionGroup.MarkDirtyRepaint();
        }

        private void RefreshAfterOneFrame()
        {
            EditorApplication.delayCall += DelayedCall;

            void DelayedCall()
            {
                EditorApplication.delayCall -= DelayedCall;
                RefreshExistingSessions();
                RefreshWindow();
            }
        }
    }
}