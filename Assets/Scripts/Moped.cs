using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Moped : MonoBehaviour
{
    private Rigidbody RB;

    public Transform FrontDrive;
    public Transform RearDrive;
    public Transform Handles;


    public GameObject RHandOnBar;
    public GameObject LHandOnBar;

    public GameObject RHand;
    public GameObject LHand;

    public float accelerationSpeed = 5f;
    public float turningSpeed = 1f;
    public float brakeSpeed = 10f;
    public float maxSpeed = 10f;

    public InputActionProperty LeftHandTrigger;
    public InputActionProperty RightHandTrigger;

    public bool LeftHandGrab { get; set; }
    public bool RightHandGrab { get; set; }


    private float acceleratorInput;
    private float brakeInput;
    public bool Grabbed;

    private Vector3 HandlesOffset;

    void Start()
    {
        RB = GetComponent<Rigidbody>();
        //HandlesOffset = Handles.position - transform.position;
    }
    // Update is called once per frame
    void Update()
    {        
       // Handles.transform.position = HandlesOffset + transform.position;
        if (Grabbed)
        {
            brakeInput = LeftHandTrigger.action.ReadValue<float>();
            acceleratorInput = RightHandTrigger.action.ReadValue<float>();

            // Calculate acceleration force based on the accelerator input
            float accelerationForce = acceleratorInput * accelerationSpeed;
            
            // Calculate brake force based on the brake input
            float brakeForce = brakeInput * brakeSpeed * -1;

            // Apply the acceleration and brake forces to the car's rigidbody
            RB.AddForce(RearDrive.forward * accelerationForce * Time.deltaTime);
            RB.AddForce(RearDrive.forward * brakeForce * Time.deltaTime);

            // Limit the car's speed to the maximum speed
            RB.velocity = Vector3.ClampMagnitude(RB.velocity, maxSpeed);

            // Calculate how much you have turned
            float dot =  Vector3.Dot(RearDrive.right, Handles.forward);

            // Turn the front wheel
            float newRot = Mathf.Clamp(FrontDrive.rotation.y + (dot * 35 * Time.deltaTime), -35, 35);
            FrontDrive.rotation = Quaternion.Euler(FrontDrive.rotation.x, newRot, FrontDrive.rotation.z);
            
            if(Mathf.Abs(dot) < 0.25)
            {
                dot = 0;
            }
            float angle = Vector3.Angle(RearDrive.forward, Handles.forward) * -1;
            RB.AddTorque(Vector3.up * dot * turningSpeed * Time.deltaTime);

        }
    }

    public void OnGrab()
    {
        Grabbed = true;

        if (LeftHandGrab)
        {
            LHand.SetActive(false);
            LHandOnBar.SetActive(true);
        }
        if (RightHandGrab)
        {
            RHand.SetActive(false);
            RHandOnBar.SetActive(true);
        }
    }

    public void OnRelease()
    {
        Grabbed = false;

        if (!LeftHandGrab)
        {
            LHand.SetActive(true);
            LHandOnBar.SetActive(false);
        }
        if (!RightHandGrab)
        {
            RHand.SetActive(true);
            RHandOnBar.SetActive(false);
        }
    }
}
