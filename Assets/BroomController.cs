using System;
using UnityEngine;
using UnityEngine.UIElements;
using CustomAttributes;
using ControllerMethodClasses;
using UnityEditor;
using TextMappings;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public enum SteeringMethod
{
    PointingMethod,
    LeaningMethod,
    HeadMethod
}

public enum SpeedMethod
{
    ControllerMethod,
    ControllerHeadMethod,
    ControllerBodyMethod
}

public enum Invert
{
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

    public bool allowMovement = false;

    public float sensitivity = 1f;

    public float verticalSensitivity = 1f;



    public SpeedMethod selectedSpeedMethod = SpeedMethod.ControllerMethod;

    private SpecificSpeedMethod _speedMethod
    {
        get
        {
            if (selectedSpeedMethod == SpeedMethod.ControllerMethod)
            {
                return ControllerMethod;
            }
            else if (selectedSpeedMethod == SpeedMethod.ControllerHeadMethod)
            {
                return ControllerHeadMethod;
            }
            else
            {
                return ControllerBodyMethod;
            }
        }
    }

    [SerializeField] public ControllerMethod ControllerMethod;
    [SerializeField] public ControllerHeadMethod ControllerHeadMethod;
    [SerializeField] public ControllerBodyMethod ControllerBodyMethod;


    [Header("Steering Method")]

    public SteeringMethod selectedSteeringMethod = SteeringMethod.PointingMethod;

    private SpecificMethod _steeringMethod
    {
        get
        {
            if (selectedSteeringMethod == SteeringMethod.PointingMethod)
            {
                return PointingMethod;
            }
            else if (selectedSteeringMethod == SteeringMethod.LeaningMethod)
            {
                return LeaningMethod;
            }
            else
            {
                return HeadMethod;
            }
        }
    }

    [SerializeField] public PointingMethod PointingMethod;
    [SerializeField] public LeaningMethod LeaningMethod;
    [SerializeField] public HeadMethod HeadMethod;

    public OVRInput.Button OffsetButton = OVRInput.Button.None;


    [Header("Dependencies")]
    public Camera head;
    public Transform localReference;
    public InputActionAsset actions;
    public Transform rightHand;

    #region  "private"

    private InputAction _allowMovement;
    private InputAction _setOffset;
    private InputAction _locomotionIsActive;

    private InputAction _rightControllerPosition;

    private InputAction _rightControllerRotation;
    private Vector3 _offset;
    //private Vector3 _rightHandOffset;
    private AudioSource _audioSource;

    #endregion

    void Start()
    {
        _offset = head.transform.localPosition - localReference.localPosition;

        _allowMovement = actions.FindActionMap("XRI RightHand Locomotion").FindAction("Grab Move");
        if (_allowMovement != null)
        {
            Debug.Log("allow move");
        }

        _setOffset = actions.FindActionMap("XRI RightHand Interaction").FindAction("Offset");

        if (_setOffset != null)
        {
            Debug.Log("allow offset");
        }
        _rightControllerPosition = actions.FindActionMap("XRI RightHand").FindAction("Position");
        _rightControllerRotation = actions.FindActionMap("XRI RightHand").FindAction("Rotation");



        InfoBoard.Instance.FollowMe(gameObject);
    }

    private void OnEnable()
    {
        _locomotionIsActive = actions.FindActionMap("XRI RightHand Interaction").FindAction("Toggle Locomotion");
        _locomotionIsActive.performed += ToggleLocomotion;
        Debug.Log("shdfiushdfiudsf");
    }

    private void OnDisable()
    {
        _locomotionIsActive.performed -= ToggleLocomotion;
    }

    public void ToggleLocomotion(InputAction.CallbackContext context)
    {
        Debug.Log("aLLOW LOCOMOTION");
        allowMovement = !allowMovement;
    }

    public void UseControllerSpeed()
    {
        selectedSpeedMethod = SpeedMethod.ControllerMethod;
        InfoBoard.Instance.SetSpeedMethodText("Controller");
    }

    public void UseHeadSpeed()
    {
        selectedSpeedMethod = SpeedMethod.ControllerHeadMethod;
        InfoBoard.Instance.SetSpeedMethodText("Head");
    }

    public void UseBodySpeed()
    {
        selectedSpeedMethod = SpeedMethod.ControllerBodyMethod;
        InfoBoard.Instance.SetSpeedMethodText("Body");
    }

