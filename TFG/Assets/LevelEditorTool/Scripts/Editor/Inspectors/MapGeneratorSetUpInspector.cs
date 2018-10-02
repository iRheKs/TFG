using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace LMGTool
{
	[CustomEditor (typeof(MapGeneratorSetUp))]
	public class MapGeneratorSetUpInspector : Editor
	{
		LMGEditorWindow myWindow;

		bool rotCollapse;
		bool scaleCollapse;
		bool objCollapse;
		bool colorCollapse;

		static SerializedProperty level;
		static SerializedProperty size;
		static SerializedProperty entities;
		static SerializedProperty objects;
		static SerializedProperty misc;
		static SerializedProperty minRotation;
		static SerializedProperty maxRotation;
		static SerializedProperty minScale;
		static SerializedProperty maxScale;
		static SerializedProperty exOp;
		static SerializedProperty player;
		static SerializedProperty horizontal;
		static SerializedProperty straight;
		static SerializedProperty curve;
		static SerializedProperty cross;
		static SerializedProperty begin;
		static SerializedProperty end;
		static SerializedProperty pathColor;
		static SerializedProperty beginColor;
		static SerializedProperty endColor;

		void Init ()
		{
			level = serializedObject.FindProperty ("editorGeneratorLevel");
			size = serializedObject.FindProperty ("blockSize");
			entities = serializedObject.FindProperty ("entities");
			objects = serializedObject.FindProperty ("objects");
			misc = serializedObject.FindProperty ("misc");
			minScale = serializedObject.FindProperty ("minScale");
			maxScale = serializedObject.FindProperty ("maxScale");
			minRotation = serializedObject.FindProperty ("minRotation");
			maxRotation = serializedObject.FindProperty ("maxRotation");
			exOp = serializedObject.FindProperty ("extraOptions");
			player = serializedObject.FindProperty ("player");
			horizontal = serializedObject.FindProperty ("horizontal");
			straight = serializedObject.FindProperty ("straight");
			curve = serializedObject.FindProperty ("curve");
			cross = serializedObject.FindProperty ("cross");
			begin = serializedObject.FindProperty ("begin");
			end = serializedObject.FindProperty ("end");
			pathColor = serializedObject.FindProperty ("pathColor");
			beginColor = serializedObject.FindProperty ("beginColor");
			endColor = serializedObject.FindProperty ("endColor");
		}

		public override void OnInspectorGUI ()
		{
//			if (myWindow == null)
//				myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
			serializedObject.Update ();
			if (serializedObject == null) {
				return;
			}
			Init ();

			var trg = (MapGeneratorSetUp)target;
			if (trg == null) {
				return;
			}

			EditorGUI.BeginChangeCheck ();

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("Level/Map to Generate"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUI.indentLevel++;

			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (level, new GUIContent ("Level/Map Texture"));
			if (EditorGUI.EndChangeCheck ()) {
				if (myWindow == null)
					myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
				myWindow.SetTexture ((Texture2D)level.objectReferenceValue);
			}

			EditorGUI.indentLevel--;

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("Block Size", "Unity units (1/100 pixel)"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField (size, new GUIContent ("Size", "Size to represent one pixel"));

			EditorGUI.indentLevel--;

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("Entities and Blocks to Generate"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField (entities, new GUIContent ("Entities", "Entities in Level/Map"), true);

			EditorGUILayout.PropertyField (objects, new GUIContent ("Objects", "Objects to make Level/Map"), true);

			EditorGUILayout.PropertyField (misc, new GUIContent ("Misc Objects", "Decoration for the Level/Map"), true);
			if (misc.arraySize > 0) {
				EditorGUI.indentLevel++;

				rotCollapse = EditorGUILayout.Foldout (rotCollapse, new GUIContent ("Random Rotation", "Random values between 2 vectors."), true);

				if (rotCollapse) {
					EditorGUI.indentLevel++;

					EditorGUILayout.PropertyField (minRotation, new GUIContent ("Minimun", "Minimun rotation factor"));

					EditorGUILayout.PropertyField (maxRotation, new GUIContent ("Maximun", "Maximun rotation factor"));

					EditorGUILayout.Space ();
					EditorGUI.indentLevel--;
				}

				scaleCollapse = EditorGUILayout.Foldout (scaleCollapse, new GUIContent ("Random Scale", "Random values between 2 vectors."), true);

				if (scaleCollapse) {
					EditorGUI.indentLevel++;

					EditorGUILayout.PropertyField (minScale, new GUIContent ("Minimun", "Minimun scale factor"));

					EditorGUILayout.PropertyField (maxScale, new GUIContent ("Maximun", "Maximun scale factor"));

					EditorGUILayout.Space ();
					EditorGUI.indentLevel--;
				}
			}
			EditorGUI.indentLevel--;

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("More Options", "Check if need"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField (exOp, new GUIContent ("Extra Options", "Extra objects to place"));
			if (!exOp.boolValue)
				horizontal.boolValue = false;
			using (var group = new EditorGUILayout.FadeGroupScope (Convert.ToSingle (trg.extraOptions))) {
				if (group.visible) {
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField (player, new GUIContent ("Player", "If player is needed to be place"));
					EditorGUILayout.PropertyField (horizontal, new GUIContent ("Horizontal", "Is your game horizontaly placed"));
					EditorGUI.indentLevel--;
//					using (var group2 = new EditorGUILayout.FadeGroupScope (Convert.ToSingle (trg.horizontal))) {
//						if (group2.visible) {
					objCollapse = EditorGUILayout.Foldout (objCollapse, new GUIContent ("Path Objects", "Prefabs for creating a path."), true);
					if (objCollapse) {
						EditorGUI.indentLevel++;
						EditorGUILayout.PropertyField (straight, new GUIContent ("Straight Path", "Straight path prefab"));
						EditorGUILayout.PropertyField (curve, new GUIContent ("Corner Path", "Corner path prefab"));
						EditorGUILayout.PropertyField (cross, new GUIContent ("Cross Section Path", "Cross Section path prefab"));
						EditorGUILayout.PropertyField (begin, new GUIContent ("Begining Path", "Begining path prefab"));
						EditorGUILayout.PropertyField (end, new GUIContent ("Ending Path", "Ending path prefab"));
						EditorGUILayout.Space ();
						EditorGUI.indentLevel--;
					}
					colorCollapse = EditorGUILayout.Foldout (colorCollapse, new GUIContent ("Path Colors", "Color for placing the path."), true);
					if (colorCollapse) {
						EditorGUI.indentLevel++;

						EditorGUILayout.BeginHorizontal ();
						EditorGUI.BeginChangeCheck ();
						EditorGUILayout.PropertyField (pathColor, new GUIContent ("Path Color", "Straight and corner path color"));
						if (EditorGUI.EndChangeCheck () && myWindow != null) {
							myWindow.SetColor (pathColor.colorValue, "Path");
						}
						if (GUILayout.Button ("+", EditorStyles.miniButton, GUILayout.MaxWidth (40))) {
							if (myWindow == null)
								myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
							if (straight.objectReferenceValue != null && curve.objectReferenceValue != null) {
								myWindow.SetColor (pathColor.colorValue, "Path");
							} else
								Debug.LogWarning ("No prefab for straight path or corner has been asigned, check you references");
						}
						EditorGUILayout.EndHorizontal ();

						EditorGUILayout.BeginHorizontal ();
						EditorGUI.BeginChangeCheck ();
						EditorGUILayout.PropertyField (beginColor, new GUIContent ("Begining Color", "Begining path color"));
						if (EditorGUI.EndChangeCheck () && myWindow != null) {
							myWindow.SetColor (beginColor.colorValue, "Begining");
						}
						if (GUILayout.Button ("+", EditorStyles.miniButton, GUILayout.MaxWidth (40))) {
							if (myWindow == null)
								myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
							if (begin.objectReferenceValue != null) {
								myWindow.SetColor (beginColor.colorValue, "Begining");
							} else
								Debug.LogWarning ("No prefab for begining has been asigned, check you references");
						}
						EditorGUILayout.EndHorizontal ();

						EditorGUILayout.BeginHorizontal ();
						EditorGUI.BeginChangeCheck ();
						EditorGUILayout.PropertyField (endColor, new GUIContent ("Ending Color", "Ending path color"));
						if (EditorGUI.EndChangeCheck () && myWindow != null) {
							myWindow.SetColor (endColor.colorValue, "Ending");
						}
						if (GUILayout.Button ("+", EditorStyles.miniButton, GUILayout.MaxWidth (40))) {
							if (myWindow == null)
								myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
							if (end.objectReferenceValue != null) {
								myWindow.SetColor (endColor.colorValue, "Ending");
							} else
								Debug.LogWarning ("No prefab for ending has been asigned, check you references");
						}
						EditorGUILayout.EndHorizontal ();

						EditorGUILayout.Space ();
						EditorGUI.indentLevel--;
					}
//						}
//					}
				}

			}
			//
			//		EditorGUILayout.Space ();
			//
			//		EditorGUILayout.BeginHorizontal();
			//
			//		GUILayout.FlexibleSpace ();
			//
			//		if(GUILayout.Button (generate, EditorStyles.miniButtonLeft, miniButtonWidth)){
			//
			//			trg.GenerateLevel ();
			//
			//		}
			//
			//		if(GUILayout.Button (createPrefab, EditorStyles.miniButtonMid, miniButtonWidth)){
			//
			//			trg.CreatePrefab ();
			//
			//		}
			//			
			//		if(GUILayout.Button (clear, EditorStyles.miniButtonRight, miniButtonWidth)){
			//
			//			trg.Clean();
			//
			//		}
			//		GUILayout.FlexibleSpace ();
			//
			//		EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space ();
			EditorGUI.indentLevel = 0;
			if (EditorGUI.EndChangeCheck ()) {
				serializedObject.ApplyModifiedProperties ();
			}
			EditorUtility.SetDirty (trg);
		}

	}

	[CustomEditor (typeof(MapGenerator))]
	public class MapGeneratorInspector : Editor
	{
		LMGEditorWindow win;
		MapGeneratorSetUp setUp;

		private static GUILayoutOption miniButtonWidth = GUILayout.Width (100f);

		private static GUIContent generate = new GUIContent ("G", "Generate Blocks");
		private static GUIContent createPrefab = new GUIContent ("CP", "Create Prefab");
		private static GUIContent clear = new GUIContent ("Clear", "Clear");

		public override void OnInspectorGUI ()
		{
			var trg = (MapGenerator)target;
			if (GUILayout.Button ("Edit")) {
				if (win == null)
					win = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
				win.SetType (GENERATOR_TO_EDIT.Map, trg);
				win.OpenWindow ();
			}
		}

		public void OnWindowGUI ()
		{

			var trg = (MapGenerator)target;

			EditorGUI.BeginChangeCheck ();

			DrawDefaultInspector ();

			EditorGUILayout.Space ();

			EditorGUILayout.BeginHorizontal ();

			GUILayout.FlexibleSpace ();

			if (GUILayout.Button (generate, EditorStyles.miniButtonLeft, miniButtonWidth)) {

				trg.GenerateLevel ();

			}

			if (GUILayout.Button (createPrefab, EditorStyles.miniButtonMid, miniButtonWidth)) {

				trg.CreatePrefab ();

			}

			if (GUILayout.Button (clear, EditorStyles.miniButtonRight, miniButtonWidth)) {

				trg.Clean ();

			}
			GUILayout.FlexibleSpace ();

			EditorGUILayout.EndHorizontal ();

			EditorGUILayout.Space ();

			if (EditorGUI.EndChangeCheck ()) {
				serializedObject.ApplyModifiedProperties ();
				if (win == null)
					win = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
				setUp = (MapGeneratorSetUp)serializedObject.FindProperty ("setUp").objectReferenceValue;
				if (setUp != null)
					win.SetTexture ((Texture2D)setUp.editorGeneratorLevel);
			}

			EditorUtility.SetDirty (trg);
		}

	}
}