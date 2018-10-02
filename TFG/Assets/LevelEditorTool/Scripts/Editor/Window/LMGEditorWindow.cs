using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using FMB_Tools;
using System.IO;

namespace LMGTool
{
	public enum GENERATOR_TO_EDIT
	{
		Map,
		Level,
		None
	}

	public class LMGEditorWindow : EditorWindow
	{

		#region VARIABLES

		//WINDOW RELATED INFO
		bool needsRepaint;
		int borderSize = 10;
		float scaleFactor = 0.95f;
		int xSize = 80;
		int ySize = 50;
		float cellWidth;
		float cellHeight;
		float yOffsetImage = 0;
		float xOffsetImage = 0;
		string prefabName = "Air";
		Color leftMoseButtonColor = Color.white;
		Color rightMouseButtonColor = new Color (1, 1, 1, 0);
		Vector2 mouseToPixel;
		GUISkin infoSkin;
		GENERATOR_TO_EDIT type = GENERATOR_TO_EDIT.None;
		public MapGenerator map;
		LevelPlatformerGenerator level;
		LevelGeneratorInspector levelEditor;
		MapGeneratorInspector mapEditor;

		//EXTERNAL REFERENCES
		//Color[] toDrawTextureColors;
		NewTextureWindow newTexWin;

		//WINDOW FIELDS
		Texture2D textureInfo;
		Texture2D toDrawTexture;
		Texture2D helper;
		Rect bodyTextureRect;
		Rect settingsRect;
		Vector2 setUpScrollView;

		//WINDOW SECTIONS
		EditorSection body;
		EditorSection windowSettings;
		EditorSection setUpSettings;
		EditorSection textureInfoSection;
		List <EditorSection> sections;

		//EVENT FIELDS
		Vector2 downMousePosition;
		Vector2 upMousePosition;

		#endregion

		#region DRAW_METHODS

		void Draw ()
		{
			DrawSections ();
			DrawSettings ();
			if (map == null && level == null) {
				type = GENERATOR_TO_EDIT.None;
			}
			if (level != null && level.setUp != null) {
				UpdateTextures ();
			}
			if (map != null && map.setUp != null) {
				UpdateTextures ();
			}
			switch (type) {
			case GENERATOR_TO_EDIT.Level:
				DrawLevelSettings ();
				break;
			case GENERATOR_TO_EDIT.Map:
				DrawMapSettings ();
				break;
			default:
				DrawSettingsReferenceless ();
				break;
			}
			if (type == GENERATOR_TO_EDIT.Level) {
				if (level.setUp != null) {
					if (level.setUp.editorGeneratorLevel != null) {
						DrawBody ();
					}
				}
			}
			if (type == GENERATOR_TO_EDIT.Map) {
				if (map.setUp != null) {
					if (map.setUp.editorGeneratorLevel != null) {
						DrawBody ();
					}
				}
			}
			DrawTextureInfo ();
		}

		void DrawSections ()
		{
			windowSettings.SetRect (new Rect (0, 0, position.width, 40));
			setUpSettings.SetRect (new Rect (position.width - 450, windowSettings.GetRect ().height, 450, position.height - windowSettings.GetRect ().height - 100));
			body.SetRect (new Rect (0, windowSettings.GetRect ().height, position.width - setUpSettings.GetRect ().width, position.height - windowSettings.GetRect ().height));
			textureInfoSection.SetRect (new Rect (body.GetRect ().width, setUpSettings.GetRect ().height + windowSettings.GetRect ().height, setUpSettings.GetRect ().width, position.height - setUpSettings.GetRect ().height - windowSettings.GetRect ().height));

			foreach (var item in sections) {
				GUI.DrawTexture (item.GetRect (), item.GetTexture ());
			}
		}