    public void UsePointerSteering()
    {
        selectedSteeringMethod = SteeringMethod.PointingMethod;
        InfoBoard.Instance.SetSteeringMethodText("Pointing");
    }
    public void UseHeadSteering()
    {
        selectedSteeringMethod = SteeringMethod.HeadMethod;
        InfoBoard.Instance.SetSpeedMethodText("Head");
    }
    public void UseLeanSteering()
    {
        selectedSteeringMethod = SteeringMethod.LeaningMethod;
        InfoBoard.Instance.SetSpeedMethodText("Leaning");
    }
    public void SetInvertHorizontal(bool b)
    {
        if (b)
        {
            invertHorizontal = Invert.yes;
        }
        else
        {
            invertHorizontal = Invert.no;
        }
    }

    public void SetInvertLateral(bool b)
    {
        if (b)
        {
            invertLateral = Invert.yes;
        }
        else
        {
            invertLateral = Invert.no;
        }
    }
    public void SetInvertVertical(bool b)
    {
        if (b)
        {
            invertVertical = Invert.yes;
        }
        else
        {
            invertVertical = Invert.no;
        }
    }

    public void SetInvertForward(bool b)
    {
        if (b)
        {
            invertForward = Invert.yes;
        }
        else
        {
            invertForward = Invert.no;
        }
    }

    Vector3 previousPosHead = new Vector3 (0, 0, 0);

    void FixedUpdate()
    {

        // OVRInput.Get(OVRInput.Button.One

        if (allowMovement)
        {
            //SetHorizontalMovement();
            //SetLateralMovement();
            // SetVerticalMovement();
            //SetForwardMovement();
            Vector3 leaning = head.transform.localPosition - _offset;
            Vector3 leaningNoVert = new Vector3(leaning.x, 0, leaning.z);


            float magnitude = leaningNoVert.magnitude;

            float magnitudeVert = leaning.y;


            Translate(magnitude, leaningNoVert, sensitivity, false);
            Translate(magnitudeVert, transform.up, verticalSensitivity, true);




        }
        else if (_setOffset.IsPressed())
        {
            // OVRInput.Get(OffsetButton)
            Debug.Log("Set offset");
            _offset = head.transform.localPosition - localReference.localPosition;
        }



    }


    void SetHorizontalMovement()
    {
        if (selectedSteeringMethod == SteeringMethod.LeaningMethod)
        {
            // Horizontal handled by physical rotations
            return;
        }
        InputMode inputMode = _steeringMethod.HorizontalInputMode;
        InputSource inputSource = _steeringMethod.HorizontalInputSource;
        RotationAxis axis = _steeringMethod.HorizontaltAxis;
        OutputMode outputMode = _steeringMethod.HorizontalOutputMode;
        Vector3 outputAxis = transform.up;
        float value = 0;

        if (inputMode == InputMode.Angle)
        {
            Quaternion input = GetInputOrientation(inputSource);
            value = GetAngle(input, axis);
        }
        else if (inputMode == InputMode.Distance)
        {
            Vector3 input = GetInputPosition(inputSource);
            value = GetDistance(input, axis);
        }
        else
        {
            return;
        }

        float sensitivity;
        if (usePrevious)
        {
            sensitivity = _steeringMethod.rotationSensitivityUsePrevious;
        }
        else
        {
            sensitivity = _steeringMethod.rotationSensitivity;
        }

        value = (float)invertHorizontal * value;

        TranslateOrRotate(outputMode, outputAxis, value, sensitivity);

    }

    void SetLateralMovement()
    {
        InputMode inputMode = _steeringMethod.LateralInputMode;
        InputSource inputSource = _steeringMethod.LateralInputSource;
        RotationAxis axis = _steeringMethod.LateralAxis;
        OutputMode outputMode = _steeringMethod.LateralOutputMode;
        Vector3 outputAxis;


        float value;

        if (inputMode == InputMode.Angle)
        {
            Quaternion input = GetInputOrientation(inputSource);
            value = GetAngle(input, axis);
            outputAxis = transform.right;
        }
        else if (inputMode == InputMode.Distance)
        {
            Vector3 input = GetInputPosition(inputSource);
            value = GetDistance(input, axis);
            if (selectedSteeringMethod == SteeringMethod.LeaningMethod)
            {
                outputAxis = new Vector3(rightHand.right.x, 0, rightHand.right.z).normalized;
            }
            else
            {
                outputAxis = transform.right;
            }
        }
        else
        {
            return;
        }

        float sensitivity;
        if (usePrevious)
        {
            sensitivity = _steeringMethod.lateralSensitivityUsePrevious;
        }
        else
        {
            sensitivity = _steeringMethod.lateralSensitivity;
        }

        value = (float)invertLateral * value;

       // Vector3 axis = outputAxis;

        //if ()

        //axis = new Vector3(0, 0, _rightControllerRotation.ReadValue<Quaternion>().eulerAngles.z).normalized;

        //Vector3 newAxis = new Vector3(0, _rightControllerRotation.ReadValue<Quaternion>().eulerAngles.y, 0).normalized;


        TranslateOrRotate(outputMode, outputAxis, value, sensitivity);

    }

