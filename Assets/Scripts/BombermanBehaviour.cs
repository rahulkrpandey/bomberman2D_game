using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class BombermanBehaviour : MonoBehaviour
{
    public Animator amin;
    public Tilemap ground;
    public Tilemap collisionGround;
    private bool isMoving;
    private float timeToMove;
    private int maxNumberOfBombs, spawned;
    private float originX, originY;
    [SerializeField] private GameObject bomb;
    public FloatingJoystick joystick;


    enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    };

    private void Awake()
    {
        originX = -4.5f;
        originY = 4.64f;
        //Debug.Log(ground.WorldToCell(transform.position));
    }

    private void Start()
    {
        //Vector3Int pos = ground.WorldToCell(transform.position);
        //transform.position = pos;
        isMoving = false;
        timeToMove = 0.2f;
        maxNumberOfBombs = 1;
        spawned = 0;
        Grid.grid.mat[0, 0] = 4;
    }

    private void OnEnable()
    {
        GameEvents.OnBomBlastInvoke += UpdateSpawned;
        //GameEvents.OnCollection += CollectionHandler;
    }

    private void OnDisable()
    {
        GameEvents.OnBomBlastInvoke -= UpdateSpawned;
        //GameEvents.OnCollection -= CollectionHandler;
    }

    //private void CollectionHandler() {
        
    //    Debug.Log("collected bomberman");
    //}

    private void UpdateSpawned(int x, int y)
    {
        Vector2Int pos = PositionToMatrix(transform.position);
        Vector2Int bomb = new(x, y);
        if (Grid.grid.CanDestroy(pos, bomb)) {
            SoundManager.sm.PlayHurt();
            Destroy(gameObject);
        }

        spawned--;
    }

    private void FixedUpdate()
    {
        if (gameObject == null)
        {
            return;
        }

        float x = joystick.Horizontal, y = joystick.Vertical;
        if (y == 1f || y == -1f) {
            x = 0;
        }

        if ((y == 1f && x == 0f) && !isMoving)
        {
            if (CanMove(Direction.UP))
            {
                StartCoroutine(Move(Direction.UP));
            }
        }
        else if ((y == -1f && x == 0f) && !isMoving)
        {
            if (CanMove(Direction.DOWN))
            {
                StartCoroutine(Move(Direction.DOWN));
            }
        }
        else if ((x == -1f && y == 0f) && !isMoving)
        {
            if (CanMove(Direction.LEFT))
            {
                StartCoroutine(Move(Direction.LEFT));
            }
        }
        else if ((x == 1f && y == 0f) && !isMoving)
        {
            if (CanMove(Direction.RIGHT))
            {
                StartCoroutine(Move(Direction.RIGHT));
            }
        }

        amin.SetBool("Moving", isMoving);
        amin.SetInteger("Explode", 1);
    }

    public void BombInstantiator() {
	    Vector2Int pos = PositionToMatrix(transform.position);
	    Grid.grid.mat[pos.x, pos.y] = 3;
	    InstantiateBomb();
    }

    private void Update()
    {
        if (gameObject == null)
        {
            return;
        }

        if (Input.GetKey(KeyCode.W) && !isMoving)
        {
            if (CanMove(Direction.UP))
            {
                StartCoroutine(Move(Direction.UP));
            }
        }
        else if (Input.GetKey(KeyCode.S) && !isMoving)
        {
            if (CanMove(Direction.DOWN))
            {
                StartCoroutine(Move(Direction.DOWN));
            }
        }
        else if (Input.GetKey(KeyCode.A) && !isMoving)
        {
            if (CanMove(Direction.LEFT))
            {
                StartCoroutine(Move(Direction.LEFT));
            }
        }
        else if (Input.GetKey(KeyCode.D) && !isMoving)
        {
            if (CanMove(Direction.RIGHT))
            {
                StartCoroutine(Move(Direction.RIGHT));
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Vector2Int pos = PositionToMatrix(transform.position);
            Grid.grid.mat[pos.x, pos.y] = 3;
            InstantiateBomb();
        }
    }

    private void InstantiateBomb()
    {
        if (spawned >= maxNumberOfBombs)
        {
            return;
        }

        Instantiate(bomb, transform.position, transform.rotation);
        spawned++;
    }
    private bool CanMove(Direction direction)
    {
        Vector3 dir = GetDirectionUtil(direction);
        Vector3 dest = transform.position + dir;
        Vector3Int gridPos = ground.WorldToCell(dest);

        if (!ground.HasTile(gridPos) || collisionGround.HasTile(gridPos))
        {
            return false;
        }

        int y = gridPos.x - Grid.grid.gridX;
        int x = gridPos.y - Grid.grid.gridY;
        x *= -1;
        if (Grid.grid.mat[x, y] == 2)
        {
            return false;
        }

        return true;

    }

    private IEnumerator Move(Direction direction)
    {
        isMoving = true;
        Vector3 dir = GetDirectionUtil(direction);
        Vector3 end = transform.position + dir, start = transform.position;
        float timeElapsed = 0;

        while (timeElapsed < timeToMove)
        {
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

    Vector3 GetDirectionUtil(Direction direction)
    {
        Vector3 dir = new(0, 0, 0);
        float delta = 1;

        switch (direction)
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

    Vector2Int PositionToMatrix(Vector3 pos)
    {
        Vector3Int gridPos = ground.WorldToCell(pos);

        int y = gridPos.x - Grid.grid.gridX;
        int x = gridPos.y - Grid.grid.gridY;
        x *= -1;

        Vector2Int matPos = new(x, y);
        return matPos;
    }

    private void OnDestroy()
    {
        if (ground == null)
        {
            return;
        }
        Vector2Int pos = PositionToMatrix(transform.position);
        Grid.grid.mat[pos.x, pos.y] = 0;
        GameEvents.OnGameOverInvoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Zombie")
        {
            //nDebug.Log("collided");
            Destroy(gameObject);
        }
    }
}