		void DrawBody ()
		{
			GUILayout.BeginArea (body.GetRect ());
			{
				if (textureInfo == null)
					return;
				
				float aspectRatio = (float)ySize / (float)xSize;
				float scaleX = body.GetRect ().width * scaleFactor;
				float scaleY = scaleX * aspectRatio;
				Vector2 position = new Vector2 ((body.GetRect ().width / 2 - scaleX / 2) - xOffsetImage, (body.GetRect ().center.y - scaleY / 2) - windowSettings.GetRect ().height - yOffsetImage);
				Vector2 size = new Vector2 (scaleX, scaleY);

				bodyTextureRect = new Rect (position, size);

				cellWidth = bodyTextureRect.width / xSize;
				cellHeight = bodyTextureRect.height / ySize;

				EditorGUI.DrawTextureTransparent (bodyTextureRect, textureInfo);
				GUI.DrawTexture (bodyTextureRect, toDrawTexture);
				DrawGrid (bodyTextureRect);
				EditorTextureUtilities.ClearTexture (toDrawTexture);
			}
			GUILayout.EndArea ();
		}

		//----DrawRects----(Textures)
		//	void DrawRects ()
		//	{
		//		Vector2 position;
		//		Vector2 size = new Vector2 (50, 50);
		//
		//		horizontalRect = EditorGUILayout.BeginHorizontal ();
		//		yOffset = (int)horizontalRect.position.y;
		//		position = new Vector2 (0, yOffset);
		//		pixels.pixelRects [0] = new Rect (position, size * 20);
		//		EditorGUI.DrawTextureTransparent (pixels.pixelRects [0], pixelTexture);
		//		GUI.DrawTexture (pixels.pixelRects [0], pixelTexture);
		//		EditorGUILayout.EndHorizontal ();
		//		//------Pixel Rows-----(Textures)
		//		for (int i = 0; i < ySizeRects; i++) {
		//
		//			for (int j = 0; j < xSizeRects; j++) {
		//				position = new Vector2 (size.x * j, size.y * i + horizontalRect.position.y);
		//				pixels.pixelRects [i * xSizeRects + j] = new Rect (position, size);
		//				GUI.DrawTexture (pixels.pixelRects [i * xSizeRects + j], pixelTexture, ScaleMode.ScaleToFit, false, 0, pixels.pixelColors [i * xSizeRects + j], 0, 0);
		//			}
		//
		//		}
		//
		//	}
		//----DrawRects----(Rects)
		//	void DrawRects ()
		//	{
		//		Vector2 position;
		//		Vector2 size = new Vector2 (5, 5);
		//
		//		horizontalRect = EditorGUILayout.BeginHorizontal ();
		//		EditorGUILayout.EndHorizontal ();
		//		//-------Pîxel Rows------(Rects)
		//		for (int i = 0; i < ySizeRects; i++) {
		//			for (int j = 0; j < xSizeRects; j++) {
		//				position = new Vector2 (size.x * j, size.y * i + horizontalRect.position.y);
		//				pixels.pixelRects [i * xSizeRects + j] = new Rect (position, size);
		//				EditorGUI.DrawRect (pixels.pixelRects [i * xSizeRects + j], pixels.pixelColors [i * xSizeRects + j]);
		//			}
		//		}
		//	}

		void DrawSettings ()
		{
			settingsRect = new Rect (borderSize, borderSize, windowSettings.GetRect ().width - borderSize * 2, windowSettings.GetRect ().height - borderSize * 2);
//			GUILayout.BeginArea (windowSettings.GetRect ());
//			{
			GUILayout.BeginArea (settingsRect);
			{
				GUILayout.FlexibleSpace ();
				EditorGUILayout.BeginHorizontal ();
				{
					//------Colors------
					EditorGUILayout.LabelField (prefabName, EditorStyles.whiteBoldLabel);
					EditorGUI.BeginChangeCheck ();
					leftMoseButtonColor = EditorGUILayout.ColorField ("", leftMoseButtonColor);
					if (EditorGUI.EndChangeCheck ()) {
						if (type == GENERATOR_TO_EDIT.Level) {
							level.setUp.SetColorObject (leftMoseButtonColor, prefabName);
						}
						if (type == GENERATOR_TO_EDIT.Map) {
							map.setUp.SetColorObject (leftMoseButtonColor, prefabName);
						}
					}

					DrawButtons ();

					EditorGUILayout.EndHorizontal ();
					GUILayout.FlexibleSpace ();
				}
			}
			GUILayout.EndArea ();
//			}
//			GUILayout.EndArea ();

		}

