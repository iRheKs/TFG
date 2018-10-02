using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LMGTool
{
	[CustomPropertyDrawer (typeof(MiscObject))]
	public class MiscObjectDrawer : PropertyDrawer
	{
		LMGEditorWindow myWindow;

		SerializedProperty prefab;
		SerializedProperty color;
		SerializedProperty rotation;
		SerializedProperty scale;

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
//			if (myWindow == null)
//				myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
			prefab = property.FindPropertyRelative ("prefab");
			color = property.FindPropertyRelative ("color");
			rotation = property.FindPropertyRelative ("rotation");
			scale = property.FindPropertyRelative ("scale");

			int oldIndentLevel = EditorGUI.indentLevel;
			label = EditorGUI.BeginProperty (position, label, property);
			Rect contentPosition = EditorGUI.PrefixLabel (position, label);
			if (position.height > 16f) {
				position.height = 16f;
				EditorGUI.indentLevel += 1;
				contentPosition = EditorGUI.IndentedRect (position);
				contentPosition.y += 20f;
			}
			contentPosition.width *= 0.45f;
			EditorGUI.indentLevel = 0;
			EditorGUI.PropertyField (contentPosition, prefab, GUIContent.none);
			contentPosition.x += contentPosition.width;
			contentPosition.width /= 2f;
			EditorGUIUtility.labelWidth = 12f;
			EditorGUI.BeginChangeCheck ();
			EditorGUI.PropertyField (contentPosition, color, new GUIContent ("C"));
			if (EditorGUI.EndChangeCheck () && myWindow != null) {
				if (prefab.objectReferenceValue != null) {
					myWindow.SetColor (color.colorValue, (prefab.objectReferenceValue as GameObject).name);
				} else
					Debug.LogWarning ("No prefab has been asigned, check you references");
				if (color != null) {
					Color aux = color.colorValue;
					aux.a = 1.0f;
					color.colorValue = aux;
				}
			}
			contentPosition.x += contentPosition.width;
			contentPosition.width /= 2f;
			EditorGUI.PropertyField (contentPosition, rotation, new GUIContent ("R"));
			contentPosition.x += contentPosition.width;
			EditorGUI.PropertyField (contentPosition, scale, new GUIContent ("S"));
			contentPosition.x += contentPosition.width;
			contentPosition.width /= 1.1f;
			if (GUI.Button (contentPosition, "+", EditorStyles.miniButton)) {
				// set color left button editor window
				if (myWindow == null)
					myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
				if (prefab.objectReferenceValue != null) {
					myWindow.SetColor (color.colorValue, (prefab.objectReferenceValue as GameObject).name);
				} else
					Debug.LogWarning ("No prefab has been asigned, check you references");
			}
			//añadir un boton de seteo de color en la ventana
			EditorGUI.EndProperty ();
			EditorGUI.indentLevel = oldIndentLevel;
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return label != GUIContent.none && Screen.width < 333 ? (16f + 18f) : 16f;
		}
	}
}
