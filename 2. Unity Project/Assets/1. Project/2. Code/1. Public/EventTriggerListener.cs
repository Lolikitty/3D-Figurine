﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger{

	public delegate void VoidDelegate (GameObject go);
	public VoidDelegate onClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUpdateSelect;
	public VoidDelegate onBeginDrag;
	public VoidDelegate onDrag;
	public VoidDelegate onCancel;
	public VoidDelegate onDeselect;
	public VoidDelegate onDrop;
	public VoidDelegate onEndDrag;
	public VoidDelegate onInitializePotentialDrag;
	public VoidDelegate onMove;
	public VoidDelegate onScroll;
	public VoidDelegate onSubmit;


	static public EventTriggerListener Get (GameObject go)
	{
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
		if (listener == null) listener = go.AddComponent<EventTriggerListener>();
		return listener;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if(onClick != null) 	onClick(gameObject);
	}

	public override void OnPointerDown (PointerEventData eventData){
		if(onDown != null) onDown(gameObject);
	}

	public override void OnPointerEnter (PointerEventData eventData){
		if(onEnter != null) onEnter(gameObject);
	}

	public override void OnPointerExit (PointerEventData eventData){
		if(onExit != null) onExit(gameObject);
	}

	public override void OnPointerUp (PointerEventData eventData){
		if(onUp != null) onUp(gameObject);
	}

	public override void OnSelect (BaseEventData eventData){
		if(onSelect != null) onSelect(gameObject);
	}

	public override void OnUpdateSelected (BaseEventData eventData){
		if(onUpdateSelect != null) onUpdateSelect(gameObject);
	}

	public override void OnBeginDrag (PointerEventData eventData){
		if(onBeginDrag != null) onBeginDrag(gameObject);
	}

	public override void OnDrag (PointerEventData eventData){
		if(onDrag != null) onDrag(gameObject);
	}

	public override void OnCancel (BaseEventData eventData){
		if(onCancel != null) onCancel(gameObject);
	}

	public override void OnDeselect (BaseEventData eventData){
		if(onDeselect != null) onDeselect(gameObject);
	}

	public override void OnDrop (PointerEventData eventData){
		if(onDrop != null) onDrop(gameObject);
	}

	public override void OnEndDrag (PointerEventData eventData){
		if(onEndDrag != null) onEndDrag(gameObject);
	}

	public override void OnInitializePotentialDrag (PointerEventData eventData){
		if(onInitializePotentialDrag != null) onInitializePotentialDrag(gameObject);
	}

	public override void OnMove (AxisEventData eventData){
		if(onMove != null) onMove(gameObject);
	}

	public override void OnScroll (PointerEventData eventData){
		if(onScroll != null) onScroll(gameObject);
	}

	public override void OnSubmit (BaseEventData eventData){
		if(onSubmit != null) onSubmit(gameObject);
	}

}
