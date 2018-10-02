using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Reflection;
using System;

namespace LMGTool
{public class CreateGenerator_WS : Editor {

	[MenuItem("GameObject/Map WS",false,10)]
	static void MapGenerator(MenuCommand menuCommand){

			//AssetDatabase.CreateAsset (ScriptableObject.CreateInstance (typeof(MapGeneratorSetUp_BS)), "Assets/MapSetUp.asset");

		//GameObject go = GameObject.Find ("MapGenerator");
		GameObject go2 = GameObject.Find ("MapGenerator_WS");

		if (go2 == null) {
			
			//go = new GameObject ("MapGenerator", typeof(MapGenerator));
			go2 = new GameObject ("MapGenerator_WS", typeof(InGameMapGenerator_WS));

//			GameObject pools = new GameObject ("Pools", typeof(MapMultiPool_BS));
				GameObject block = new GameObject ("Blocks", typeof(MeshFilter), typeof(MeshRenderer));

			block.transform.SetParent (go2.transform);
//			pools.transform.SetParent (go2.transform);

			//set blue label to game object
			var egu = typeof(EditorGUIUtility);
			//var icon = EditorGUIUtility.IconContent ("sv_label_1");
			var icon2 = EditorGUIUtility.IconContent ("sv_label_3");
			var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
			//var args = new object[]{ go, icon.image };
			var args2 = new object[]{ go2, icon2.image };
			var setIcon = egu.GetMethod ("SetIconForObject",flags,null,new Type[]{typeof(UnityEngine.Object),typeof(Texture2D)},null);
			//setIcon.Invoke (null, args);
			setIcon.Invoke (null, args2);

			//GameObjectUtility.SetParentAndAlign (go,menuCommand.context as GameObject);
			GameObjectUtility.SetParentAndAlign (go2,menuCommand.context as GameObject);

			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs")) {
				AssetDatabase.CreateFolder ("Assets","Prefabs");
			}

			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs/Generators_WS")) {
				AssetDatabase.CreateFolder ("Assets/Prefabs","Generators_WS");
			}

			//PrefabUtility.CreatePrefab ("Assets/Prefabs/Generators/" + go.name + ".prefab", go, ReplacePrefabOptions.ConnectToPrefab);
				PrefabUtility.CreatePrefab ("Assets/Prefabs/Generators_WS/" + go2.name + ".prefab", go2, ReplacePrefabOptions.ConnectToPrefab);

			//Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);
			Undo.RegisterCreatedObjectUndo (go2, "Create " + go2.name);

		}
//			else if(go != null && go2 == null){
//			go2 = new GameObject ("InGameMapGenerator", typeof(InGameMapGenerator));
//
//			GameObject pools = new GameObject ("Pools", typeof(MapMultiPool));
//
//			pools.transform.SetParent (go2.transform);
//
//			//set blue label to game object
//			var egu = typeof(EditorGUIUtility);
//			var icon2 = EditorGUIUtility.IconContent ("sv_label_3");
//			var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
//			var args2 = new object[]{ go2, icon2.image };
//			var setIcon = egu.GetMethod ("SetIconForObject",flags,null,new Type[]{typeof(UnityEngine.Object),typeof(Texture2D)},null);
//			setIcon.Invoke (null, args2);
//
//			GameObjectUtility.SetParentAndAlign (go2,menuCommand.context as GameObject);
//
//			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs")) {
//				AssetDatabase.CreateFolder ("Assets","Prefabs");
//			}
//
//			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs/Generators")) {
//				AssetDatabase.CreateFolder ("Assets/Prefabs","Generators");
//			}
//
//			PrefabUtility.CreatePrefab ("Assets/Prefabs/Generators/" + go2.name + ".prefab", go2, ReplacePrefabOptions.ConnectToPrefab);
//
//			Undo.RegisterCreatedObjectUndo (go2, "Create " + go2.name);
//			
//		}else if(go == null && go2 != null){
//
//			go = new GameObject ("MapGenerator", typeof(MapGenerator));
//
//			GameObject block = new GameObject ("Blocks");
//
//			block.transform.SetParent (go.transform);
//
//			//set blue label to game object
//			var egu = typeof(EditorGUIUtility);
//			var icon = EditorGUIUtility.IconContent ("sv_label_1");
//			var flags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
//			var args = new object[]{ go, icon.image };
//			var setIcon = egu.GetMethod ("SetIconForObject",flags,null,new Type[]{typeof(UnityEngine.Object),typeof(Texture2D)},null);
//			setIcon.Invoke (null, args);
//
//			GameObjectUtility.SetParentAndAlign (go,menuCommand.context as GameObject);
//
//			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs")) {
//				AssetDatabase.CreateFolder ("Assets","Prefabs");
//			}
//
//			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs/Generators")) {
//				AssetDatabase.CreateFolder ("Assets/Prefabs","Generators");
//			}
//
//			PrefabUtility.CreatePrefab ("Assets/Prefabs/Generators/" + go.name + ".prefab", go, ReplacePrefabOptions.ConnectToPrefab);
//
//			Undo.RegisterCreatedObjectUndo (go, "Create " + go.name);
//
//		}else if(go != null && go2 != null){
//
//			Debug.LogWarning ("Generators already created");
//
//		}

		Selection.activeObject = go2;

			AssetDatabase.SaveAssets ();
			AssetDatabase.Refresh ();
	}

}
}