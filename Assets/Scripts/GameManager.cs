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
    public int totalScore;
    public int currentLevel;
    private bool isGameOver = false;

    public List<int> LevelDistances;
    public List<float> LevelSpeeds;

    public GameObject ghostPrefab;

    public GameObject ghost;
    private int newGame;
    public bool isActiveInstance; //whether a game is currently being played. 


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            currentLevel = 0;
            currentScore = 0;
            totalScore = 0;
            newGame = 1;
            isActiveInstance = true;
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
        generator.levelDistance = LevelDistances[currentLevel % LevelDistances.Count];
        generator.Generate(); //add ability to pass paramaters and base them on level

        //spawn ghosts
        ghost = Instantiate(ghostPrefab, generator.roads[0].road.transform.position, Quaternion.identity);
        ghost.GetComponent<GhostMovement>().speed = LevelSpeeds[currentLevel % LevelSpeeds.Count] * Mathf.Log(newGame + 2);
        ghost.GetComponent<GhostMovement>().generator = generator;

        currentScore = 0;
    }

    public void EndLevel()
    {
        totalScore += currentScore;
        currentLevel++;
        if (currentLevel % LevelDistances.Count == 0)
        {
            newGame++;
        }
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


    void Update()
    {
        if (!isGameOver)
        {
            currentScore = generator.currentNodeIndex - generator.lookAhead;
            UpdateText();

            // Calculate the distance between the player and the ghost
            float distance = Vector3.Distance(generator.player.position, ghost.transform.position);

            // If the distance is less than 0.1, call Die method
            if(distance < 0.5f)
            {
                Die("Ghost caught the player");
                Debug.Log("Ghost caught the player");
                isActiveInstance = false;
            }

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
