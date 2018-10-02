using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;
using System;

namespace LMGTool
{
	[CustomEditor (typeof(LevelGeneratorSetUp))]
	public class LevelGeneratorSetUpInspector : Editor
	{
		LMGEditorWindow myWindow;

		static SerializedProperty level;
		static SerializedProperty size;
		static SerializedProperty entities;
		static SerializedProperty objects;
		static SerializedProperty exOp;
		static SerializedProperty deathZone;
		static SerializedProperty player;
		static SerializedProperty material;
		static SerializedProperty material2D;
		static SerializedProperty dim2;
		static SerializedProperty airColor;
		static SerializedProperty floorObjects;
		static SerializedProperty wallObjects;
		static SerializedProperty floor;
		static SerializedProperty wall;
		static SerializedProperty singleWF;
		static SerializedProperty singleMat;
		static SerializedProperty singleMaterial;
		static SerializedProperty singleMaterial2D;

		void Init ()
		{
			level = serializedObject.FindProperty ("editorGeneratorLevel");
			size = serializedObject.FindProperty ("blockSize");
			entities = serializedObject.FindProperty ("entities");
			objects = serializedObject.FindProperty ("objects");
			exOp = serializedObject.FindProperty ("extraOptions");
			deathZone = serializedObject.FindProperty ("deathZonePrefab");
			player = serializedObject.FindProperty ("player");
			material = serializedObject.FindProperty ("physicsMaterial");
			material2D = serializedObject.FindProperty ("physicsMaterial2D");
			dim2 = serializedObject.FindProperty ("dim2");
			airColor = serializedObject.FindProperty ("airColor");
			floorObjects = serializedObject.FindProperty ("floorObjects");
			wallObjects = serializedObject.FindProperty ("wallObjects");
			floor = serializedObject.FindProperty ("floor");
			wall = serializedObject.FindProperty ("wall");
			singleWF = serializedObject.FindProperty ("singleWallFloor");
			singleMat = serializedObject.FindProperty ("singleMat");
			singleMaterial = serializedObject.FindProperty ("singleMaterial");
			singleMaterial2D = serializedObject.FindProperty ("singleMaterial2D");
		}

