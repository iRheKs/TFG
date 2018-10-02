using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Analytics;

namespace LMGTool
{
	#if UNITY_EDITOR

	using UnityEditor;
	using UnityEditorInternal;

	[ExecuteInEditMode]
	public class LevelPlatformerGenerator : MonoBehaviour
	{

		public LevelGeneratorSetUp setUp;

		//	public Texture2D setUp.editorGeneratorLevel;

		//	public int setUp.blockSize;

		//	public Entity[] setUp.entities;

		//	public SolidObjects[] setUp.objects;

		//	public bool setUp.singleWallFloor;

		//	public SolidObjects[] setUp.wallObjects;

		//	public SolidObjects[] setUp.floorObjects;

		//	public SolidObjects /*setUp.floor,*/ setUp.wall;

		//	public bool setUp.extraOptions;

		//	public GameObject setUp.deathZonePrefab;

		//	public PhysicMaterial setUp.physicsMaterial;

		//	public Player setUp.player;

		//	public PhysicsMaterial2D setUp.physicsMaterial2D;

		//	public bool setUp.dim2;

		//	public Color setUp.airColor = Color.white;

		int floorCount, wallCount;

		int rows, columns;

		string wallTag = "Wall";

		string floorTag = "Floor";

		Transform prefabs;

		Transform colliders;

		GameObject colliderAtachment;

		void Update ()
		{
			colliders = transform.GetChild (0);
			prefabs = transform.GetChild (1);

		}

		public void GenerateColliders ()
		{

			Color temp;

			rows = setUp.editorGeneratorLevel.height;
			columns = setUp.editorGeneratorLevel.width;

			bool found = false;

			if (setUp.singleWallFloor) {

				for (int r = 0; r < rows; r++) {
					for (int c = 0; c <= columns; c++) {

						temp = setUp.editorGeneratorLevel.GetPixel (c, r);

						if (temp == Color.clear)
							return;

						found = false;

						for (int i = 0; i < setUp.entities.Length && !found; i++) {
							if (temp == setUp.entities [i].color) {
								found = true;
							}
						}

						if (temp == setUp.airColor) {
							if (floorCount > 1) {
								NewCollider (floorCount * setUp.blockSize, new Vector2 (c * setUp.blockSize, r * setUp.blockSize), setUp.floor.color);
								floorCount = 0;
							} else {
								floorCount = 0;
							}
						} else if (!found && temp == setUp.floor.color) {
							floorCount++;
						} else {
							if (floorCount > 1) {
								NewCollider (floorCount * setUp.blockSize, new Vector2 (c * setUp.blockSize, r * setUp.blockSize), setUp.floor.color);
								floorCount = 0;
							} else {
								floorCount = 0;
							}
						}
						if (c == columns) {
							if (floorCount > 2) {
								NewCollider ((floorCount - 1) * setUp.blockSize, new Vector2 (c * setUp.blockSize, r * setUp.blockSize), setUp.floor.color);
								floorCount = 0;
							} else {
								floorCount = 0;
							}
						}

					}
					floorCount = 0;
				}

				for (int r = 0; r < columns; r++) {
					for (int c = 0; c <= rows; c++) {

						temp = setUp.editorGeneratorLevel.GetPixel (r, c);

						found = false;

						for (int i = 0; i < setUp.entities.Length && !found; i++) {
							if (temp == setUp.entities [i].color) {
								found = true;
							}
						}

						if (temp == setUp.airColor) {
							if (wallCount > 1) {
								NewCollider (wallCount * setUp.blockSize, new Vector2 (r * setUp.blockSize, c * setUp.blockSize), setUp.wall.color);
								wallCount = 0;
							} else {
								wallCount = 0;
							}
						} else if (!found && temp == setUp.wall.color) {
							wallCount++;
						} else {
							if (wallCount > 1) {
								NewCollider (wallCount * setUp.blockSize, new Vector2 (r * setUp.blockSize, c * setUp.blockSize), setUp.wall.color);
								wallCount = 0;
							} else {
								wallCount = 0;
							}
						}
						if (c == rows) {
							if (wallCount > 2) {
								NewCollider ((wallCount - 1) * setUp.blockSize, new Vector2 (r * setUp.blockSize, c * setUp.blockSize), setUp.wall.color);
								wallCount = 0;
							} else {
								wallCount = 0;
							}
						}

					}
					wallCount = 0;
				}
			} else {

				bool floor, wall;
				Color foundColor;

				for (int r = 0; r < rows; r++) {
					for (int c = 0; c <= columns; c++) {

						temp = setUp.editorGeneratorLevel.GetPixel (c, r);

						found = false;
						floor = false;
						foundColor = setUp.airColor;

						for (int i = 0; i < setUp.entities.Length && !found; i++) {
							if (temp == setUp.entities [i].color) {
								found = true;
							}
						}

						for (int i = 0; i < setUp.floorObjects.Length; i++) {
							if (temp == setUp.floorObjects [i].color) {
								foundColor = setUp.floorObjects [i].color;
								floor = true;
							}
						}

						if (temp == setUp.airColor) {
							if (floorCount > 1) {
								NewCollider (floorCount * setUp.blockSize, new Vector2 (c * setUp.blockSize, r * setUp.blockSize), foundColor, false);
								floorCount = 0;
							} else {
								floorCount = 0;
							}
						} else if (!found && floor) {
							floorCount++;
						} else {
							if (floorCount > 1) {
								NewCollider (floorCount * setUp.blockSize, new Vector2 (c * setUp.blockSize, r * setUp.blockSize), foundColor, false);
								floorCount = 0;
							} else {
								floorCount = 0;
							}
						}
						if (c == columns) {
							if (floorCount > 2) {
								NewCollider ((floorCount - 1) * setUp.blockSize, new Vector2 (c * setUp.blockSize, r * setUp.blockSize), foundColor, false);
								floorCount = 0;
							} else {
								floorCount = 0;
							}
						}

					}
					floorCount = 0;
				}

				for (int r = 0; r < columns; r++) {
					for (int c = 0; c <= rows; c++) {

						temp = setUp.editorGeneratorLevel.GetPixel (r, c);

						found = false;
						wall = false;
						foundColor = setUp.airColor;

						for (int i = 0; i < setUp.entities.Length && !found; i++) {
							if (temp == setUp.entities [i].color) {
								found = true;
							}
						}

						for (int i = 0; i < setUp.wallObjects.Length; i++) {
							if (temp == setUp.wallObjects [i].color) {
								foundColor = setUp.wallObjects [i].color;
								wall = true;
							}
						}

						if (temp == setUp.airColor) {
							if (wallCount > 1) {
								NewCollider (wallCount * setUp.blockSize, new Vector2 (r * setUp.blockSize, c * setUp.blockSize), foundColor, false);
								wallCount = 0;
							} else {
								wallCount = 0;
							}
						} else if (!found && wall) {
							wallCount++;
						} else {
							if (wallCount > 1) {
								NewCollider (wallCount * setUp.blockSize, new Vector2 (r * setUp.blockSize, c * setUp.blockSize), foundColor, false);
								wallCount = 0;
							} else {
								wallCount = 0;
							}
						}
						if (c == rows) {
							if (wallCount > 2) {
								NewCollider ((wallCount - 1) * setUp.blockSize, new Vector2 (r * setUp.blockSize, c * setUp.blockSize), foundColor, false);
								wallCount = 0;
							} else {
								wallCount = 0;
							}
						}

					}
					wallCount = 0;
				}
			}


		}

