using UnityEngine;
using System.Collections;

public class ImageCropControl : MonoBehaviour {
	
	public UITexture img;
	public Texture2D controlPointImage;
	public UIRoot uiRoot;
	public int controlPointWidth = 20, controlPointHeight = 20;
	public int minHeightLimit = 50, minWidthLimit = 50;
	[Range(0, 1)]
	public float moveAlpha = 0.5f;
	
	public Texture2D mouseDrag;
	public Texture2D mouseRL;
	public Texture2D mouseTB;
	public Texture2D mouse17CLK;
	public Texture2D mouse115CLK;
	
	GameObject [] controlPoint = new GameObject[8];
	Vector3 [] ControlPointPosition = new Vector3[8];
	
	public bool controlEnable = true;
	
	public bool lockR = false;
	public float lockValue = 0;
	
	void Start () {
		ImageInit ();
		ControlPointInit ();
		SetControlEnable (controlEnable);
	}
	
	void ControlPointInit(){
		
		for(int i = 0; i < controlPoint.Length; i++){
			controlPoint[i] = new GameObject("Control Point " + i, typeof(BoxCollider), typeof(UITexture));
			
			UIEventListener.Get (controlPoint[i]).onDrag = ControlPoint;
			UIEventListener.Get (controlPoint[i]).onHover = ControlPointEnter;
			
			Transform transform = controlPoint[i].transform;
			transform.parent = uiRoot.transform;
			transform.localScale = Vector3.one;
			
			UITexture texture = controlPoint[i].GetComponent<UITexture>();
			texture.mainTexture = controlPointImage;
			texture.width = controlPointWidth;
			texture.height = controlPointHeight;
			
			BoxCollider boxCollider = controlPoint[i].GetComponent<BoxCollider>();
			boxCollider.size = new Vector3(controlPointWidth, controlPointHeight);
		}
		ControlPointPositionRefresh ();
	}
	
	void ControlPointPositionRefresh(){
		
		float X = img.transform.localPosition.x;
		float Y = img.transform.localPosition.y;
		
		float L = X - img.width / 2 - controlPointWidth / 2;
		float R = X + img.width / 2 + controlPointWidth / 2;
		float T = Y + img.height / 2 + controlPointHeight / 2;
		float B = Y - img.height / 2 - controlPointHeight / 2;
		
		ControlPointPosition [0] = new Vector3 (L, T);
		ControlPointPosition [1] = new Vector3 (X, T);
		ControlPointPosition [2] = new Vector3 (R, T);
		ControlPointPosition [3] = new Vector3 (R, Y);
		ControlPointPosition [4] = new Vector3 (R, B);
		ControlPointPosition [5] = new Vector3 (X, B);
		ControlPointPosition [6] = new Vector3 (L, B);
		ControlPointPosition [7] = new Vector3 (L, Y);
		
		for(int i = 0; i < controlPoint.Length; i++){
			controlPoint[i].transform.localPosition = ControlPointPosition[i];
		}
	}
	
	void ImageInit(){
		UIEventListener.Get (img.gameObject).onDrag = ControlImage;
		UIEventListener.Get (img.gameObject).onDragEnd = ControlImageEnd;
		UIEventListener.Get (img.gameObject).onHover = ControlImageEnter;
	}
	
	void ControlImage(GameObject obj, Vector2 dragVector){
		if(!controlEnable){
			return;
		}
		float x = img.transform.localPosition.x;
		float y = img.transform.localPosition.y;
		float speed = 1680 / (float)(Screen.width + Screen.height);
		float imgX = x + dragVector.x * speed;
		float imgY = y + dragVector.y * speed;
		
		if(lockR && imgX < 20){
			return;
		}
		
		// Move Image
		SetXY (obj, imgX, imgY);
		// Move Control Point
		ControlPointPositionRefresh ();
		// If Move Image, Use Alpha
		obj.GetComponent<UITexture> ().alpha = moveAlpha;
		foreach(GameObject cp in controlPoint){
			cp.GetComponent<UITexture> ().alpha = moveAlpha;
		}
	}
	
	void ControlImageEnd(GameObject obj){
		if(!controlEnable){
			return;
		}
		obj.GetComponent<UITexture> ().alpha = 1;
		foreach(GameObject cp in controlPoint){
			cp.GetComponent<UITexture> ().alpha = 1;
		}
	}
	
	// Change Mouse icon
	void ControlImageEnter(GameObject obj, bool b){
		if(!controlEnable){
			return;
		}
		Cursor.SetCursor (b ? mouseDrag : null, new Vector2(16,16), CursorMode.Auto);
	}
	
