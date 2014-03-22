using UnityEngine;
using System.Collections;

public class CameraMotor : MonoBehaviour {
    public Transform Target;    // The object we focus the camera on (typically, the player)
    public float FollowLength;  // How far from the target are we?

    Transform mTransform;

	void Start () {
        mTransform = transform;
	}
	
	void LateUpdate () {
        mTransform.position = Target.position - (mTransform.forward * FollowLength);
	}
}
