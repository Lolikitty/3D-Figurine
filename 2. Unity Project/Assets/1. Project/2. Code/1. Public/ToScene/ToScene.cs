using UnityEngine;
using System.Collections;

public class ToScene : MonoBehaviour {
	
	public Camera camera;
	private static Camera CAMERA;
		
	private static readonly int CameraPositionY_ImageCrop = 1000;
	private static readonly int CameraPositionY_ImageControl = 0;
	private static readonly int CameraPositionY_Show3D = -1000;
	
	void Awake(){
		CAMERA = camera;
	}
	
	public static void GoTo(int scene){
		Transform ct = CAMERA.transform;

		if(scene == Scene.ImageCrop){
			ct.localPosition = new Vector3(0, CameraPositionY_ImageCrop);
		}else if(scene == Scene.ImageControl){
			ct.localPosition = new Vector3(0, CameraPositionY_ImageControl);
		}else if(scene == Scene.Show3D){
			ct.localPosition = new Vector3(0, CameraPositionY_Show3D);
		}
	}
	
	void Update () {
		
	}
}