		void NewCollider (int xSize, Vector2 pos, Color color, bool single = true)
		{
			GameObject go = new GameObject ();

			if (single) {
				if (color == setUp.floor.color) {

					if (!setUp.dim2) {

						BoxCollider bc = go.AddComponent <BoxCollider> ();

						go.name = floorTag;
						bc.material = setUp.singleMaterial;

						bc.size = new Vector3 (xSize, setUp.blockSize, setUp.blockSize);

						if (xSize % 2 == 0) {
							bc.center = new Vector3 (pos.x - (float)(xSize / 2) - (0.5f * setUp.blockSize), pos.y, 0);
						} else
							bc.center = new Vector3 (pos.x - setUp.blockSize - (xSize / 2), pos.y, 0);

					} else {
						BoxCollider2D bc = go.AddComponent <BoxCollider2D> ();

						go.name = floorTag;
						bc.sharedMaterial = setUp.singleMaterial2D;

						bc.size = new Vector3 (xSize, setUp.blockSize, setUp.blockSize);

						if (xSize % 2 == 0) {
							bc.offset = new Vector3 (pos.x - (float)(xSize / 2) - (0.5f * setUp.blockSize), pos.y, 0);
						} else
							bc.offset = new Vector3 (pos.x - setUp.blockSize - (xSize / 2), pos.y, 0);
					}

				} else if (color == setUp.wall.color) {

					if (!setUp.dim2) {

						BoxCollider bc = go.AddComponent <BoxCollider> ();

						go.name = wallTag;
						bc.material = setUp.singleMaterial;

						bc.size = new Vector3 (setUp.blockSize, xSize, setUp.blockSize);

						if (xSize % 2 == 0) {
							bc.center = new Vector3 (pos.x, pos.y - (float)(xSize / 2) - (0.5f * setUp.blockSize), 0);
						} else
							bc.center = new Vector3 (pos.x, pos.y - setUp.blockSize - (xSize / 2), 0);

					} else {
						BoxCollider2D bc = go.AddComponent <BoxCollider2D> ();

						go.name = wallTag;
						bc.sharedMaterial = setUp.singleMaterial2D;

						bc.size = new Vector3 (setUp.blockSize, xSize, setUp.blockSize);

						if (xSize % 2 == 0) {
							bc.offset = new Vector3 (pos.x, pos.y - (float)(xSize / 2) - (0.5f * setUp.blockSize), 0);
						} else
							bc.offset = new Vector3 (pos.x, pos.y - setUp.blockSize - (xSize / 2), 0);
					}

				}
			} else {
				bool floor = false, wall = false;

				for (int i = 0; i < setUp.floorObjects.Length; i++) {
					if (setUp.floorObjects [i].color == color) {
						floor = true;
					}
				}
				for (int i = 0; i < setUp.wallObjects.Length; i++) {
					if (setUp.wallObjects [i].color == color) {
						wall = true;
					}
				}

				if (floor) {

					if (!setUp.dim2) {

						BoxCollider bc = go.AddComponent <BoxCollider> ();

						go.name = floorTag;

						for (int i = 0; i < setUp.physicsMaterial.Length; i++) {
							if (setUp.physicsMaterial [i].color == color) {
								bc.material = setUp.physicsMaterial [i].material;
							}
						}

						bc.size = new Vector3 (xSize, setUp.blockSize, setUp.blockSize);

						if (xSize % 2 == 0) {
							bc.center = new Vector3 (pos.x - (float)(xSize / 2) - (0.5f * setUp.blockSize), pos.y, 0);
						} else
							bc.center = new Vector3 (pos.x - setUp.blockSize - (xSize / 2), pos.y, 0);

					} else {
						BoxCollider2D bc = go.AddComponent <BoxCollider2D> ();

						go.name = floorTag;

						for (int i = 0; i < setUp.physicsMaterial2D.Length; i++) {
							if (setUp.physicsMaterial [i].color == color) {
								bc.sharedMaterial = setUp.physicsMaterial2D [i].material;
							}
						}

						bc.size = new Vector3 (xSize, setUp.blockSize, setUp.blockSize);

						if (xSize % 2 == 0) {
							bc.offset = new Vector3 (pos.x - (float)(xSize / 2) - (0.5f * setUp.blockSize), pos.y, 0);
						} else
							bc.offset = new Vector3 (pos.x - setUp.blockSize - (xSize / 2), pos.y, 0);
					}

				} else if (wall) {

					if (!setUp.dim2) {

						BoxCollider bc = go.AddComponent <BoxCollider> ();

						go.name = wallTag;
						for (int i = 0; i < setUp.physicsMaterial.Length; i++) {
							if (setUp.physicsMaterial [i].color == color) {
								bc.material = setUp.physicsMaterial [i].material;
							}
						}

						bc.size = new Vector3 (setUp.blockSize, xSize, setUp.blockSize);

						if (xSize % 2 == 0) {
							bc.center = new Vector3 (pos.x, pos.y - (float)(xSize / 2) - (0.5f * setUp.blockSize), 0);
						} else
							bc.center = new Vector3 (pos.x, pos.y - setUp.blockSize - (xSize / 2), 0);

					} else {
						BoxCollider2D bc = go.AddComponent <BoxCollider2D> ();

						go.name = wallTag;

						for (int i = 0; i < setUp.physicsMaterial2D.Length; i++) {
							if (setUp.physicsMaterial [i].color == color) {
								bc.sharedMaterial = setUp.physicsMaterial2D [i].material;
							}
						}

						bc.size = new Vector3 (setUp.blockSize, xSize, setUp.blockSize);

						if (xSize % 2 == 0) {
							bc.offset = new Vector3 (pos.x, pos.y - (float)(xSize / 2) - (0.5f * setUp.blockSize), 0);
						} else
							bc.offset = new Vector3 (pos.x, pos.y - setUp.blockSize - (xSize / 2), 0);
					}

				}
			}


			go.transform.parent = colliders;

		}

