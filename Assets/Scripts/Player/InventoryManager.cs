using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour {
    public GameObject FloatingTextPrefab;
    public List<string> ShoppingList;
    public int ShoppingListLength;

    Transform mTransform;
    GameState state;
    int itemsBought;
    public int ItemsBought
    {
        get { return itemsBought; }
    }

    void Start()
    {
        mTransform = transform;
        state = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameState>();
    }

    public void Reset()
    {
        itemsBought = 0;
    }

    public void SelectWantedGroceries(List<string> availableGroceries)
    {
        ShoppingList = new List<string>();
        for (int i = 0; i < ShoppingListLength; i++)
        {
            int index = Random.Range(0, availableGroceries.Count);
            ShoppingList.Add(availableGroceries[index]);
            availableGroceries.Remove(availableGroceries[index]);
        }
    }

    public void Pickup(Vector3 direction)
    {
        Debug.Log(ShoppingList.Count);
        Ray ray = new Ray(new Vector3(mTransform.position.x, 1.5f, mTransform.position.z), direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f, Layers.GroceriesMask))
        {
            GameObject foundObj = hit.collider.gameObject;
            if (ShoppingList.Contains(foundObj.name))
            {
                FireFloatingInventoryText(foundObj.name);    // Fire off the floating text of what we picked up
                ShoppingList.Remove(foundObj.name);
                Destroy(foundObj);

                if (ShoppingList.Count == 0)
                {
                    state.CurrentState = GameState.PossibleStates.CHECKOUT;
                    itemsBought = 0;
                }
            }            
        }

        if (state.CurrentState == GameState.PossibleStates.CHECKOUT)
        {
            if (Physics.Raycast(ray, out hit, 1f, Layers.CashRegisterMask))
            {
                GameObject foundObj = hit.collider.gameObject;
                itemsBought++;

                if (itemsBought == ShoppingListLength)
                    state.CurrentState = GameState.PossibleStates.LEAVE;
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
