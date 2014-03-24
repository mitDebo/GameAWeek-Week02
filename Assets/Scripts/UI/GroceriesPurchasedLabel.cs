using UnityEngine;
using System.Collections;

public class GroceriesPurchasedLabel : MonoBehaviour {
    InventoryManager inventory;
    UILabel label;

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<InventoryManager>();
        label = GetComponent<UILabel>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory.ShoppingList != null)
            label.text = "Groceries Purchased: " + (inventory.ItemsBought) + " / " + inventory.ShoppingListLength;
    }
}
