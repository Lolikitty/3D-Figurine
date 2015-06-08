using UnityEngine;
using System.Collections;

public class Show3D : MonoBehaviour {

	public GameObject button_Back;

	public Color ac;

//	[Range(0.0f, 1.0f)]
	float small = 0.1f;

//	[Range(0.0f, 1.0f)]
	float large = 0.2f; 

	float value = 0.2f;

	void Awake(){
		UIEventListener.Get (button_Back).onClick = Button_Back;
	}

	void Button_Back(GameObject obj){
		print ("okokok");
		ToScene.GoTo (Scene.ImageControl);
	}

	public void Init () {

		ac = ChooseControl.AVERAGE_COLOR;



//		Texture2D t = ChooseControl.NEW_FACE_TEXTURE;
//
//
//
//		for(int y = 0; y < t.height; y++){
//			for(int x = 0; x < t.width; x++){
//				float gs = t.GetPixel(x, y).grayscale;
//				float r = t.GetPixel(x, y).r;
//				float g = t.GetPixel(x, y).g;
//				float b = t.GetPixel(x, y).b;
//				
//				if(
//				      r < g + value && r > g - value
//				   && r < b + value && r > b - value
//
//				   && g < r + value && g > r - value
//				   && g < b + value && g > b - value
//
//				   && b < r + value && b > r - value
//			       && b < g + value && b > g - value
//				   ){
//
//				}else{
//					t.SetPixel(x, y, ac);
//				}
//			}
//		}




//		for(int y = 0; y < t.height; y++){
//			for(int x = 0; x < t.width; x++){
//				float gs = t.GetPixel(x, y).grayscale;
//				float r = t.GetPixel(x, y).r;
//				float g = t.GetPixel(x, y).g;
//				float b = t.GetPixel(x, y).b;
//
//				if(r > ac.r - 0.28f && r < ac.r + small
//				   && g > ac.g - large && g < ac.g + small
//				   && b > ac.b - large && b < ac.b + small){
//						t.SetPixel(x, y, ac);
//				}
//			}
//		}

//		t.Apply ();

		GetComponent<Renderer> ().material.mainTexture = ChooseControl.NEW_FACE_TEXTURE;


	}

}
