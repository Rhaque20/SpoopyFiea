using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class randomsounds : MonoBehaviour
{ 
public List<AudioClip> audioClips;
public AudioClip currentClip;
public AudioSource source;
public float minWaitBetweenPlays = 1f;
public float maxWaitBetweenPlays = 5f;
public float waitTimeCountdown = -1f;

void Start()
{
    source = GetComponent<AudioSource>();
}

void Update()
{
    if (!source.isPlaying)
    {
        if (waitTimeCountdown < 0f)
        {
            currentClip = audioClips[Random.Range(0, audioClips.Count)];
            source.clip = currentClip;
            source.Play();
            waitTimeCountdown = Random.Range(minWaitBetweenPlays, maxWaitBetweenPlays);
        }
        else
        {
            waitTimeCountdown -= Time.deltaTime;
        }
    }
}
}
