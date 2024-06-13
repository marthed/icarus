using UnityEngine;

public class AccelerationSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource _audioSource;

    private Rigidbody _rb;

    private Vector3 previousVelocity;
    void Start()
    {

        _audioSource = GetComponent<AudioSource>();

        _rb = GetComponent<Rigidbody>();

        previousVelocity = _rb.linearVelocity;


    }

    float maxAcc = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        float acceleration = ((_rb.linearVelocity - previousVelocity) / Time.fixedDeltaTime).magnitude;

        if (acceleration > maxAcc)
        {
            maxAcc = acceleration;
        }

        float scaled = acceleration / 30;

        float scaledPitch = acceleration / 10;
        Debug.Log(maxAcc);


        _audioSource.volume = Mathf.Max(scaled, 0.16f);
        _audioSource.pitch = Mathf.Clamp(scaledPitch, 0, 3);



        previousVelocity = _rb.linearVelocity;
        
    }
}
