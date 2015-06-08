using UnityEngine;
using System.Collections;

public class ToScene : MonoBehaviour {
	
	public Camera camera;
	private static Camera CAMERA;
		
	private static readonly int CameraPositionY_ImageCrop = 1000;
	private static readonly int CameraPositionY_ImageControl = 0;
	private static readonly int CameraPositionY_Show3D = -1000;

	public GameObject [] imageCropObj;

	void Awake(){
		CAMERA = camera;
	}

	public static int scene;

	public static void GoTo(int scene){
		Transform ct = CAMERA.transform;

		ToScene.scene = scene;

		if(scene == Scene.ImageCrop){
			ct.localPosition = new Vector3(0, CameraPositionY_ImageCrop);
		}else if(scene == Scene.ImageControl){
			ct.localPosition = new Vector3(0, CameraPositionY_ImageControl);
		}else if(scene == Scene.Show3D){
			ct.localPosition = new Vector3(0, CameraPositionY_Show3D);
		}
	}
	
	void Update () {
		if(scene == Scene.ImageCrop){
			for(int i = 0; i<imageCropObj.Length; i++){
				imageCropObj[i].SetActive(true);
			}
		}else if(scene == Scene.ImageControl){
			for(int i = 0; i<imageCropObj.Length; i++){
				imageCropObj[i].SetActive(false);
			}
		}else if(scene == Scene.Show3D){
			for(int i = 0; i<imageCropObj.Length; i++){
				imageCropObj[i].SetActive(false);
			}
		}
	}
}