		void DrawMapSettings ()
		{
			GUILayout.BeginArea (setUpSettings.GetRect ());
			{
				
				setUpScrollView = EditorGUILayout.BeginScrollView (setUpScrollView, GUILayout.MaxHeight (setUpSettings.GetRect ().height));
				EditorGUILayout.Space ();
				EditorGUI.BeginChangeCheck ();
				map = (MapGenerator)EditorGUILayout.ObjectField (map, typeof(MapGenerator), true);
				if (EditorGUI.EndChangeCheck ()) {
					if (map == null) {
						type = GENERATOR_TO_EDIT.None;	
					}
				}
				if (map != null) {
					if (mapEditor == null)
						mapEditor = (MapGeneratorInspector)Editor.CreateEditor (map);
					mapEditor.OnWindowGUI ();
					if (map.setUp != null)
						UpdateTextures ();
					
				}
				EditorGUILayout.EndScrollView ();
			}
			GUILayout.EndArea ();
		}

		void DrawLevelSettings ()
		{
			GUILayout.BeginArea (setUpSettings.GetRect ());
			{
				setUpScrollView = EditorGUILayout.BeginScrollView (setUpScrollView, GUILayout.MaxHeight (setUpSettings.GetRect ().height));
				EditorGUILayout.Space ();
				EditorGUI.BeginChangeCheck ();
				level = (LevelPlatformerGenerator)EditorGUILayout.ObjectField (level, typeof(LevelPlatformerGenerator), true);
				if (EditorGUI.EndChangeCheck ()) {
					if (level == null) {
						type = GENERATOR_TO_EDIT.None;	
					}
				}
				if (level != null) {
					if (levelEditor == null)
						levelEditor = (LevelGeneratorInspector)Editor.CreateEditor (level);
					levelEditor.OnWindowGUI ();
					if (level.setUp != null)
						UpdateTextures ();
				}

				EditorGUILayout.EndScrollView ();
			}
			GUILayout.EndArea ();
		}

		void DrawSettingsReferenceless ()
		{
			GUILayout.BeginArea (setUpSettings.GetRect ());
			{
				EditorGUILayout.Space ();
				EditorGUI.indentLevel = 1;
				EditorGUI.BeginChangeCheck ();
				level = (LevelPlatformerGenerator)EditorGUILayout.ObjectField (level, typeof(LevelPlatformerGenerator), true);
				if (EditorGUI.EndChangeCheck ()) {
					if (level != null) {
						type = GENERATOR_TO_EDIT.Level;
						if (level.setUp != null && level.setUp.editorGeneratorLevel != null)
							SetTexture (level.setUp.editorGeneratorLevel);
					}
				}
				EditorGUI.BeginChangeCheck ();
				map = (MapGenerator)EditorGUILayout.ObjectField (map, typeof(MapGenerator), true);
				if (EditorGUI.EndChangeCheck ()) {
					if (map != null) {
						type = GENERATOR_TO_EDIT.Map;
						if (map.setUp != null && map.setUp.editorGeneratorLevel != null)
							SetTexture (map.setUp.editorGeneratorLevel);
					}
				}
				EditorGUI.indentLevel = 0;
			}
			GUILayout.EndArea ();
		}

