using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;

namespace LMGTool
{
	#if UNITY_EDITOR

	[ExecuteInEditMode]
	public class MapGenerator : MonoBehaviour
	{

		public MapGeneratorSetUp setUp;

		//	public Texture2D editorGeneratorLevel;
		//
		//	public int blockSize;
		//
		//	public Entity[] entities;
		//
		//	public SolidObjects[] objects;
		//
		//	public bool extraOptions;
		//
		//	public Player player;
		//
		//	public bool horizontal;

		int floorCount;

		int rows, columns;

		int[,] map;
		List<Vector2> path;

		Transform prefabs;

		GameObject newPrefab;

		List<MiscObject> misc;

		void Start ()
		{
			prefabs = transform.GetChild (0);
		}

		public void GenerateLevel ()
		{
			Clean ();
			if (setUp != null) {
				rows = setUp.editorGeneratorLevel.width;
				columns = setUp.editorGeneratorLevel.height;
				misc = new List<MiscObject> ();

//				if (setUp.horizontal) {
				path = new List<Vector2> ();
				map = new int[rows, columns];
				SetMapNumbers ();
//				}

				Color temp;
				bool found = false;
				Vector3 randomRotation = new Vector3 (0, 0, 0);
				Vector3 randomScale = new Vector3 (1, 1, 1);

				for (int r = 0; r < rows; r++) {
					for (int c = 0; c < columns; c++) {
						temp = setUp.editorGeneratorLevel.GetPixel (r, c);
						found = false;
						randomRotation = Vector3.zero;
						if (setUp.extraOptions) {
							if (setUp.player.prefab != null && temp == setUp.player.color) {
								InstancePrefab (setUp.player.prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
								found = true;
							}
						}
						for (int i = 0; i < setUp.objects.Length && !found; i++) {
							if (temp == setUp.objects [i].color) {
								InstancePrefab (setUp.objects [i].prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
								found = true;
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
						if (!found) {
							for (int i = 0; i < setUp.misc.Length && !found; i++) {
								if (temp == setUp.misc [i].color) {
									if (setUp.misc [i].rotation) {
										randomRotation.x = Random.Range (setUp.minRotation.x, setUp.maxRotation.x);
										randomRotation.y = Random.Range (setUp.minRotation.y, setUp.maxRotation.y);
										randomRotation.z = Random.Range (setUp.minRotation.z, setUp.maxRotation.z);
									}
									if (setUp.misc [i].scale) {
										randomScale.x = Random.Range (setUp.minScale.x, setUp.maxScale.x);
										randomScale.y = Random.Range (setUp.minScale.y, setUp.maxScale.y);
										randomScale.z = Random.Range (setUp.minScale.z, setUp.maxScale.z);
									}
									InstancePrefab (i, setUp.misc [i].prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), randomScale, randomRotation);
									found = true;
								}
							}
						}
//						if (setUp.horizontal) {
						if (!found) {
							if (temp == setUp.beginColor) {
								InstancePrefab (setUp.begin, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), GetRotation (r, c));
							} else if (temp == setUp.endColor) {
								InstancePrefab (setUp.end, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), GetRotation (r, c));
							} else if (temp == setUp.pathColor) {
								if (map [r, c] == 15) {
									InstancePrefab (setUp.cross, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), GetRotation (r, c));
								} else if (map [r, c] == 6 || map [r, c] == 9) {
									InstancePrefab (setUp.straight, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), GetRotation (r, c));
								} else {
									InstancePrefab (setUp.curve, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), GetRotation (r, c));
								}
							}
						}
//						}
					}
				}
			} else
				Debug.LogError ("Set up is missing. Create or reference one.");
		}

		void SetMapNumbers ()
		{
			Color temp = Color.white;

			for (int x = 0; x < rows; x++) {
				for (int y = 0; y < columns; y++) {
					temp = setUp.editorGeneratorLevel.GetPixel (x, y);
					if (temp == setUp.pathColor) {
						map [x, y] = 1;
						path.Add (new Vector2 (x, y));
					} else if (temp == setUp.beginColor || temp == setUp.endColor) {
						map [x, y] = 2;
						path.Add (new Vector2 (x, y));
					} else {
						map [x, y] = 0;
					}
				}
			}
			SetPathNumbers ();
		}

		void SetPathNumbers ()
		{
			for (int i = 0; i < path.Count; i++) {
				map [(int)path [i].x, (int)path [i].y] = GetCorrespondingNumber ((int)path [i].x, (int)path [i].y);
			}
		}

		int GetCorrespondingNumber (int toCheckX, int toCheckY)
		{

			int[] surrondings = GetSurroundings (toCheckX, toCheckY);

			int sum = 0;

			if (surrondings [0] >= 1) {
				sum += 1;
			}
			if (surrondings [1] >= 1) {
				sum += 2;
			}
			if (surrondings [2] >= 1) {
				sum += 4;
			}
			if (surrondings [3] >= 1) {
				sum += 8;
			}
			return sum;
		}

		int[] GetSurroundings (int toCheckX, int toCheckY)
		{

			int[] surroundings = new int[4];
			if (toCheckY - 1 < 0) {
				surroundings [0] = 0;
			} else
				surroundings [0] = map [toCheckX, toCheckY - 1];
			if (toCheckX - 1 < 0) {
				surroundings [1] = 0;
			} else
				surroundings [1] = map [toCheckX - 1, toCheckY];
			if (toCheckX + 1 >= rows) {
				surroundings [2] = 0;
			} else
				surroundings [2] = map [toCheckX + 1, toCheckY];
			if (toCheckY + 1 >= columns) {
				surroundings [3] = 0;
			} else
				surroundings [3] = map [toCheckX, toCheckY + 1];


			return surroundings;
		}

		float GetRotation (int posX, int posY)
		{

			int current = map [posX, posY];

			switch (current) {

			case 1:
				return 180f;
			case 2:
				return -90f;
			case 3:
				return 90f;
			case 4:
				return 90f;
			case 5:
				return 0f;
			case 6:
				return 90f;
			case 8:
				return 0f;
			case 9:
				return 0f;
			case 10:
				return 180f;
			case 12:
				return -90f;
			default:
				return 0;
			}

		}

		void InstancePrefab (GameObject index, Vector3 position)
		{
			if (index != null) {
				if (!setUp.horizontal)
					GameObject.Instantiate (index, position, Quaternion.identity, prefabs);
				else
					InstancePrefab (index, position, 0);
			} else
				Debug.Log ("Nothing assigned to objects or entities");
		}

		void InstancePrefab (GameObject index, Vector3 position, float rotation)
		{
			//Debug.Log ("Generated: " + index.ToString () + " at position: " + position.ToString ());
			if (index != null)
				GameObject.Instantiate (index, new Vector3 (position.x, 0, position.y), Quaternion.Euler (new Vector3 (index.transform.rotation.eulerAngles.x, rotation, index.transform.rotation.eulerAngles.z)), prefabs);
			else
				Debug.Log ("Nothing assigned to path");
		}

		void InstancePrefab (int objectIndex, GameObject index, Vector3 position, Vector3 scale, Vector3 rotation)
		{
			if (index != null) {
				index.transform.localScale = scale;
				GameObject temp;
				MiscObject tempMisc = new MiscObject ();
				tempMisc.color = setUp.misc [objectIndex].color;
				if (setUp.horizontal) {
					temp = GameObject.Instantiate (index, new Vector3 (position.x, 0, position.y), Quaternion.Euler (rotation), prefabs);
				} else
					temp = GameObject.Instantiate (index, position, Quaternion.Euler (rotation), prefabs);
				tempMisc.prefab = temp;
				misc.Add (tempMisc);
				//Debug.Log ("position: " + tempEn.prefab.transform.position.ToString ());
				//temp.SetActive (false);
			} else
				Debug.Log ("Nothing assigned to misc");
		}

		public void CreatePrefab ()
		{

			newPrefab = GameObject.Find (setUp.editorGeneratorLevel.name);

			if (newPrefab == null) {
				newPrefab = new GameObject (setUp.editorGeneratorLevel.name);
			}

			GameObject go = new GameObject ("Blocks");

			go.transform.SetParent (transform);

			if (prefabs.childCount != 0) {
				var temp1 = prefabs.Cast<Transform> ().ToList ();
				foreach (var child in temp1) {
					child.SetParent (go.transform);
				}
			}

			go.transform.SetParent (newPrefab.transform);

			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs")) {
				AssetDatabase.CreateFolder ("Assets", "Prefabs");
			}

			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs/Maps")) {
				AssetDatabase.CreateFolder ("Assets/Prefabs", "Maps");
			}

			PrefabUtility.CreatePrefab ("Assets/Prefabs/Maps/" + newPrefab.name + ".prefab", newPrefab, ReplacePrefabOptions.ConnectToPrefab);
		}

		public void Clean ()
		{
			if (prefabs.childCount != 0) {
				var temp1 = prefabs.Cast<Transform> ().ToList ();
				foreach (var child in temp1) {
					DestroyImmediate (child.gameObject);
				}
			}
//			if (setUp.player.prefab != null) {
//				setUp.player.prefab.transform.position = new Vector3 (0, 0, 0);
//			}
		}

	}
	#endif
}