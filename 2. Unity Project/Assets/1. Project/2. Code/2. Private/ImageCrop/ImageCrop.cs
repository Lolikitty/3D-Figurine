using UnityEngine;
using System.Collections;

public class ImageCrop : MonoBehaviour {

	public GameObject button_Next;

	void Awake(){
		UIEventListener.Get (button_Next).onClick = Button_Next;
	}
	
	void Start () {
	
	}

	void Update () {
	
	}

	void Button_Next(GameObject obj){
		ToScene.GoTo (Scene.ImageControl);
	}
}
