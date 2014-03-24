using UnityEngine;
using System.Collections;

public class CheckoutAisleVisibility : MonoBehaviour {
    // Use this for initialization
	void Start () 
    {
	    // Add the see through component to each child
        int numChildren = transform.childCount;
        for (int i = 0; i < numChildren; i++)
            if (transform.GetChild(i).gameObject.renderer != null)
                transform.GetChild(i).gameObject.AddComponent<SeeThrough>();
	}
}
