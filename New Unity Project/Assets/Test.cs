using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

public class Test : MonoBehaviour{

	public Sprite img;

	void Start () {
		GameObject g = new GameObject ("My Button", typeof(RectTransform), typeof(Image), typeof(Button), typeof(EventTrigger));

		Transform t = g.transform;
		t.SetParent (transform);
		t.localPosition = Vector3.zero;

		Image img2 = g.GetComponent<Image> ();
		img2.sprite = img;
		img2.SetNativeSize ();

//		EventTriggerListener.Get (g).onBeginDrag = OnBeginDrag;
//		EventTriggerListener.Get (g).onDrag = OnDrag;
//		EventTriggerListener.Get (g).onEndDrag = OnEndDrag;


		EventTrigger e = GetComponent<EventTrigger>();

//		e.OnDrag ();

//		Button.ButtonClickedEvent e = new Button.ButtonClickedEvent(); 
//		e.AddListener(Kiss);
//
//		EventTrigger et = g.GetComponent<EventTrigger> ();
//		et.OnDrag = e;

//		g.

//		Button b = g.GetComponent<Button> ();
//		b.onClick = e;

//		GetComponent<EventTrigger> ().OnPointerClick = A;

	}

//	public override void OnPointerClick(PointerEventData eventData){
//
//	}

	public void A(BaseEventData b){
		PointerEventData p = (PointerEventData)b;
		print(p.delta);
//		print ("A");
	}

	public void OnBeginDrag(GameObject g){
		print ("----------- 1");
	}
	
//	public void OnDrag(GameObject g){
//		EventTriggerListener ee =  g.GetComponent<EventTriggerListener> ();
//		print ();
//	}
	
//	public void OnEndDrag(GameObject g){
//		print ("-------------------- 2");
//	}

}
