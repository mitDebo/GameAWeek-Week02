using UnityEngine;
using System.Collections;

/**
 * This component handles all the input logic for the character.
 * Might be a little overkill to separate the logic out this much, but it's
 * clean(ish) code
 */
public class PlayerInput : MonoBehaviour {
    public Vector3 MovementVector;

    void Update()
    {
        MovementVector = Vector3.zero;
        // Update the movement vector based off the input
        if (Input.GetKey(KeyCode.LeftArrow))
            MovementVector.x -= 1;
        if (Input.GetKey(KeyCode.RightArrow))
            MovementVector.x += 1;
        if (MovementVector == Vector3.zero)
        {
            if (Input.GetKey(KeyCode.UpArrow))
                MovementVector.z += 1;
            if (Input.GetKey(KeyCode.DownArrow))
                MovementVector.z -= 1;
        }
    }
}
