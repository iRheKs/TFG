using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LMGTool
{
	[CustomPropertyDrawer (typeof(ScriptableObject), true)]
	[System.Serializable]
	public class ScriptableObjectDrawer : PropertyDrawer
	{
		private Editor editor = null;

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.PropertyField (position, property, label, true);

			if (property.objectReferenceValue != null) {
				
				property.isExpanded = EditorGUI.Foldout (position, property.isExpanded, GUIContent.none);

				if (property.objectReferenceValue == null)
					return;

				if (property.isExpanded) {

					EditorGUI.indentLevel++;
					if (property.objectReferenceValue == null) {
						return;
					}
					if (!editor) {
						Editor.CreateCachedEditor (property.objectReferenceValue, null, ref editor);
					}
					if (editor)
						editor.OnInspectorGUI ();

					EditorGUI.indentLevel--;

				}
			}
		}
	}
}
