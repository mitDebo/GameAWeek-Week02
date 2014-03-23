using UnityEngine;
using System.Collections;

public class GroceryMotor : MonoBehaviour {
    void Awake()
    {
        transform.rotation = Camera.main.transform.rotation;   
    }
}
