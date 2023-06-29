using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Moped : MonoBehaviour
{
    public GameObject Parent;
    public CharacterController character;
    public Transform Director;
    public Transform HandleBars;
    public Transform FrontWheel;

    public GameObject RHandOnBar;
    public GameObject LHandOnBar;

    public GameObject RHand;
    public GameObject LHand;

    public float speed;
    public float accelerationSpeed;
    public float brakeSpeed;
    public float maxSpeed;
    public int rotationValue;

    public InputActionProperty LeftHandTrigger;
    public InputActionProperty RightHandTrigger;

    private Quaternion defaultRot;
    private float RtriggerValue;
    private float LtriggerValue;
    public bool Grabbed;

    public bool Accelerating;
    public bool Deccelerating;

    public float value;

    void Start()
    {
        defaultRot = HandleBars.localRotation;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(Grabbed)
        {
            LtriggerValue = LeftHandTrigger.action.ReadValue<float>();
            RtriggerValue = RightHandTrigger.action.ReadValue<float>();

            FrontWheel.localRotation = Quaternion.Euler(FrontWheel.localRotation.x, FrontWheel.localRotation.y, HandleBars.localRotation.y * 50);
            
            if(RtriggerValue == 1)
            {
                Accelerating = true;
            }
            else
            {
                Accelerating = false;
            }
            if(LtriggerValue == 1)
            {
                Deccelerating = true;
            }
            else
            {
                Deccelerating = false;
            }

            if(Accelerating && Deccelerating)
            {
                Debug.Log("stop fucking doing that");
            }
            else if(Accelerating && !Deccelerating)
            {
                speed = Mathf.Clamp(speed + (accelerationSpeed * Time.deltaTime), 0, maxSpeed);
                character.Move(Director.transform.forward * Time.fixedDeltaTime * speed);

                if(value >= 0.7)
                {
                    Parent.transform.rotation *= Quaternion.Euler(0, 0, rotationValue - .7f);
                }
                if(value <= 0.3)
                {
                    Parent.transform.rotation *= Quaternion.Euler(0, 0, -rotationValue + .3f);                
                }
            }
            else if(Deccelerating && !Accelerating)
            {
                speed = Mathf.Clamp(speed + (brakeSpeed * Time.deltaTime), 0, maxSpeed);
                character.Move(Director.transform.forward * Time.fixedDeltaTime * -speed);

                if(value >= 0.8)
                {
                    Parent.transform.rotation *= Quaternion.Euler(0, 0, -rotationValue);
                }
                if(value <= 0.2)
                {
                    Parent.transform.rotation *= Quaternion.Euler(0, 0, rotationValue);                
                }
            }
        }
    }

    public void OnGrab()
    {
        Grabbed = true;
        RHand.SetActive(false);
        RHandOnBar.SetActive(true);
        LHand.SetActive(false);
        LHandOnBar.SetActive(true);
    }

    public void OnRelease()
    {
        Grabbed = false;
        RHand.SetActive(true);
        RHandOnBar.SetActive(false);
        LHand.SetActive(true);
        LHandOnBar.SetActive(false);
    }

    // public void WhipThisShit()
    // {
        
    // }

    // public void Slow()
    // {
    //     Accelerating = false;
    //     Deccelerating = false;
    // }
}