using UnityEngine;
using System.Collections;

public class UpAndDownArrow : MonoBehaviour {
    public float Top;
    public float Bottom;
    public float Speed;

    Transform mTransform;

	// Use this for initialization
	void Start () 
    {
        mTransform = transform;
        GoTweenChain chain = new GoTweenChain(new GoTweenCollectionConfig().setIterations(-1));
        chain.append(new GoTween(mTransform, Speed, new GoTweenConfig().localPosition(new Vector3(mTransform.localPosition.x, Bottom, mTransform.localPosition.z))));
        chain.append(new GoTween(mTransform, Speed, new GoTweenConfig().localPosition(new Vector3(mTransform.localPosition.x, Top, mTransform.localPosition.z))));        
        chain.play();
	}
}
