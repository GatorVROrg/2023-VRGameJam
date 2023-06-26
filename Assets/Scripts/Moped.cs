using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moped : MonoBehaviour
{
    public GameObject Parent;
    public GameObject Director;
    public GameObject Player;
    public Transform HandleBars;
    public Transform FrontWheel;

    public GameObject RHandOnBar;
    public GameObject LHandOnBar;

    public GameObject RHand;
    public GameObject LHand;

    public float speed;
    private Quaternion defaultRot;
    private bool accelerating;

    void Start()
    {
        defaultRot = HandleBars.localRotation;
    }
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(HandleBars.localRotation.z);
        //Debug.Log(FrontWheel.localRotation.x);
        FrontWheel.localRotation = Quaternion.Euler(FrontWheel.localRotation.x, FrontWheel.localRotation.y, HandleBars.localRotation.y * 50);

        if(accelerating)
        {
            Vector3 forwardDirection = FrontWheel.forward;

            // Calculate the new position based on the forward direction
            Vector3 newPosition = transform.position + Director.transform.forward * speed * Time.deltaTime;

            // Move the game object to the new position
            Parent.transform.position = newPosition;
            Debug.Log("Moving");
        }
    }

    public void OnGrab()
    {
        Debug.Log("Grabbed Handlebars");
        Player.transform.SetParent(Parent.transform);
        RHand.SetActive(false);
        RHandOnBar.SetActive(true);
        LHand.SetActive(false);
        LHandOnBar.SetActive(true);
    }

    public void OnRelease()
    {
        Debug.Log("Released Handlebars");
        Player.transform.SetParent(null);
        RHand.SetActive(true);
        RHandOnBar.SetActive(false);
        LHand.SetActive(true);
        LHandOnBar.SetActive(false);
        HandleBars.localRotation = Quaternion.Euler(defaultRot.x, defaultRot.y, defaultRot.z);
    }

    public void WhipThisShit()
    {
        Debug.Log("Accelerating");
        accelerating = true;
    }

    public void Slow()
    {
        Debug.Log("Braking");
        accelerating = false;
    }
}
