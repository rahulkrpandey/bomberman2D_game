using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    // Start is called before the first frame update
    private int score;
    private int zombieScore;
    private int tileScore;
    private int collectibleScore;
    private TextMeshProUGUI text;

    void Start()
    {
        score = 0;
        zombieScore = 200;
        tileScore = 100;
        collectibleScore = 50;
        text = transform.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        GameEvents.OnDestoyZombie += AddZombieScore;
        GameEvents.OnDestoyTile += AddTileScore;
        GameEvents.OnCollection += AddCollectibleScore;
    }

    // Update is called once per frame
    void OnDisable()
    {
        GameEvents.OnDestoyZombie -= AddZombieScore;
        GameEvents.OnDestoyTile -= AddTileScore;
        GameEvents.OnCollection -= AddCollectibleScore;
    }

    private void Update()
    {
        text.text = "Score: " + score;
    }

    void AddZombieScore() {
        score += zombieScore;
    }

    void AddTileScore() {
        score += tileScore;
    }

    void AddCollectibleScore() {
        score += collectibleScore;
    }
}
