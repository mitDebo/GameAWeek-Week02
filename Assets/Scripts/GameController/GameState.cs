using UnityEngine;
using System.Collections;

public class GameState : MonoBehaviour {
    public enum PossibleStates  { GET_GROCERIES, CHECKOUT, LEAVE };

    public PossibleStates CurrentState;
    public ScreenOverlay overlay;

    public static bool AcceptInput;

    InventoryManager inventory;
    int currentWeek;
    GameObject player;

    Vector3 originalPosition;
    Quaternion originalQuat;
    LevelGenerator level;

	// Use this for initialization
	void Start () 
    {
        CurrentState = PossibleStates.GET_GROCERIES;
        inventory = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<InventoryManager>();
        currentWeek = 1;
        AcceptInput = false;
        player = GameObject.FindGameObjectWithTag(Tags.Player) as GameObject;
        originalPosition = player.transform.position;
        originalQuat = player.transform.rotation;

        level = GetComponent<LevelGenerator>();
        level.GenerateWorld();

        StartCoroutine("startGame");
	}
	
	public void DoTransition()
    {
        currentWeek++;
        overlay.UpdateWeek(currentWeek);
        overlay.FadeIn();
        StartCoroutine("makeAllTransition");
    }

    IEnumerator startGame()
    {
        yield return new WaitForSeconds(1.5f);
        overlay.FadeOut();
    }

    IEnumerator makeAllTransition()
    {
        yield return new WaitForSeconds(3.5f);
        player.transform.position = originalPosition;
        player.transform.rotation = originalQuat;
        level.DestroyWorld();
        level.GenerateWorld();
        player.GetComponent<InventoryManager>().Reset();
        CurrentState = PossibleStates.GET_GROCERIES;
        StartCoroutine("pauseThenFadeOut");
    }

    IEnumerator pauseThenFadeOut()
    {
        yield return new WaitForSeconds(1.5f);
        overlay.FadeOut();
    }
}