		public void GenerateLevel ()
		{

			rows = setUp.editorGeneratorLevel.width;
			columns = setUp.editorGeneratorLevel.height;

			Color temp;
			bool found = false;

			for (int r = 0; r < rows; r++) {
				for (int c = 0; c < columns; c++) {
					temp = setUp.editorGeneratorLevel.GetPixel (r, c);

					found = false;

					if (setUp.player.prefab != null && temp == setUp.player.color) {
						InstancePrefab (setUp.player.prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
						found = true;
					}
					if (setUp.singleWallFloor) {
						if (temp == setUp.wall.color) {
							InstancePrefab (setUp.wall.prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
							found = true;
						}
						if (temp == setUp.floor.color) {
							InstancePrefab (setUp.floor.prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
							found = true;
						}
					} else {
						for (int i = 0; i < setUp.floorObjects.Length && !found; i++) {
							if (temp == setUp.floorObjects [i].color) {
								InstancePrefab (setUp.floorObjects [i].prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
								found = true;
							}
						}
						for (int i = 0; i < setUp.wallObjects.Length && !found; i++) {
							if (temp == setUp.wallObjects [i].color) {
								InstancePrefab (setUp.wallObjects [i].prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
								found = true;
							}
						}
					}
					if (!found) {
						for (int i = 0; i < setUp.objects.Length && !found; i++) {
							if (temp == setUp.objects [i].color) {
								InstancePrefab (setUp.objects [i].prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
								found = true;
							}
						}
					}
					if (!found) {
						for (int i = 0; i < setUp.entities.Length && !found; i++) {
							if (temp == setUp.entities [i].color) {
								InstancePrefab (setUp.entities [i].prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
								found = true;
							}
						}
					}
				}
			}
		}

		void InstancePrefab (GameObject index, Vector3 position)
		{
			//Debug.Log ("Generated: "+ index.ToString () + " at position: " + position.ToString ());
			GameObject.Instantiate (index, position, Quaternion.identity, prefabs);
		}

		public void CreatePrefab ()
		{

			colliderAtachment = GameObject.Find (setUp.editorGeneratorLevel.name);

			if (colliderAtachment == null) {
				colliderAtachment = new GameObject (setUp.editorGeneratorLevel.name);
			}

			GameObject go = new GameObject ("Blocks");

			go.transform.SetParent (transform);

			if (prefabs.childCount != 0) {
				var temp1 = prefabs.Cast<Transform> ().ToList ();
				foreach (var child in temp1) {
					child.SetParent (go.transform);
				}
			}

			go.transform.SetParent (colliderAtachment.transform);

			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs")) {
				AssetDatabase.CreateFolder ("Assets", "Prefabs");
			}

			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs/Levels")) {
				AssetDatabase.CreateFolder ("Assets/Prefabs", "Levels");
			}

			PrefabUtility.CreatePrefab ("Assets/Prefabs/Levels/" + colliderAtachment.name + ".prefab", colliderAtachment, ReplacePrefabOptions.ConnectToPrefab);

		}

		public void AttachColliders ()
		{

			colliderAtachment = GameObject.Find (setUp.editorGeneratorLevel.name);

			if (colliderAtachment == null) {
				colliderAtachment = new GameObject (setUp.editorGeneratorLevel.name);
			}

			if (colliderAtachment.transform.childCount != 0) {
				var temp = colliderAtachment.transform.Cast<Transform> ().ToList ();
				foreach (var child in temp) {
					if (child.gameObject.name != "Blocks") {
						DestroyImmediate (child.gameObject);
					}
				}
			}

			GameObject go;

			GameObject wall = new GameObject ("WallColliders");
			GameObject floor = new GameObject ("FloorColliders");

			SerializedObject tagManager = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath ("ProjectSettings/TagManager.asset") [0]);
			SerializedProperty tagsProp = tagManager.FindProperty ("tags");

			bool found = false;

			for (int i = 0; i < tagsProp.arraySize; i++) {
				SerializedProperty temp = tagsProp.GetArrayElementAtIndex (i);
				if (temp.stringValue.Equals (wallTag)) {
					found = true;
					break;
				}
			}
			if (!found) {
				tagsProp.InsertArrayElementAtIndex (0);
				SerializedProperty temp = tagsProp.GetArrayElementAtIndex (0);
				temp.stringValue = wallTag;
			}
			found = false;
			for (int i = 0; i < tagsProp.arraySize; i++) {
				SerializedProperty temp = tagsProp.GetArrayElementAtIndex (i);
				if (temp.stringValue.Equals (floorTag)) {
					found = true; 
					break;
				}
			}
			if (!found) {
				tagsProp.InsertArrayElementAtIndex (0);
				SerializedProperty temp = tagsProp.GetArrayElementAtIndex (0);
				temp.stringValue = floorTag;
			}

			tagManager.ApplyModifiedProperties ();

			wall.tag = wallTag;
			floor.tag = floorTag;

			for (int i = 0; i < colliders.childCount; i++) {
				go = colliders.GetChild (i).gameObject;

				ComponentUtility.CopyComponent (go.GetComponent <BoxCollider> ());

				if (go.name == floorTag) {
				
					ComponentUtility.PasteComponentAsNew (floor);

				} else if (go.name == wallTag) {

					ComponentUtility.PasteComponentAsNew (wall);

				}

			}

			wall.transform.parent = colliderAtachment.transform;
			floor.transform.parent = colliderAtachment.transform;

			if (setUp.deathZonePrefab != null) {
				go = Instantiate (setUp.deathZonePrefab, new Vector3 (0, -1, 0), Quaternion.identity, colliderAtachment.transform);
			}

			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs/Levels")) {
				AssetDatabase.CreateFolder ("Assets", "Prefabs");
				AssetDatabase.CreateFolder ("Assets/Prefabs", "Levels");
			}

			PrefabUtility.CreatePrefab ("Assets/Prefabs/Levels/" + colliderAtachment.name + ".prefab", colliderAtachment, ReplacePrefabOptions.ConnectToPrefab);

		}

		public void Clean ()
		{
			if (colliders.childCount != 0) {
				var temp = colliders.Cast<Transform> ().ToList ();
				foreach (var child in temp) {
					DestroyImmediate (child.gameObject);
				}
			}
			if (prefabs.childCount != 0) {
				var temp1 = prefabs.Cast<Transform> ().ToList ();
				foreach (var child in temp1) {
					DestroyImmediate (child.gameObject);
				}
			}
			if (setUp.player.prefab != null) {
				setUp.player.prefab.transform.position = new Vector3 (0, 0, 0);
			}
		}

		public void CheckMaterials ()
		{
			bool found;
			if (!setUp.singleMat) {

				if (setUp.physicsMaterial != null && setUp.physicsMaterial [0] != null && setUp.physicsMaterial [0].material != null) {
					found = false;
					for (int i = 0; i < setUp.physicsMaterial.Length && !found; i++) {
						if (setUp.physicsMaterial [i].color == setUp.floor.color) {
							found = true;
						} else if (setUp.physicsMaterial [i].color == setUp.wall.color) {
							found = true;
						}
					}
					for (int i = 0; i < setUp.physicsMaterial.Length && !found; i++) {
						for (int j = 0; j < setUp.floorObjects.Length && !found; j++) {
							if (setUp.physicsMaterial [i].color == setUp.floorObjects [j].color) {
								found = true;
							}
						}
					}
					for (int i = 0; i < setUp.physicsMaterial.Length && !found; i++) {
						for (int j = 0; j < setUp.wallObjects.Length && !found; j++) {
							if (setUp.physicsMaterial [i].color == setUp.wallObjects [j].color) {
								found = true;
							}
						}
					}

					if (!found) {
						Debug.LogError ("Not color match between materials and walls/floors");
					} else {
						GenerateColliders ();
					}

				} else if (setUp.dim2 && setUp.physicsMaterial2D != null && setUp.physicsMaterial2D [0] != null && setUp.physicsMaterial2D [0].material != null) {

					found = false;
					for (int i = 0; i < setUp.physicsMaterial2D.Length && !found; i++) {
						if (setUp.physicsMaterial2D [i].color == setUp.floor.color) {
							found = true;
						} else if (setUp.physicsMaterial2D [i].color == setUp.wall.color) {
							found = true;
						}
					}
					for (int i = 0; i < setUp.physicsMaterial2D.Length && !found; i++) {
						for (int j = 0; j < setUp.floorObjects.Length && !found; j++) {
							if (setUp.physicsMaterial2D [i].color == setUp.floorObjects [j].color) {
								found = true;
							}
						}
					}
					for (int i = 0; i < setUp.physicsMaterial2D.Length && !found; i++) {
						for (int j = 0; j < setUp.wallObjects.Length && !found; j++) {
							if (setUp.physicsMaterial2D [i].color == setUp.wallObjects [j].color) {
								found = true;
							}
						}
					}

					if (!found) {
						Debug.LogError ("Not color match between materials and walls/floors");
					} else {
						GenerateColliders ();
					}

				} else {
					Debug.LogError ("No material added or material missing.");
				}
					
			} else {
				GenerateColliders ();
			}
		}
	}
	#endif
}