		void DrawGrid (Rect rect)
		{

			if (cellWidth <= 6 || cellHeight <= 6)
				return;

			Handles.BeginGUI ();
			{
				Handles.color = Color.black;

				for (float offsetX = 0; offsetX <= rect.width; offsetX += cellWidth) {
					Handles.DrawLine (new Vector2 (rect.x + offsetX, rect.y), new Vector2 (rect.x + offsetX, rect.y + rect.height));
				}
				for (float offsetY = 0; offsetY <= rect.height; offsetY += cellHeight) {
					Handles.DrawLine (new Vector2 (rect.x, rect.y + offsetY), new Vector2 (rect.x + rect.width, rect.y + +offsetY));

				}
				Handles.color = Color.white;
			}
			Handles.EndGUI ();
		}

		void DrawButtons ()
		{
			//----SaveButton----
			if (level != null) {
				if (level.setUp != null && level.setUp.editorGeneratorLevel != null) {
					if (GUILayout.Button ("Save Texture", EditorStyles.miniButtonLeft)) {
						SaveTexture ();
					}
					if (GUILayout.Button ("Save As", EditorStyles.miniButtonMid)) {
						SaveAsTexture ();
					}
					if (GUILayout.Button ("New Texture", EditorStyles.miniButtonRight)) {
						NewTexture ();
					}
					return;
				}
				if (level.setUp != null && level.setUp.editorGeneratorLevel == null) {
					if (GUILayout.Button ("New Texture", EditorStyles.miniButton)) {
						NewTexture ();
					}
					return;
				}
				if (level.setUp == null) {
					if (GUILayout.Button ("New Set Up", EditorStyles.miniButton)) {
						CreateLevelSetUp ();
					}
					return;
				}
			}
			if (map != null) {
				if (map.setUp != null && map.setUp.editorGeneratorLevel != null) {
					if (GUILayout.Button ("Save Texture", EditorStyles.miniButtonLeft)) {
						SaveTexture ();
					}
					if (GUILayout.Button ("Save As", EditorStyles.miniButtonMid)) {
						SaveAsTexture ();
					}
					if (GUILayout.Button ("New Texture", EditorStyles.miniButtonRight)) {
						NewTexture ();
					}
					return;
				}
				if (map.setUp != null && map.setUp.editorGeneratorLevel == null) {
					if (GUILayout.Button ("New Texture", EditorStyles.miniButton)) {
						NewTexture ();
					}
					return;
				}
				if (map.setUp == null) {
					if (GUILayout.Button ("New Set Up", EditorStyles.miniButton)) {
						CreateMapSetUp ();
					}
					return;
				}
			}

		}

		void DrawTextureInfo ()
		{
			
			Vector2 currentpos = mouseToPixel;
			GUILayout.BeginArea (textureInfoSection.GetRect ());
			{
				GUILayout.FlexibleSpace ();
				EditorGUILayout.BeginHorizontal ();
				{
					GUILayout.FlexibleSpace ();
					EditorGUILayout.BeginVertical ();
					{
						if (textureInfo != null) {
							GUILayout.Label ("[" + textureInfo.width + "x" + textureInfo.height + "]", infoSkin.label);
							if (mouseToPixel.x < 0 || mouseToPixel.y < 0 || mouseToPixel.x >= toDrawTexture.width || mouseToPixel.y >= toDrawTexture.height) {
								currentpos = new Vector2 (0, 0);
							}
							GUILayout.Label (currentpos.ToString (), infoSkin.label);
							needsRepaint = true;
						} 
						GUILayout.Label (prefabName, infoSkin.label);
						GUILayout.Label (leftMoseButtonColor.ToString (), infoSkin.label);
					}
					EditorGUILayout.EndVertical ();
					GUILayout.FlexibleSpace ();


				}
				EditorGUILayout.EndHorizontal ();
				GUILayout.FlexibleSpace ();

			}
			GUILayout.EndArea ();
		}

		#endregion

