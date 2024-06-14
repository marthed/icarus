using UnityEngine;
using System.Collections.Generic;
public class targetScript : MonoBehaviour
{
    public List<AudioClip> audioClips; // List of audio clips to choose from
    public float playInterval = 5f; // Time interval in seconds between playing clips

    public AudioSource audioSource;
    private float timer;
    public bool atSea;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = playInterval;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= playInterval)
        {
            PlayRandomClip();
            timer = 0f; // Reset the timer
        }
    }
    void PlayRandomClip()
    {
        if (audioClips.Count > 0)
        {
            int randomIndex = Random.Range(0, audioClips.Count);
            audioSource.clip = audioClips[randomIndex];
            audioSource.Play();
        }
        Debug.Log("playing");
        Debug.Log(audioSource.clip);
    }
}
