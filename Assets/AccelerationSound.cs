using UnityEngine;

public class AccelerationSound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource _audioSource;

    private Rigidbody _rb;

    private Vector3 previousPosition;
    void Start()
    {


        _rb = GetComponent<Rigidbody>();

        previousPosition = new Vector3(0, 0, 0);


    }


    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = transform.position - previousPosition;
        //float acceleration = ((_rb.linearVelocity - previousVelocity) / Time.fixedDeltaTime).magnitude;

        //float scaledVelocity = _rb.linearVelocity.magnitude / 40; // 40 is approx Max speed

        //float scaledAccleration = acceleration / 30; // 30 is approx max acceleration

        //float scaledPitch = acceleration / 10;



        _audioSource.volume = Mathf.Max(currentVelocity.magnitude / 50, 0.16f);
        _audioSource.pitch = Mathf.Clamp(currentVelocity.magnitude / 50, 0, 3);



        previousPosition = transform.position;

    }
}
