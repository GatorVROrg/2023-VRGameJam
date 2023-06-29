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

    public AudioClip[] audioClips;
    public AudioSource audioSource;

    private Quaternion defaultRot;
    private float RtriggerValue;
    private float LtriggerValue;
    private bool Grabbed;

    private bool Accelerating;
    private bool Deccelerating;

    private float value;

    private float index = 0;
    private bool play = true;

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

            if(Accelerating)
            {
                if (play)
                {
                    int randomIndex = Random.Range(0, audioClips.Length);
                    if(randomIndex != index)
                    {
                        audioSource.clip = audioClips[randomIndex];
                        audioSource.Play();
                        index = randomIndex;
                        StartCoroutine(Pause());
                    }
                }
            }
            if(Accelerating && !Deccelerating)
            {
                speed = Mathf.Clamp(speed + (accelerationSpeed * Time.deltaTime), 0, maxSpeed);
                character.Move(Director.transform.forward * Time.fixedDeltaTime * speed);

                if(value >= 0.7)
                {
                    Parent.transform.rotation *= Quaternion.Euler(0, 0, rotationValue);
                }
                if(value <= 0.3)
                {
                    Parent.transform.rotation *= Quaternion.Euler(0, 0, -rotationValue);                
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

    public IEnumerator Pause()
    {
        play = false;
        yield return new WaitForSeconds(10);
        play = true;
    }
}