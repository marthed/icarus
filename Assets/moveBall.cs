using UnityEngine;

public class moveBall : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition=new Vector3(-6.1f , 9.8f , transform.localPosition.z -3 * Time.deltaTime);
    }
}
