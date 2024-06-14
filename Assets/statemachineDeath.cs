using UnityEngine;
using UnityEngine.SceneManagement;

public class statemachineDeath : MonoBehaviour
{
    public float timeCount = 0.0f;
    public VRSceneTransition transitionManager;
    public AudioSource scream;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        if (timeCount > 49.0f && !scream.isPlaying && timeCount < 49.5f ){ //STATE 3 transition to whiteroom
            scream.Play();
            Debug.Log("Screeaam");
        }
        if (timeCount > 52.0f){ //STATE 3 transition to whiteroom
            transitionManager.TransitionToScene("WhiteRoom");//
        }
    }
}
