using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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
        Generator3 generator3 = FindObjectOfType<Generator3>();
        if (generator3 != null) 
        {
            // Set your reference here
        } 
        else 
        {
            Debug.LogError("No Generator3 instance found in scene");
        }
    }

    void Start()
    {
        StartLevel();
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
