using UnityEngine;
using System.Collections;

public class CashRegisterState : MonoBehaviour {
    public GameObject Arrow;

    GameState state;

	// Use this for initialization
	void Start () 
    {
        Arrow.SetActive(false);
        state = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameState>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (state.CurrentState == GameState.PossibleStates.CHECKOUT && !Arrow.activeInHierarchy)
            Arrow.SetActive(true);
        if (state.CurrentState == GameState.PossibleStates.LEAVE && Arrow.activeInHierarchy)
            Arrow.SetActive(false);
	}
}
