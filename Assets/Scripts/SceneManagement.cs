using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public AudioSource start;
    public AudioClip[] voiceClips;

    private float VoicesIndex = 0;
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
            StartCoroutine(PauseVoices(start.clip.length));
        }
    }

    public IEnumerator PauseVoices(float Seconds)
    {
        yield return new WaitForSeconds(Seconds + 1);
        SceneManager.LoadScene("Level Generation Testing Scene");
    }
}
