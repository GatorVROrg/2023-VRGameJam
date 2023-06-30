using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DeathScreen : MonoBehaviour
{
    public TextMeshPro text;

    void Update()
    {
        if (GameManager.instance != null)
        {
            text.text = "Total Score: " + GameManager.instance.totalScore + "\nDeliveries Made: " + GameManager.instance.currentLevel;
        }
        else
        {
            text.text = "GameManager instance not found.";
            Debug.Log("GameManager instance: " + GameManager.instance);
        }
    }
}

