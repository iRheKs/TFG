
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace LMGTool
{
	[ExecuteInEditMode]
	public class InGameMapGenerator_WS : MonoBehaviour
	{

		[SerializeField]
		private MapGeneratorSetUp_WS setUp;

		int floorCount;

		int rows, columns;

		int[,] map;
		List<Vector2> path;

		Transform prefabs;

		GameObject newPrefab;

		List<Entity_WB> misc;


		void Start ()
		{
			prefabs = transform.GetChild (0);
		}

		/// <summary>
		/// Generates the map. Based on your personal set up. If pools are not generated, they will before generating the map.
		/// </summary>
		public void GenerateMap ()
		{
			Clean ();
			if (setUp != null) {

				rows = setUp.editorGeneratorLevel.width;
				columns = setUp.editorGeneratorLevel.height;
				map = new int[rows, columns];
				path = new List<Vector2> ();
				misc = new List<Entity_WB> ();

				SetMapNumbers ();

				Color temp;
				bool found = false;
				float randomRotation = 0f;
				Vector3 randomScale = new Vector3 (1, 1, 1);

				for (int r = 0; r < rows; r++) {
					for (int c = 0; c < columns; c++) {
						temp = setUp.editorGeneratorLevel.GetPixel (r, c);
						found = false;
						randomRotation = 0f;
						//				if (setUp.player.prefab != null && temp == setUp.player.color) {
						//					setUp.player.prefab.transform.position = new Vector3 (r * setUp.blockSize, c * setUp.blockSize);
						//					found = true;
						//				}

						for (int i = 0; i < setUp.entities.Length && !found; i++) {
							if (temp == setUp.entities [i].color) {
								InstancePrefab (setUp.entities [i].prefab, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
								found = true;
							}
						}

						if (!found) {
							for (int i = 0; i < setUp.misc.Length && !found; i++) {
								if (temp == setUp.misc [i].color) {
									if (setUp.misc [i].rotation) {
										randomRotation = (float)Random.Range (0, 360);
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

						if (!found) {

							if (temp == setUp.beginColor) {
								InstancePrefab (setUp.begin, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), GetRotation (r, c));
							} else if (temp == setUp.endColor) {
								InstancePrefab (setUp.end, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), GetRotation (r, c));
							} else if (temp == setUp.floorColor) {
								if (map [r, c] == 6 || map [r, c] == 9) {
									InstancePrefab (setUp.recta, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), GetRotation (r, c));
								} else {
									InstancePrefab (setUp.curva, new Vector3 (r * setUp.blockSize, c * setUp.blockSize), GetRotation (r, c));
								}
							}
						}
					}
				}
				MergeMiscMeshes ();
			} else {
				Debug.LogError ("Set up is missing. Create or reference one.");
			}
		}

		/// <summary>
		/// Sets a new map.
		/// </summary>
		/// <param name="newMap">New map. Only if set up of all maps are the same</param>
		public void SetNewMap (Texture2D newMap)
		{
			setUp.editorGeneratorLevel = newMap;
		}

		/// <summary>
		/// Changes the set up.
		/// </summary>
		/// <param name="newSetUp">New set up. Change the set up for different maps set ups. (If only want to change texture use SetNewMap)</param>
		public void ChangeSetUp (MapGeneratorSetUp_WS newSetUp)
		{
			setUp = newSetUp;
		}

		/// <summary>
		/// Returns the set up you are currently using.
		/// </summary>
		/// <returns>Set up.</returns>
		public MapGeneratorSetUp_WS GetSetUp ()
		{
			return setUp;
		}

		void MergeMiscMeshes ()
		{
			Mesh mergedMeshes = new Mesh ();

			mergedMeshes.name = setUp.editorGeneratorLevel.name + "_MiscMesh";

			List<Vector3> vertices = new List<Vector3> ();

			List<List<int>> differentTriangles = new List<List<int>> ();

			for (int i = 0; i < setUp.misc.Length; i++) {
				List<int> tempList = new List<int> ();

				differentTriangles.Add (tempList);
			}
			int cumulatedVertexCount = 0;
			for (int i = 0; i < misc.Count; i++) {

				for (int j = 0; j < setUp.misc.Length; j++) {

					if (misc [i].color == setUp.misc [j].color) {
						
						Vector3 position = misc [i].prefab.transform.position;

						int vertexCount = misc [i].prefab.GetComponent <MeshFilter> ().sharedMesh.vertexCount;

						//Debug.Log ("vertex: " + vertexCount.ToString ());

						for (int k = 0; k < vertexCount; k++) {
							
							Vector3 vertex = misc [i].prefab.GetComponent <MeshFilter> ().sharedMesh.vertices [k];

							Quaternion rotation = Quaternion.Euler (0, misc [i].prefab.transform.rotation.eulerAngles.y, 0);

							Vector3 scale = misc [i].prefab.transform.localScale;

							Vector3 rotatedVertex = vertex + position;

							if (setUp.misc [j].scale) {
								rotatedVertex = new Vector3 (vertex.x * scale.x, vertex.y * scale.y, vertex.z * scale.z) + position;
							}

							if (setUp.misc [j].rotation) {
								rotatedVertex = rotation * (rotatedVertex - position) + position;
							}

							vertices.Add (rotatedVertex);

							//Debug.Log ("rotation: "+rotation.ToString () + ", rotatedVertex: " + rotatedVertex.ToString ());
						}
						if (i > 0) {
							cumulatedVertexCount += misc [i - 1].prefab.GetComponent <MeshFilter> ().sharedMesh.vertexCount;
						}
						for (int k = 0; k < misc [i].prefab.GetComponent <MeshFilter> ().sharedMesh.triangles.Length; k++) {
							if (i > 0) {
								int triangle = misc [i].prefab.GetComponent <MeshFilter> ().sharedMesh.triangles [k];
								int sum = triangle + cumulatedVertexCount;
								differentTriangles [j].Add (sum);
								//Debug.Log ("last: " + i.ToString () + " last: " + (i - 1).ToString ());
								//Debug.Log ("triangle: " + (triangle + vertexCount).ToString () + " vertex count: " + vertexCount);
							} else
								differentTriangles [j].Add (misc [i].prefab.GetComponent <MeshFilter> ().sharedMesh.triangles [k]);
						}

					}

				}

			}

			mergedMeshes.subMeshCount = setUp.misc.Length;
			mergedMeshes.vertices = vertices.ToArray ();

			for (int i = 0; i < setUp.misc.Length; i++) {
				mergedMeshes.SetTriangles (differentTriangles [i], i);
				//mergedMeshes.triangles = differentTriangles [i].ToArray ();
			}


			mergedMeshes.RecalculateNormals ();
			mergedMeshes.RecalculateBounds ();

			prefabs.gameObject.GetComponent <MeshFilter> ().sharedMesh = mergedMeshes;

		}

		void SetMapNumbers ()
		{
			Color temp = Color.white;

			for (int x = 0; x < rows; x++) {
				for (int y = 0; y < columns; y++) {
					temp = setUp.editorGeneratorLevel.GetPixel (x, y);
					if (temp == setUp.floorColor) {
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

		void InstancePrefab (GameObject index, Vector3 position, float rotation = 0f)
		{
//			Debug.Log ("Generated: " + index.ToString () + " at position: " + position.ToString ());
//			GameObject obj;
//			if (setUp.horizontal) {
			GameObject.Instantiate (index, new Vector3 (position.x, 0, position.y), Quaternion.Euler (new Vector3 (index.transform.rotation.eulerAngles.x, rotation, index.transform.rotation.eulerAngles.z)), prefabs);
//			obj = index;
//			obj.transform.position = new Vector3 (position.x,0,position.y);
//			obj.transform.rotation = Quaternion.Euler (new Vector3 (obj.transform.rotation.eulerAngles.x, rotation, obj.transform.rotation.eulerAngles.z));
//			obj.SetActive (true);
//			} else {
//				GameObject.Instantiate (index, position, Quaternion.Euler (new Vector3 (index.transform.rotation.eulerAngles.x, rotation, index.transform.rotation.eulerAngles.z)), prefabs);
//			obj = index;
//			obj.transform.position = position;
//			obj.SetActive (true);
//			}
		}

		void InstancePrefab (int objectIndex, GameObject index, Vector3 position, Vector3 scale, float rotation = 0f)
		{
			index.transform.localScale = scale;
			GameObject temp;
			Entity_WB tempEn = new Entity_WB ();
			tempEn.color = setUp.misc [objectIndex].color;
			temp = GameObject.Instantiate (index, new Vector3 (position.x, 0, position.y), Quaternion.Euler (new Vector3 (index.transform.rotation.eulerAngles.x, rotation, index.transform.rotation.eulerAngles.z)), prefabs);
			tempEn.prefab = temp;
			misc.Add (tempEn);
			//Debug.Log ("position: " + tempEn.prefab.transform.position.ToString ());
			temp.SetActive (false);
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

			if (!AssetDatabase.IsValidFolder ("Assets/Prefabs/Maps_WS")) {
				AssetDatabase.CreateFolder ("Assets/Prefabs", "Maps_WS");
			}

			PrefabUtility.CreatePrefab ("Assets/Prefabs/Maps_WS/" + newPrefab.name + ".prefab", newPrefab, ReplacePrefabOptions.ConnectToPrefab);
		}

		public void SaveMesh ()
		{

			Mesh newMesh = prefabs.gameObject.GetComponent <MeshFilter> ().sharedMesh;

			if (!AssetDatabase.IsValidFolder ("Assets/Level mesh")) {
				AssetDatabase.CreateFolder ("Assets", "Level mesh");
			}

			if (!AssetDatabase.IsValidFolder ("Assets/Level mesh/Maps_WS")) {
				AssetDatabase.CreateFolder ("Assets/Level mesh", "Maps_WS");
			}

			AssetDatabase.CreateAsset (newMesh, "Assets/Level mesh/Maps_WS/" + newMesh.name + ".asset");
			AssetDatabase.SaveAssets ();

		}

		public void Clean ()
		{
			if (prefabs.childCount != 0) {
				var temp1 = prefabs.Cast<Transform> ().ToList ();
				foreach (var child in temp1) {
					DestroyImmediate (child.gameObject);
				}
			}

		}

	}
}
#endif