    void SetVerticalMovement()
    {
        InputMode inputMode = _steeringMethod.VerticalInputMode;
        InputSource inputSource = _steeringMethod.VerticalInputSource;
        RotationAxis axis = _steeringMethod.VerticalAxis;

        float value;

        if (inputMode == InputMode.Angle)
        {
            Quaternion input = GetInputOrientation(inputSource);
            value = GetAngle(input, axis);
        }
        else if (inputMode == InputMode.Distance)
        {
            Vector3 input = GetInputPosition(inputSource);
            value = GetDistance(input, axis);
        }
        else
        {
            return;
        }

        float sensitivity;
        if (usePrevious)
        {
            sensitivity = _steeringMethod.upDownSensitivityUsePrevious;
        }
        else
        {
            sensitivity = _steeringMethod.upDownSensitivity;
        }

        value = (float)invertVertical * value;

        TranslateOrRotate(_steeringMethod.VerticalOutputMode, transform.up, value, sensitivity);
    }


    void SetForwardMovement()
    {

        float rawInput = 0;
        Vector3 axis;

        if (selectedSpeedMethod == SpeedMethod.ControllerMethod)
        {
            //rawInput = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger);
            rawInput = _allowMovement.ReadValue<float>();
            axis = rightHand.forward; // Output of forward is forward axis of controller

        }
        else if (selectedSpeedMethod == SpeedMethod.ControllerHeadMethod)
        {
            Vector3 controllerPosition = GetInputPosition(InputSource.Controller);
            Vector3 headPosition = GetInputPosition(InputSource.Head);

            rawInput = Mathf.Clamp(1.2f - (headPosition - controllerPosition).magnitude, 0, 1);
            axis = transform.forward;
        }
        else
        {
            Vector3 headPosition = GetInputPosition(InputSource.Head);


            //axis = new Vector3(0, 0, _rightControllerRotation.ReadValue<Quaternion>().eulerAngles.z).normalized; // Output of forward is forward axis of controller
            axis = new Vector3(rightHand.forward.x, 0, rightHand.forward.z).normalized;


           rawInput = GetDistance(headPosition, RotationAxis.Roll);

        }

        rawInput = (float)invertForward * rawInput;


        if (usePrevious)
        {
            Translate(rawInput, axis, _speedMethod.usePreviousAmplification, false);
        }
        else
        {
            Translate(rawInput, axis, _speedMethod.amplification, false);
        }
    }
    void TranslateOrRotate(OutputMode outputMode, Vector3 outputAxis, float value, float sensitivity)
    {

        if (outputMode == OutputMode.Angle)
        {
            Rotate(value, outputAxis, sensitivity);
        }
        else
        {
            Translate(value, outputAxis, sensitivity, false);
        }
    }

    Quaternion GetInputOrientation(InputSource sourceType)
    {
        if (sourceType == InputSource.Controller)
        {
            // return OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);
            return _rightControllerRotation.ReadValue<Quaternion>();
        }
        else if (sourceType == InputSource.Head)
        {
            return head.transform.localRotation;
        }
        else
        {
            return Quaternion.identity;
        }
    }

    Vector3 GetInputPosition(InputSource sourceType)
    {
        if (sourceType == InputSource.Controller)
        {
            //return OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
            return _rightControllerPosition.ReadValue<Vector3>();
        }
        else if (sourceType == InputSource.Head)
        {
            return head.transform.localPosition;
        }
        else
        {
            return Vector3.zero;
        }
    }


