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
    public float smashDuration = 1f;
    public float rewindDuration = 20f;

    #region private
    private Wall _wallLeft;
    private Wall _wallRight;
    private Vector3 _wallLeftOriginPosition;
    private Vector3 _wallRightOriginPosition;
    private SoundManager _soundManager;

    private Coroutine _currentCoroutine;
    #endregion
    void Start()
    {
        _soundManager = GetComponent<SoundManager>();
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
                    if (_currentCoroutine != null)
                    {
                        StopCoroutine(_currentCoroutine);
                    }
                    yield return new WaitForSeconds(5);
                    break;
                case MechanicPressState.Ready:
                    if (_currentCoroutine != null)
                    {
                        StopCoroutine(_currentCoroutine);
                    }
                    _currentCoroutine = StartCoroutine(Ready());
                    yield return _currentCoroutine;
                    mechanicPressState = MechanicPressState.Smash;
                    yield return new WaitForSeconds(1);
                    break;
                case MechanicPressState.Smash:
                    if (_currentCoroutine != null)
                    {
                        StopCoroutine(_currentCoroutine);
                    }
                    _currentCoroutine = StartCoroutine(Smash());
                    yield return _currentCoroutine;
                    mechanicPressState = MechanicPressState.Rewind;
                    yield return new WaitForSeconds(2);
                    break;
                case MechanicPressState.Rewind:
                    if (_currentCoroutine != null)
                    {
                        StopCoroutine(_currentCoroutine);
                    }
                    _currentCoroutine = StartCoroutine(Rewind());
                    yield return _currentCoroutine;
                    mechanicPressState = MechanicPressState.Ready;
                    yield return new WaitForSeconds(3);
                    break;
            }
        }
    }


    IEnumerator Ready()
    {

        Debug.Log("Ready");
        // Play ready sounds (clu-clunk clu-clunk clu-clunk)

        float initialAmplitude = 1f; // Initial amplitude of the pendulum
        float frequency = 20f; // Frequency of the oscillation
        float dampingFactor = 1f; // Damping factor to reduce the amplitude over time


        float elapsedTime = 0f; // Time elapsed since the start

        while (elapsedTime < readyDuration)
        {
            elapsedTime += Time.deltaTime;

            float amplitude = initialAmplitude * Mathf.Exp(-dampingFactor * elapsedTime);

            float xOffset = amplitude * Mathf.Sin(frequency * elapsedTime);
            float xOffsetRight = -1 * xOffset;

            _wallLeft.transform.position = new Vector3(_wallLeftOriginPosition.x + xOffset, _wallLeftOriginPosition.y, _wallLeftOriginPosition.z);
            _wallRight.transform.position = new Vector3(_wallRightOriginPosition.x + xOffsetRight, _wallRightOriginPosition.y, _wallRightOriginPosition.z);

            yield return null;
        }
        Debug.Log("Ready Finished");
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
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / smashDuration;
            _wallLeft.transform.position = Vector3.Lerp(_wallLeftOriginPosition, leftEndPosition, t);
            _wallRight.transform.position = Vector3.Lerp(_wallRightOriginPosition, rightEndPosition, t);
            yield return null;
        }
        Debug.Log("Smash finished");

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
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rewindDuration;
            _wallLeft.transform.position = Vector3.Lerp(leftStart, leftEndPosition, t);
            _wallRight.transform.position = Vector3.Lerp(rightStart, rightEndPosition, t);
            yield return null;
        }

        // Play finish sound

        Debug.Log("Rewind Finished");

    }



}
