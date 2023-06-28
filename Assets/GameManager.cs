using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Generator3 generator;

    private int currentScore;
    private int totalScore;
    private int currentLevel;
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartLevel();
        currentLevel = 0;
        currentScore = 0;
        totalScore = 0;
    }

    public void StartLevel()
    {
        isGameOver = false;
        //generator.StartGeneration();
        currentScore = 0;
    }

    public void EndLevel()
    {
        totalScore += currentScore;
    }

    public void Die()
    {
        // Any additional game over logic
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            currentScore = generator.currentNodeIndex - generator.lookAhead;
        }
        if (currentScore == generator.levelDistance)
        {
            EndLevel();
        }
    }
}
