using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageCrop : MonoBehaviour {

	public RawImage downloadImage;

	public RawImage img;

	public static Texture2D DOWNLOAD_IMG;
	public static Texture2D RESCALE_IMG;

	public RectTransform mark;

	void Awake(){
		StartCoroutine (LoadImg());

		ImageCropControl icc = img.GetComponent<ImageCropControl>();
		icc.SetControlEnable (true);
//		icc.SetDepth (10);

		mark.anchoredPosition = Vector2.zero;
		mark.sizeDelta = new Vector2 (Screen.width+2, Screen.height+2);
	}
	
	void Start () {

	}

	void Update () {
	
	}

	IEnumerator LoadImg(){
		using (WWW w = new WWW (@"https://dl.dropboxusercontent.com/u/49791736/X.jpg")) {
			yield return w;

			DOWNLOAD_IMG = w.texture;

			Texture2D newImg = w.texture;

			RectTransform rt = downloadImage.GetComponent<RectTransform>();
			Vector2 v = rt.sizeDelta;
			int imggMaxSize = 600;
			int newWidth = (int)(imggMaxSize *((float)w.texture.width / w.texture.height));
			int newHeight = imggMaxSize;

			print (newWidth + " , " + newHeight);

			TextureScale.Bilinear(newImg, newWidth, newHeight);

			RESCALE_IMG = newImg;

			rt.sizeDelta = new Vector2(newWidth, newHeight);


			downloadImage.texture = newImg;
			downloadImage.GetComponent<RectTransform>().sizeDelta = rt.sizeDelta;
			rt.anchoredPosition = new Vector2(Screen.width/2 - rt.sizeDelta.x/2, 0);


			RectTransform imgRT = img.GetComponent<RectTransform>();
			img.texture = newImg;
			imgRT.sizeDelta = rt.sizeDelta / 2;
			imgRT.anchoredPosition = new Vector2(Screen.width/2 - rt.sizeDelta.x/4, Screen.height/2 - rt.sizeDelta.y/4);


			ImageCropControl icc = img.GetComponent<ImageCropControl>();
			//		icc.SetControlEnable (true);
			icc.ControlPointPositionRefresh ();
		}

	}

	public void Button_Next(){
		ToScene.GoTo (Scene.ImageControl);
	}

}





