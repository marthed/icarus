using UnityEngine;
using UnityEngine.SceneManagement;

public class statemachine : MonoBehaviour
{
    public Light envLight;
    public float timeCount = 0.0f;
    public AudioSource meadowSound;
    public AudioSource doomSound;
    public AudioSource woobwoob;
    public AudioSource niceMusic;
    public bool enableTimer;
    public GameObject target;
    public GameObject halfsphere1;
    public GameObject halfsphere2;
    public GameObject mist;
    public GameObject player;
    public bool mistActivated;
    public VRSceneTransition transitionManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool asd = IsSceneInBuildSettings("Death");//
    }

    // Update is called once per frame
    void Update()
    {
        //if (!mistActivated && player.transform.localPosition.y){

        //}
        //if (gameObject.GetComponent<Rigidbody>().linearVelocity > 10 && transform.position.y > 30) {
        //    mist.SetActive(true);
        //}
        //else {
          //  mist.SetActive(false);
        //}


        if (Input.GetKeyDown("space"))
        {
            enableTimer = true;
        }
        if (enableTimer){
            timeCount += Time.deltaTime;
        }

        if (timeCount < 20.0f){ //STATE 1 beginnig

        }
        if (timeCount > 20.0f && timeCount < 55.0f){ //STATE 2 doom
            halfsphere1.SetActive(true);
            halfsphere2.SetActive(true);
            if (envLight.colorTemperature > 1500) {
                meadowSound.Stop();
                niceMusic.volume = 0.1f;
                
                if(!doomSound.isPlaying){
                    doomSound.Play();
                }
                envLight.colorTemperature = envLight.colorTemperature - 5.0f;
            }
            else {
                envLight.intensity = envLight.intensity - 0.02f;
                if (!woobwoob.isPlaying){  
                    woobwoob.Play();
                }   
                if (RenderSettings.fogEndDistance > 30){
                    RenderSettings.fogEndDistance = RenderSettings.fogEndDistance - 30;
                }
            }
        }
        if (timeCount > 70.0f){ //STATE 3 transition to whiteroom
            transitionManager.TransitionToScene("Death");
            timeCount=0;
        }
        float distance = Vector3.Distance (transform.position, target.transform.position);//
        Debug.Log(distance);
        if (distance < 1500.0f && !enableTimer){
            enableTimer = true;
        }
    }

    private bool IsSceneInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            Debug.Log(name);
            if (name == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}
