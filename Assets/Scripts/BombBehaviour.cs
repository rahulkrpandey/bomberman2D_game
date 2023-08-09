using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombBehaviour : MonoBehaviour
{
    public GameObject explode;
    private float originX, originY;
    public Tilemap ground;
    private float burstTime;
    private float time;
    // Start is called before the first frame update
    private void Awake()
    {
        originX = -4.56f; 
	    originY = 4.56f;
    }

    void Start()
    {
        time = 0;
        burstTime = 2;
        ground = FindObjectOfType<Tilemap>(tag == "Ground");
    }

    // Update is called once per frame
    public void BombDetonator()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnCollection += CollectionHandler;
    }

    private void CollectionHandler() {
        //Debug.Log("collected bomb");
    }

    private void OnDisable()
    {
        GameEvents.OnCollection -= CollectionHandler;
    }

    private void FixedUpdate()
    {
        if (gameObject == null) {
            return;
        }

        time += Time.deltaTime;
        if (time >= burstTime) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        Vector2Int pos = PositionToMatrix(transform.position);
        Grid.grid.mat[pos.x, pos.y] = 0;
        GameEvents.OnBomBlast(pos.x, pos.y);
        InstantiateExplosion();
        SoundManager.sm.PlayExplosion();
    }

    private void InstantiateExplosion() {
        Instantiate(explode, transform.position, transform.rotation);
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
