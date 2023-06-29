using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSounds : MonoBehaviour
{

    public AudioClip[] HonkClips;
    public AudioSource HonkSource;
    private bool playHonks;
    private int HonksIndex;

    // Update is called once per frame
    void Update()
    {
        if (playHonks)
        {
            int randomIndex = Random.Range(0, HonkClips.Length);
            if(randomIndex != HonksIndex)
            {
                HonkSource.clip = HonkClips[randomIndex];
                HonkSource.Play();
                HonksIndex = randomIndex;
                StartCoroutine(PauseHonks());
            }
        }
    }

    public IEnumerator PauseHonks()
    {
        playHonks = false;
        yield return new WaitForSeconds(60);
        playHonks = true;
    }
}