		//----EventManagement----(Textures)
		//	void EventManagement ()
		//	{
		//		Event guiEvent = Event.current;
		//
		//		if ((guiEvent.type == EventType.MouseDrag || guiEvent.type == EventType.MouseDown) && guiEvent.button == 0) {
		//			for (int i = 0; i < pixelRects.Length; i++) {
		//				if (pixelRects [i].Contains (guiEvent.mousePosition)) {
		//					pixelTextures [i].SetPixel (0, 0, leftMoseButtonColor);
		//					pixelTextures [i].Apply ();
		//					needsRepaint = true;
		//					break;
		//				}
		//			}
		//		}
		//		if ((guiEvent.type == EventType.MouseDrag || guiEvent.type == EventType.MouseDown) && guiEvent.button == 1) {
		//			for (int i = 0; i < pixelRects.Length; i++) {
		//				if (pixelRects [i].Contains (guiEvent.mousePosition)) {
		//					pixelTextures [i].SetPixel (0, 0, rightMouseButtonColor);
		//					pixelTextures [i].Apply ();
		//					needsRepaint = true;
		//					break;
		//				}
		//			}
		//		}
		//	}
		//
		//----EventManagement----(Rects)
		//	void EventManagement ()
		//	{
		//		Event guiEvent = Event.current;
		//
		//		if ((guiEvent.type == EventType.MouseDrag || guiEvent.type == EventType.MouseDown) && guiEvent.button == 0) {
		//			for (int i = 0; i < pixelRects.Length; i++) {
		//				if (pixels.pixelRects [i].Contains (guiEvent.mousePosition)) {
		//					pixels.pixelColors [i] = leftMoseButtonColor;
		//					needsRepaint = true;
		//					break;
		//				}
		//			}
		//		}
		//		if ((guiEvent.type == EventType.MouseDrag || guiEvent.type == EventType.MouseDown) && guiEvent.button == 1) {
		//			for (int i = 0; i < pixelRects.Length; i++) {
		//				if (pixels.pixelRects [i].Contains (guiEvent.mousePosition)) {
		//					pixels.pixelColors [i] = rightMouseButtonColor;
		//					needsRepaint = true;
		//					break;
		//				}
		//			}
		//		}
		//
		//	}

		#region UTILITY_METHODS

		void CalculateFilledRectangle (Texture2D texture, Vector2 firstPosition, Vector2 lastPosition, Color color)
		{
			Rect rectangle;

			EditorTextureUtilities.TransformToLeftTop (ref firstPosition, texture.height);
			EditorTextureUtilities.TransformToLeftTop (ref lastPosition, texture.height);

			if (firstPosition.x > lastPosition.x)
				EditorTextureUtilities.SwapX (ref firstPosition, ref lastPosition);
			if (firstPosition.y < lastPosition.y)
				EditorTextureUtilities.SwapY (ref firstPosition, ref lastPosition);

			rectangle = new Rect (firstPosition.x, firstPosition.y, Mathf.Abs (lastPosition.x - firstPosition.x), Mathf.Abs (lastPosition.y - firstPosition.y));

			for (int x = (int)rectangle.x; x <= rectangle.xMax; x++) {
				for (int y = (int)rectangle.y; y > rectangle.y - rectangle.height - 1; y--) {
					texture.SetPixel (x, y, color);
				}
			}
			texture.Apply ();
		}

		public void SetColor (Color newColor, string label)
		{
			leftMoseButtonColor = newColor;
			prefabName = label;
		}

		public void SetType (GENERATOR_TO_EDIT _type, object generator)
		{
			if (_type == GENERATOR_TO_EDIT.Level) {
				level = (LevelPlatformerGenerator)generator;
			}
			if (_type == GENERATOR_TO_EDIT.Map) {
				map = (MapGenerator)generator;
			}
			type = _type;
		}

		public void SetTexture (Texture2D tex)
		{
			if (tex != null) {
				if (type == GENERATOR_TO_EDIT.Level) {
					level.setUp.editorGeneratorLevel = tex;
				}
				if (type == GENERATOR_TO_EDIT.Map) {
					map.setUp.editorGeneratorLevel = tex;
				}
				textureInfo = tex;
				toDrawTexture = null;
				UpdateTextures ();
			}
		}

