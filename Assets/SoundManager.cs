using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(int index)
    {
        if (index >= 0 && index < audioClips.Length)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.Log("Invalid Audioclip");
        }
    }
}
