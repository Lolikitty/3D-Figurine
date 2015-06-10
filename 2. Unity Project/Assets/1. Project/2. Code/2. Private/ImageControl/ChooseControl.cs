using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using System.IO;
#endif

public class ChooseControl : MonoBehaviour {

	public GameObject button_BG;
	public GameObject button_Face;
	public GameObject button_Eyes;
	public GameObject button_Nose;
	public GameObject button_Mouth;
	public GameObject button_Back;
	public GameObject button_Next;

	public ImageControl uiTexture_Face;
	public ImageControl uiTexture_Eyes;
	public ImageControl uiTexture_Nose;
	public ImageControl uiTexture_Mouth;

	public GameObject eyes2;

	public GameObject button_Face1, button_Face2, button_Face3;
	public Texture2D face1, face2, face3;

	public static int CHOOSE_FACE = 1;

	public GameObject head;
	public Texture2D head_img;

	public UITexture head_texture;

	Texture2D www_Head_Texture;

	public static Color AVERAGE_COLOR = Color.black;

	public Show3D s3d;

	public Texture2D circleMark;

	void Awake(){
		AVERAGE_COLOR = Color.black;
	}

	void Start () {

//		StartCoroutine (LoadImg());
//		LoadImg ();
	}

	public void  LoadImg(){

		ButtonInit ();

//		WWW w = new WWW (@"https://dl.dropboxusercontent.com/u/49791736/A.jpg");
//		yield return w;

		Texture2D t = new Texture2D (TextureData.CROP_TEXTURE_FIX_AND_CIRCLE.width, TextureData.CROP_TEXTURE_FIX_AND_CIRCLE.height);
		t.SetPixels (TextureData.CROP_TEXTURE_FIX_AND_CIRCLE.GetPixels());
		t.Apply ();

		www_Head_Texture = t;

//		www_Head_Texture = w.texture;

//		TextureScale.Bilinear (www_Head_Texture, 1000, (int)(((float)w.texture.height / w.texture.width) * 1000));

		head_texture.mainTexture = www_Head_Texture;

#if UNITY_EDITOR
		File.WriteAllBytes (@"C:\Users\Loli\Desktop\A.png", www_Head_Texture.EncodeToPNG());
#endif


	}

	void ButtonInit(){
		UIEventListener.Get (button_BG).onClick = Button_BG;
		UIEventListener.Get (button_Face).onClick = Button_Face;
		UIEventListener.Get (button_Eyes).onClick = Button_Eyes;
		UIEventListener.Get (button_Nose).onClick = Button_Nose;
		UIEventListener.Get (button_Mouth).onClick = Button_Mouth;
		UIEventListener.Get (button_Face1).onClick = Change_Face;
		UIEventListener.Get (button_Face2).onClick = Change_Face;
		UIEventListener.Get (button_Face3).onClick = Change_Face;
		UIEventListener.Get (button_Next).onClick = Button_Next;
		UIEventListener.Get (button_Back).onClick = Button_Back;
	}

	void Button_Back(GameObject obj){
		ToScene.GoTo (Scene.ImageCrop);
	}

