using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform Target;

    [Range(0,1)]
    public float posDampening;

    [Range(0,1)]
    public float rotDampening;
    // Start is called before the first frame update
    void OnEnable()
    {
        transform.position = Target.position;
        transform.rotation = Target.rotation;
    }

    // Update is called once per frame
    void Update()
    {       
        transform.position = Vector3.Lerp(transform.position, Target.position, posDampening);
        transform.rotation = Quaternion.Lerp(transform.rotation, Target.rotation, rotDampening);
    }
}
