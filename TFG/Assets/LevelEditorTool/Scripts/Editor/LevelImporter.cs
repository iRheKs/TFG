using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using FMB_Tools;

namespace LMGTool
{
	public class LevelImporter : AssetPostprocessor
	{

		private const int textureSize = 128;

		private void OnPreprocessTexture ()
		{

			var fileNameIndex = assetPath.LastIndexOf ('/');
			var fileName = assetPath.Substring (fileNameIndex + 1);

			fileName = Path.GetExtension (fileName);

			if (!fileName.Equals (".png"))
				return;

			var importer = assetImporter as TextureImporter;

			importer.textureType = TextureImporterType.Sprite;
			importer.isReadable = true;
			importer.mipmapEnabled = false;
			importer.wrapMode = TextureWrapMode.Clamp;
			importer.filterMode = FilterMode.Point;
			importer.compressionQuality = 0;
			importer.textureCompression = TextureImporterCompression.Uncompressed;
			importer.npotScale = TextureImporterNPOTScale.None;

		}

	}
}

