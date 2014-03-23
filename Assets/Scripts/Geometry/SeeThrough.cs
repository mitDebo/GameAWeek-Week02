using UnityEngine;
using System.Collections;

public class SeeThrough : MonoBehaviour {
    GameObject player;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag(Tags.Player) as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (player.transform.position.z < -0.25f)
        {
            Color curColor = gameObject.renderer.material.color;
            gameObject.renderer.material.color = new Color(curColor.r, curColor.g, curColor.b, 1f);
        }
        else
        {
            Color curColor = gameObject.renderer.material.color;
            gameObject.renderer.material.color = new Color(curColor.r, curColor.g, curColor.b, 0.25f);
        }
	}
}
