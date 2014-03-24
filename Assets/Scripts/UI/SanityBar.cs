using UnityEngine;
using System.Collections;

public class SanityBar : MonoBehaviour {
    Sanity playerSanity;
    UISlider slider;

	// Use this for initialization
	void Start () 
    {
        playerSanity = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<Sanity>();
        slider = GetComponent<UISlider>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        slider.value = ( (float) playerSanity.CurrentSanity / (float) Sanity.MAX_SANITY);
	}
}
