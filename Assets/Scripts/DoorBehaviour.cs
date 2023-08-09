using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private int zombieCount;
    void Start()
    {
        zombieCount = 0;
    }

    private void OnEnable()
    {
        GameEvents.OnDestoyZombie += ZombieCounter;
    }

    private void OnDisable()
    {
        GameEvents.OnDestoyZombie -= ZombieCounter;
    }

    private void ZombieCounter() 
    {
        zombieCount++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (zombieCount == 5) {
            GameEvents.OnGameOverInvoke();
        }
    }
}
