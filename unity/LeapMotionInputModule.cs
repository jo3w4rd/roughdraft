using System.Text;
using  System.Collections.Generic;
using Leap;

namespace UnityEngine.EventSystems
{
	//Closely based on Touch Input module, uses Leap Motion API touch emulation
	[AddComponentMenu("Event/Leap Motion Input Module")]
	public class LeapMotionInputModule : TouchInputModule
	{
		protected LeapMotionInputModule()
		{}
		
		private Controller controller = new Controller();
		private Vector2 m_LastMousePosition;
		private Vector2 m_MousePosition;
		
		protected Dictionary<int, PointerEventData> m_PointerData = new Dictionary<int, PointerEventData> ();
		
		[SerializeField]
		private bool m_AllowActivationOnStandalone;
		
		public bool allowActivationOnStandalone
		{
			get { return m_AllowActivationOnStandalone; }
			set { m_AllowActivationOnStandalone = value; }
		}
		
		public override void UpdateModule()
		{
			m_LastMousePosition = m_MousePosition;
			m_MousePosition = Input.mousePosition;
		}
		
		public override bool IsModuleSupported()
		{
			return controller.IsServiceConnected();
		}
		
		public override bool ShouldActivateModule()
		{
			if (!base.ShouldActivateModule ())
				return false;
			
			if (controller.Frame().Pointables.Count > 0 ) return true;

			//else
			return false;
		}
				
		public override void Process()
		{
			//Create or update pointer data
			foreach(Pointable pointable in controller.Frame().Pointables){
				bool pressed = false;
				bool released = false;
				Pointable previous = controller.Frame (1).Pointable(pointable.Id);
				PointerEventData pointData = GetPointerEventDataForPointable(pointable);
				if(pointable.TouchDistance >0 && previous.TouchDistance <= 0) pressed = true;	
				if(pointable.TouchDistance <= 0 && previous.TouchDistance > 0) released = true;	
				//ProcessTouchPress(pointData, pressed, released);
			}

			//Remove any pointables from last frame that are no longer valid
			//foreach(Pointable pointable in controller.Frame(1).Pointables){
			//	if(!controller.Frame(0).Pointable(pointable.Id).IsValid) m_PointerData.Remove ((pointable.Id));	
			//}
			foreach( KeyValuePair<int, PointerEventData> kvp in m_PointerData )
			{
				if(!controller.Frame(1).Pointable(kvp.Key).IsValid) m_PointerData.Remove (kvp.Key);
			}

			//Now process the pointers
			//foreach( KeyValuePair<int, PointerEventData> kvp in m_PointerData )
			//{
				//Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
			//	Debug.Log (this.ToString());
			//}
		}
						
