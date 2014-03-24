using UnityEngine;
using System.Collections;

public class InventoryUpdater : MonoBehaviour {
    public GameObject GroceryListItemPrefab;

    InventoryManager inventory;
    Transform groceryListIteams;

	// Use this for initialization
	void Start () 
    {
        inventory = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<InventoryManager>();
        groceryListIteams = transform.FindChild("GroceryListItems");
	}

    public void UpdateList()
    {
        int childCount = groceryListIteams.childCount;
        for (int i = 0; i < childCount; i++)
            Destroy(groceryListIteams.GetChild(i).gameObject);

        float steps = 0.05f;
        float top = 0.25f;
        for (int i = 0; i < inventory.ShoppingList.Count; i++)
        {
            GameObject listItem = Instantiate(GroceryListItemPrefab) as GameObject;
            listItem.transform.parent = groceryListIteams;
            
            UILabel label = listItem.GetComponent<UILabel>();
            label.text = inventory.ShoppingList[i];
            
            listItem.transform.localPosition = new Vector3(-0.15f, top, 0f);
            listItem.transform.eulerAngles = new Vector3(30f, 0f, 0f);
            
            top -= steps;
        }
    }
}