		#endregion

		#region EVENT_METHODS

		//----EventManagement----(SingleTexture)
		void EventManagement ()
		{
			Event guiEvent = Event.current;

			MoveTexture (guiEvent);
			SetZoom (guiEvent);
			if (!guiEvent.control) {
				PaintErase (guiEvent);	
			}
			FilledRectangle (guiEvent);
			if (textureInfo != null) {
				textureInfo.Apply ();	
			}

		}

		void MoveTexture (Event e)
		{
			if (e.type == EventType.KeyDown) {
				switch (e.keyCode) {
				case KeyCode.W:
					yOffsetImage -= 580f * Time.fixedDeltaTime;
					break;

				case KeyCode.S:
					yOffsetImage += 580f * Time.fixedDeltaTime;

					break;
				case KeyCode.A:
					xOffsetImage -= 580f * Time.fixedDeltaTime;

					break;

				case KeyCode.D:
					xOffsetImage += 580f * Time.fixedDeltaTime;

					break;
				}
			}
			needsRepaint = true;
		}

		void SetZoom (Event e)
		{
			if (e.type == EventType.ScrollWheel) {
				float deltaY = e.delta.y * (-1) * Time.fixedDeltaTime;
				scaleFactor += 0.7f * deltaY;
				scaleFactor = Mathf.Max (0.2f, scaleFactor);
			}
			needsRepaint = true;
		}

