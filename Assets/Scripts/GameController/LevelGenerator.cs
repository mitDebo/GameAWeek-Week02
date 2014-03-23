using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {
    public GameObject FloorPrefab;
    public GameObject ShelfPrefab;
    public GameObject WallPrefab;

    public int NumAisles;           // The number of aisles in the store
    public int AisleLength;         // The length of each aisle
    public int ItemsPerAisle;       // The number of items per aisle - must be 3 + 2n

    Transform geometryRoot;
    const int aisleWidth = 5;
    const int mainhallHeight = 3;
    const int wallHeight = 3;

    GroceryTypes groceriesPrefabs;
    List<string> groceriesInLevel;

    void Start()
    {
        groceriesPrefabs = GetComponent<GroceryTypes>();
        groceriesInLevel = new List<string>();
        GenerateWorld();
        GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<InventoryManager>().SelectWantedGroceries(groceriesInLevel); // Set up the player grocery list
    }

    public void GenerateWorld()
    {
        // First, we generate the geometry
        geometryRoot = new GameObject("GeometryRoot").transform;
        geometryRoot.position = Vector3.zero;

        int totalWidth = NumAisles * aisleWidth;
        int totalHeight = AisleLength + 2 * mainhallHeight;
        // Make the floors and main shelves
        for (int y = 0; y < totalHeight; y++)
        {
            for (int x = 0; x < totalWidth; x++)
            {
                GameObject floor = Instantiate(FloorPrefab) as GameObject;
                floor.transform.position = new Vector3(x, 0f, y);
                floor.transform.parent = geometryRoot;

                if ( (x % aisleWidth == 0 || x % aisleWidth == aisleWidth - 1) &&
                     (y >= mainhallHeight && y < totalHeight - mainhallHeight) )
                {
                    GameObject shelf = Instantiate(ShelfPrefab) as GameObject;
                    shelf.transform.position = new Vector3(x, 0.5f, y);
                    shelf.transform.parent = geometryRoot;
                }
            }
        }

        // Make the back row shelves
        for (int x = 0; x < totalWidth; x++)
        {
            if (x % aisleWidth > 0 && x % aisleWidth < aisleWidth - 1)
            {
                GameObject shelf = Instantiate(ShelfPrefab) as GameObject;
                shelf.transform.position = new Vector3(x, 0.5f, totalHeight);
                shelf.transform.parent = geometryRoot;
            }
            else
            {
                GameObject floor = Instantiate(FloorPrefab) as GameObject;
                floor.transform.position = new Vector3(x, 0f, totalHeight);
                floor.transform.parent = geometryRoot;
            }
        }

        // Now make all the walls
        for (int y = 0; y <= totalHeight; y++)
        {
            for (int x = 0; x < totalWidth; x++)
            {
                // Left walls
                if (x == 0)
                {
                    for (int h = 0; h < wallHeight; h++)
                    {
                        GameObject wall = Instantiate(WallPrefab) as GameObject;
                        wall.transform.position = new Vector3(x - 0.5f, h + 0.5f, y);
                        wall.transform.localEulerAngles = new Vector3(0f, -90f, 0f);
                        wall.transform.parent = geometryRoot;
                    }
                }
                // Right Walls
                if (x == totalWidth - 1)
                {
                    for (int h = 0; h < wallHeight; h++)
                    {
                        GameObject wall = Instantiate(WallPrefab) as GameObject;
                        wall.transform.position = new Vector3(x + 0.5f, h + 0.5f, y);
                        wall.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
                        wall.transform.parent = geometryRoot;
                    }
                }
                // Front walls
                if (y == 0)
                {
                    for (int h = 0; h < wallHeight; h++)
                    {
                        if (x < 2 || x > 10)
                        {
                            GameObject wall = Instantiate(WallPrefab) as GameObject;
                            wall.transform.position = new Vector3(x, h + 0.5f, y - 0.5f);
                            wall.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                            wall.transform.parent = geometryRoot;

                            wall = Instantiate(WallPrefab) as GameObject;
                            wall.AddComponent<SeeThrough>();
                            wall.transform.position = new Vector3(x, h + 0.5f, y - 0.5f);
                            wall.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                            wall.transform.parent = geometryRoot;
                        }
                    }
                }
                // Back walls
                if (y == totalHeight)
                {
                    for (int h = 0; h < wallHeight; h++)
                    {
                        GameObject wall = Instantiate(WallPrefab) as GameObject;
                        wall.transform.position = new Vector3(x, h + 0.5f, y + 0.5f);
                        wall.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                        wall.transform.parent = geometryRoot;
                    }
                }
            }
        }

        // Now, we generate the groceries to pick up
        Transform groceriesRoot = new GameObject("GroceriesRoot").transform;
        List<GameObject> g = new List<GameObject>();
        for (int i = 0; i < groceriesPrefabs.GroceryPrefabs.Length; i++)
            g.Add(groceriesPrefabs.GroceryPrefabs[i]);

        int currentAisle = 0;
        int currentItemInAisle = 0;
        for (int i = 0; i < ItemsPerAisle * NumAisles; i++)
        {
            // First, we get an available grocery item, and remove it from the list of available items
            int index = Random.Range(0, g.Count);
            GameObject grocery = g[index];
            groceriesInLevel.Add(grocery.name);
            g.Remove(grocery);

            // Back row
            if (currentItemInAisle == Mathf.FloorToInt(ItemsPerAisle / 2f))
            {
                for (int k = 1; k < aisleWidth - 1; k++)
                {
                    GameObject curGrocery = Instantiate(grocery) as GameObject;
                    curGrocery.transform.parent = groceriesRoot.transform;
                    curGrocery.transform.position = new Vector3(currentAisle * aisleWidth + k, 1.5f, totalHeight);
                    curGrocery.name = curGrocery.name.Substring(0, curGrocery.name.IndexOf("(Clone)"));
                }
            }
            else if (currentItemInAisle < (ItemsPerAisle / 2f))
            {
                // Left side of aisle
                int lowerBound = currentItemInAisle * (AisleLength / Mathf.FloorToInt(ItemsPerAisle / 2f));
                int upperBound = (currentItemInAisle + 1) * (AisleLength / Mathf.FloorToInt(ItemsPerAisle / 2f));
                for (int k = lowerBound; k < upperBound; k++)
                {
                    GameObject curGrocery = Instantiate(grocery) as GameObject;
                    curGrocery.transform.parent = groceriesRoot.transform;
                    curGrocery.transform.position = new Vector3(currentAisle * aisleWidth, 1.5f, k + mainhallHeight);
                    curGrocery.name = curGrocery.name.Substring(0, curGrocery.name.IndexOf("(Clone)"));
                }
            } 
            else if (currentItemInAisle > (ItemsPerAisle / 2f))
            {
                // Right side of aisle
                int lowerBound = (currentItemInAisle - Mathf.CeilToInt(ItemsPerAisle / 2f)) * (AisleLength / Mathf.FloorToInt(ItemsPerAisle / 2f));
                int upperBound = (currentItemInAisle - Mathf.CeilToInt(ItemsPerAisle / 2f) + 1) * (AisleLength / Mathf.FloorToInt(ItemsPerAisle / 2f));
                for (int k = lowerBound; k < upperBound; k++)
                {
                    GameObject curGrocery = Instantiate(grocery) as GameObject;
                    curGrocery.transform.parent = groceriesRoot.transform;
                    curGrocery.transform.position = new Vector3((currentAisle + 1) * aisleWidth - 1, 1.5f, k + mainhallHeight);
                    curGrocery.name = curGrocery.name.Substring(0, curGrocery.name.IndexOf("(Clone)"));
                }
            }


            currentItemInAisle++;
            if (currentItemInAisle >= ItemsPerAisle)
            {
                currentItemInAisle = 0;
                currentAisle++;
            }
        }
    }
}
