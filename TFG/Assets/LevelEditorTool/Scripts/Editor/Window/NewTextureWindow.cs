using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using FMB_Tools;
using LMGTool;

public class NewTextureWindow : EditorWindow
{
	//	Texture2D newTexture;
	public LMGEditorWindow win;
	int xSize = 20;
	int ySize = 20;
	bool reTry = false;

	void OnEnable ()
	{
		maxSize = new Vector2 (400, 50);
		minSize = new Vector2 (400, 50);
	}

	void OnGUI ()
	{
		EditorGUILayout.BeginHorizontal ();
		xSize = EditorGUILayout.IntField ("Width", xSize);
		ySize = EditorGUILayout.IntField ("Height", ySize);
		EditorGUILayout.EndHorizontal ();
		if (GUILayout.Button ("Create")) {
			win.SetTexture (Create ());
			AssetDatabase.Refresh ();
			if (!reTry) {
				Close ();	
			}
		}
	}

	Texture2D Create ()
	{
		int totalPixels = xSize * ySize;
		if (totalPixels <= 22500) {
			return EditorTextureUtilities.CreateNewImage (xSize, ySize);
		} else {
			if (EditorUtility.DisplayDialog ("Image is too big to edit", "Unity does not support big size images in editor, try a smaller size.", "OK")) {
				reTry = true;
			}
			return null;
		}
	}
}
