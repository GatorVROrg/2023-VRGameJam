using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
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
        SceneManager.LoadScene("Level Generation Testing Scene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
