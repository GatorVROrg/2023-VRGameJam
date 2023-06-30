using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    public TextMeshPro text;
    public GameManager GM;
    void Update()
    {
        text.text = "Total Score: " + GM.totalScore + "\nDeliveries Made: " + GM.currentLevel;
    }
}
