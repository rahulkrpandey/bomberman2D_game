using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SoftTileCloner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject SoftTile;
    public GameObject collectible;
    public GameObject door;
    private bool spawnedDoor;
    private bool collectibleSpawned;
    private float originX, originY;
    public Tilemap ground, collisionGround;

    void Awake()
    {
        originX = -4.5f; 
	    originY = 4.5f;
        transform.position = new Vector3(originX, originX, 0);
        int count = 25;
        spawnedDoor = false;
        
        System.Random rnd = new System.Random();
        collectibleSpawned = false;
        while(count > 0) {
            int x = rnd.Next(-10, 1);
            int y = rnd.Next(0, 11);

            if (InstantiateSoftTile(y, x, count)) {
                count--;
            }
        }
    }

    private bool InstantiateSoftTile(int i, int j, int count) {
        Vector3 position = new(originX + i, originY + j);
        Vector2Int matrixPos = PositionToMatrix(position);

        if (Grid.grid.mat[matrixPos.x, matrixPos.y] == 0) {
            if ((matrixPos.x == 0 || matrixPos.x == 1) && (matrixPos.y == 0 || matrixPos.y == 1)) {
                return false;
            }

            //int rand = Mathf.RoundToInt(Random.Range(0f, 1f));
            System.Random rnd = new System.Random();
            int rand = rnd.Next(0, 2);
            if (rand == 0) {
                return false;
            }

            rand = rnd.Next(1, count+1);
            if (rand == count) {
                if (!spawnedDoor && (count == 1 || count % 2 == 0)) {
                    Instantiate(door, position, transform.rotation);
                    spawnedDoor = true;
                }

                Instantiate(collectible, position, transform.rotation);
            }

            Instantiate(SoftTile, position, transform.rotation);
            Grid.grid.mat[matrixPos.x, matrixPos.y] = 2;

            return true;
        }

        return false;
    }


    Vector2Int PositionToMatrix(Vector3 pos) {
        Vector3Int gridPos = ground.WorldToCell(pos);

        int y = gridPos.x - Grid.grid.gridX;
        int x = gridPos.y - Grid.grid.gridY;
        x *= -1;

        Vector2Int matPos = new(x, y);
        return matPos;
    }
}
