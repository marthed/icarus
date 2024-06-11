using System;
using UnityEngine;
using UnityEditor;

[Serializable] public enum RotationAxis {
    [SerializeField] Roll,
    [SerializeField] Pitch,
    [SerializeField] Yaw
}

[Serializable] public enum InputSource {
    [SerializeField] Controller,
    [SerializeField] Head
}

[Serializable] public enum InputMode {
    [SerializeField] Angle,
    [SerializeField] Distance
}

[Serializable] public enum OutputMode {
    [SerializeField] Angle,
    [SerializeField] Distance
}


namespace ControllerMethodClasses {

    public interface IBaseController  {
    
        public bool UsePrevious {get; set;}
        public float Speed { get; set; }
        public float UsePreviousSpeed { get; set; }
    
    }

    public interface ISpecificController {
        public InputSource HorizontalInputSource  { get; set; }
        public InputMode HorizontalInputMode { get; set; }
        public OutputMode HorizontalOutputMode { get; set; } 
        public RotationAxis HorizontaltAxis  { get; set; }
        public float rotationSensitivity  { get; set; }
        public float rotationSensitivityUsePrevious  { get; set; }

        public InputSource LateralInputSource  { get; set; }
        public RotationAxis LateralAxis  { get; set; }
        public InputMode LateralInputMode { get; set; } 

        public OutputMode LateralOutputMode { get; set; } 
        public float lateralSensitivity  { get; set; }
        public float lateralSensitivityUsePrevious  { get; set; }


        public InputSource VerticalInputSource { get; set; }
        public InputMode VerticalInputMode { get; set; }
        public OutputMode VerticalOutputMode { get; set; } 
        public RotationAxis VerticalAxis { get; set; }
        public float upDownSensitivity { get; set; }
        public float upDownSensitivityUsePrevious  { get; set; }
    }

    [Serializable] public class BaseMethod : ScriptableObject, IBaseController {
         [field: SerializeField] public bool UsePrevious {get; set;}
         [field: SerializeField] public float Speed { get; set; }
         [field: SerializeField] public float UsePreviousSpeed { get; set; }
    }

    [Serializable] public class SpecificMethod : ISpecificController {

        [Header("Horizontal Rotation")]
        private float hor = 1;
        [field: SerializeField] public InputSource HorizontalInputSource  { get; set; }
        [field: SerializeField] public InputMode HorizontalInputMode { get; set; } 
        [field: SerializeField] public OutputMode HorizontalOutputMode { get; set; } 
        [field: SerializeField] public RotationAxis HorizontaltAxis  { get; set; }
        [field: SerializeField] public float rotationSensitivity  { get; set; }
        [field: SerializeField] public float rotationSensitivityUsePrevious  { get; set; }

        [Header("Horizontal Lateral")]
        private float hor_lat = 1;
        [field: SerializeField] public InputSource LateralInputSource  { get; set; }
        [field: SerializeField] public InputMode LateralInputMode { get; set; } 
         [field: SerializeField] public OutputMode LateralOutputMode { get; set; } 
        [field: SerializeField] public RotationAxis LateralAxis  { get; set; }
        [field: SerializeField] public float lateralSensitivity  { get; set; }
        [field: SerializeField] public float lateralSensitivityUsePrevious  { get; set; }

         [Header("Vertical")]
        private float vert = 1;
        [field: SerializeField] public InputSource VerticalInputSource { get; set; }
        [field: SerializeField] public RotationAxis VerticalAxis { get; set; }
        [field: SerializeField] public InputMode VerticalInputMode { get; set; } 
                [field: SerializeField] public OutputMode VerticalOutputMode { get; set; } 
        [field: SerializeField] public float upDownSensitivity { get; set; }
        [field: SerializeField] public float upDownSensitivityUsePrevious  { get; set; }
    }

    [Serializable] public class PointingMethod : SpecificMethod {

    }
    [Serializable] public class LeaningMethod : SpecificMethod {
        
    }
    [Serializable] public class HeadMethod : SpecificMethod {
        
    }

    public interface ISpecificSpeedMethod {
        public float amplification { get; set; }
        public float usePreviousAmplification { get; set; }

    }

    [Serializable] public class SpecificSpeedMethod : ISpecificSpeedMethod {
        [field: SerializeField ]public float amplification { get; set; }
        [field: SerializeField] public float usePreviousAmplification { get; set; }
    }

    [Serializable] public class ControllerMethod : SpecificSpeedMethod {

    }

    [Serializable] public class ControllerHeadMethod : SpecificSpeedMethod {

    }

    [Serializable] public class ControllerBodyMethod : SpecificSpeedMethod {

    }




}

