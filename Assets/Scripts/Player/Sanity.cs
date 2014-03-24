using UnityEngine;
using System.Collections;

public class Sanity : MonoBehaviour {
    public static int MAX_SANITY = 1000;
    public int LossOfSanityPerSteps;

    int sanity;
    public int CurrentSanity
    {
        get { return sanity; }
    }

    void Start()
    {
        sanity = MAX_SANITY;
    }

    public void SubtractSanity()
    {
        sanity -= LossOfSanityPerSteps;
        if (sanity <= 0)
        {
            sanity = 0;
            Destroy(gameObject);
        }
    }
}
