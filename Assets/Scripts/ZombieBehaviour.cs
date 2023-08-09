using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class ZombieBehaviour : MonoBehaviour
{
    public Animator anim;
    public Tilemap ground;
    public Tilemap collisionGround;
    private bool isMoving;
    private float timeToMove;

    enum Direction {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    };

    private void Awake()
    {
        //collisionGround = FindObjectOfType<Tilemap>(name == "Top");
        //Debug.Log(ground.WorldToCell(transform.position));
        anim = transform.GetComponent<Animator>();
    }

    private void Start()
    {
        isMoving = false;
        timeToMove = 0.5f;
        Tilemap[] arr = FindObjectsByType<Tilemap>(0);
        if (arr[0].name == "Ground") {
            ground = arr[0];
            collisionGround = arr[1];
        } else {
            ground = arr[1];
            collisionGround = arr[0];
        }
    }

    private void OnEnable()
    {
        GameEvents.OnBomBlastInvoke += DestroySelf;
        //GameEvents.OnCollection += CollectionHandler;
    }

    private void OnDisable()
    {
        GameEvents.OnBomBlastInvoke -= DestroySelf;
        //GameEvents.OnCollection -= CollectionHandler;
    }

    private void OnDestroy()
    {
        SoundManager.sm.PlayHurt();
    }

    //private void CollectionHandler() {
        
    //    Debug.Log("collected zombie");
    //}

    private void DestroySelf(int x, int y)
    {
        Vector2Int pos = PositionToMatrix(transform.position);
        Vector2Int bomb = new(x, y);
        if (Grid.grid.CanDestroy(pos, bomb)) {
            GameEvents.OnDestroyZombieInvoke();
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (isMoving) {
            return;
        }

        Direction dir = Direction.UP;
        System.Random rnd = new System.Random();
        foreach(Direction x in Enum.GetValues(typeof(Direction))) {
            int rand = rnd.Next(0, 2);
            if (rand == 1) {
                dir = x;
            }
        }

        if (CanMove (dir)) {
            StartCoroutine(Move(dir));
        }
    }

    private void Update()
    {
        anim.SetBool("Moving", isMoving);
    }

    private bool CanMove(Direction direction) {
        Vector3 dir = GetDirectionUtil(direction);
        Vector3 dest = transform.position + dir;
        Vector3Int gridPos = ground.WorldToCell(dest);

        if (!ground.HasTile(gridPos) || collisionGround.HasTile(gridPos)) {
            return false;
        }

        int y = gridPos.x - Grid.grid.gridX;
        int x = gridPos.y - Grid.grid.gridY;
        x *= -1;
        if (Grid.grid.mat[x, y] == 2) {
            return false;
        }

        return true;

    }

    private IEnumerator Move(Direction direction) {
        isMoving = true;
        Vector3 dir = GetDirectionUtil(direction);
        Vector3 end = transform.position + dir, start = transform.position;
        float timeElapsed = 0;

        while (timeElapsed < timeToMove) {
            transform.position = Vector3.Lerp(start, end, timeElapsed / timeToMove);
            timeElapsed += Time.deltaTime;
            yield return null;
	    }

        Vector2Int initialPos = PositionToMatrix(start);
        Vector2Int finalPos = PositionToMatrix(end);
        Grid.grid.mat[initialPos.x, initialPos.y] = 0;
        Grid.grid.mat[finalPos.x, finalPos.y] = 4;

        transform.position = end;
        isMoving = false;
    }

    Vector3 GetDirectionUtil(Direction direction) {
        Vector3 dir = new(0, 0, 0);
        float delta = 1;

        switch(direction)
        {
            case Direction.UP:
                dir.y += delta;
                break;

            case Direction.DOWN:
                dir.y -= delta;
                break;

            case Direction.LEFT:
                dir.x -= delta;
                break;

            case Direction.RIGHT:
                dir.x += delta;
                break;

            default:
                break;
        }

        return dir;
    }

    Vector2Int PositionToMatrix(Vector3 pos) {
        Vector3Int gridPos = ground.WorldToCell(pos);

        int y = gridPos.x - Grid.grid.gridX;
        int x = gridPos.y - Grid.grid.gridY;
        x *= -1;

        Vector2Int matPos = new(x, y);
        return matPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("collided");
    }

}
