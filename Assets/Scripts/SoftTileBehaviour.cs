using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SoftTileBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private float originX, originY;
    private bool destroyed;
    private Tilemap ground;
    void Start()
    {
        originX = -4.5f;
        originY = 4.5f;
        ground = FindObjectOfType<Tilemap>(CompareTag("Ground"));
        destroyed = false;
    }

    private void OnEnable()
    {
        GameEvents.OnBomBlastInvoke += TileDestroyer;
    }

    private void OnDisable()
    {
        GameEvents.OnBomBlastInvoke -= TileDestroyer;
    }

    private void TileDestroyer(int x, int y) {
        if (destroyed) {
            return;
        }

		Vector2Int pos = PositionToMatrix(transform.position);
		Vector2Int bomb = new(x, y);
		if (Grid.grid.CanDestroy(pos, bomb)) {
		    destroyed = true;
		    GameEvents.OnDestroyTileInvoke();
	        Destroy(gameObject);
		}
    }

    private void OnDestroy()
    {
        if (ground == null) {
            return;
        }

        Vector2Int pos = PositionToMatrix(transform.position);
        Grid.grid.mat[pos.x, pos.y] = 0;
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
