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
    InventoryManager inventory;
    bool isMoving;

    const float LEAN_DEPTH = 10;

    void Start()
    {
        mTransform = transform;
        input = GetComponent<PlayerInput>();
        controller = GetComponent<CharacterController>();
        inventory = GetComponent<InventoryManager>();
        isMoving = false;
    }

    void FixedUpdate()
    {
        if (canMoveInDirection(input.MovementVector))
        {
            // If we're not already moving and there's input, move
            if (!isMoving && input.MovementVector != Vector3.zero)
                StartCoroutine("move", input.MovementVector);
        }
        else 
        {
            if (input.MovementVector != Vector3.zero)
            {
                // Even if we can't go in that direction, face the way the player inputs
                mTransform.forward = input.MovementVector;
            }
            
            // We can't move this way, huh? Attempt to pick up an item in this direction then
            inventory.Pickup(input.MovementVector);            
        }
        
        // If we're not moving, leaned over, and there's no input, stand back straight
        if (!isMoving && mTransform.localEulerAngles.x > 0)
            standStraight();
    }

    bool canMoveInDirection(Vector3 direction)
    {
        Ray ray = new Ray(new Vector3(mTransform.position.x, 0.5f, mTransform.position.z), direction);
        if (Physics.Raycast(ray, 1f))
            return false;
        return true;
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
            if (moveVector.z < 0)
                newPosition.z = Mathf.Clamp(newPosition.z, destinationPosition.z, mTransform.position.z);
            else if (moveVector.z > 0)
                newPosition.z = Mathf.Clamp(newPosition.z, mTransform.position.z, destinationPosition.z);

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
        if (mTransform.localEulerAngles.x > 0)
        {
            float newLean = Mathf.Clamp(mTransform.localEulerAngles.x - localLeanSpeed * Time.deltaTime, 0f, LEAN_DEPTH);
            mTransform.localRotation = Quaternion.Euler(new Vector3(newLean, mTransform.localEulerAngles.y, mTransform.localEulerAngles.z));
        }
    }
}
