using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using System.IO;
#endif

public class ChooseControl : MonoBehaviour {

	public GameObject button_BG;
	public GameObject button_Face;
	public GameObject button_Eyes;
	public GameObject button_Mouth;
	public GameObject button_Back;
	public GameObject button_Next;

	public ImageControl uiTexture_Face;
	public ImageControl uiTexture_Eyes;
	public ImageControl uiTexture_Mouth;

	public GameObject eyes2;

	public GameObject button_Face1, button_Face2, button_Face3;
	public Texture2D face1, face2, face3;

	public static int CHOOSE_FACE = 1;

	public GameObject head;
	public Texture2D head_img;

	public UITexture head_texture;

	Texture2D www_Head_Texture;

	public static Texture2D NEW_FACE_TEXTURE;

	public static Color AVERAGE_COLOR = Color.black;

	public Show3D s3d;

	void Awake(){
		AVERAGE_COLOR = Color.black;
	}

	void Start () {
		ButtonInit ();
		StartCoroutine (LoadImg());
	}

	IEnumerator LoadImg(){
		WWW w = new WWW (@"https://dl.dropboxusercontent.com/u/49791736/A.jpg");
		yield return w;

		www_Head_Texture = w.texture;

//		TextureScale.Bilinear (www_Head_Texture, 1000, (int)(((float)w.texture.height / w.texture.width) * 1000));

#if UNITY_EDITOR
		File.WriteAllBytes (@"C:\Users\Loli\Desktop\A.png", www_Head_Texture.EncodeToPNG());
#endif
		head_texture.mainTexture = www_Head_Texture;

	}

	void ButtonInit(){
		UIEventListener.Get (button_BG).onClick = Button_BG;
		UIEventListener.Get (button_Face).onClick = Button_Face;
		UIEventListener.Get (button_Eyes).onClick = Button_Eyes;
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
							print ("ok");
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

			int offsetEyeL_X = 0;
			int offsetEyeL_Y = 100;

			for (int x = (int)(eyesV3.x); x < eyesV3.x + eyesTexture.width+20; x++) {
				for (int y = (int)(eyesV3.y); y < eyesV3.y + eyesTexture.height+20; y++) {
					int newX = (int)(x * 0.68f - eyesTexture.width / 2.5f) + 140;
					int newY = (int)(y * 0.68f - eyesTexture.height / 3f) + 140;
					t.SetPixel (newX + offsetEyeL_X, newY + offsetEyeL_Y, www_Head_Texture.GetPixel(newX, newY));
				}
			}

			// Paint Eye R ------------------------------------------------------------------------------


			// -------------------------------------------------------------------------------------

			t.Apply ();

			NEW_FACE_TEXTURE = t;
		}
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
		uiTexture_Mouth.SetControlEnable (false);
	}

	void Button_Face(GameObject obj){
		uiTexture_Face.SetDepth (1);
		uiTexture_Eyes.SetDepth (0);
		uiTexture_Mouth.SetDepth (0);

		uiTexture_Face.SetControlEnable (true);
		uiTexture_Eyes.SetControlEnable (false);
		uiTexture_Mouth.SetControlEnable (false);
	}

	void Button_Eyes(GameObject obj){
		uiTexture_Face.SetDepth (0);
		uiTexture_Eyes.SetDepth (1);
		uiTexture_Mouth.SetDepth (0);

		uiTexture_Face.SetControlEnable (false);
		uiTexture_Eyes.SetControlEnable (true);
		uiTexture_Mouth.SetControlEnable (false);
	}

	void Button_Mouth(GameObject obj){
		uiTexture_Face.SetDepth (0);
		uiTexture_Eyes.SetDepth (0);
		uiTexture_Mouth.SetDepth (1);

		uiTexture_Face.SetControlEnable (false);
		uiTexture_Eyes.SetControlEnable (false);
		uiTexture_Mouth.SetControlEnable (true);
	}

	public int offSetX = 100, offSetY = -100;

	void Update () {
		eyes2.transform.localPosition = new Vector3 (-uiTexture_Eyes.transform.localPosition.x, uiTexture_Eyes.transform.localPosition.y, uiTexture_Eyes.transform.localPosition.z);


//		if (www_Head_Texture != null) {
//			Texture2D t = new Texture2D (www_Head_Texture.width, www_Head_Texture.height);			
//			t.SetPixels (www_Head_Texture.GetPixels());			
//
//			Vector3 eyesV3 = uiTexture_Eyes.transform.localPosition;
//
//			UITexture eyesTexture = uiTexture_Eyes.GetComponent<UITexture>();
//
//			for(int x = (int)(eyesV3.x); x < eyesV3.x + eyesTexture.width+20; x++){
//				for(int y = (int)(eyesV3.y); y < eyesV3.y + eyesTexture.height+20; y++){
//					int newX = (int)(x * 0.68f - eyesTexture.width / 2.5f) + 140;
//					int newY = (int)(y * 0.68f - eyesTexture.height / 3f) + 140;
//
////					t.SetPixel (newX, newY, www_Head_Texture.GetPixel(newX, newY));
//					if(y == (int)(eyesV3.y)){
//						t.SetPixel(newX, newY, Color.blue);
//					}else{
//						t.SetPixel(newX, newY, Color.red);
//					}
//				}
//			}
//
//			t.Apply ();
//
//			head_texture.mainTexture = t;
//
//		}

	}

	void OnGUI(){
		GUILayout.Label ("Choose Face : " + CHOOSE_FACE);
		GUILayout.Label ("Face :     Position : " + uiTexture_Face.transform.localPosition + " \t Scale : " + uiTexture_Face.GetComponent<UITexture>().width + ", " + uiTexture_Face.GetComponent<UITexture>().height);
		GUILayout.Label ("Eye :      Position : " + uiTexture_Eyes.transform.localPosition + " \t Scale : " + uiTexture_Eyes.GetComponent<UITexture>().width + ", " + uiTexture_Eyes.GetComponent<UITexture>().height);
		GUILayout.Label ("Mouth :   Position : " + uiTexture_Mouth.transform.localPosition + " \t Scale : " + uiTexture_Mouth.GetComponent<UITexture>().width + ", " + uiTexture_Mouth.GetComponent<UITexture>().height);
	}

}
