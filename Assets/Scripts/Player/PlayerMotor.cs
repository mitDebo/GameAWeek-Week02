using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
/**
 * Handles the movement logic from the PlayerInput component
 */
public class PlayerMotor : MonoBehaviour {
    public float MoveSpeed;
    public float LeanSpeed;

    Transform mTransform;
    PlayerInput input;
    CharacterController controller;
    bool isMoving;

    const float LEAN_DEPTH = 10;

    void Start()
    {
        mTransform = transform;
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        isMoving = false;
    }

    void FixedUpdate()
    {
        // If we're not already moving and there's input, move
        if (!isMoving && input.MovementVector != Vector3.zero)
            StartCoroutine("move", input.MovementVector);
        
        // If we're not moving, leaned over, and there's no input, stand back straight
        if (input.MovementVector == Vector3.zero && !isMoving && mTransform.eulerAngles.x > 0)
            standStraight();
    }

    // The sequence of animation to move to a new square
    IEnumerator move(Vector3 moveVector)
    {
        isMoving = true;
        Vector3 destinationPosition = mTransform.position + moveVector;     // Calculate our destination
        
        // Set-up lean
        float currentLean = mTransform.eulerAngles.x;
        mTransform.forward = moveVector;
        mTransform.localRotation = Quaternion.Euler(new Vector3(currentLean, mTransform.eulerAngles.y, mTransform.eulerAngles.z));

        float localMoveSpeed = 1 / MoveSpeed;
        float localLeanSpeed = LEAN_DEPTH / LeanSpeed;

        while (mTransform.position != destinationPosition)
        {
            // First, let's move our character
            Vector3 newPosition = mTransform.position + moveVector * localMoveSpeed * Time.deltaTime;  // Calculate where we are this frame
            // The next few lines make sure we don't overshoot our destination
            if (moveVector.x < 0)
                newPosition.x = Mathf.Clamp(newPosition.x, destinationPosition.x, mTransform.position.x);
            else if (moveVector.x > 0)
                newPosition.x = Mathf.Clamp(newPosition.x, mTransform.position.x, destinationPosition.x);
            if (moveVector.y < 0)
                newPosition.y = Mathf.Clamp(newPosition.y, destinationPosition.y, mTransform.position.y);
            else if (moveVector.x > 0)
                newPosition.y = Mathf.Clamp(newPosition.y, mTransform.position.y, destinationPosition.y);
            mTransform.position = newPosition;

            // And now animate the lean, if needed
            if (mTransform.eulerAngles.x < LEAN_DEPTH)
            {
                float newLean = Mathf.Clamp(mTransform.eulerAngles.x + localLeanSpeed * Time.deltaTime, 0f, LEAN_DEPTH);
                mTransform.localRotation = Quaternion.Euler(new Vector3(newLean, mTransform.eulerAngles.y, mTransform.eulerAngles.z));
            }

            yield return new WaitForFixedUpdate();
        }

        isMoving = false;
    }

    // Try to stand straight; called when not moving but not standing straight
    void standStraight()
    {
        float localLeanSpeed = LEAN_DEPTH / LeanSpeed;
        if (mTransform.eulerAngles.x > 0)
        {
            float newLean = Mathf.Clamp(mTransform.eulerAngles.x - localLeanSpeed * Time.deltaTime, 0f, LEAN_DEPTH);
            mTransform.rotation = Quaternion.Euler(new Vector3(newLean, mTransform.eulerAngles.y, mTransform.eulerAngles.z));
        }
    }
}
