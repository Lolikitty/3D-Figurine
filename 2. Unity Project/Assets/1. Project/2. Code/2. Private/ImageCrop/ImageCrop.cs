using UnityEngine;
using System.Collections;

public class ImageCrop : MonoBehaviour {

	public GameObject button_Next;

	public ImageCropControl icc;

	void Awake(){
		UIEventListener.Get (button_Next).onClick = Button_Next;


	}
	
	void Start () {
		icc.SetControlEnable (true);
		icc.SetDepth (10);
	}

	void Update () {
	
	}

	void Button_Next(GameObject obj){
		ToScene.GoTo (Scene.ImageControl);
	}
}
