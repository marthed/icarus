using UnityEngine;

public class DeathSceneControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject _player;

    public MechanicPress mechanicPress;

    public float speed = 0.001f;
    void Start()
    {
        _player = GameObject.Find("Player");
        if (InfoBoard.Instance.IsMenuOpen())
        {
            InfoBoard.Instance.ToggleMenu();
            _player.GetComponentInChildren<CustomRayInteractor>().ToggleRayInteractor();
        }
        _player.GetComponent<BroomController>().enabled = false;

        Camera camera = _player.GetComponentInChildren<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        camera.backgroundColor = Color.black;

        
    }

    private void FixedUpdate()
    {
        if (_player.transform.position.z < 150)
        {
            _player.transform.position = _player.transform.position + new Vector3(0, 0, speed);
        }
        if (mechanicPress.numberOfSmash == 2)
        {
            mechanicPress.StopAllCoroutines();
            mechanicPress.HideWalls();
        }
    }


}
