using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggredsound : MonoBehaviour
{
    public List<AudioClip> audioClips;
    public AudioClip currentClip;
    public AudioSource source;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {

            currentClip = audioClips[Random.Range(0, audioClips.Count)];
            source.clip = currentClip;
            source.Play();

        }


    }
}