		public override void OnInspectorGUI ()
		{
			serializedObject.Update ();

			if (serializedObject == null) {
				return;
			}

			Init ();

			var trg = (LevelGeneratorSetUp)target;

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

			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (objects, new GUIContent ("Objects", "Objects to make Level/Map (Not Wall or Floor)"), true);

			EditorGUILayout.Space ();

			EditorGUILayout.PropertyField (singleWF, new GUIContent ("Single Wall/Floor", "Mark if you only need one block for the wall and the floor"));

			using (var group = new EditorGUILayout.FadeGroupScope (Convert.ToSingle (trg.singleWallFloor))) {

				if (group.visible) {
					EditorGUILayout.Space ();
					EditorGUILayout.LabelField (new GUIContent ("Single Wall and Floor blocks"), EditorStyles.boldLabel);
					EditorGUILayout.Space ();

					EditorGUI.indentLevel++;

					EditorGUILayout.PropertyField (wall, new GUIContent ("Single Wall", "Only one block for the wall"));
					EditorGUILayout.PropertyField (floor, new GUIContent ("Single Floor", "Only one block for the floor"));

					EditorGUI.indentLevel--;

				} else {

					EditorGUILayout.Space ();
					EditorGUILayout.LabelField (new GUIContent ("Multiple Wall and Floor blocks"), EditorStyles.boldLabel);
					EditorGUILayout.Space ();

					EditorGUI.indentLevel++;

					EditorGUILayout.PropertyField (wallObjects, new GUIContent ("Multiple Wall Objects", "Multiple blocks for the wall"), true);
					EditorGUILayout.PropertyField (floorObjects, new GUIContent ("Multiple Floor Objects", "Multiple blocks for the floor"), true);

					EditorGUI.indentLevel--;
				}
			}

			EditorGUI.indentLevel--;

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("Color for the air", "Default color white"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUI.indentLevel++;

			EditorGUILayout.BeginHorizontal ();
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (airColor, new GUIContent ("Air Color", "Color representing air"));
			if (EditorGUI.EndChangeCheck () && myWindow != null) {
				if (myWindow == null)
					myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
				myWindow.SetColor (airColor.colorValue, "Air");
			}
			if (GUILayout.Button ("+", EditorStyles.miniButton, GUILayout.MaxWidth (40))) {
				if (myWindow == null)
					myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
				myWindow.SetColor (airColor.colorValue, "Air");
			}
			EditorGUILayout.EndHorizontal ();

			EditorGUI.indentLevel--;

			EditorGUILayout.Space ();
			EditorGUILayout.LabelField (new GUIContent ("More Options", "Check if need"), EditorStyles.boldLabel);
			EditorGUILayout.Space ();

			EditorGUI.indentLevel++;

			EditorGUILayout.PropertyField (exOp, new GUIContent ("Extra Options", "Extra objects to place"));


			using (var group = new EditorGUILayout.FadeGroupScope (Convert.ToSingle (trg.extraOptions))) {
				if (group.visible) {
					EditorGUI.indentLevel++;
					EditorGUILayout.PropertyField (deathZone, new GUIContent ("Death Area", "For platformer, under level death"));
					EditorGUILayout.PropertyField (player, new GUIContent ("Player", "If player is needed to be place"));
					EditorGUILayout.PropertyField (dim2, new GUIContent ("2D Mode", "Is your game in 2D"));
					EditorGUILayout.Space ();

					EditorGUI.indentLevel--;

					EditorGUILayout.LabelField (new GUIContent ("Physical Material", "Physical Material added to all colliders (specified color if not single material)"), EditorStyles.boldLabel);

					EditorGUI.indentLevel++;

					EditorGUILayout.Space ();
					EditorGUILayout.PropertyField (singleMat, new GUIContent ("Single Material", "Are you using only one material?"));

					using (var group2 = new EditorGUILayout.FadeGroupScope (Convert.ToSingle (trg.singleMat))) {
						
						if (group2.visible) {
							using (var group3 = new EditorGUILayout.FadeGroupScope (Convert.ToSingle (trg.dim2))) {
								if (group3.visible) {
									EditorGUILayout.PropertyField (singleMaterial2D, new GUIContent ("Physical Material 2D", "Physics Material for every collider (2D only)"));
								} else {
									EditorGUILayout.PropertyField (singleMaterial, new GUIContent ("Physical Material", "Physics Material for every collider (3D only)"));
								}
							}
						} else {
							using (var group4 = new EditorGUILayout.FadeGroupScope (Convert.ToSingle (trg.dim2))) {
								if (group4.visible) {
									EditorGUILayout.PropertyField (material2D, new GUIContent ("Physical Material 2D", "Physics Material for color collider (2D only)"), true);
								} else {
									EditorGUILayout.PropertyField (material, new GUIContent ("Physical Material", "Physics Material for color collider (3D only)"), true);
								}
							}
						}
					}

				}

			}

			EditorGUI.indentLevel--;

			EditorGUILayout.Space ();
			if (EditorGUI.EndChangeCheck ()) {
				serializedObject.ApplyModifiedProperties ();
			}

			EditorUtility.SetDirty (trg);

		}

	}

	[CustomEditor (typeof(LevelPlatformerGenerator))]
	public class LevelGeneratorInspector : Editor
	{
		LMGEditorWindow win;
		LevelGeneratorSetUp setUp;

		private static GUILayoutOption miniButtonWidth = GUILayout.Width (50f);

		private static GUIContent generate = new GUIContent ("G", "Generate Blocks");
		private static GUIContent generateColliders = new GUIContent ("GC", "Generate Colliders");
		private static GUIContent setColliders = new GUIContent ("SC", "Set Colliders");
		private static GUIContent createPrefab = new GUIContent ("CP", "Create Prefab");
		private static GUIContent clear = new GUIContent ("Clear", "Clear");


		public override void OnInspectorGUI ()
		{
			var trg = (LevelPlatformerGenerator)target;
			if (GUILayout.Button ("Edit")) {
				if (win == null)
					win = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
				win.SetType (GENERATOR_TO_EDIT.Level, trg);
				win.OpenWindow ();
			}
		}

		public void OnWindowGUI ()
		{
			var trg = (LevelPlatformerGenerator)target;
			EditorGUI.BeginChangeCheck ();

			DrawDefaultInspector ();

			EditorGUILayout.Space ();

			EditorGUILayout.BeginHorizontal ();

			GUILayout.FlexibleSpace ();

			if (GUILayout.Button (generate, EditorStyles.miniButtonLeft, miniButtonWidth)) {

				trg.GenerateLevel ();

			}
			if (GUILayout.Button (generateColliders, EditorStyles.miniButtonMid, miniButtonWidth)) {

				trg.CheckMaterials ();

			}
			if (GUILayout.Button (setColliders, EditorStyles.miniButtonMid, miniButtonWidth)) {

				trg.AttachColliders ();

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
				setUp = (LevelGeneratorSetUp)serializedObject.FindProperty ("setUp").objectReferenceValue;
				if (setUp != null)
					win.SetTexture ((Texture2D)setUp.editorGeneratorLevel);
			}

			EditorUtility.SetDirty (trg);
		}

	}

}