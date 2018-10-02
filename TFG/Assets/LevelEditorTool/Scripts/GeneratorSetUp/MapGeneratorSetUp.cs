using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMGTool
{
	[CreateAssetMenu (menuName = "Set Up/Map Generator")]
	public class MapGeneratorSetUp : ScriptableObject
	{

		public Texture2D editorGeneratorLevel;

		public int blockSize = 1;

		public Entity[] entities;

		public SolidObjects[] objects;

		public MiscObject[] misc;

		public Vector3 minScale = new Vector3 (1, 1, 1);
		public Vector3 maxScale = new Vector3 (1, 1, 1);

		public Vector3 minRotation = new Vector3 (0, 0, 0);
		public Vector3 maxRotation = new Vector3 (0, 0, 0);

		public bool extraOptions;

		public Player player;

		public bool horizontal;

		public GameObject straight, curve, cross, begin, end;

		public Color pathColor = Color.red;
		public Color beginColor = Color.green;
		public Color endColor = Color.blue;

		public int GetTotal ()
		{
		
			return entities.Length + objects.Length + misc.Length + 5;
		
		}

		public string GetName (int index)
		{

			string auxName;

			if (index < entities.Length) {
			
				auxName = entities [index].prefab.name;

			} else if (index >= entities.Length && index - entities.Length < objects.Length) {

				auxName = objects [index - entities.Length].prefab.name;

			} else {
				auxName = null;
			}

			return auxName;
		}

		public GameObject GetObject (int index)
		{

			GameObject aux;

			if (index < entities.Length) {

				aux = entities [index].prefab;

			} else if (index >= entities.Length && index - entities.Length < objects.Length) {

				aux = objects [index - entities.Length].prefab;

			} else {
				aux = null;
			}

			return aux;

		}

		public void SetColorObject (Color newColor, string objectName)
		{
			foreach (var item in entities) {
				if (item.prefab.name.Equals (objectName)) {
					item.color = newColor;
					return;
				}
			}
			foreach (var item in objects) {
				if (item.prefab.name.Equals (objectName)) {
					item.color = newColor;
					return;
				}
			}
			foreach (var item in misc) {
				if (item.prefab.name.Equals (objectName)) {
					item.color = newColor;
					return;
				}
			}
			if (straight.name.Equals (objectName) || curve.name.Equals (objectName)) {
				pathColor = newColor;
				return;
			}
			if (begin.name.Equals (objectName)) {
				beginColor = newColor;
			}
			if (end.name.Equals (objectName)) {
				endColor = newColor;
			}
		
		}


	}
}