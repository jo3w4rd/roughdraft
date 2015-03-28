using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using  System.Collections.Generic;
using Leap;

public class LeapMotionHandInputModule : StandaloneInputModule {
	public HandController handController;
	public Canvas parentCanvas;

	protected Dictionary<int, PointerEventData> m_PointerData = new Dictionary<int, PointerEventData> ();
	
	
	//public override void UpdateModule()
	//{
	//}
	
	public override bool IsModuleSupported()
	{
		return handController.GetLeapController().IsServiceConnected();
	}
	
	public override bool ShouldActivateModule()
	{	
		if (handController.GetLeapController().Frame().Pointables.Count > 0 ) return true;
		
		//else
		return base.ShouldActivateModule();
	}
	
	protected void UpdatePointerEventData(FingerModel finger){
		PointerEventData data;
		//Look for data in dictionary, create if none
		if (!m_PointerData.TryGetValue (finger.GetLeapFinger().Id, out data))
		{
			data = new PointerEventData (eventSystem);
			data.Reset();
			data.position = Vector2.zero;
			data.delta = Vector2.zero;
			m_PointerData.Add (finger.GetLeapFinger().Id, data);
		} else {
			Vector3 tip = finger.GetTipPosition();
			Vector2 screenTipPos = parentCanvas.worldCamera.WorldToScreenPoint(tip);
			
			data.delta = data.position - screenTipPos;
			data.position = screenTipPos;
		}
		
		data.worldPosition = finger.GetTipPosition();
		eventSystem.RaycastAll (data, m_RaycastResultCache);
		
		var raycast = FindFirstRaycast (m_RaycastResultCache);
		data.pointerCurrentRaycast = raycast;
		m_RaycastResultCache.Clear ();
	}

	protected void CleanOldPointers(){
		int[] toRemove = new int[m_PointerData.Count];
		int f = -1;
		foreach( KeyValuePair<int, PointerEventData> kvp in m_PointerData )
		{
			if(!handController.GetLeapController().Frame().Pointable(kvp.Key).IsValid) {
				HandlePointerExitAndEnter(kvp.Value, null);
				toRemove[++f] = kvp.Key;
			}
		}
		for(;f >= 0;){
			m_PointerData.Remove (toRemove[f--]);
			
		}
	}
	
	private bool SendUpdateEventToSelectedObject() {
		if (eventSystem.currentSelectedGameObject == null)
			return false;
		BaseEventData data = GetBaseEventData ();
		ExecuteEvents.Execute (eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
		return data.used;
	}
	
	public override void Process() {
		CleanOldPointers();
		// send update events if there is a selected object - this is important for InputField to receive keyboard events
		SendUpdateEventToSelectedObject();
		HandModel[] hands = handController.GetAllGraphicsHands();
		foreach(HandModel hand in hands){
			foreach(FingerModel finger in hand.fingers){
				UpdatePointerEventData(finger);
			}
		}
		
		foreach( KeyValuePair<int, PointerEventData> kvp in m_PointerData )
		{
			PointerEventData pointerData = kvp.Value;
			HandlePointerExitAndEnter(pointerData, pointerData.pointerCurrentRaycast.gameObject);
		}
		base.Process();
	}   
}
