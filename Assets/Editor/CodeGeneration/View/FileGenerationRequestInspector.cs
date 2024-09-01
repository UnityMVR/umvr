using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace pindwin.umvr.Editor.CodeGeneration.Window
{
	[CustomEditor(typeof(FileGenerationRequest))]
	public class FileGenerationRequestInspector : UnityEditor.Editor
	{
		[SerializeField] private VisualTreeAsset _requestUXML;
		
		public override VisualElement CreateInspectorGUI()
		{
			VisualElement root = new VisualElement();
			_requestUXML.CloneTree(root);
			var fileName = root.Q<Label>("FileNameLabel");
			fileName.bindingPath = nameof(FileGenerationRequest.Name);
			var toggle = root.Q<Toggle>();
			toggle.bindingPath = nameof(FileGenerationRequest.IsRequested);
			var errors = root.Q<Label>("ErrorsLabel");
			errors.bindingPath = nameof(FileGenerationRequest.Messages);
			return root;
		}
	}
}