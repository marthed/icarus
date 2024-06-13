using UnityEngine;

public class moveBall : MonoBehaviour
{
    public GameObject player;
    public AudioSource woobwoob;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition=new Vector3(-6.1f , 9.8f , transform.localPosition.z -3 * Time.deltaTime);
        float distance = Vector3.Distance (transform.position, player.transform.position);
        Debug.Log(distance);
        if (distance < 100 && !woobwoob.isPlaying){
            woobwoob.Play();
        }
    }
}