		void PaintErase (Event e)
		{
			helper = toDrawTexture;
			if (body.GetRect ().Contains (e.mousePosition)) {
				mouseToPixel = EditorTextureUtilities.CalculatePixel (e.mousePosition, body.GetRect ().position, bodyTextureRect, cellWidth, cellHeight);
				if (mouseToPixel.x < 0 || mouseToPixel.y < 0 || mouseToPixel.x >= toDrawTexture.width || mouseToPixel.y >= toDrawTexture.height) {
					needsRepaint = true;
					if (helper != null)
						EditorTextureUtilities.ClearTexture (helper, true);
					return;
				}
				if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown)) {
					if (e.button == 0) {
						textureInfo.SetPixel ((int)mouseToPixel.x, toDrawTexture.height - (int)mouseToPixel.y - 1, leftMoseButtonColor);
					}
					if (e.button == 1) {
						textureInfo.SetPixel ((int)mouseToPixel.x, toDrawTexture.height - (int)mouseToPixel.y - 1, rightMouseButtonColor);
					}
				} else {
					helper.SetPixel ((int)mouseToPixel.x, toDrawTexture.height - (int)mouseToPixel.y - 1, new Color (leftMoseButtonColor.r, leftMoseButtonColor.g, leftMoseButtonColor.b, 0.5f));
					helper.Apply ();
				}
			} else {
				mouseToPixel = new Vector2 (0, 0);
				if (helper != null)
					EditorTextureUtilities.ClearTexture (helper, true);
			}
			needsRepaint = true;
		}

		void FilledRectangle (Event e)
		{
			Vector2 pixelCoord = EditorTextureUtilities.CalculatePixel (e.mousePosition, body.GetRect ().position, bodyTextureRect, cellWidth, cellHeight);
			helper = toDrawTexture;
			if (e.control) {
				if (e.type == EventType.MouseDown) {
					downMousePosition = EditorTextureUtilities.CalculatePixel (e.mousePosition, body.GetRect ().position, bodyTextureRect, cellWidth, cellHeight);
					if (e.button == 0) {
						CalculateFilledRectangle (helper, downMousePosition, pixelCoord, new Color (leftMoseButtonColor.r, leftMoseButtonColor.g, leftMoseButtonColor.b, 0.5f));
					}
					if (e.button == 1) {
						CalculateFilledRectangle (helper, downMousePosition, pixelCoord, new Color (1 - leftMoseButtonColor.r, 1 - leftMoseButtonColor.g, 1 - leftMoseButtonColor.b, 0.5f));
					}
				}
				if (e.type == EventType.MouseDrag) {
					if (e.button == 0) {
						CalculateFilledRectangle (helper, downMousePosition, pixelCoord, new Color (leftMoseButtonColor.r, leftMoseButtonColor.g, leftMoseButtonColor.b, 0.5f));
					}
					if (e.button == 1) {
						CalculateFilledRectangle (helper, downMousePosition, pixelCoord, new Color (1 - leftMoseButtonColor.r, 1 - leftMoseButtonColor.g, 1 - leftMoseButtonColor.b, 0.5f));
					}
				}
				if (e.type == EventType.MouseUp) {
					upMousePosition = EditorTextureUtilities.CalculatePixel (e.mousePosition, body.GetRect ().position, bodyTextureRect, cellWidth, cellHeight);
					if (e.button == 0) {
						CalculateFilledRectangle (textureInfo, downMousePosition, upMousePosition, leftMoseButtonColor);
					}
					if (e.button == 1) {
						CalculateFilledRectangle (textureInfo, downMousePosition, upMousePosition, rightMouseButtonColor);
					}
					EditorTextureUtilities.ClearTexture (helper, true);
				}
			} else
				return;
		}

		#endregion

		#region INITIALIZATION

		void InitSections ()
		{
			sections = new List<EditorSection> ();
		
			windowSettings = new EditorSection (new Rect (0, 0, position.width, 40), new Color (0.25f, 0.25f, 0.25f, 1.0000f));
			setUpSettings = new EditorSection (new Rect (position.width - 450, windowSettings.GetRect ().height, 450, position.height - windowSettings.GetRect ().height - 100), new Color (0.4f, 0.4f, 0.4f, 1.0f));
			body = new EditorSection (new Rect (0, windowSettings.GetRect ().height, position.width - setUpSettings.GetRect ().width, position.height - windowSettings.GetRect ().height), new Color (0.7f, 0.7f, 0.7f, 1.0000f));
			textureInfoSection = new EditorSection (new Rect (body.GetRect ().width, setUpSettings.GetRect ().height + windowSettings.GetRect ().height, setUpSettings.GetRect ().width, position.height - setUpSettings.GetRect ().height - windowSettings.GetRect ().height), new Color (0.55f, 0.55f, 0.55f, 1.0f));

			sections.Add (windowSettings);
			sections.Add (body);
			sections.Add (setUpSettings);
			sections.Add (textureInfoSection);

			setUpScrollView = new Vector2 (setUpSettings.GetRect ().x + borderSize / 2, setUpSettings.GetRect ().y);
		}

		void UpdateTextures ()
		{
			if (type == GENERATOR_TO_EDIT.Map && map.setUp.editorGeneratorLevel != null) {
				textureInfo = map.setUp.editorGeneratorLevel;
			} else if (type == GENERATOR_TO_EDIT.Level && level.setUp.editorGeneratorLevel != null) {
				textureInfo = level.setUp.editorGeneratorLevel;
			} else
				textureInfo = null;
			if (textureInfo != null) {
				textureInfo.filterMode = FilterMode.Point;
				textureInfo.wrapMode = TextureWrapMode.Clamp;

				xSize = textureInfo.width;
				ySize = textureInfo.height;
			}
				
//			if (!texturesInitiated) {
			if (toDrawTexture == null) {
				toDrawTexture = new Texture2D (xSize, ySize);
				toDrawTexture.filterMode = FilterMode.Point;
				toDrawTexture.wrapMode = TextureWrapMode.Clamp;
				EditorTextureUtilities.ClearTexture (toDrawTexture, true);
			} 
//			else
//				toDrawTexture.Resize (xSize, ySize);
//			} 

		}

		#endregion

		#region ASSET_MANAGEMENT

		void SaveTexture ()
		{
			if (textureInfo != null)
				EditorTextureUtilities.ExportImage (textureInfo, false);

		}

		void SaveAsTexture ()
		{
			if (textureInfo != null) {
				EditorTextureUtilities.ExportImage (textureInfo);

				if (type == GENERATOR_TO_EDIT.Level) {
					level.setUp.editorGeneratorLevel = textureInfo;
				}
				if (type == GENERATOR_TO_EDIT.Map) {
					map.setUp.editorGeneratorLevel = textureInfo;
				}

				AssetDatabase.Refresh ();
			}
		}

		void NewTexture ()
		{
			if (textureInfo != null) {
				if (EditorUtility.DisplayDialog ("Save image", "Do yo want to save you current image before creating a new one?", "YES", "NO")) {
					SaveTexture ();
				}
			}
			newTexWin = (NewTextureWindow)EditorWindow.GetWindow (typeof(NewTextureWindow));
			newTexWin.win = this;
			newTexWin.Show ();
		}

		void CreateLevelSetUp ()
		{
			
			LevelGeneratorSetUp asset = ScriptableObject.CreateInstance<LevelGeneratorSetUp> ();

			AssetDatabase.CreateAsset (asset, "Assets/New Level Generator Set Up.asset");
		
			AssetDatabase.SaveAssets ();

			level.setUp = asset;

			EditorUtility.FocusProjectWindow ();

			Selection.activeObject = asset;

		}

		void CreateMapSetUp ()
		{
			MapGeneratorSetUp asset = ScriptableObject.CreateInstance<MapGeneratorSetUp> ();

			AssetDatabase.CreateAsset (asset, "Assets/New Map Generator Set Up.asset");

			AssetDatabase.SaveAssets ();

			map.setUp = asset;

			EditorUtility.FocusProjectWindow ();

			Selection.activeObject = asset;
		}

		#endregion

		#region UNITY_DEFAULT_METHODS

		[MenuItem ("Window/Generator Editor")]
		static void Init ()
		{
			LMGEditorWindow myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
			myWindow.titleContent = new GUIContent ("Generator Editor");
			myWindow.Show ();
		}

		public void OpenWindow ()
		{
			//LMGEditorWindow myWindow = (LMGEditorWindow)EditorWindow.GetWindow (typeof(LMGEditorWindow));
			this.titleContent = new GUIContent ("Generator Editor");
			Show ();
		}

		void OnGUI ()
		{
			Draw ();

			EventManagement ();

			if (needsRepaint) {
				Repaint ();
				needsRepaint = false;
			}

//			if (type == GENERATOR_TO_EDIT.Map) {
//				EditorUtility.SetDirty (map);
//			}
//			if (type == GENERATOR_TO_EDIT.Level) {
//				EditorUtility.SetDirty (level);
//			}
		}

		void OnEnable ()
		{

			minSize = new Vector2 (1100, 700);
			maxSize = new Vector2 (1920, 1080);

			infoSkin = Resources.Load<GUISkin> ("LevelEditorToolSkin");

			InitSections ();
			//InitTextures ();
		}

		void OnDisable ()
		{
			UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty (UnityEngine.SceneManagement.SceneManager.GetActiveScene ());
			if (textureInfo != null && (level != null || map == null) && (level == null || map != null) && (level.setUp != null || map.setUp != null)) {
				Debug.Log ("saved");
				EditorTextureUtilities.ExportImage (textureInfo, false);
			}

			if (newTexWin != null) {
				newTexWin.Close ();	
			}
		}

		#endregion
	}
}

////[System.Serializable]
//struct PixelInfo
//{
//	public Rect[] pixelRects;
//	public Color[] pixelColors;
//
//	public void InitInfo (int xSizeRects, int ySizeRects)
//	{
//		pixelRects = new Rect[xSizeRects * ySizeRects];
//		pixelColors = new Color[xSizeRects * ySizeRects];
//
//		for (int i = 0; i < pixelColors.Length; i++) {
//			pixelColors [i] = Color.red;
//		}
//	}
//}