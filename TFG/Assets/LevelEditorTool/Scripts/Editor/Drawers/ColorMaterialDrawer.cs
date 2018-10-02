using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LMGTool
{
	[CustomPropertyDrawer (typeof(PhyMat))]
	[CustomPropertyDrawer (typeof(PhyMat2D))]
	public class ColorMaterialDrawer : PropertyDrawer
	{
		LMGEditorWindow myWindow;
		SerializedProperty color;
		SerializedProperty material;

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			color = property.FindPropertyRelative ("color");
			material = property.FindPropertyRelative ("material");
			int oldIndentLevel = EditorGUI.indentLevel;
			label = EditorGUI.BeginProperty (position, label, property);
			Rect contentPosition = EditorGUI.PrefixLabel (position, label);
			if (position.height > 16f) {
				position.height = 16f;
				EditorGUI.indentLevel += 1;
				contentPosition = EditorGUI.IndentedRect (position);
				contentPosition.y += 18f;
			}
			contentPosition.width *= 0.55f;
			EditorGUI.indentLevel = 0;
			EditorGUI.PropertyField (contentPosition, material, GUIContent.none);
			contentPosition.x += contentPosition.width;
			contentPosition.width /= 2f;
			EditorGUIUtility.labelWidth = 14f;
			EditorGUI.BeginChangeCheck ();
			EditorGUI.PropertyField (contentPosition, color, new GUIContent ("C"));
			if (EditorGUI.EndChangeCheck () && myWindow != null) {
				if (material.objectReferenceValue != null) {
					myWindow.SetColor (color.colorValue, (material.objectReferenceValue as GameObject).name);
				} else
					Debug.LogWarning ("No prefab has been asigned, check you references");
			}
			contentPosition.x += contentPosition.width;
			contentPosition.width /= 2f;
			if (GUI.Button (contentPosition, "+", EditorStyles.miniButton)) {
				// set color left button editor window
				if (myWindow == null)
					myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
				if (material.objectReferenceValue != null) {
					myWindow.SetColor (color.colorValue, (material.objectReferenceValue as GameObject).name);
				} else
					Debug.LogWarning ("No prefab has been asigned, check you references");
			}
			EditorGUI.EndProperty ();
			EditorGUI.indentLevel = oldIndentLevel;
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return label != GUIContent.none && Screen.width < 333 ? (16f + 18f) : 16f;
		}

	}
}