	void Button_Next(GameObject obj){


		/***************************************************
		 * 
		 * If Texture Already Exists, Must Be Deleted
		 * 
		 */

		if(TextureData.LEFT_CROP_EYE){
			Destroy(TextureData.LEFT_CROP_EYE);
			TextureData.LEFT_CROP_EYE = null;
		}

		if(TextureData.RIGHT_CROP_EYE){
			Destroy(TextureData.RIGHT_CROP_EYE);
			TextureData.RIGHT_CROP_EYE = null;
		}

		if(TextureData.LEFT_CROP_EYE_AND_CIRCLE){
			Destroy(TextureData.LEFT_CROP_EYE_AND_CIRCLE);
			TextureData.LEFT_CROP_EYE_AND_CIRCLE = null;
		}

		if(TextureData.RIGHT_CROP_EYE_AND_CIRCLE){
			Destroy(TextureData.RIGHT_CROP_EYE_AND_CIRCLE);
			TextureData.RIGHT_CROP_EYE_AND_CIRCLE = null;
		}

		if(TextureData.CROP_MOUTH){
			Destroy(TextureData.CROP_MOUTH);
			TextureData.CROP_MOUTH = null;
		}

		if(TextureData.CROP_MOUTH_AND_CIRCLE){
			Destroy(TextureData.CROP_MOUTH_AND_CIRCLE);
			TextureData.CROP_MOUTH_AND_CIRCLE = null;
		}


		/**************************************************************************************
		 * 
		 * Compute New Texture
		 * 
		 */

		if (www_Head_Texture != null) {
			Texture2D t = new Texture2D (www_Head_Texture.width, www_Head_Texture.height);
			t.SetPixels (www_Head_Texture.GetPixels ());
		
			Vector3 eyesV3 = uiTexture_Eyes.transform.localPosition;
		
			UITexture eyesTexture = uiTexture_Eyes.GetComponent<UITexture> ();
		
			// Get Face Average Color ------------------------------------------------------------------------------

			for (int x = (int)(eyesV3.x); x < eyesV3.x + eyesTexture.width+20; x++) {
				for (int y = (int)(eyesV3.y); y < eyesV3.y + eyesTexture.height+20; y++) {
					int newX = (int)(x * 0.68f - eyesTexture.width / 2.5f) + 140;
					int newY = (int)(y * 0.68f - eyesTexture.height / 3f) + 140;
					if(y == (int)(eyesV3.y)){
						if(AVERAGE_COLOR == Color.black){
							AVERAGE_COLOR = www_Head_Texture.GetPixel(newX, newY);
						}
						AVERAGE_COLOR = (www_Head_Texture.GetPixel(newX, newY) + AVERAGE_COLOR )/2;
					}
				}
			}

			// Using Face Average Color To Paint Background ------------------------------------------------------------------------------

			for(int x = 0; x < www_Head_Texture.width; x++){
				for(int y = 0; y < www_Head_Texture.height; y++){
					t.SetPixel (x, y, AVERAGE_COLOR);
				}
			}

			// Paint Eye L ------------------------------------------------------------------------------

			TextureData.LEFT_CROP_EYE = new Texture2D(eyesTexture.width+40, eyesTexture.height+30); // + 40 + 30 = 增加貼圖切割範圍

			for (int x = 0; x < TextureData.LEFT_CROP_EYE.width; x++) {
				for (int y = 0; y < TextureData.LEFT_CROP_EYE.height; y++) {
					int newX = (int)((x+eyesV3.x) * 0.68f - eyesTexture.width / 2.5f) + 135;// + 135 往上調整，整體貼圖會往左
					int newY = (int)((y+eyesV3.x) * 0.68f - eyesTexture.height / 3f) + 140; // + 140 往上調整，整體貼圖會往下
					TextureData.LEFT_CROP_EYE.SetPixel (x,y, www_Head_Texture.GetPixel(newX, newY));
				}
			}

			TextureData.LEFT_CROP_EYE.Apply();


			//  Using Left Eye AND-Compute to Circle

			Texture2D t2d = new Texture2D(circleMark.width, circleMark.height);
			t2d.SetPixels(circleMark.GetPixels());
			t2d.Apply();

			TextureScale.Bilinear(t2d, TextureData.LEFT_CROP_EYE.width, TextureData.LEFT_CROP_EYE.height);

			Texture2D andImg = new Texture2D(t2d.width, t2d.height);

			for(int y = 0; y < andImg.height; y++){
				for(int x = 0; x < andImg.width; x++){
					if(t2d.GetPixel(x, y).a != 0){
						Color c = TextureData.LEFT_CROP_EYE.GetPixel(x, y);
						c.a = t2d.GetPixel(x, y).a;
						andImg.SetPixel(x, y, c);
					}else{
						andImg.SetPixel(x, y, new Color(0,0,0,0));
					}
				}
			}

			andImg.Apply();

			TextureData.LEFT_CROP_EYE_AND_CIRCLE = andImg;

			// Paint Eye R ------------------------------------------------------------------------------

			// Using Left Eye Mirror 

			TextureData.RIGHT_CROP_EYE_AND_CIRCLE = new Texture2D(andImg.width, andImg.height);

			for(int y = 0; y < andImg.height; y++){
				for(int x = 0; x < andImg.width; x++){
					TextureData.RIGHT_CROP_EYE_AND_CIRCLE.SetPixel(andImg.width - x, y, andImg.GetPixel(x, y));
				}
			}

			TextureData.RIGHT_CROP_EYE_AND_CIRCLE.Apply();

			// Paint Moush ------------------------------------------------------------------------------

			UITexture mouthTexture = uiTexture_Mouth.GetComponent<UITexture>();

			TextureData.CROP_MOUTH = new Texture2D(mouthTexture.width+80, mouthTexture.height+60); // + 40 + 30 = 增加貼圖切割範圍

			Vector3 mouthV3 = uiTexture_Mouth.transform.localPosition;

			for (int x = 0; x < TextureData.CROP_MOUTH.width; x++) {
				for (int y = 0; y < TextureData.CROP_MOUTH.height; y++) {
					int newX = (int)((x+mouthV3.x) * 0.68f - mouthTexture.width / 2.5f) + 125;// + 135 往上調整，整體貼圖會往左
					int newY = (int)((y+mouthV3.x) * 0.68f - mouthTexture.height / 3f) + 60; // + 140 往上調整，整體貼圖會往下
					TextureData.CROP_MOUTH.SetPixel (x,y, www_Head_Texture.GetPixel(newX, newY));
				}
			}
			
			TextureData.CROP_MOUTH.Apply();

			//  Create Mouse Circle

			Texture2D mouseCircle = new Texture2D(circleMark.width, circleMark.height);
			mouseCircle.SetPixels(circleMark.GetPixels());
			mouseCircle.Apply();
			
			TextureScale.Bilinear(mouseCircle, TextureData.CROP_MOUTH.width, TextureData.CROP_MOUTH.height);

			//  Using Mouse AND-Compute to Circle

			TextureData.CROP_MOUTH_AND_CIRCLE = new Texture2D(mouseCircle.width, mouseCircle.height);
			
			for(int y = 0; y < TextureData.CROP_MOUTH_AND_CIRCLE.height; y++){
				for(int x = 0; x < TextureData.CROP_MOUTH_AND_CIRCLE.width; x++){
					if(mouseCircle.GetPixel(x, y).a != 0){
						Color c = TextureData.CROP_MOUTH.GetPixel(x, y);
						c.a = mouseCircle.GetPixel(x, y).a;
						TextureData.CROP_MOUTH_AND_CIRCLE.SetPixel(x, y, c);
					}else{
						TextureData.CROP_MOUTH_AND_CIRCLE.SetPixel(x, y, new Color(0,0,0,0));
					}
				}
			}
			
			TextureData.CROP_MOUTH_AND_CIRCLE.Apply();


			// -------------------------------------------------------------------------------------

			t.Apply ();
		}

		// Set Position
		TextureData.FACE_POSITION = uiTexture_Face.transform.localPosition;
		TextureData.LEFT_EYE_POSITION = uiTexture_Eyes.transform.localPosition;
		TextureData.RIGHT_POSITION = eyes2.transform.localPosition;
		TextureData.NOSE_POSITION = uiTexture_Nose.transform.localPosition;
		TextureData.MOUTH_POSITION = uiTexture_Mouth.transform.localPosition;


		ToScene.GoTo (Scene.Show3D);
		s3d.Init ();
	}

