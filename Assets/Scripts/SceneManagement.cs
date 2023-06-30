using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public AudioSource start;
    public AudioClip[] voiceClips;
    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits Scene");
    }
    public void LeaveCredits()
    {
        SceneManager.LoadScene("Menu Scene");
    }
    public void StartGame()
    {
        start.Play();
        StartCoroutine(wait());
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Menu Scene");
    }

    public IEnumerator wait()
    {
        yield return new WaitForSeconds(16);
        SceneManager.LoadScene("Level Generation Testing Scene");
    }

    public void Start()
    {
        if(SceneManager.GetActiveScene().name == "Delivered")
        {
            int randomIndex = Random.Range(0, voiceClips.Length);
            start.clip = voiceClips[randomIndex];
            start.Play();
            StartCoroutine(PauseVoices());
        }
    }

    public IEnumerator PauseVoices()
    {
        yield return new WaitForSeconds(7);
        SceneManager.LoadScene("Level Generation Testing Scene");
    }
}
