using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace LMGTool
{
	[CustomPropertyDrawer (typeof(Entity_WB))]
//	[CustomPropertyDrawer (typeof(SolidObjects))]
//	[CustomPropertyDrawer (typeof(Player))]
	public class ColorPrefabDrawer_WB : PropertyDrawer
	{

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			int oldIndentLevel = EditorGUI.indentLevel;
			label = EditorGUI.BeginProperty (position, label, property);
			Rect contentPosition = EditorGUI.PrefixLabel (position, label);
			if (position.height > 16f) {
				position.height = 16f;
				EditorGUI.indentLevel += 1;
				contentPosition = EditorGUI.IndentedRect (position);
				contentPosition.y += 20f;
			}
			contentPosition.width *= 0.50f;
			EditorGUI.indentLevel = 0;
			EditorGUI.PropertyField (contentPosition, property.FindPropertyRelative ("prefab"), GUIContent.none);
			contentPosition.x += contentPosition.width;
			contentPosition.width /= 2f;
			EditorGUIUtility.labelWidth = 14f;
			EditorGUI.PropertyField (contentPosition, property.FindPropertyRelative ("color"), new GUIContent ("C"));
			contentPosition.x += contentPosition.width;
			contentPosition.width /= 2f;
			EditorGUI.PropertyField (contentPosition, property.FindPropertyRelative ("rotation"), new GUIContent ("R"));
			contentPosition.x += contentPosition.width;
			EditorGUI.PropertyField (contentPosition, property.FindPropertyRelative ("scale"), new GUIContent ("S"));
			EditorGUI.EndProperty ();
			EditorGUI.indentLevel = oldIndentLevel;
		}

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return label != GUIContent.none && Screen.width < 333 ? (16f + 18f) : 16f;
		}

	}
}