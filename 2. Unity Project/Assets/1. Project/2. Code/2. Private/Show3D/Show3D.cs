using UnityEngine;
using System.Collections;

public class Show3D : MonoBehaviour {

	public UITexture face;
	public UITexture eyeLeft;
	public UITexture eyeRight;
	public UITexture mouth;


	public GameObject button_Back;
	public Color ac;

	void Awake(){
		UIEventListener.Get (button_Back).onClick = Button_Back;
	}

	void Button_Back(GameObject obj){
		ToScene.GoTo (Scene.ImageControl);
	}

	public void Init () {

		// 皮膚平均色 :D
		ac = ChooseControl.AVERAGE_COLOR;

		// Face
		face.mainTexture = TextureData.CROP_TEXTURE_FIX_AND_CIRCLE;
		face.width = TextureData.CROP_TEXTURE_FIX_AND_CIRCLE.width;
		face.height = TextureData.CROP_TEXTURE_FIX_AND_CIRCLE.height;

		// Left Eye
		eyeLeft.mainTexture = TextureData.LEFT_CROP_EYE_AND_CIRCLE;
		eyeLeft.width = TextureData.LEFT_CROP_EYE_AND_CIRCLE.width;
		eyeLeft.height = TextureData.LEFT_CROP_EYE_AND_CIRCLE.height;

		// Right Eye
		eyeRight.mainTexture = TextureData.RIGHT_CROP_EYE_AND_CIRCLE;
		eyeRight.width = TextureData.RIGHT_CROP_EYE_AND_CIRCLE.width;
		eyeRight.height = TextureData.RIGHT_CROP_EYE_AND_CIRCLE.height;

		// Mouth
		mouth.mainTexture = TextureData.CROP_MOUTH_AND_CIRCLE;
		mouth.width = TextureData.CROP_MOUTH_AND_CIRCLE.width;
		mouth.height = TextureData.CROP_MOUTH_AND_CIRCLE.height;

	}

}
