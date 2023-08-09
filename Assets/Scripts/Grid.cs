using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grid : MonoBehaviour
{
    public int[,] mat;
    static public Grid grid;
    public readonly float stepSize = 0.73f;
    public int gridX, gridY;
    private int k;

    private void Awake()
    {
        k = 1;
        mat = new int[11, 11];
        for (int i = 0; i < 11; i++) { 
            for (int j = 0; j < 11; j++) {
                mat[i, j] = 0;
            }
	    }

        // 0 = ground, 1 = solid tile, 2 = soft tile, 3 = bomb/collectibles, 4 = bomberman, 6 = enemy, 
        for (int i = 1; i < 11; i+=2) { 
	        for (int j = 1; j < 11; j+=2) {
                mat[i, j] = 1;
            }
	    }

        gridX = -5; gridY = 4;
        grid = this;
    }

    private void OnEnable()
    {
        GameEvents.OnCollection += CollectionHandler;
    }

    private void CollectionHandler() {
        k++;
    }

    private void OnDisable()
    {
        GameEvents.OnCollection -= CollectionHandler;
    }

    // a is object and b is bomb
    public bool CanDestroy(Vector2Int a, Vector2Int b) {
        for (int x = b.x, count = 0; count <= k && x >= 0; x--, count++) {
            int y = b.y;

            if (mat[x, y] == 1) {
                break;
            }

            if (x == a.x && y == a.y) {
                return true;
            }
        }

        for (int x = b.x, count = 0; count <= k && x < 11; x++, count++) {
            int y = b.y;

            if (mat[x, y] == 1) {
                break;
            }

	        if (x == a.x && y == a.y) {
                return true;
            }
        }

        for (int y = b.y, count = 0; count <= k && y >= 0; y--, count++) {
            int x = b.x;

            if (mat[x, y] == 1) {
                break;
            }

            if (x == a.x && y == a.y) {
                return true;
            }
        }

        for (int y = b.y, count = 0; count <= k && y < 11; y++, count++) {
            int x = b.x;

            if (mat[x, y] == 1) {
                break;
            }

            if (x == a.x && y == a.y) {
                return true;
            }
        }

        return false;
    }

    public bool PlaceBombAnimation(Vector2Int pos, int bombs) {
        if (pos.x >= 0 && pos.x < 11 && pos.y >= 0 && pos.y < 11 && mat[pos.x, pos.y] != 1 && bombs <= k) {
            return true;
        }

        return false;
    }

}
