using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace LMGTool
{

	[CustomEditor(typeof(MapGeneratorSetUp_WS))]
	public class MapGeneratorSetUpInspector_WS : Editor {

		private bool collapsed;

		static SerializedProperty level;
		static SerializedProperty size;
		static SerializedProperty entities;
		static SerializedProperty recta;
		static SerializedProperty curva;
		static SerializedProperty begin;
		static SerializedProperty end;
		static SerializedProperty pathColor;
		static SerializedProperty beginColor;
		static SerializedProperty endColor;
		static SerializedProperty misc;
		static SerializedProperty minScale;
		static SerializedProperty maxScale;
		//static SerializedProperty player;
		//static SerializedProperty dim2;
		//static SerializedProperty horizontal;

		void OnEnable(){
			
			level = serializedObject.FindProperty ("editorGeneratorLevel");
			size = serializedObject.FindProperty ("blockSize");
			entities = serializedObject.FindProperty ("entities");
			recta = serializedObject.FindProperty ("recta");
			curva = serializedObject.FindProperty ("curva");
			begin = serializedObject.FindProperty ("begin");
			end = serializedObject.FindProperty ("end");
			pathColor = serializedObject.FindProperty ("floorColor");
			beginColor = serializedObject.FindProperty ("beginColor");
			endColor = serializedObject.FindProperty ("endColor");
			misc = serializedObject.FindProperty ("misc");
			minScale = serializedObject.FindProperty ("minScale");
			maxScale = serializedObject.FindProperty ("maxScale");
			//player = serializedObject.FindProperty ("player");
			//	dim2 = serializedObject.FindProperty ("dim2");
			//horizontal = serializedObject.FindProperty ("horizontal");
		}

		public override void OnInspectorGUI(){
			
			serializedObject.Update ();

			var trg = (MapGeneratorSetUp_WS)target;

			EditorGUI.BeginChangeCheck ();

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("Level/Map to Generate"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (level, new GUIContent ("Map Texture", "Pixel based texture"));

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("Block Size", "Unity units (1/100 pixel)"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (size, new GUIContent ("Size", "Size to represent one pixel"));

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("Path and Blocks to Generate"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (entities, new GUIContent ("Objects","Neutral objects in Map"), true);

			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (misc, new GUIContent ("Misc objects", "Decoration objects to generate"), true);

			collapsed = EditorGUILayout.Foldout (collapsed, new GUIContent ("Random Scale", "Random values between 2 vectors."), true);

			if (collapsed) {
				EditorGUI.indentLevel++;
				EditorGUILayout.Space ();

				EditorGUILayout.PropertyField (minScale, new GUIContent ("Minimun", "Minimun scale factor"));

				EditorGUILayout.Space ();

				EditorGUILayout.PropertyField (maxScale, new GUIContent ("Maximun", "Maximun scale factor"));

				EditorGUILayout.Space ();
				EditorGUI.indentLevel--;
			}

//			using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle ())){
//				if (group.visible) {
//					EditorGUI.indentLevel++;
//					EditorGUILayout.Space ();
//
//					EditorGUILayout.PropertyField (minScale, new GUIContent ("Minimun", "Minimun scale factor"));
//
//					EditorGUILayout.Space ();
//
//					EditorGUILayout.PropertyField (maxScale, new GUIContent ("Maximun", "Maximun scale factor"));
//
//					EditorGUILayout.Space ();
//					EditorGUI.indentLevel--;
//				}
//
//			}

			EditorGUILayout.PropertyField (recta, new GUIContent ("Straight path","Straight path to make Map"));
			EditorGUILayout.PropertyField (curva, new GUIContent ("Corner path","Corner path to make Map"));
			EditorGUILayout.PropertyField (begin, new GUIContent ("Begining path","Begining path to make Map"));
			EditorGUILayout.PropertyField (end, new GUIContent ("Ending path","Ending path to make Map"));

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("Path colors"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUI.indentLevel++;
			EditorGUILayout.PropertyField (pathColor, new GUIContent ("General","Color for the general path"));
			EditorGUILayout.PropertyField (beginColor, new GUIContent ("Begining","Color for the begining of the path"));
			EditorGUILayout.PropertyField (endColor, new GUIContent ("Ending","Color for the ending of the path"));
			EditorGUI.indentLevel--;
//			EditorGUILayout.Space ();
//			EditorGUILayout.LabelField (new GUIContent ("More Options", "Check if need"), EditorStyles.boldLabel);
//			EditorGUILayout.Space ();

			//EditorGUILayout.PropertyField (horizontal, new GUIContent ("Horizontal","Is your game horizontaly placed"));
			/*EditorGUILayout.PropertyField (exOp, new GUIContent ("Extra Options", "Extra objects to place"));

			using (var group = new EditorGUILayout.FadeGroupScope(Convert.ToSingle (trg.extraOptions))){
				if (group.visible) {
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField (player,new GUIContent ("Player","If player is needed to be place"));
					EditorGUILayout.PropertyField (horizontal, new GUIContent ("Horizontal","Is your game horizontaly placed"));
					//				using (var group2 = new EditorGUILayout.FadeGroupScope(Convert.ToSingle (trg.horizontal))){
					//					if (group2.visible) {
					//						EditorGUILayout.PropertyField (dim2, new GUIContent ("2D Mode", "Is your game in 2D"));
					//					}
					//				}
				}

			}*/
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
			if (EditorGUI.EndChangeCheck ()) {
				serializedObject.ApplyModifiedProperties ();
			}
			EditorUtility.SetDirty (trg);
		}


	}
	[CustomEditor(typeof(InGameMapGenerator_WS))]
	public class MapGeneratorInspector_WS : Editor{

		private static GUILayoutOption miniButtonWidth = GUILayout.Width (75f);

		private static GUIContent generate = new GUIContent("G","Generate Blocks");
		private static GUIContent createPrefab = new GUIContent ("CP", "Create Prefab");
		private static GUIContent saveMesh = new GUIContent ("SM", "Save Mesh");
		private static GUIContent clear = new GUIContent("Clear","Clear");

		public override void OnInspectorGUI ()
		{

			DrawDefaultInspector ();

			EditorGUI.BeginChangeCheck ();

			var trg = (InGameMapGenerator_WS)target;

			EditorGUILayout.Space ();

			EditorGUILayout.BeginHorizontal();

			GUILayout.FlexibleSpace ();

			if(GUILayout.Button (generate, EditorStyles.miniButtonLeft, miniButtonWidth)){

				trg.GenerateMap ();

			}

			if(GUILayout.Button (createPrefab, EditorStyles.miniButtonMid, miniButtonWidth)){

				trg.CreatePrefab ();

			}

			if(GUILayout.Button (saveMesh, EditorStyles.miniButtonMid, miniButtonWidth)){

				trg.SaveMesh ();

			}

			if(GUILayout.Button (clear, EditorStyles.miniButtonRight, miniButtonWidth)){

				trg.Clean();

			}
			GUILayout.FlexibleSpace ();

			EditorGUILayout.EndHorizontal ();

			if (EditorGUI.EndChangeCheck ()) {
				serializedObject.ApplyModifiedProperties ();
			}

			EditorUtility.SetDirty (trg);
		}

	}

}