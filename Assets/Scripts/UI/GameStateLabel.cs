using UnityEngine;
using System.Collections;

public class GameStateLabel : MonoBehaviour {
    GameState state;
    UILabel label;

	// Use this for initialization
	void Start () 
    {
        state = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameState>();
        label = GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (state.CurrentState == GameState.PossibleStates.GET_GROCERIES)
            label.text = "Get your groceries";
        if (state.CurrentState == GameState.PossibleStates.CHECKOUT)
            label.text = "Checkout";
        if (state.CurrentState == GameState.PossibleStates.LEAVE)
            label.text = "Leave";
	}
}
