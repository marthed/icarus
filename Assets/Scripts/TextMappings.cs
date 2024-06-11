using System.Collections;
using UnityEngine;
using Singleton;
using System;
using CustomAttributes;
using GameLogic;

namespace TextMappings {


    public static class MethodsTextMappings {

        public static string SteeringMethodToText(SteeringMethod method) {

            switch (method) {
                case SteeringMethod.PointingMethod:
                 return "Pointing";
                case SteeringMethod.HeadMethod:
                 return "Head";
                case SteeringMethod.LeaningMethod:
                    return "Leaning";
            }
            return "";
        }

         public static string SpeedMethodToText(SpeedMethod method) {

            switch (method) {
                case SpeedMethod.ControllerMethod:
                 return "Controller";
                case SpeedMethod.ControllerHeadMethod:
                 return "Head";
                case SpeedMethod.ControllerBodyMethod:
                    return "Body";
            }
            return "";
        }

    }

}