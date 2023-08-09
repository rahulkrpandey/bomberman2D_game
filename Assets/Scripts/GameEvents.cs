using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameEvents : MonoBehaviour
{
    static public GameEvents events;

    private void Awake()
    {
        events = this;
    }

    public delegate void BombBlast(int x, int y);
    public static event BombBlast OnBomBlastInvoke;
    public static void OnBomBlast(int x, int y) {
        OnBomBlastInvoke(x, y);
    }

    public delegate void Collection();
    public static event Collection OnCollection;
    public static void OnCollectionEnter() {
        OnCollection();
    }

    public delegate void DestroyObjects();
    public static event DestroyObjects OnDestoyTile;
    public static event DestroyObjects OnDestoyZombie;
    public static event DestroyObjects OnGameOver;

    public static void OnDestroyTileInvoke() {
        OnDestoyTile();
    }

    public static void OnDestroyZombieInvoke() {
        OnDestoyZombie();
    }

    public static void OnGameOverInvoke() {
        OnGameOver?.Invoke();
    }
}
