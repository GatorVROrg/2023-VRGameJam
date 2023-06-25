using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moped : MonoBehaviour
{
    public Transform HandleBars;
    public Transform FrontWheel;

    public GameObject RHandOnBar;
    public GameObject LHandOnBar;

    public GameObject RHand;
    public GameObject LHand;
    private Quaternion defaultRot;

    void Start()
    {
        defaultRot = HandleBars.localRotation;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(HandleBars.localRotation.z);
        //Debug.Log(FrontWheel.localRotation.z);
        FrontWheel.localRotation = Quaternion.Euler(FrontWheel.localRotation.x, FrontWheel.localRotation.y, HandleBars.localRotation.z * 50);
    }

    public void OnGrab()
    {
        Debug.Log("Grabbed Handlebars");
        RHand.SetActive(false);
        RHandOnBar.SetActive(true);
        LHand.SetActive(false);
        LHandOnBar.SetActive(true);
    }

    public void OnRelease()
    {
        Debug.Log("Released Handlebars");
        RHand.SetActive(true);
        RHandOnBar.SetActive(false);
        LHand.SetActive(true);
        LHandOnBar.SetActive(false);
        HandleBars.localRotation = Quaternion.Euler(defaultRot.x, defaultRot.y, defaultRot.z);
    }
}
