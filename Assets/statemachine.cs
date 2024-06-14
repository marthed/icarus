using UnityEngine;
using UnityEngine.SceneManagement;

public class statemachine : MonoBehaviour
{
    public Light envLight;
    public float timeCount = 0.0f;
    public AudioSource meadowSound;
    public AudioSource doomSound;
    public bool enableTimer;
    public GameObject target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            enableTimer = true;
        }
        if (enableTimer){
            timeCount += Time.deltaTime;
        }

        if (timeCount < 20.0f){ //STATE 1 beginnig

        }
        if (timeCount > 20.0f && timeCount < 45.0f){ //STATE 2 doom
            if (envLight.colorTemperature > 1500) {
                meadowSound.Stop();
                if(!doomSound.isPlaying){
                    doomSound.Play();
                }
                envLight.colorTemperature = envLight.colorTemperature - 5.0f;
            }
            else {
                envLight.intensity = envLight.intensity - 0.02f;
                if (RenderSettings.fogEndDistance > 1){
                    RenderSettings.fogEndDistance = RenderSettings.fogEndDistance - 2;
                }
            }
        }
        if (timeCount > 50.0f){ //STATE 3 transition to whiteroom
            SceneManager.LoadScene("Death", LoadSceneMode.Single);
        }
        float distance = Vector3.Distance (transform.position, target.transform.position);//
        Debug.Log(distance);
    }
}
