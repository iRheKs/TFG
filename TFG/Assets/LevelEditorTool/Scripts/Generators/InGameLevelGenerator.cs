using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMGTool;

public class InGameLevelGenerator : MonoBehaviour
{
	[SerializeField]
	private LevelGeneratorSetUp setUp;

	private LvlMultiPool pools;

	int floorCount, wallCount;

	int rows, columns;

	GameObject colliderAtachment;

	void OnEnable ()
	{
		pools = transform.GetChild (0).gameObject.GetComponent <LvlMultiPool> ();
		GenerateLevel ();
	}

	/// <summary>
	/// Generates the level. Based on your personal set up.
	/// </summary>
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
					setUp.player.prefab.transform.position = new Vector3 (r * setUp.blockSize, c * setUp.blockSize);
					found = true;
				}
				if (setUp.singleWallFloor) {
					if (temp == setUp.wall.color) {
						InstancePrefab (setUp.entities.Length + setUp.objects.Length + 1, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
						found = true;
					}
					if (temp == setUp.floor.color) {
						InstancePrefab (setUp.entities.Length + setUp.objects.Length, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
						found = true;
					}
				} else {
					for (int i = 0; i < setUp.floorObjects.Length && !found; i++) {
						if (temp == setUp.floorObjects [i].color) {
							InstancePrefab (i + setUp.entities.Length + setUp.objects.Length, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
							found = true;
						}
					}
					for (int i = 0; i < setUp.wallObjects.Length && !found; i++) {
						if (temp == setUp.wallObjects [i].color) {
							InstancePrefab (i + setUp.entities.Length + setUp.objects.Length + setUp.floorObjects.Length, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
							found = true;
						}
					}
				}
				if (!found) {
					for (int i = 0; i < setUp.entities.Length && !found; i++) {
						if (temp == setUp.entities [i].color) {
							InstancePrefab (i, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
							found = true;
						}
					}
				}
				if (!found) {
					for (int i = 0; i < setUp.objects.Length && !found; i++) {
						if (temp == setUp.objects [i].color) {
							InstancePrefab (i + setUp.entities.Length, new Vector3 (r * setUp.blockSize, c * setUp.blockSize));
							found = true;
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// Sets a new level.
	/// </summary>
	/// <param name="newLevel">New level. Only if set up of all levels are the same</param>
	public void SetNewLevel(Texture2D newLevel){
		setUp.editorGeneratorLevel = newLevel;
	}

	/// <summary>
	/// Changes the set up.
	/// </summary>
	/// <param name="newSetUp">New set up. Change the set up for different levels set ups. (If only want to change texture use SetNewLevel)</param>
	public void ChangeSetUp(LevelGeneratorSetUp newSetUp){
		setUp = newSetUp;
	}

	public LevelGeneratorSetUp GetSetUp(){
		return setUp;
	}

	void InstancePrefab (int index, Vector3 position)
	{
		GameObject obj;
//		GameObject.Instantiate (index,position,Quaternion.identity,prefabs);
		obj = pools.GetPool (index).GetPooledObject ();
		obj.transform.position = position;
		obj.SetActive (true);
	}

}
