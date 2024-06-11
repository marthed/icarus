using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public float maxVelocity = 50;
    public float maxHeight = 20;
 
    private AudioSource _source;

    private Rigidbody _rb;
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _rb = transform.GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log("rb speed: " + _rb.linearVelocity.magnitude);

        _source.volume = Mathf.Lerp(0, 1, _rb.linearVelocity.magnitude / maxVelocity);

        _source.pitch = Mathf.Lerp(0, 2.5f, _rb.linearVelocity.magnitude / maxVelocity);

        //AudioLowPassFilter lowPassFilter = _source.GetComponent<AudioLowPassFilter>();
        //lowPassFilter.cutoffFrequency = Mathf.Lerp(minCutoffFreq, maxCutoffFreq, speed / maxSpeed);

        
    }
}
