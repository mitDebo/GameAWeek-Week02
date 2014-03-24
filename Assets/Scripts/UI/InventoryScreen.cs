using UnityEngine;
using System.Collections;

public class InventoryScreen : MonoBehaviour {
    public GameObject InventoryWindow;

    InventoryUpdater updater;

	// Use this for initialization
	void Start () 
    {
        InventoryWindow.SetActive(false);
        updater = InventoryWindow.GetComponent<InventoryUpdater>();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryWindow.activeInHierarchy)
                InventoryWindow.SetActive(false);
            else
            {
                InventoryWindow.SetActive(true);
                updater.UpdateList();
            }
        }
	}
}
