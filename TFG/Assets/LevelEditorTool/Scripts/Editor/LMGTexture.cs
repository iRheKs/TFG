using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LMGTexture
{
	[SerializeField]
	Color[] importColors;
	[SerializeField]
	int width = 50;
	[SerializeField]
	int height = 50;

	Texture2D texture;

}