	void Change_Face(GameObject obj){
		if(obj.name == "Face 1"){
			CHOOSE_FACE = 1;
			uiTexture_Face.GetComponent<UITexture>().mainTexture = face1;
		}else if(obj.name == "Face 2"){
			CHOOSE_FACE = 2;
			uiTexture_Face.GetComponent<UITexture>().mainTexture = face2;
		}else if(obj.name == "Face 3"){
			CHOOSE_FACE = 3;
			uiTexture_Face.GetComponent<UITexture>().mainTexture = face3;
		}
	}

	void Button_BG(GameObject obj){
		uiTexture_Face.SetControlEnable (false);
		uiTexture_Eyes.SetControlEnable (false);
		uiTexture_Nose.SetControlEnable (false);
		uiTexture_Mouth.SetControlEnable (false);
	}

	void Button_Face(GameObject obj){
		uiTexture_Face.SetDepth (1);
		uiTexture_Eyes.SetDepth (0);
		uiTexture_Nose.SetDepth (0);
		uiTexture_Mouth.SetDepth (0);

		uiTexture_Face.SetControlEnable (true);
		uiTexture_Eyes.SetControlEnable (false);
		uiTexture_Nose.SetControlEnable (false);
		uiTexture_Mouth.SetControlEnable (false);
	}

