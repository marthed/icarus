using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonState : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    [Header("Selected State Settings")]
    public Color selectedColor = Color.green;
    private Button _button;
    private bool _isSelected = false;
    private Color _originalColor;
    public UnityEvent Enabled;
    void Start()
    {
        _button = GetComponent<Button>();

       _originalColor = _button.image.color;
        
        // Add a listener to handle the button click event
        _button.onClick.AddListener(OnClick);
        

        Debug.Log("Invoke!");

        Enabled?.Invoke();
        
    }

    public void OnPointerEnter(PointerEventData e) {
        if (!_isSelected) {
            _button.image.color = Color.blue;
        }
    }

    public void OnPointerExit(PointerEventData e) {
        if (!_isSelected) {
            _button.image.color = _originalColor;
        }
    }


    public void Select() {
        _isSelected = true;
        UpdateVisualState();
    }


    public void Deselect() {
        _isSelected = false;
        UpdateVisualState();
    }

    private void OnClick()
    {
        if (!_isSelected) {
          _isSelected = true;
          UpdateVisualState();
        }
    }

    private void UpdateVisualState()
    {
        if (_isSelected)
        {
            _button.image.color = selectedColor;
        }
        else
        {
            _button.image.color = _originalColor;
        }
    }
}
