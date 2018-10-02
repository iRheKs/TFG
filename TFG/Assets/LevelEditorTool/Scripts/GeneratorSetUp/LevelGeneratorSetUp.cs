using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LMGTool
{
	[CreateAssetMenu (menuName = "Set Up/Level Generator")]
	public class LevelGeneratorSetUp : ScriptableObject
	{

		public Texture2D editorGeneratorLevel;

		public int blockSize;

		public Entity[] entities;

		public SolidObjects[] objects;

		public bool singleWallFloor;

		public SolidObjects[] wallObjects;

		public SolidObjects[] floorObjects;

		public SolidObjects floor, wall;

		public bool extraOptions;

		public GameObject deathZonePrefab;

		public PhyMat[] physicsMaterial;

		public Player player;

		public PhyMat2D[] physicsMaterial2D;

		public bool dim2;

		public Color airColor = Color.white;

		public bool singleMat;

		public PhysicMaterial singleMaterial;

		public PhysicsMaterial2D singleMaterial2D;

		public int GetTotal ()
		{
			if (singleWallFloor) {
				return entities.Length + objects.Length + 2;
			} else
				return entities.Length + objects.Length + wallObjects.Length + floorObjects.Length;
		}

		public string GetName (int index)
		{

			string auxName;
			if (singleWallFloor) {

				if (index < entities.Length) {

					auxName = entities [index].prefab.name;

				} else if (index >= entities.Length && index - entities.Length < objects.Length) {

					auxName = objects [index - entities.Length].prefab.name;

				} else if (index - entities.Length - objects.Length == 0) {
				
					auxName = floor.prefab.name;

				} else {

					auxName = wall.prefab.name;

				}

			} else {

				if (index < entities.Length) {

					auxName = entities [index].prefab.name;

				} else if (index >= entities.Length && index - entities.Length < objects.Length) {

					auxName = objects [index - entities.Length].prefab.name;

				} else if (index >= entities.Length + objects.Length && index - entities.Length - objects.Length < floorObjects.Length) {
				
					auxName = floorObjects [index - entities.Length - objects.Length].prefab.name;

				} else {

					auxName = wallObjects [index - entities.Length - objects.Length - floorObjects.Length].prefab.name;

				}
			}

			return auxName;
		}

		public GameObject GetObject (int index)
		{

			GameObject aux;
			if (singleWallFloor) {

				if (index < entities.Length) {

					aux = entities [index].prefab;

				} else if (index >= entities.Length && index - entities.Length < objects.Length) {

					aux = objects [index - entities.Length].prefab;

				} else if (index - entities.Length - objects.Length == 0) {

					aux = floor.prefab;

				} else {

					aux = wall.prefab;

				}

			} else {

				if (index < entities.Length) {

					aux = entities [index].prefab;

				} else if (index >= entities.Length && index - entities.Length < objects.Length) {

					aux = objects [index - entities.Length].prefab;

				} else if (index >= entities.Length + objects.Length && index - entities.Length - objects.Length < floorObjects.Length) {

					aux = floorObjects [index - entities.Length - objects.Length].prefab;

				} else {

					aux = wallObjects [index - entities.Length - objects.Length - floorObjects.Length].prefab;

				}
			}

			return aux;

		}

		public void SetColorObject (Color newColor, string objectName)
		{
			if (objectName.Equals ("Air")) {
				airColor = newColor;
				return;
			}
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
			foreach (var item in wallObjects) {
				if (item.prefab.name.Equals (objectName)) {
					item.color = newColor;
					return;
				}
			}
			foreach (var item in floorObjects) {
				if (item.prefab.name.Equals (objectName)) {
					item.color = newColor;
					return;
				}
			}
			foreach (var item in physicsMaterial) {
				if (item.material.name.Equals (objectName)) {
					item.color = newColor;
					return;
				}
			}
			foreach (var item in physicsMaterial2D) {
				if (item.material.name.Equals (objectName)) {
					item.color = newColor;
					return;
				}
			}
		}

	}
}