	void Button_Eyes(GameObject obj){
		uiTexture_Face.SetDepth (0);
		uiTexture_Eyes.SetDepth (1);
		uiTexture_Nose.SetDepth (0);
		uiTexture_Mouth.SetDepth (0);

		uiTexture_Face.SetControlEnable (false);
		uiTexture_Eyes.SetControlEnable (true);
		uiTexture_Nose.SetControlEnable (false);
		uiTexture_Mouth.SetControlEnable (false);
	}

	void Button_Nose(GameObject obj){
		uiTexture_Face.SetDepth (0);
		uiTexture_Eyes.SetDepth (0);
		uiTexture_Nose.SetDepth (1);
		uiTexture_Mouth.SetDepth (0);
		
		uiTexture_Face.SetControlEnable (false);
		uiTexture_Eyes.SetControlEnable (false);
		uiTexture_Nose.SetControlEnable (true);
		uiTexture_Mouth.SetControlEnable (false);
	}

	void Button_Mouth(GameObject obj){
		uiTexture_Face.SetDepth (0);
		uiTexture_Eyes.SetDepth (0);
		uiTexture_Nose.SetDepth (0);
		uiTexture_Mouth.SetDepth (1);

		uiTexture_Face.SetControlEnable (false);
		uiTexture_Eyes.SetControlEnable (false);
		uiTexture_Nose.SetControlEnable (false);
		uiTexture_Mouth.SetControlEnable (true);
	}

	public int offSetX = 100, offSetY = -100;

	void Update () {
		eyes2.transform.localPosition = new Vector3 (-uiTexture_Eyes.transform.localPosition.x, uiTexture_Eyes.transform.localPosition.y, uiTexture_Eyes.transform.localPosition.z);
		eyes2.GetComponent<UITexture> ().width = uiTexture_Eyes.GetComponent<UITexture> ().width;
		eyes2.GetComponent<UITexture> ().height = uiTexture_Eyes.GetComponent<UITexture> ().height;
	}

//	void OnGUI(){
//		GUILayout.Label ("Choose Face : " + CHOOSE_FACE);
//		GUILayout.Label ("Face :     Position : " + uiTexture_Face.transform.localPosition + " \t Scale : " + uiTexture_Face.GetComponent<UITexture>().width + ", " + uiTexture_Face.GetComponent<UITexture>().height);
//		GUILayout.Label ("Eye :      Position : " + uiTexture_Eyes.transform.localPosition + " \t Scale : " + uiTexture_Eyes.GetComponent<UITexture>().width + ", " + uiTexture_Eyes.GetComponent<UITexture>().height);
//		GUILayout.Label ("Mouth :   Position : " + uiTexture_Mouth.transform.localPosition + " \t Scale : " + uiTexture_Mouth.GetComponent<UITexture>().width + ", " + uiTexture_Mouth.GetComponent<UITexture>().height);
//	}

}
