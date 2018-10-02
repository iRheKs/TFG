using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace FMB_Tools
{
	public static class EditorTextureUtilities
	{

		public static Vector2 CalculatePixel (Vector2 worldPosition, Vector2 localPosition, Rect textureRect, float cellWidth, float cellHeight)
		{
			localPosition = worldPosition - localPosition;

			float relX = (localPosition.x - textureRect.x) / cellWidth;
			float relY = (localPosition.y - textureRect.y) / cellHeight;
			if (relX < 0)
				relX = -1;
			if (relY < 0)
				relY = -1;
			Vector2 aux = new Vector2 ((int)relX, (int)relY);
			return aux;
		}

		public static void ClearTexture (Texture2D tex, bool apply = false, Color color = default(Color))
		{
			Color[] col = new Color[tex.width * tex.height];
			for (int i = 0; i < tex.width; i++) {
				for (int j = 0; j < tex.height; j++) {
					col [j * tex.width + i] = color;
				}
			}
			tex.SetPixels (col);
			if (apply)
				tex.Apply ();
		}

		public static void TransformToLeftTop (ref Vector2 point, int height)
		{
			point = new Vector2 (point.x, height - point.y - 1);
		}

		public static void SwapX (ref Vector2 point0, ref Vector2 point1)
		{
			Vector2 tmp = point0;
			point0 = new Vector2 (point1.x, point0.y);
			point1 = new Vector2 (tmp.x, point1.y);
		}

		public static void SwapY (ref Vector2 point0, ref Vector2 point1)
		{
			Vector2 tmp = point0;
			point0 = new Vector2 (point0.x, point1.y);
			point1 = new Vector2 (point1.x, tmp.y);
		}

		public static string ExportImage (Texture2D textureInfo, bool showWindow = true)
		{
			// --- JPG AND PNG ONLY SUPPORTED ----
			string path = null;
			if (!showWindow) {
				path = AssetDatabase.GetAssetPath (textureInfo);
				if (!string.IsNullOrEmpty (path))
					path = Application.dataPath + path.Substring (6);
			}
			if (showWindow)
				path = EditorUtility.SaveFilePanel ("Export Image", "", textureInfo.name, "png");

			if (string.IsNullOrEmpty (path))
				return null;
			byte[] encodedBytes;
			string extension = Path.GetExtension (path);
			switch (extension) {
			case ".png":
				encodedBytes = textureInfo.EncodeToPNG ();
				break;
			case ".jpg":
				encodedBytes = textureInfo.EncodeToJPG ();
				break;
			default:
				encodedBytes = textureInfo.EncodeToPNG ();
				break;
			}
			File.WriteAllBytes (path, encodedBytes);
			AssetDatabase.Refresh ();

			// MODIFYING TEXTURE IMPORT SETTINGS
			TextureImporter importer = TextureImporter.GetAtPath (FileUtil.GetProjectRelativePath (path)) as TextureImporter;
			importer.textureType = TextureImporterType.Sprite;
			importer.alphaIsTransparency = false;
			importer.filterMode = FilterMode.Point;
			importer.maxTextureSize = GetTextureImporterMaxSize (textureInfo);
			importer.textureCompression = TextureImporterCompression.Uncompressed;
			importer.isReadable = true;
			importer.SaveAndReimport ();

			AssetDatabase.Refresh ();

			return FileUtil.GetProjectRelativePath (path);

		}

		public static Texture2D CreateNewImage (int x, int y)
		{
			string path = null;
			Texture2D tex = new Texture2D (x, y);
			EditorTextureUtilities.ClearTexture (tex, true, Color.white);

			path = ExportImage (tex);

			tex = AssetDatabase.LoadAssetAtPath <Texture2D> (path);

			AssetDatabase.Refresh ();

			return tex;
		}

		public static int GetTextureImporterMaxSize (Texture2D tex)
		{
			int value = tex.width > tex.height ? tex.width : tex.height;
			int[] values = new int[] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };

			for (int i = 0; i < values.Length; i++) {
				if (value <= values [i]) {
					value = values [i];
					return value;
				}
			}
			return 2048;
		}

	}
}

