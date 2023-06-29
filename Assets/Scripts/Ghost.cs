using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public float degreesPerSecond = 15.0f;
    public float amplitude = 0.5f;
    public float frequency = 1f;

    public AudioClip[] audioClips;
    public AudioSource audioSource;

    private float originalY;
    private float index = 0;
    private bool play = true;

    void Start()
    {
        // Store the starting y position of the object
        originalY = transform.position.y;
    }

    void Update()
    {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin() on y axis only
        Vector3 tempPos = transform.position;
        tempPos.y = originalY + Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;
        transform.position = tempPos;

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

    public IEnumerator Pause()
    {
        play = false;
        yield return new WaitForSeconds(40);
        play = true;
    }
}
