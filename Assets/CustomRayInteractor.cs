using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CustomRayInteractor : MonoBehaviour
{
    // Start is called before the first frame update

    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor _rayInteractor;
    void Start()
    {
        _rayInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor>();
        
    }

    public void ToggleRayInteractor() {

        _rayInteractor.enabled = !_rayInteractor.enabled;
    }
}
