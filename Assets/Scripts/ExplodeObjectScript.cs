using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExplodeObjectScript : MonoBehaviour
{
    float time, dur;
    bool exploded;
    public Tilemap ground;
    public Tilemap collisionGround;
    private GameObject anim;
    private Stack<GameObject> s;

    private void Awake()
    {
        anim = transform.GetChild(0).gameObject;
        s = new Stack<GameObject>();
    }

    void Start()
    {
        time = 0; dur = 1;
        exploded = false;
        Tilemap[] arr = FindObjectsByType<Tilemap>(0);
        if (arr[0].name == "Ground") {
            ground = arr[0];
            collisionGround = arr[1];
        } else {
            ground = arr[1];
            collisionGround = arr[0];
        }

        InstantiateAnim();
    }

    private void InstantiateAnim() {
        Vector2Int pos = PositionToMatrix(transform.position);
        int delta = 1;

        Vector2Int p = new(pos.x-1, pos.y);
        Vector2 worldPos = new(transform.position.x, transform.position.y + delta);
        while(true) {
            if (Grid.grid.PlaceBombAnimation(p, Mathf.Abs(p.x - pos.x))) {
                InstantiateAnimObject(worldPos);
                worldPos.y += delta;
                p.x--;
            } else {
                break;
            }
        }

        p.x = pos.x + 1; p.y = pos.y;
        worldPos.y = transform.position.y - delta;
        worldPos.x = transform.position.x;
        while(true) {
            if (Grid.grid.PlaceBombAnimation(p, Mathf.Abs(p.x - pos.x))) {
                InstantiateAnimObject(worldPos);
                worldPos.y -= delta;
                p.x++;
            } else {
                break;
            }
        }

        p.x = pos.x; p.y = pos.y - 1;
        worldPos.x = transform.position.x - delta;
        worldPos.y = transform.position.y;
        while(true) {
            if (Grid.grid.PlaceBombAnimation(p, Mathf.Abs(p.y - pos.y))) {
                InstantiateAnimObject(worldPos);
                worldPos.x -= delta;
                p.y--;
            } else {
                break;
            }
        }

        p.x = pos.x; p.y = pos.y + 1;
        worldPos.x = transform.position.x + delta;
        worldPos.y = transform.position.y;
        while(true) {
            if (Grid.grid.PlaceBombAnimation(p, Mathf.Abs(p.y - pos.y))) {
                InstantiateAnimObject(worldPos);
                worldPos.x += delta;
                p.y++;
            } else {
                break;
            }
        }

        worldPos.x = transform.position.x;
        worldPos.y = transform.position.y;
        InstantiateAnimObject(worldPos);
    }

    private void InstantiateAnimObject(Vector2 pos) {
        s.Push(Instantiate(anim, pos, transform.rotation));
    }

    // Update is called once per frame
    void Update()
    {
        if (exploded) {
            return;
        }

        if (time >= dur) {
            time = 0;

            while (s.Count > 0) {
                GameObject g = s.Pop();
                Destroy(g);
            }

            Destroy(gameObject);
            exploded = true;
        } else {
            time += Time.deltaTime;
        }
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