	// Change Control-Point icon
	void ControlPointEnter(GameObject obj, bool b){
		
		switch (obj.name) {
		case "Control Point 3" :
		case "Control Point 7" :
			Cursor.SetCursor (b ? mouseRL : null, new Vector2(16,16), CursorMode.Auto);
			break;
		case "Control Point 1" :
		case "Control Point 5" :
			Cursor.SetCursor (b ? mouseTB : null, new Vector2(16,16), CursorMode.Auto);
			break;
		case "Control Point 2" :
		case "Control Point 6" :
			Cursor.SetCursor (b ? mouse17CLK : null, new Vector2(16,16), CursorMode.Auto);
			break;
		case "Control Point 0" :
		case "Control Point 4" :
			Cursor.SetCursor (b ? mouse115CLK : null, new Vector2(16,16), CursorMode.Auto);
			break;
		}
		
	}
	
	// 0   1   2
	//
	// 7       3
	//
	// 6   5   4
	
	//	float tempX = 0;
	
	void ControlPoint(GameObject obj, Vector2 dragVector){
		
		Transform t = img.transform;
		float x = t.localPosition.x;
		float y = t.localPosition.y;
		
		// To Top
		if(obj.name == "Control Point 0" || obj.name == "Control Point 1" || obj.name == "Control Point 2"){
			int height = img.height + (int) dragVector.y;
			if(height > minHeightLimit){
				img.height = height;
				float ty = y + dragVector.y / 2;
				float cpy = y + (controlPointHeight + img.height + dragVector.y) / 2;
				SetY(img.gameObject, ty);
				SetY(controlPoint[7], ty);
				SetY(controlPoint[0], cpy);
				SetY(controlPoint[1], cpy);
				SetY(controlPoint[2], cpy);
				SetY(controlPoint[3], ty);
			}
		}
		
		// To Right
		if(obj.name == "Control Point 2" || obj.name == "Control Point 3" || obj.name == "Control Point 4"){
			int width = img.width + (int) dragVector.x;
			
			//			int width = img.width + (int)((Camera.main.ScreenToWorldPoint(Input.mousePosition).x - tempX)*250.0032134847016f);
			
			if(width > minWidthLimit){
				img.width = width;
				float tx = x + dragVector.x / 2;			
				float cpx = x + (controlPointWidth + img.width + dragVector.x) / 2;
				SetX(img.gameObject, tx);
				SetX(controlPoint[1], tx);
				SetX(controlPoint[2], cpx);
				SetX(controlPoint[3], cpx);
				SetX(controlPoint[4], cpx);
				SetX(controlPoint[5], tx);
			}
			
			//			tempX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
		}
		
		// To Bottom
		if(obj.name == "Control Point 4" || obj.name == "Control Point 5" || obj.name == "Control Point 6"){
			int height = img.height - (int) dragVector.y;
			if(height > minHeightLimit){
				img.height = height;
				float ty = y + dragVector.y / 2;
				float cpy = y - (controlPointHeight + img.height + dragVector.y) / 2;
				SetY(img.gameObject, ty);
				SetY(controlPoint[3], ty);
				SetY(controlPoint[4], cpy);
				SetY(controlPoint[5], cpy);
				SetY(controlPoint[6], cpy);
				SetY(controlPoint[7], ty);
			}
		}
		
		// To Left
		if(obj.name == "Control Point 6" || obj.name == "Control Point 7" || obj.name == "Control Point 0"){
			int width = img.width - (int) dragVector.x;
			if(width > minWidthLimit){
				img.width = width;
				float tx = x + dragVector.x / 2;
				float cpx = x - (controlPointWidth + img.width + dragVector.x) / 2;
				SetX(img.gameObject, tx);
				SetX(controlPoint[5], tx);
				SetX(controlPoint[6], cpx);
				SetX(controlPoint[7], cpx);
				SetX(controlPoint[0], cpx);
				SetX(controlPoint[1], tx);
			}
		}
	}
	
	public void SetControlEnable(bool value){
		controlEnable = value;
		foreach (GameObject obj in controlPoint) {
			obj.SetActive(value);
		}
	}
	
	public void SetDepth(int value){
		img.depth = value;
		foreach (GameObject obj in controlPoint) {
			obj.GetComponent<UITexture>().depth = value;
		}
	}
	
	void SetX(GameObject obj, float x){
		obj.transform.localPosition =  new Vector3 (x, obj.transform.localPosition.y);
	}
	
	void SetY(GameObject obj, float y){
		obj.transform.localPosition =  new Vector3 (obj.transform.localPosition.x, y);
	}
	
	void SetXY(GameObject obj, float x, float y){
		obj.transform.localPosition =  new Vector3 (x, y);
	}
	
}
