using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MyButton2 : MonoBehaviour {

	public Sprite img;

	void Awake (){

		GameObject g = new GameObject ("My Button", typeof(Image), typeof(EventTrigger));
		
		Transform t = g.transform;
		t.SetParent (transform);
		t.localPosition = Vector3.zero;
		t.localScale = Vector3.one;
		
		Image img2 = g.GetComponent<Image> ();
		img2.sprite = img;
		img2.SetNativeSize ();


		// ------------------------------- OnDrag

		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.Drag;
		entry.callback = new EventTrigger.TriggerEvent();
		entry.callback.AddListener(new UnityAction<BaseEventData>(OnDrag));

		// ------------------------------- OnBeginDrag
				
		EventTrigger.Entry entry2 = new EventTrigger.Entry();
		entry2.eventID = EventTriggerType.BeginDrag;
		entry2.callback = new EventTrigger.TriggerEvent();
		entry2.callback.AddListener(new UnityAction<BaseEventData>(OnBeginDrag));
		
		// ------------------------------- OnEndDrag

		EventTrigger.Entry entry3 = new EventTrigger.Entry();
		entry3.eventID = EventTriggerType.EndDrag;
		entry3.callback = new EventTrigger.TriggerEvent();
		entry3.callback.AddListener(new UnityAction<BaseEventData>(OnEndDrag));
		
		// -------------------------------

		EventTrigger trigger = g.GetComponent<EventTrigger>();
		trigger.delegates = new List<EventTrigger.Entry>();
		trigger.delegates.Add(entry);
		trigger.delegates.Add(entry2);
		trigger.delegates.Add(entry3);
	}

	public void OnDrag(BaseEventData data){
		PointerEventData p = (PointerEventData)data;
		print(p.delta);
	}

	public void OnBeginDrag (BaseEventData eventData){
		print ("--------------- Begin Drag");
	}

	public void OnEndDrag (BaseEventData eventData){
		print ("--------------- End Drag");
	}
	
}
