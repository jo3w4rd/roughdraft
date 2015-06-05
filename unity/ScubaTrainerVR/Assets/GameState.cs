using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {
	public enum State {
		NO_STATE,
		INFLATE_BCD_ENTIRELY,
		HOLD_DURING_GIANT_STRIDE,
		SIGNAL_DESCENT,
		VENT_BCD_A_BIT_TO_START_DESCENT,
		YOURE_AT_THE_BOTTOM,
		SWIM_AROUND_FOR_A_BIT,
		VENT_AIR_FREQUENTLY_DURING_ASCENT,
		YOURE_AT_THE_SURFACE,
		COMPLETE,
		FAILURE
	};

	private Dictionary<State, string> stateMessages = new Dictionary<State, string>	{
		{State.INFLATE_BCD_ENTIRELY,
			"Welcome to Scuba Diving Crash Course for Leap Motion VR! Start off by inflating your BCD completely."},
		{State.HOLD_DURING_GIANT_STRIDE, 
			"Hold the regulator in your mouth to keep it still during the giant stride entrance to deep water"},
		{State.SIGNAL_DESCENT, 
			"Signal “descent” to your buddies by pointing your thumb downwards"},
		{State.VENT_BCD_A_BIT_TO_START_DESCENT, 
			"Vent your BCD a bit to start descending using the Low Pressure Inflator (LPI), but be ready to inflate it frequently as the water pressure and air density increases"},
		{State.YOURE_AT_THE_BOTTOM, 
			"You’re at the bottom! Signal “ascent” by holding your thumb upwards to start ascending with a small kick with your fins."},
		{State.SWIM_AROUND_FOR_A_BIT, 
			"Swim around for a bit. Become comfortable with reading the air, depth, and compass on your dive computer or dive gauge."},
		{State.VENT_AIR_FREQUENTLY_DURING_ASCENT, 
			"Vent air frequently during your ascent as the air expands while the pressure decreases"},
		{State.YOURE_AT_THE_SURFACE,
			"You’re at the surface! Inflate your BCD and signal “okay” to your buddies. Also, go practice in real water after finishing diver's handbook!"},
		{State.FAILURE, 
			"Sorry, you lost the game. Please try again after reading diver's handbook"}
	};
	public State state;

	// Use this for initialization
	void Start () {
		state = State.INFLATE_BCD_ENTIRELY;
	}
	
	// Update is called once per frame
	void Update () {
		showCurrentMessage ();
		if (Input.GetKeyDown (KeyCode.A)) {
			AdvanceState ();
		}
	}

	public void AdvanceState (State nextState = State.NO_STATE) {
		if (state != State.FAILURE && state != State.COMPLETE) {
			if (nextState == State.NO_STATE) {
				state += 1;
			} else {
				state = nextState;
			}
		}
	}

	public void showCurrentMessage () {
		try {
			Debug.Log (stateMessages [state]);
		}  catch(KeyNotFoundException e) {
			Debug.Log (state);
		}
	}
}