using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Moped : MonoBehaviour
{
    public GameObject Parent;
    public Rigidbody RB;
    public GameObject Director;
    public GameObject Player;
    public Transform HandleBars;
    public Transform FrontWheel;

    public GameObject RHandOnBar;
    public GameObject LHandOnBar;

    public GameObject RHand;
    public GameObject LHand;

    public float accelerationSpeed = 5f;
    public float brakeSpeed = 10f;
    public float maxSpeed = 10f;

    public InputActionProperty LeftHandTrigger;
    public InputActionProperty RightHandTrigger;

    private Quaternion defaultRot;
    private float RtriggerValue;
    private float LtriggerValue;
    public bool Grabbed;


    void Start()
    {
        defaultRot = HandleBars.localRotation;
    }
    // Update is called once per frame
    void Update()
    {
        if(Grabbed)
        {
            LtriggerValue = LeftHandTrigger.action.ReadValue<float>();
            RtriggerValue = RightHandTrigger.action.ReadValue<float>();
            //Debug.Log(LtriggerValue);
            //Debug.Log(RtriggerValue);

            //Debug.Log(HandleBars.localRotation.z);
            //Debug.Log(FrontWheel.localRotation.x);
            FrontWheel.localRotation = Quaternion.Euler(FrontWheel.localRotation.x, FrontWheel.localRotation.y, HandleBars.localRotation.y * 50);

            float acceleratorInput = RtriggerValue;
            float brakeInput = LtriggerValue;

            // Calculate acceleration force based on the accelerator input
            float accelerationForce = acceleratorInput * accelerationSpeed;
            
            // Calculate brake force based on the brake input
            float brakeForce = brakeInput * brakeSpeed;

            // Apply the acceleration and brake forces to the car's rigidbody
            RB.AddForce(Director.transform.forward * accelerationForce);
            RB.AddForce(-Director.transform.forward * brakeForce);

            // Limit the car's speed to the maximum speed
            RB.velocity = Vector3.ClampMagnitude(RB.velocity, maxSpeed);
            
            // Rotate the car towards the direction of the front wheel
            Parent.transform.localRotation = Quaternion.Euler(-90, 0, FrontWheel.transform.localRotation.z * 200);
            Debug.Log(FrontWheel.transform.localRotation.z * 200);
        }
    }

    public void OnGrab()
    {
        Grabbed = true;
        Debug.Log("Grabbed Handlebars");
        RHand.SetActive(false);
        RHandOnBar.SetActive(true);
        LHand.SetActive(false);
        LHandOnBar.SetActive(true);
    }

    public void OnRelease()
    {
        Grabbed = false;
        Debug.Log("Released Handlebars");
        RHand.SetActive(true);
        RHandOnBar.SetActive(false);
        LHand.SetActive(true);
        LHandOnBar.SetActive(false);
        HandleBars.localRotation = Quaternion.Euler(defaultRot.x, defaultRot.y, defaultRot.z);
    }

    public void WhipThisShit()
    {
        if(RtriggerValue > 0)
        {
            Debug.Log("Accelerating");
        }
        if(LtriggerValue > 0)
        {
            Debug.Log("Braking");
        }
    }

    public void Slow()
    {
        Debug.Log("Stopped");
    }
}
