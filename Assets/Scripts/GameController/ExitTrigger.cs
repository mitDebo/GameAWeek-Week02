using UnityEngine;
using System.Collections;

public class ExitTrigger : MonoBehaviour {
    GameState state;
    GameObject player;

    void Start()
    {
        state = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameState>();
        player = GameObject.FindGameObjectWithTag(Tags.Player).gameObject;
    }
    
    void OnTriggerEnter(Collider col)
    {
        if (state.CurrentState == GameState.PossibleStates.LEAVE && col.gameObject == player)
            state.DoTransition();
    }
}
