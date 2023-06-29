using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public AudioSource start;
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
}
