using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMGTool
{
	[CreateAssetMenu (menuName = "Map Generator WS")]
	public class MapGeneratorSetUp_WS : ScriptableObject
	{

		public Texture2D editorGeneratorLevel;

		public int blockSize = 1;

		public Color floorColor;
		public Color beginColor ;
		public Color endColor;

		public Entity[] entities;

		public Entity_WB[] misc;

		public GameObject recta, curva, begin, end;

		public Vector3 minScale = new Vector3(1,1,1);
		public Vector3 maxScale = new Vector3(1,1,1);

		public bool horizontal = true;

		public int GetTotal ()
		{
		
			return entities.Length + 4;
		
		}

		public string GetName (int index)
		{

			string auxName;

			if (index < entities.Length) {
			
				auxName = entities [index].prefab.name;

			} else if (index >= entities.Length && index - entities.Length < 4) {

				auxName = "Path";

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

			} else if (index >= entities.Length && index - entities.Length < 4) {

				int i = index - entities.Length;
				switch (i) {

				case 0:
					aux = recta;
					break;
				case 1:
					aux = curva;
					break;
				case 2:
					aux = begin;
					break;
				case 3:
					aux = end;
					break;
				default:
					aux = null;
					break;
				}

			} else {
				aux = null;
			}

			return aux;

		}

	}
}