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
    public InputActionProperty LeftHandGrip;
    public InputActionProperty RightHandGrip;

    public AudioClip[] voiceClips;
    public AudioSource voiceSource;
    public AudioSource engineSource;

    public AudioClip[] ambianceClips;
    public AudioSource ambianceSource;

    private Quaternion defaultRot;
    private float RtriggerValue;
    private float LtriggerValue;
    private float RgripValue;
    private float LgripValue;
    private bool Grabbed;

    private bool Accelerating;
    private bool Deccelerating;

    public float value;

    private float VoicesIndex = 0;
    private bool playVoices = true;

    private float AmbianceIndex = 0;
    private bool playAmbiance = true;

    void Start()
    {
        defaultRot = HandleBars.localRotation;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        LgripValue = LeftHandGrip.action.ReadValue<float>();
        RgripValue = RightHandGrip.action.ReadValue<float>();

        if (playVoices)
        {
            int randomIndex = Random.Range(0, voiceClips.Length);
            if(randomIndex != VoicesIndex)
            {
                voiceSource.clip = voiceClips[randomIndex];
                voiceSource.Play();
                VoicesIndex = randomIndex;
                StartCoroutine(PauseVoices());
            }
        }

        if (playAmbiance)
        {
            int randomIndex = Random.Range(0, ambianceClips.Length);
            if(randomIndex != AmbianceIndex)
            {
                ambianceSource.clip = ambianceClips[randomIndex];
                ambianceSource.Play();
                AmbianceIndex = randomIndex;
                StartCoroutine(PauseAmbiance());
            }
        }

        if(RgripValue == 0)
        {
            RHand.SetActive(true);
            RHandOnBar.SetActive(false);
        }
        else
        {
            RHand.SetActive(false);
            RHandOnBar.SetActive(true);
        }

        if(LgripValue == 0)
        {
            LHand.SetActive(true);
            LHandOnBar.SetActive(false);
        }
        else
        {
            LHand.SetActive(false);
            LHandOnBar.SetActive(true);
        }

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
                engineSource.Play();
            }

            if(Accelerating && !Deccelerating)
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
    }

    public void OnRelease()
    {
        Grabbed = false;
    }

    public IEnumerator PauseVoices()
    {
        playVoices = false;
        yield return new WaitForSeconds(60);
        playVoices = true;
    }

    public IEnumerator PauseAmbiance()
    {
        playAmbiance = false;
        yield return new WaitForSeconds(30);
        playAmbiance = true;
    }
}