		private PointerEventData GetPointerEventDataForPointable(Pointable pointable){
			PointerEventData data;
			Pointable previous = controller.Frame (1).Pointable(pointable.Id);
			if (!m_PointerData.TryGetValue (pointable.Id, out data))
			{
				data = new PointerEventData (eventSystem);
				//data.id = pointable.Id;
				data.position = Vector2.zero;
				data.delta = Vector2.zero;
				m_PointerData.Add (pointable.Id, data);
			} else {
				InteractionBox ibox = controller.Frame ().InteractionBox;
				Leap.Vector normalizedTipPosition = ibox.NormalizePoint(pointable.TipPosition); 
				Vector2 currentPosition = new Vector2(Screen.width  * normalizedTipPosition.x, 
				                                      Screen.height * (1 - normalizedTipPosition.y));
				data.delta = data.position - currentPosition;
				data.position = currentPosition;
				//if(pointable.TouchDistance >0 && previous.TouchDistance <= 0){
				//	data.eligibleForClick = true;
				//	data.delta = Vector2.zero;
				//	data.pressPosition = data.position;
				//	data.pointerPressRaycast = data.pointerCurrentRaycast;
					
				//}
			}
			
			
			eventSystem.RaycastAll (data, m_RaycastResultCache);
			
			var raycast = FindFirstRaycast (m_RaycastResultCache);
			data.pointerCurrentRaycast = raycast;
			m_RaycastResultCache.Clear ();
			
			return data;
			
		}
/*
		private void ProcessTouchPress(PointerEventData pointerEvent, bool pressed, bool released)
		{
			GameObject currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;
			
			// PointerDown notification
			if (pressed)
			{
				pointerEvent.eligibleForClick = true;
				pointerEvent.delta = Vector2.zero;
				pointerEvent.pressPosition = pointerEvent.position;
				pointerEvent.pointerPressRaycast = pointerEvent.pointerCurrentRaycast;
				
				if (pointerEvent.pointerEnter != currentOverGo)
				{
					// send a pointer enter to the touched element if it isn't the one to select...
					HandlePointerExitAndEnter (pointerEvent, currentOverGo);
					pointerEvent.pointerEnter = currentOverGo;
				}
				
				// search for the control that will receive the press
				// if we can't find a press handler set the press 
				// handler to be what would receive a click.
				var newPressed = ExecuteEvents.ExecuteHierarchy (currentOverGo, pointerEvent, ExecuteEvents.pointerDownHandler);
				
				// didnt find a press handler... search for a click handler
				if (newPressed == null)
					newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler> (currentOverGo);
				
				//Debug.Log("Pressed: " + newPressed);
				
				if (newPressed != pointerEvent.pointerPress)
				{
					pointerEvent.pointerPress = newPressed;
					pointerEvent.rawPointerPress = currentOverGo;
					pointerEvent.clickCount = 0;
				}
				
				// Save the drag handler as well
				pointerEvent.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler> (currentOverGo);
				
				if (pointerEvent.pointerDrag != null)
					ExecuteEvents.Execute<IBeginDragHandler> (pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
				
				//	Debug.Log("Setting Drag Handler to: " + pointer.pointerDrag);
				
				// Selection tracking
				GameObject selectHandlerGO = ExecuteEvents.GetEventHandler<ISelectHandler> (currentOverGo);
				eventSystem.SetSelectedGameObject (selectHandlerGO, pointerEvent);
			}
			
			// PointerUp notification
			if (released)
			{
				//Debug.Log("Executing pressup on: " + pointer.pointerPress);
				ExecuteEvents.Execute (pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);
				
				//Debug.Log("KeyCode: " + pointer.eventData.keyCode);
				
				// see if we mouse up on the same element that we clicked on...
				GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler> (currentOverGo);
				
				// PointerClick and Drop events
				if (pointerEvent.pointerPress == pointerUpHandler && pointerEvent.eligibleForClick)
				{
					float time = Time.unscaledTime;
					
					if (time - pointerEvent.clickTime < 0.3f)
						++pointerEvent.clickCount;
					else
						pointerEvent.clickCount = 1;
					pointerEvent.clickTime = time;
					
					ExecuteEvents.Execute (pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerClickHandler);
				}
				else if (pointerEvent.pointerDrag != null)
				{
					ExecuteEvents.ExecuteHierarchy (currentOverGo, pointerEvent, ExecuteEvents.dropHandler);
				}
				
				pointerEvent.eligibleForClick = false;
				pointerEvent.pointerPress = null;
				pointerEvent.rawPointerPress = null;
				
				if (pointerEvent.pointerDrag != null)
					ExecuteEvents.Execute (pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.endDragHandler);
				
				pointerEvent.pointerDrag = null;
				
				// send exit events as we need to simulate this on touch up on touch device
				ExecuteEvents.ExecuteHierarchy (pointerEvent.pointerEnter, pointerEvent, ExecuteEvents.pointerExitHandler);
				pointerEvent.pointerEnter = null;
			}
		}
*/
		
		public override void DeactivateModule()
		{
			base.DeactivateModule ();
			ClearSelection ();
			m_PointerData.Clear ();
		}
		
		public override string ToString()
		{
			var sb = new StringBuilder ();
			foreach (var pointerEventData in m_PointerData)
				sb.AppendLine (pointerEventData.ToString ());
			return sb.ToString ();
		}
	}
}