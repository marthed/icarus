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


    // Update is called once per frame
    void FixedUpdate()
    {
        float acceleration = ((_rb.linearVelocity - previousVelocity) / Time.fixedDeltaTime).magnitude;

        float scaledVelocity = _rb.linearVelocity.magnitude / 40; // 40 is approx Max speed

        float scaledAccleration = acceleration / 30; // 30 is approx max acceleration

        float scaledPitch = acceleration / 10;

        _audioSource.volume = Mathf.Max(scaledVelocity * scaledAccleration, 0.16f);
        _audioSource.pitch = Mathf.Clamp(scaledPitch, 0, 3);



        previousVelocity = _rb.linearVelocity;

    }
}
