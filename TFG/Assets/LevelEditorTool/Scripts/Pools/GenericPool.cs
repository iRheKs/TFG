using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMGTool
{
	public class GenericPool : MonoBehaviour {

	List<GameObject> pooledObjects;

	public GameObject pooledObject;
	public int initialPooledAmount;
	public bool willGrow;


	void OnEnable () {
		InitializePool ();
	}
	public GameObject GetPooledObject(){
		for (int i = 0; i < pooledObjects.Count; i++) {
			if(!pooledObjects[i].activeInHierarchy){
				return pooledObjects [i];
			}
		}
		if(willGrow){
			GameObject obj = (GameObject)Instantiate (pooledObject);
			pooledObjects.Add (obj);
			return obj;
		}
		return null;
	}
	public void InitializePool(){
		pooledObjects = new List<GameObject> ();
		for(int i = 0; i<initialPooledAmount; i++){
			GameObject obj = (GameObject)Instantiate (pooledObject);
			obj.SetActive (false);
			pooledObjects.Add (obj);
		}
	}
	public void ResetPool(){
		for (int i = 0; i < pooledObjects.Count; i++) {
			if(pooledObjects[i].activeInHierarchy){
				pooledObjects [i].SetActive (false);
			}
		}
	}
}
}
