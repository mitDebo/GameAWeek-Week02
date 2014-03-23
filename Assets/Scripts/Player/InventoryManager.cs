using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {
    public GameObject FloatingTextPrefab;
    
    Transform mTransform;
    List<string> desiredItems;

    void Start()
    {
        mTransform = transform;
        desiredItems = new List<string>();
        desiredItems.Add("Banana");
        desiredItems.Add("Apple");
    }

    public void Pickup(Vector3 direction)
    {
        Ray ray = new Ray(new Vector3(mTransform.position.x, 1.5f, mTransform.position.z), direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f, Layers.GroceriesMask))
        {
            GameObject foundObj = hit.collider.gameObject;
            if (desiredItems.Contains(foundObj.name))
            {
                FireFloatingInventoryText(foundObj.name);    // Fire off the floating text of what we picked up
                desiredItems.Remove(foundObj.name);
                Destroy(foundObj);
            }            
        }
    }

    void FireFloatingInventoryText(string name)
    {
        GameObject floatingText = Instantiate(FloatingTextPrefab) as GameObject;
        floatingText.transform.parent = GameObject.Find("UI Root").transform;
        floatingText.transform.position = new Vector3(mTransform.position.x, mTransform.position.y + 1, mTransform.position.z);
        floatingText.transform.localScale = new Vector3(3f, 3f, 3f);
        floatingText.GetComponent<UILabel>().text = name.ToUpper() + " GET!";
    }
}
