using System.Collections.Generic;
using System.Linq;
using pindwin.umvr.Editor.Utilities;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.Window
{
	[CustomEditor(typeof(CodeGenWindowSession))]
	public class SessionInspector : UnityEditor.Editor
	{
		[SerializeField] private VisualTreeAsset _inspectorUXML;
		
		private DropdownField _modelTypeDropdown;
		private PropertyField _templateSetProperty;
		
		private VisualElement _generateMembersGroup;
		private RadioButtonGroup _generateMembersMode;
		private Button _processButton;
		private Label _resultsLabel;

		private CodeGenWindowSession Session => (CodeGenWindowSession)serializedObject.targetObject;

		public override VisualElement CreateInspectorGUI()
		{
			CodeGenWindowSession session = Session;
			session.Refresh();

			VisualElement root = new VisualElement();
			_inspectorUXML.CloneTree(root);

			_processButton = root.Q<Button>("ProcessButton");
			_processButton.clickable.clicked += () => session.Process();

			_modelTypeDropdown = root.Q<DropdownField>("ModelTypeDropdown");
			_modelTypeDropdown.choices = session.TypeChoices;
			if (_modelTypeDropdown.index < 0)
			{
				_modelTypeDropdown.index = 0;
			}

			_modelTypeDropdown.RegisterValueChangedCallback(OnModelChanged);

			_templateSetProperty = root.Q<PropertyField>("TemplateProperty");
			_templateSetProperty.RegisterValueChangeCallback(OnTemplateSetChanged);

			_generateMembersMode = root.Q<RadioButtonGroup>("GenerateMembersMode");
			_generateMembersMode.choices = session.GenerateMembersChoices;
			if (_generateMembersMode.value < 0)
			{
				_generateMembersMode.value = 0;
			}

			_generateMembersMode.RegisterValueChangedCallback(OnGenerateMembersModeChanged);

			_generateMembersGroup = root.Q("GeneratedFilesGroup");
			_resultsLabel = root.Q<Label>("ResultsLabel");
			_resultsLabel.bindingPath = nameof(CodeGenWindowSession.LastOutput);

			Refresh(session);
			return root;
		}

		private void OnModelChanged(ChangeEvent<string> e)
		{
			CodeGenWindowSession session = Session;
			session.OnModelChanged(e);

			Refresh(session);
		}

		private void OnGenerateMembersModeChanged(ChangeEvent<int> e)
		{
			CodeGenWindowSession session = Session;
			session.OnGenerateMembersModeChanged(e);

			Refresh(session);
		}

		private void OnTemplateSetChanged(SerializedPropertyChangeEvent e)
		{
			CodeGenWindowSession session = Session;
			session.OnTemplateSetChanged(e);

			Refresh(session);
		}

		private void Refresh(CodeGenWindowSession session)
		{
			_generateMembersGroup.style.display =
				(session.IsValid && session.GenerateMembersMode == GenerateMembersMode.Custom).ToDisplayStyle();
			_generateMembersMode.style.display = session.IsValid.ToDisplayStyle();
			_processButton.style.display = session.IsValid.ToDisplayStyle();
			_resultsLabel.style.display = session.IsValid.ToDisplayStyle();
			_generateMembersGroup.Clear();
			if (session.Requests == null)
			{
				return;
			}
			_generateMembersGroup.Add(new Label("Essential:"));
			foreach (KeyValuePair<string, FileGenerationRequest> request in session.Requests.Where(r => r.Value.IsEssential))
			{
				InspectorElement ie = new InspectorElement(request.Value);
				_generateMembersGroup.Add(ie);
			}
			
			_generateMembersGroup.Add(new Label("Optional:"));
			foreach (KeyValuePair<string, FileGenerationRequest> request in session.Requests.Where(r => r.Value.IsEssential == false))
			{
				InspectorElement ie = new InspectorElement(request.Value);
				_generateMembersGroup.Add(ie);
			}
		}
	}
}