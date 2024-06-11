using UnityEngine;
using Singleton;
using TMPro;
using CustomAttributes;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public enum MenuState {
    GameState,
    Settings,
}


public class InfoBoard : Singleton<InfoBoard>
{

    [Header("Following Settings")]
    public float smoothness = 0.5f;
    public float offset = 75f;

    [Header("Dependencies")]
    //public OVRInputModule ovrInputModule; // Reference to the OVR Input Module
    public TMP_Text _gameStateText;
    public TMP_Text _statsText;
    public TMP_Text _selectedSteeringMethod;
    public TMP_Text _selectedSpeedMethod; 
    #region "internal"
    private float _duration = 0f;

    private Color _originalButtonColor;

    private GameObject _canvasGameObject;
    
    #endregion

    [field: SerializeField, ReadOnlyAttribute] GameObject target;

    [field: SerializeField] private bool showMetrics { get; set; }


    void Awake()
    {
        _canvasGameObject = GetComponentInChildren<Canvas>().gameObject;

    }

    public void SetSteeringButtonState(string sm) {

        Button[] _buttons = GetComponentsInChildren<Button>();

        if (sm.ToUpper() == _selectedSteeringMethod.text.ToUpper()) {
             foreach (Button b in _buttons) {
                Debug.Log(b.gameObject.GetComponentInChildren<TMP_Text>().text.ToUpper());
                if (b.gameObject.GetComponentInChildren<TMP_Text>().text.ToUpper() == _selectedSteeringMethod.text.ToUpper()) {
                    Debug.Log("Set button for " + _selectedSpeedMethod.text + " to active");
                    b.gameObject.GetComponent<ButtonState>().Select();
                    
                }
            }
        }
    }

    public void SetSpeedButtonState(string sm) {
        Button[] _buttons = GetComponentsInChildren<Button>();

        if (sm.ToUpper() == _selectedSpeedMethod.text.ToUpper()) {
             foreach (Button b in _buttons) {
                if (b.gameObject.GetComponentInChildren<TMP_Text>().text.ToUpper() == _selectedSpeedMethod.text.ToUpper()) {
                    Debug.Log("Set button for " + _selectedSpeedMethod.text + " to active");
                    b.gameObject.GetComponent<ButtonState>().Select();
                }
            }
        }
    }



    public void ToggleMenu() {
        Debug.Log("Toggle Menu");
        _canvasGameObject.SetActive(!_canvasGameObject.activeSelf);

    }

     public void SetSpeedMethodText(string text) {
        _selectedSpeedMethod.text = text;
    }

    public void SetSteeringMethodText(string text) {
        _selectedSteeringMethod.text = text;
    }
   
    public void SetGameStateText(string text) {
        _gameStateText.text = text.ToUpper();
    }

    public void SetStatsText(string text) {
        _statsText.text =  text;
    }

    public void FollowMe(GameObject g) {
        target = g;
    }

}
