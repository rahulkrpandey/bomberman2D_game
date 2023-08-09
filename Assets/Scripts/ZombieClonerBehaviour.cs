using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ZombieClonerBehaviour : MonoBehaviour
{
    public Tilemap ground;
    public Tilemap collisionGround;
    public GameObject zombie;
    private float originX, originY;

    private void Start()
    {
        //originX = -4.5f;
        //originY = 4.5f;
        originX = -4.5f;
        originY = 4.64f;
        transform.position = new Vector3(originX, originX, 0);
        int count = 5;

        System.Random rnd = new System.Random();
        while (count > 0) {
            int x = rnd.Next(-10, 1);
            int y = rnd.Next(0, 11);
            
            if (InstantiateZombies(y, x)) {
                count--;
            }
        }
    }

    private bool InstantiateZombies(int i, int j) {
        Vector3 position = new(originX + i, originY + j);
        Vector2Int matrixPos = PositionToMatrix(position);

        if (Grid.grid.mat[matrixPos.x, matrixPos.y] == 0 || (matrixPos.x >= 0 && matrixPos.x < 4 && matrixPos.y >= 0 && matrixPos.y < 4)) {
            if ((matrixPos.x == 0 || matrixPos.x == 1) && (matrixPos.y == 0 || matrixPos.y == 1)) {
                return false;
            }

            int rand = Mathf.RoundToInt(Random.Range(0f, 1f));
            if (rand == 0) {
                return false;
            }

            Instantiate(zombie, position, transform.rotation);
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
