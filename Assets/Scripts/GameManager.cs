using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private TextMeshPro scoreText;


    public Generator3 generator;
    private int currentScore;
    private int totalScore;
    private int currentLevel;
    private bool isGameOver = false;

    public List<int> LevelDistances;
    public List<float> LevelSpeeds;

    public GameObject ghostPrefab;

    private GameObject ghost;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            currentLevel = 0;
            currentScore = 0;
            totalScore = 0;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

   private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        // Find the Generator3 instance and assign it to the generator3 member
        if (SceneManager.GetActiveScene().name == "Level Generation Testing Scene")
        {
            Generator3 generator3 = FindObjectOfType<Generator3>();
            scoreText = GameObject.Find("Score Text").GetComponent<TextMeshPro>();
            if (generator3 != null) 
            {
                generator = generator3;
            } 
            else 
            {
                Debug.LogError("No Generator3 instance found in scene");
            }
            StartLevel();
        }
    }

    public void StartLevel()
    {
        isGameOver = false;
        generator.levelDistance = LevelDistances[currentLevel];
        generator.Generate(); //add ability to pass paramaters and base them on level

        //spawn ghosts
        ghost = Instantiate(ghostPrefab, generator.roads[0].road.transform.position, Quaternion.identity);
        ghost.GetComponent<GhostMovement>().speed = LevelSpeeds[currentLevel];
        ghost.GetComponent<GhostMovement>().generator = generator;


        currentScore = 0;
    }

    public void EndLevel()
    {
        totalScore += currentScore;
        currentLevel++;
        generator.UnGenerate();

        //despawn ghosts
        Destroy(ghost);

        //load next level and tp player to start. trigger delivery scene if its there

        SceneManager.LoadScene("Delivered");
    }

    public void Die(string message)
    {
        // Any additional game over logic
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            currentScore = generator.currentNodeIndex - generator.lookAhead;
            UpdateText();
        }
        if (currentScore == generator.levelDistance && !isGameOver)
        {
            isGameOver = true;
            EndLevel();
        }
    }

    void UpdateText()
    {
        int total = totalScore + currentScore;
        scoreText.text = "Total Score: " + total + "\nLevel: " + (currentLevel + 1);
    }
}
