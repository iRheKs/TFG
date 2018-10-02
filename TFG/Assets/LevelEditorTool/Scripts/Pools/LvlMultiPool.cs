using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMGTool
{
	public class LvlMultiPool : MonoBehaviour
	{

		GenericPool[] pools;
		int length;

		private LevelGeneratorSetUp setUp;
		public int initialPooledAmount;
		public bool willGrow;

		void Awake ()
		{
			PoolNumber ();
			pools = new GenericPool[length];
			for (int i = 0; i < length; i++) {
				GameObject obj = new GameObject (GetName (i));
				obj.transform.SetParent (transform);
				pools [i] = obj.AddComponent <GenericPool> ();
				pools [i].initialPooledAmount = initialPooledAmount;
				pools [i].willGrow = willGrow;
				pools [i].pooledObject = GetObject (i);
				pools [i].InitializePool ();
			}

		}

		void PoolNumber ()
		{
			setUp = transform.parent.gameObject.GetComponent <InGameLevelGenerator> ().GetSetUp ();
			length = setUp.GetTotal ();
		}

		string GetName (int index)
		{
			return setUp.GetName (index);
		}

		GameObject GetObject (int index)
		{
			return setUp.GetObject (index);
		}

		public GenericPool GetPool (int index)
		{
			return pools [index];
		}

		public void ResetPools ()
		{
			for (int i = 0; i < length; i++) {
				pools [i].ResetPool ();
			}
		}
	}
}