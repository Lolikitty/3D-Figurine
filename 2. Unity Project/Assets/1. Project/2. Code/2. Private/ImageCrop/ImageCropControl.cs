/*
 * 
 * The Control-Point Array Key Number Expressed As Follows :
 * 
 *                 0 ------ 1 ------ 2
 *                 |                 |
 *                 7      image      3
 *                 |                 |
 *                 6 ------ 5 ------ 4
 * 
 * 
 */

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class ImageCropControl : MonoBehaviour {
	 
	public Texture2D controlPointImage;
	public GameObject controlPointRoot;
	public int controlPointWidth = 20, controlPointHeight = 20;
	public int minHeightLimit = 50, minWidthLimit = 50;
	[Range(0, 1)]
	public float moveAlpha = 0.5f;
	
	public Texture2D mouseDrag;
	public Texture2D mouseRL;
	public Texture2D mouseTB;
	public Texture2D mouse17CLK;
	public Texture2D mouse115CLK;

	RectTransform imgRT;

	GameObject [] controlPoint = new GameObject[8];
	Vector2 [] ControlPointPosition = new Vector2[8];
	
	public bool controlEnable = true;
	
	public bool lockR = false;
	public float lockValue = 0;

	Texture2D dragTexture;

	bool isFreeControl = false; // Free or Fixed Control Image Scale

	public static Texture2D CROP_TEXTURE;
	
	void Awake () {

		imgRT = GetComponent <RectTransform> ();

		ControlPointInit ();
		SetControlEnable (controlEnable);


		/*
		 * 
		 * You Can Enable You Need Fixed Control Point ,
		 * But Now Only "controlPoint[4]" Can Use , 
		 * Because Have A Little Problem... 
		 * In The FreeControl() Function ...
		 * 
		 */
		if(!isFreeControl){
			controlPoint [0].SetActive (false);
			controlPoint [1].SetActive (false);
			controlPoint [2].SetActive (false);
			controlPoint [3].SetActive (false);
			controlPoint [4].SetActive (true);
			controlPoint [5].SetActive (false);
			controlPoint [6].SetActive (false);
			controlPoint [7].SetActive (false);
		}
	}

	void Start(){
		dragTexture = new Texture2D (2, 2);
		for(int y=0; y < dragTexture.height; y++){
			for(int x = 0; x<dragTexture.width; x++){
				dragTexture.SetPixel (x, y, new Color (1, 1, 1, 0.15f));
			}
		}
		dragTexture.Apply ();
	}

	void Update(){
		if (Input.GetMouseButtonUp (0)) {
			Refresh ();
		}
	}
	
	void ControlPointInit(){

		for(int i = 0; i < controlPoint.Length; i++){
			controlPoint[i] = new GameObject("Control Point " + i, typeof(RectTransform), typeof(CanvasRenderer), typeof(RawImage), typeof(EventTrigger));

			// ------------------------------- OnDrag
			
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Drag;
			entry.callback.AddListener(OnDrag_ControlPoint);

			// ------------------------------- OnBeginDrag
			
			EventTrigger.Entry entry2 = new EventTrigger.Entry();
			entry2.eventID = EventTriggerType.BeginDrag;
			entry2.callback.AddListener(OnBeginDrag_ControlPoint);
			
			// ------------------------------- OnEndDrag
			
			EventTrigger.Entry entry3 = new EventTrigger.Entry();
			entry3.eventID = EventTriggerType.EndDrag;
			entry3.callback.AddListener(OnEndDrag_ControlPoint);

			// ----------------------------------- Add EventTrigger

			EventTrigger trigger = controlPoint[i].GetComponent<EventTrigger>();
			trigger.delegates = new List<EventTrigger.Entry>();
			trigger.delegates.Add(entry);
			trigger.delegates.Add(entry2);
			trigger.delegates.Add(entry3);
			// ----------------------------------- OnEnter

			EventTriggerListener.Get(controlPoint[i]).onEnter = ControlPointEnter;

			// -----------------------------------

			Transform transform = controlPoint[i].transform;
			transform.parent = controlPointRoot.transform;
			transform.localScale = Vector3.one;

			RawImage img = controlPoint[i].GetComponent<RawImage>();
			img.texture = controlPointImage;

			RectTransform rt = controlPoint[i].GetComponent<RectTransform>();
			rt.anchorMin= Vector2.zero;
			rt.anchorMax= Vector2.zero;
			rt.pivot = Vector2.zero;
			rt.sizeDelta = new Vector2(controlPointWidth, controlPointHeight);

			EventTrigger et = controlPoint[i].GetComponent<EventTrigger>();

		}
	}

	public void ControlPointPositionRefresh(){

		float X = imgRT.anchoredPosition.x;
		float Y = imgRT.anchoredPosition.y;

		ControlPointPosition [0] = new Vector2 (- controlPointWidth, imgRT.sizeDelta.y);
		ControlPointPosition [1] = new Vector2 (imgRT.sizeDelta.x / 2 - controlPointWidth/2, imgRT.sizeDelta.y);
		ControlPointPosition [2] = new Vector2 (imgRT.sizeDelta.x, imgRT.sizeDelta.y);
		ControlPointPosition [3] = new Vector2 (imgRT.sizeDelta.x, imgRT.sizeDelta.y/2- controlPointHeight/2);
		ControlPointPosition [4] = new Vector2 (imgRT.sizeDelta.x, - controlPointHeight);
		ControlPointPosition [5] = new Vector2 (imgRT.sizeDelta.x/2- controlPointWidth/2, - controlPointHeight);
		ControlPointPosition [6] = new Vector2 (- controlPointWidth, - controlPointHeight);
		ControlPointPosition [7] = new Vector2 (- controlPointWidth, imgRT.sizeDelta.y/2- controlPointHeight/2);


		for(int i = 0; i < controlPoint.Length; i++){
			controlPoint[i].GetComponent<RectTransform>().anchoredPosition = ControlPointPosition[i];
		}

		//-------------------------------

		Refresh ();
	}

	float tempDragMouseX;
	float tempDragMouseY;

	bool initDrag = true;

	public void OnUp(){
		initDrag = true;
	}



	public void OnBeginDrag(){
		RawImage ri = GetComponent<RawImage> ();
//		ri.color = new Color (0,0,0,0);
		ri.texture = dragTexture;
	}

	public void OnEndDrag(){
//		RawImage ri = GetComponent<RawImage> ();
//		ri.color = new Color (1,1,1,1);
	}

	Texture2D newImg;

	public void OnDrag(){
		if(!controlEnable){
			return;
		}

		if(initDrag){
			initDrag = false;
			tempDragMouseX = Input.mousePosition.x;
			tempDragMouseY = Input.mousePosition.y;
		}

		float dragX = Input.mousePosition.x - tempDragMouseX;
		float dragY = Input.mousePosition.y - tempDragMouseY;

		float x = imgRT.anchoredPosition.x + dragX;
		float y = imgRT.anchoredPosition.y + dragY;

		imgRT.anchoredPosition = new Vector2 (x, y);

		tempDragMouseX = Input.mousePosition.x;
		tempDragMouseY = Input.mousePosition.y;

		//---------------------------------------------

//		Refresh ();

	}

	float offsetT = 0;
	float offsetB = 0;
	float offsetR = 0;
	float offsetL = 0;

	public void Refresh(){

		if (ImageCrop.DOWNLOAD_IMG == null)
			return;

		RawImage ri = GetComponent<RawImage> ();
		
		if (newImg != null) {
			Destroy (newImg);
		}

		int x = (int)imgRT.anchoredPosition.x;
		int y = (int)imgRT.anchoredPosition.y;
		int w = (int)imgRT.sizeDelta.x;
		int h = (int)imgRT.sizeDelta.y;

		int offsetX = Screen.width / 2 - ImageCrop.RESCALE_IMG.width / 2;
		int offsetX2 = ImageCrop.RESCALE_IMG.width - w;

		newImg = new Texture2D (w, h, TextureFormat.RGBA32, false);

		float reScale = (float) Screen.width / Screen.height;

		for (int yy = 0; yy < h; yy++) {
			for(int xx = 0; xx < w; xx++) {
//				if(x + xx - offsetX < 0 || x + xx - offsetX > w-1 || y + yy < 0 || y + yy > h){
//					newImg.SetPixel(xx, yy, new Color(0,0,0,0));
//				}else{
//
//				}
				newImg.SetPixel(xx, yy, ImageCrop.RESCALE_IMG.GetPixel(x + xx - offsetX, y + yy));
			}
		}
		
		newImg.Apply (false, false);
		
		ri.texture = newImg;
		CROP_TEXTURE = newImg;
	}

	// Change Mouse icon
	public void OnEnter(){
//		if(!controlEnable || isControlPointEnter){
//			return;
//		}
//		Cursor.SetCursor (mouseDrag, new Vector2(16,16), CursorMode.Auto);
	}

	// Change Mouse icon
	public void OnExit(){
		if(!controlEnable){
			return;
		}
		Cursor.SetCursor (null, new Vector2(16,16), CursorMode.Auto);
//		Refresh ();
	}
	
	// Change Control-Point icon
	void ControlPointEnter(GameObject obj){
		switch (obj.name) {
		case "Control Point 3" :
		case "Control Point 7" :
			Cursor.SetCursor (mouseRL, new Vector2(16,16), CursorMode.Auto);
			break;
		case "Control Point 1" :
		case "Control Point 5" :
			Cursor.SetCursor (mouseTB, new Vector2(16,16), CursorMode.Auto);
			break;
		case "Control Point 2" :
		case "Control Point 6" :
			Cursor.SetCursor (mouse17CLK, new Vector2(16,16), CursorMode.Auto);
			break;
		case "Control Point 0" :
		case "Control Point 4" :
			Cursor.SetCursor (mouse115CLK, new Vector2(16,16), CursorMode.Auto);
			break;
		}
		
	}

	public void OnBeginDrag_ControlPoint (BaseEventData eventData){
		if(!controlEnable){
			return;
		}
		RawImage ri = GetComponent<RawImage> ();
		ri.texture = dragTexture;
	}
	
	public void OnEndDrag_ControlPoint (BaseEventData eventData){
		if(!controlEnable){
			return;
		}
		//		obj.GetComponent<UITexture> ().alpha = 1;
		//		foreach(GameObject cp in controlPoint){
		//			cp.GetComponent<UITexture> ().alpha = 1;
		//		}
	}
	
	void OnDrag_ControlPoint(BaseEventData data){

		if (isFreeControl) {
			FreeControl (data);
		} else {
			FixedControl (data);
		}

	}

	void FixedControl(BaseEventData data){
		PointerEventData p = (PointerEventData)data;
		Vector2 dragVector = p.delta;
		
		GameObject obj = p.pointerPress;
		
		RectTransform rt = GetComponent <RectTransform>();
		
		float x = rt.anchoredPosition.x;
		float y = rt.anchoredPosition.y;
		int w = (int) rt.sizeDelta.x;
		int h = (int) rt.sizeDelta.y;

		// To Right Down
		if(obj.name == "Control Point 4"){
			float width = imgRT.sizeDelta.x + dragVector.x;		
			if(width > minWidthLimit){
				Vector2 delta = new Vector2(p.delta.x, 0);
				Vector2 deltaMove = delta;
				imgRT.sizeDelta += delta;
				controlPoint[4].GetComponent<RectTransform>().anchoredPosition += deltaMove;
			}

			float height = imgRT.sizeDelta.y + dragVector.y;
			if(height > minHeightLimit){
				Vector2 delta = new Vector2(0, - p.delta.y);
				Vector2 deltaMove = delta;
				imgRT.sizeDelta += delta;
				imgRT.anchoredPosition -= deltaMove;
			}
		}
	}

	void FreeControl(BaseEventData data){
		PointerEventData p = (PointerEventData)data;
		Vector2 dragVector = p.delta;
		
		GameObject obj = p.pointerPress;
		
		RectTransform rt = GetComponent <RectTransform>();
		
		float x = rt.anchoredPosition.x;
		float y = rt.anchoredPosition.y;
		int w = (int) rt.sizeDelta.x;
		int h = (int) rt.sizeDelta.y;
		
		// To Top
		if(obj.name == "Control Point 0" || obj.name == "Control Point 1" || obj.name == "Control Point 2"){
			float height = imgRT.sizeDelta.y + dragVector.y;
			if(height > minHeightLimit){
				Vector2 delta = new Vector2(0, p.delta.y);
				Vector2 deltaMove = delta;
				imgRT.sizeDelta += delta;
				// imgRT.anchoredPosition += 0;
				// Top
				controlPoint[0].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				controlPoint[1].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				controlPoint[2].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				// Y Center
				controlPoint[3].GetComponent<RectTransform>().anchoredPosition += deltaMove/2;
				controlPoint[7].GetComponent<RectTransform>().anchoredPosition += deltaMove/2;
			}
		}
		
		// To Right
		if(obj.name == "Control Point 2" || obj.name == "Control Point 3" || obj.name == "Control Point 4"){
			float width = imgRT.sizeDelta.x + dragVector.x;		
			if(width > minWidthLimit){
				Vector2 delta = new Vector2(p.delta.x, 0);
				Vector2 deltaMove = delta;
				imgRT.sizeDelta += delta;
				// imgRT.anchoredPosition += 0;
				// Right
				controlPoint[2].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				controlPoint[3].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				controlPoint[4].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				// X Center
				controlPoint[1].GetComponent<RectTransform>().anchoredPosition += deltaMove/2;
				controlPoint[5].GetComponent<RectTransform>().anchoredPosition += deltaMove/2;
			}
		}
		
		// To Bottom
		if(obj.name == "Control Point 4" || obj.name == "Control Point 5" || obj.name == "Control Point 6"){
			float height = imgRT.sizeDelta.y + dragVector.y;
			if(height > minHeightLimit){
				Vector2 delta = new Vector2(0, - p.delta.y);
				Vector2 deltaMove = delta;
				imgRT.sizeDelta += delta;
				imgRT.anchoredPosition -= deltaMove;
				// Top
				controlPoint[0].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				controlPoint[1].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				controlPoint[2].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				// Y Center
				controlPoint[3].GetComponent<RectTransform>().anchoredPosition += deltaMove/2;
				controlPoint[7].GetComponent<RectTransform>().anchoredPosition += deltaMove/2;
			}
		}
		
		// To Left
		if(obj.name == "Control Point 6" || obj.name == "Control Point 7" || obj.name == "Control Point 0"){
			float width = imgRT.sizeDelta.x + dragVector.x;		
			if(width > minWidthLimit){
				Vector2 delta = new Vector2( - p.delta.x, 0);
				Vector2 deltaMove = delta;
				imgRT.sizeDelta += delta;
				imgRT.anchoredPosition -= delta;
				// Right
				controlPoint[2].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				controlPoint[3].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				controlPoint[4].GetComponent<RectTransform>().anchoredPosition += deltaMove;
				// X Center
				controlPoint[1].GetComponent<RectTransform>().anchoredPosition += deltaMove/2;
				controlPoint[5].GetComponent<RectTransform>().anchoredPosition += deltaMove/2;
			}
		}
		// Refresh ();
	}
	
	public void SetControlEnable(bool value){
		controlEnable = value;
//		foreach (GameObject obj in controlPoint) {
//			obj.SetActive(value);
//		}
	}

}