    float GetAngle(Quaternion inputLocalOrientation, RotationAxis axis)
    {
        if (axis == RotationAxis.Yaw)
        {

            return Quaternion.Angle(
            Quaternion.Euler(0f, localReference.localEulerAngles.y, 0f),
            Quaternion.Euler(0f, inputLocalOrientation.eulerAngles.y, 0f)
            ) * Mathf.Sign(inputLocalOrientation.y);
        }
        else if (axis == RotationAxis.Pitch)
        {
            return Quaternion.Angle(
            Quaternion.Euler(localReference.localEulerAngles.x, 0f, 0f),
            Quaternion.Euler(inputLocalOrientation.eulerAngles.x, 0f, 0f)
            ) * -1 * Mathf.Sign(inputLocalOrientation.x);
        }
        else if (axis == RotationAxis.Roll)
        {
            return Quaternion.Angle(
            Quaternion.Euler(0f, 0f, localReference.localEulerAngles.z),
            Quaternion.Euler(0f, 0f, inputLocalOrientation.eulerAngles.z)
            ) * Mathf.Sign(inputLocalOrientation.z);
        }
        return 0;
    }

    float GetDistance(Vector3 inputLocalPosition, RotationAxis axis)
    {

        if (axis == RotationAxis.Yaw)
        {
            //Debug.Log("localReference position: " + _offset.y);
            //Debug.Log("input localPosition: " + inputLocalPosition.z);
            float diff = inputLocalPosition.y - _offset.y;
            float sign = Mathf.Sign(diff);
            Debug.Log(diff);
            return sign * ( (sign + diff) * (sign + diff));
        }
        else if (axis == RotationAxis.Pitch)
        {
            if (selectedSteeringMethod == SteeringMethod.LeaningMethod)
            {
                Vector2 rightHandPosition = new Vector3(rightHand.localPosition.x, rightHand.localPosition.z);
                Vector2 originPosition = new Vector3(_offset.x, _offset.z);
                Vector2 headPosition = new Vector3(head.transform.localPosition.x, head.transform.localPosition.z);

                Vector2 distance = headPosition - originPosition;
                Vector2 distanceToController = (rightHandPosition - originPosition).normalized;
                Vector2 rotatedDistanceToController = new Vector2(distanceToController.y, -distanceToController.x);

                float dotProduct = Vector2.Dot(distance, rotatedDistanceToController);
                float magnitudeSquared = Vector2.Dot(rotatedDistanceToController, rotatedDistanceToController);

                Vector2 projection = (dotProduct / magnitudeSquared) * rotatedDistanceToController;

                float projectionSign = Mathf.Sign(dotProduct);

                return projectionSign * projection.magnitude;
            }

            return inputLocalPosition.x - localReference.localPosition.x;
        }
        else if (axis == RotationAxis.Roll)
        {
            if (selectedSteeringMethod == SteeringMethod.LeaningMethod)
            {
                Vector2 rightHandPosition = new Vector3(rightHand.localPosition.x, rightHand.localPosition.z);
                Vector2 originPosition = new Vector3(_offset.x, _offset.z);
                Vector2 headPosition = new Vector3(head.transform.localPosition.x, head.transform.localPosition.z);

                Vector2 distance = headPosition - originPosition;
                Vector2 distanceToController = (rightHandPosition - originPosition).normalized;

                float dotProduct = Vector2.Dot(distance, distanceToController);
                float magnitudeSquared = Vector2.Dot(distanceToController, distanceToController);

                Vector2 projection = (dotProduct / magnitudeSquared) * distanceToController;


                return projection.magnitude;
            }

            return inputLocalPosition.z - localReference.localPosition.z;
        }
        return 0;
    }



    void Rotate(float amount, Vector3 axis, float sensitivity)
    {

        float frameScalar = 70 * Time.fixedDeltaTime;

        if (usePrevious)
        {
            gameObject.GetComponent<Rigidbody>().AddTorque(axis * amount * sensitivity * frameScalar);
        }
        else
        {
            transform.Rotate(axis, amount * sensitivity * frameScalar, Space.World);
        }
    }

    void Translate(float amount, Vector3 axis, float sensitivity, bool combine)
    {

        float frameScalar = 70 * Time.fixedDeltaTime;

        if (usePrevious)
        {

            if (combine)
            {
                gameObject.GetComponent<Rigidbody>().linearVelocity += amount * sensitivity * frameScalar * axis;
            }
            else
            {
                gameObject.GetComponent<Rigidbody>().linearVelocity = amount * sensitivity * frameScalar * axis;
            }




        }
        else
        {
            transform.Translate(amount * sensitivity * frameScalar * axis, Space.World);
        }

    }





}
