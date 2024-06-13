using System;
using UnityEngine;
using UnityEngine.UIElements;
using CustomAttributes;
using ControllerMethodClasses;
using UnityEditor;
using TextMappings;
using UnityEngine.InputSystem;

public enum SteeringMethod {
    PointingMethod,
    LeaningMethod,
    HeadMethod
}

public enum SpeedMethod {
    ControllerMethod,
    ControllerHeadMethod,
    ControllerBodyMethod
}

public enum Invert {
    yes = -1,
    no = 1,
}

public class BroomController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Invert Settings")]

    public Invert invertHorizontal = Invert.no;
    public Invert invertLateral = Invert.no;
    public Invert invertVertical = Invert.no;
    public Invert invertForward = Invert.no;

    [Tooltip("If true, previous locomotion is used to compute current locomoton")]
    public bool usePrevious = true;

 


    public SpeedMethod selectedSpeedMethod = SpeedMethod.ControllerMethod;

    private SpecificSpeedMethod _speedMethod { get {
        if (selectedSpeedMethod == SpeedMethod.ControllerMethod) {
            return ControllerMethod;
        }
        else if (selectedSpeedMethod == SpeedMethod.ControllerHeadMethod) {
            return ControllerHeadMethod;
        }
        else {
            return ControllerBodyMethod;
        }
    }}

    [SerializeField] public ControllerMethod ControllerMethod;
    [SerializeField] public ControllerHeadMethod ControllerHeadMethod;
    [SerializeField] public ControllerBodyMethod ControllerBodyMethod;


    [Header("Steering Method")]

    public SteeringMethod selectedSteeringMethod = SteeringMethod.PointingMethod;

    private SpecificMethod _steeringMethod { get {
        if (selectedSteeringMethod == SteeringMethod.PointingMethod) {
            return PointingMethod;
        }
        else if (selectedSteeringMethod == SteeringMethod.LeaningMethod) {
            return LeaningMethod;
        }
        else {
            return HeadMethod;
        }
    }}

    [SerializeField] public PointingMethod PointingMethod;
    [SerializeField] public LeaningMethod LeaningMethod;
    [SerializeField] public HeadMethod HeadMethod;

    public OVRInput.Button OffsetButton = OVRInput.Button.None;


    [Header("Dependencies")]
    public Camera head;
    public Transform localReference;
    public InputActionAsset actions;

    #region  "private"

    private InputAction _allowMovement;
    private InputAction _setOffset;

    private InputAction _rightControllerPosition;

    private InputAction _rightControllerRotation;
    private Vector3 _offset;

    #endregion

    void Start()
    {
        _offset = head.transform.localPosition - localReference.localPosition; 

        _allowMovement = actions.FindActionMap("XRI RightHand Locomotion").FindAction("Grab Move");
        if (_allowMovement != null) {
            Debug.Log("allow move");
        }
        
        _setOffset = actions.FindActionMap("XRI RightHand Interaction").FindAction("Offset");

        if (_setOffset != null) {
            Debug.Log("allow offset");
        }
        _rightControllerPosition = actions.FindActionMap("XRI RightHand").FindAction("Position");
        _rightControllerRotation = actions.FindActionMap("XRI RightHand").FindAction("Rotation");

        InfoBoard.Instance.FollowMe(gameObject);
    }

    public void UseControllerSpeed() {
        selectedSpeedMethod = SpeedMethod.ControllerMethod;
        InfoBoard.Instance.SetSpeedMethodText("Controller");
    }

    public void UseHeadSpeed() {
        selectedSpeedMethod = SpeedMethod.ControllerHeadMethod;
        InfoBoard.Instance.SetSpeedMethodText("Head");
    }

    public void UseBodySpeed() {
        selectedSpeedMethod = SpeedMethod.ControllerBodyMethod;
        InfoBoard.Instance.SetSpeedMethodText("Body");
    }

    public void UsePointerSteering() {
        selectedSteeringMethod = SteeringMethod.PointingMethod;
        InfoBoard.Instance.SetSteeringMethodText("Pointing");
    }
    public void UseHeadSteering() {
        selectedSteeringMethod = SteeringMethod.HeadMethod;
        InfoBoard.Instance.SetSpeedMethodText("Head");
    }
    public void UseLeanSteering() {
        selectedSteeringMethod = SteeringMethod.LeaningMethod;
        InfoBoard.Instance.SetSpeedMethodText("Leaning");
    }
    public void SetInvertHorizontal(bool b) {
        if (b) {
            invertHorizontal = Invert.yes;
        }
        else {
            invertHorizontal = Invert.no;
        }
    }

     public void SetInvertLateral(bool b) {
        if (b) {
            invertLateral = Invert.yes;
        }
        else {
            invertLateral = Invert.no;
        }
    }
    public void SetInvertVertical(bool b) {
        if (b) {
            invertVertical = Invert.yes;
        }
        else {
            invertVertical = Invert.no;
        }
    }

    public void SetInvertForward(bool b) {
        if (b) {
            invertForward = Invert.yes;
        }
        else {
            invertForward = Invert.no;
        }
    }

    void FixedUpdate()
    {

        // OVRInput.Get(OVRInput.Button.One

        if (_allowMovement.IsPressed())
        {
            SetHorizontalMovement();
            SetLateralMovement();
            SetVerticalMovement();
            SetForwardMovement();
            
        }
        else if (_setOffset.IsPressed()){
            // OVRInput.Get(OffsetButton)
            Debug.Log("Set offset");
            _offset = head.transform.localPosition - localReference.localPosition; 
        }

        
        
    }


    void SetHorizontalMovement() {
        InputMode inputMode = _steeringMethod.HorizontalInputMode;
        InputSource inputSource = _steeringMethod.HorizontalInputSource;
        RotationAxis axis = _steeringMethod.HorizontaltAxis;
        OutputMode outputMode = _steeringMethod.HorizontalOutputMode;
        Vector3 outputAxis = transform.up;
        float value = 0;

        if (inputMode == InputMode.Angle) {
            Quaternion input = GetInputOrientation(inputSource);
            value = GetAngle(input, axis);
        }
        else if (inputMode == InputMode.Distance) { 
            Vector3 input = GetInputPosition(inputSource);
            value = GetDistance(input, axis);
        }
        else {
            return;
        }

        float sensitivity;
        if (usePrevious) {
            sensitivity = _steeringMethod.rotationSensitivityUsePrevious;
        }
        else {
            sensitivity = _steeringMethod.rotationSensitivity;
        }

        value = (float) invertHorizontal * value;

        TranslateOrRotate(outputMode, outputAxis, value, sensitivity);

    }

    void SetLateralMovement() {
        InputMode inputMode = _steeringMethod.LateralInputMode;
        InputSource inputSource = _steeringMethod.LateralInputSource;
        RotationAxis axis = _steeringMethod.LateralAxis;
        OutputMode outputMode = _steeringMethod.LateralOutputMode;
        Vector3 outputAxis = transform.right;
        
        float value;

        if (inputMode == InputMode.Angle) {
            Quaternion input = GetInputOrientation(inputSource);
            value = GetAngle(input, axis);
        }
        else if (inputMode == InputMode.Distance) {
            Vector3 input = GetInputPosition(inputSource);
            value = GetDistance(input, axis);
        }
        else {
            return;
        }

        float sensitivity;
        if (usePrevious) {
            sensitivity = _steeringMethod.lateralSensitivityUsePrevious;
        }
        else {
            sensitivity = _steeringMethod.lateralSensitivity;
        }

        value = (float) invertLateral * value;

        TranslateOrRotate(outputMode, outputAxis, value, sensitivity);

    }

    void SetVerticalMovement() {
        InputMode inputMode = _steeringMethod.VerticalInputMode;
        InputSource inputSource = _steeringMethod.VerticalInputSource;
        RotationAxis axis = _steeringMethod.VerticalAxis;
        
        float value;

        if (inputMode == InputMode.Angle) {
            Quaternion input = GetInputOrientation(inputSource);
            value = GetAngle(input, axis);
        }
        else if (inputMode == InputMode.Distance) {
            Vector3 input = GetInputPosition(inputSource);
            value = GetDistance(input, axis);
        }
        else {
            return;
        }

        float sensitivity;
        if (usePrevious) {
            sensitivity = _steeringMethod.upDownSensitivityUsePrevious;
        }
        else {
            sensitivity = _steeringMethod.upDownSensitivity;
        }

        value = (float) invertVertical * value;

        TranslateOrRotate(_steeringMethod.VerticalOutputMode, transform.up, value, sensitivity);
    }


    void SetForwardMovement() {

        float rawInput = 0;

        if (selectedSpeedMethod == SpeedMethod.ControllerMethod) {
            //rawInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
            rawInput = _allowMovement.ReadValue<float>();
            
        }
        else if (selectedSpeedMethod == SpeedMethod.ControllerHeadMethod) {
            Vector3 controllerPosition = GetInputPosition(InputSource.Controller);
            Vector3 headPosition = GetInputPosition(InputSource.Head);

            rawInput =  Mathf.Clamp(1.2f - (headPosition - controllerPosition).magnitude, 0, 1);
        }
        else {
            Vector3 controllerPosition = GetInputPosition(InputSource.Controller);
            rawInput = GetDistance(controllerPosition, RotationAxis.Roll);
        }

        rawInput = (float) invertForward * rawInput;

        if (usePrevious) {
              Translate(rawInput, transform.forward, _speedMethod.usePreviousAmplification);
            }
            else {
                Translate(rawInput, transform.forward, _speedMethod.amplification);
        }
    }
    void TranslateOrRotate(OutputMode outputMode, Vector3 outputAxis, float value, float sensitivity) {

        if (outputMode == OutputMode.Angle) {
            Rotate(value, outputAxis, sensitivity);
        }
        else {
            Translate(value, outputAxis, sensitivity);
        }
    }

    Quaternion GetInputOrientation(InputSource sourceType) {
        if (sourceType == InputSource.Controller) {
           // return OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
           return _rightControllerRotation.ReadValue<Quaternion>();
        }
        else if (sourceType == InputSource.Head) {
            return head.transform.localRotation;
        }
        else {
            return Quaternion.identity;
        }
    }

    Vector3 GetInputPosition(InputSource sourceType) {
        if (sourceType == InputSource.Controller) {
            //return OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            return _rightControllerPosition.ReadValue<Vector3>();
        }
        else if (sourceType == InputSource.Head) {
            return head.transform.localPosition;
        }
        else {
            return Vector3.zero;
        }
    }

    float GetAngle(Quaternion inputLocalOrientation, RotationAxis axis) {
        if (axis == RotationAxis.Yaw) {

            return  Quaternion.Angle(
            Quaternion.Euler(0f, localReference.localEulerAngles.y, 0f),
            Quaternion.Euler(0f, inputLocalOrientation.eulerAngles.y, 0f)
            ) *  Mathf.Sign(inputLocalOrientation.y);
        }
        else if (axis == RotationAxis.Pitch) {
            return Quaternion.Angle(
            Quaternion.Euler(localReference.localEulerAngles.x, 0f, 0f),
            Quaternion.Euler(inputLocalOrientation.eulerAngles.x,0f, 0f)
            ) *  -1 * Mathf.Sign(inputLocalOrientation.x);
        }
        else if (axis == RotationAxis.Roll) {
            return  Quaternion.Angle(
            Quaternion.Euler(0f, 0f, localReference.localEulerAngles.z),
            Quaternion.Euler(0f, 0f, inputLocalOrientation.eulerAngles.z)
            ) *  Mathf.Sign(inputLocalOrientation.z);
        }
        return 0;
    }

   float GetDistance(Vector3 inputLocalPosition, RotationAxis axis) {

        if (axis == RotationAxis.Yaw) {
            return inputLocalPosition.y - localReference.localPosition.y;
        }
        else if (axis == RotationAxis.Pitch) {
            return inputLocalPosition.x - localReference.localPosition.x;
        }
        else if (axis == RotationAxis.Roll) {
            return  inputLocalPosition.z - localReference.localPosition.z;
        }
        return 0;
    }



    void Rotate(float amount, Vector3 axis, float sensitivity) {

        float frameScalar = 70 * Time.fixedDeltaTime;

        if (usePrevious) {
            gameObject.GetComponent<Rigidbody>().AddTorque(axis * amount * sensitivity * frameScalar);
        }
        else {
            transform.Rotate(axis, amount * sensitivity * frameScalar, Space.World);
        }
     }

    void Translate(float amount, Vector3 axis, float sensitivity) {

        float frameScalar = 70 * Time.fixedDeltaTime;

        if (usePrevious) {
            gameObject.GetComponent<Rigidbody>().AddForce(axis * amount * sensitivity * frameScalar);
        }
        else {
            transform.Translate(axis * amount * sensitivity * frameScalar, Space.World);
        }

     }
    




}
