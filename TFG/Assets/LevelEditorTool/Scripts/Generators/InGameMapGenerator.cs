using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LMGTool;

public class InGameMapGenerator : MonoBehaviour {

	[SerializeField]
	private MapGeneratorSetUp setUp;

	private MapMultiPool pools;

	int floorCount;

	int rows,columns;

	void Start(){
		pools = transform.GetChild (0).gameObject.GetComponent <MapMultiPool> ();
	}

	/// <summary>
	/// Generates the map. Based on your personal set up.
	/// </summary>
	public void GenerateMap(){

		rows = setUp.editorGeneratorLevel.width;
		columns = setUp.editorGeneratorLevel.height;

		Color temp;
		bool found = false;

		for(int r = 0; r < rows; r++){
			for(int c = 0; c < columns; c++){
				temp = setUp.editorGeneratorLevel.GetPixel (r, c);
				found = false;
				if (setUp.player.prefab != null && temp == setUp.player.color) {
					setUp.player.prefab.transform.position = new Vector3 (r * setUp.blockSize, c * setUp.blockSize);
					found = true;
				}

				for (int i = 0; i < setUp.entities.Length && !found; i++) {
					if (temp == setUp.entities[i].color) {
						InstancePrefab (i, new Vector3(r * setUp.blockSize, c * setUp.blockSize));
						found = true;
					}
				}


				if (!found) {
					
					for (int i = 0; i < setUp.objects.Length && !found; i++) {

						if (temp == setUp.objects[i].color) {
							InstancePrefab (i + setUp.entities.Length, new Vector3(r * setUp.blockSize, c * setUp.blockSize));
							found = true;
						}
					}
				}
			}
		}
	}
	/// <summary>
	/// Sets a new map.
	/// </summary>
	/// <param name="newMap">New map. Only if set up of all maps are the same</param>
	public void SetNewMap(Texture2D newMap){
		setUp.editorGeneratorLevel = newMap;
	}

	/// <summary>
	/// Changes the set up.
	/// </summary>
	/// <param name="newSetUp">New set up. Change the set up for different maps set ups. (If only want to change texture use SetNewMap)</param>
	public void ChangeSetUp(MapGeneratorSetUp newSetUp){
		setUp = newSetUp;
	}

	public MapGeneratorSetUp GetSetUp(){
		return setUp;
	}

	void InstancePrefab(int index, Vector3 position){
		Debug.Log ("Generated: "+ index.ToString () + " at position: " + position.ToString ());
		GameObject obj;
		if (setUp.horizontal) {
//			GameObject.Instantiate (index,new Vector3(position.x,0,position.y),Quaternion.identity,prefabs);
			obj = pools.GetPool (index).GetPooledObject ();
			obj.transform.position = new Vector3 (position.x,0,position.y);
			obj.SetActive (true);
		}else {
//			GameObject.Instantiate (index,position,Quaternion.identity,prefabs);
			obj = pools.GetPool (index).GetPooledObject ();
			obj.transform.position = position;
			obj.SetActive (true);
		}
	}

}
