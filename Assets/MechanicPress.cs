using UnityEngine;
using System.Collections;

public enum MechanicPressState
{
    Stopped,
    Ready,
    Smash,
    Rewind,
}

public class MechanicPress : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public MechanicPressState mechanicPressState = MechanicPressState.Stopped;

    public float readyDuration = 10f;
    public float smashDuration = 0.5f;
    public float rewindDuration = 20f;

    #region private
    private Wall _wallLeft;
    private Wall _wallRight;
    private AudioSource _audioSource;
    private Vector3 _wallLeftOriginPosition;
    private Vector3 _wallRightOriginPosition;
    #endregion
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        Wall[] walls = gameObject.GetComponentsInChildren<Wall>();
        _wallLeft = walls[0];
        _wallRight = walls[1];
        _wallLeftOriginPosition = _wallLeft.transform.position;
        _wallRightOriginPosition = _wallRight.transform.position;

        StartCoroutine(StateMachince());
    }

    IEnumerator StateMachince()
    {

        while (true)
        {
            switch (mechanicPressState)
            {
                case MechanicPressState.Stopped:
                    StopAllCoroutines();
                    yield return new WaitForSeconds(5);
                    break;
                case MechanicPressState.Ready:
                    StopAllCoroutines();
                    StartCoroutine(Ready());
                    yield return new WaitForSeconds(3);
                    mechanicPressState = MechanicPressState.Smash;
                    break;
                case MechanicPressState.Smash:
                    StopAllCoroutines();
                    StartCoroutine(Smash());
                    yield return new WaitForSeconds(5);
                    mechanicPressState = MechanicPressState.Rewind;
                    break;
                case MechanicPressState.Rewind:
                    StopAllCoroutines();
                    StartCoroutine(Rewind());
                    yield return new WaitForSeconds(3);
                    mechanicPressState = MechanicPressState.Ready;
                    break;
            }
        }
    }


    IEnumerator Ready()
    {

        Debug.Log("Ready");
        // Play ready sounds (clu-clunk clu-clunk clu-clunk)

        float initialAmplitude = 5f; // Initial amplitude of the pendulum
        float frequency = 1f; // Frequency of the oscillation
        float dampingFactor = 0.1f; // Damping factor to reduce the amplitude over time


        float elapsedTime = 0f; // Time elapsed since the start

        while (elapsedTime < readyDuration)
        {
            elapsedTime += Time.deltaTime;

            float amplitude = initialAmplitude * Mathf.Exp(-dampingFactor * elapsedTime);

            float xOffset = amplitude * Mathf.Sin(frequency * elapsedTime);

            _wallLeft.transform.position = new Vector3(_wallLeftOriginPosition.x + xOffset, _wallLeftOriginPosition.y, _wallLeftOriginPosition.z);
            _wallRight.transform.position = new Vector3(_wallRightOriginPosition.x + xOffset, _wallRightOriginPosition.y, _wallRightOriginPosition.z);

            yield return null;
        }

        _wallLeft.transform.position = _wallLeftOriginPosition;
        _wallRight.transform.position = _wallRightOriginPosition;

    }

    IEnumerator Smash()
    {
        Debug.Log("Smash");
        // Play swooth sound
        // If user is in smash position: Init death scene with scream sound

        float elapsedTime = 0;

        Vector3 leftEndPosition = new Vector3(-4, 0, 0);
        Vector3 rightEndPosition = new Vector3(4, 0, 0);

        while (elapsedTime < smashDuration)
        {
            float t = elapsedTime / smashDuration;
            _wallLeft.transform.position = Vector3.Lerp(_wallLeftOriginPosition, leftEndPosition, t);
            _wallRight.transform.position = Vector3.Lerp(rightEndPosition, rightEndPosition, t);
            yield return null;
        }


        // Play Smash sound;

    }

    IEnumerator Rewind()
    {
        Debug.Log("Rewind");
        // Play rewind sound;

        float elapsedTime = 0;

        Vector3 leftStart = _wallLeft.transform.position;
        Vector3 rightStart = _wallRight.transform.position;

        Vector3 leftEndPosition = _wallLeftOriginPosition;
        Vector3 rightEndPosition = _wallRightOriginPosition;

        while (elapsedTime < rewindDuration)
        {
            float t = elapsedTime / smashDuration;
            _wallLeft.transform.position = Vector3.Lerp(leftStart, leftEndPosition, t);
            _wallRight.transform.position = Vector3.Lerp(rightStart, rightEndPosition, t);
            yield return null;
        }

    }



}
