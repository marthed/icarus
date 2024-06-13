using UnityEngine;

public class DeathSceneControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject player;

    public float speed = 0.001f;
    void Start()
    {
        if (InfoBoard.Instance.IsMenuOpen())
        {
            InfoBoard.Instance.ToggleMenu();
            player.GetComponentInChildren<CustomRayInteractor>().ToggleRayInteractor();
        }
        player.GetComponent<BroomController>().enabled = false;

        Camera camera = player.GetComponentInChildren<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Color.black;

        
    }

    private void FixedUpdate()
    {
        if (player.transform.position.z < 150)
        {
            player.transform.position = player.transform.position + new Vector3(0, 0, speed);
        }
    }


}
