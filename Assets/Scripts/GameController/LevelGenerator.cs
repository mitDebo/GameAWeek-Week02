using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
    public GameObject FloorPrefab;
    public GameObject ShelfPrefab;
    public GameObject WallPrefab;

    public int NumAisles;          // The number of aisles in the store
    public int AisleLength;     // The length of each aisle

    Transform geometryRoot;
    const int aisleWidth = 5;
    const int mainhallHeight = 3;
    const int wallHeight = 3;

    void Start()
    {
        GenerateWorld();
    }

    public void GenerateWorld()
    {
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
                        GameObject wall = Instantiate(WallPrefab) as GameObject;
                        wall.transform.position = new Vector3(x, h + 0.5f, y - 0.5f);
                        wall.transform.localEulerAngles = new Vector3(0f, 180f, 0f);
                        wall.transform.parent = geometryRoot;
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
    }
}
