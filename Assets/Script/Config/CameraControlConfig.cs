using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Config
{
    /// <summary>
    /// 摄像机配置
    /// </summary>
    [CreateAssetMenu(fileName = "CameraControlConfig", menuName = "ScriptObjects/Configs/CameraControlConfig")]
    public class CameraControlConfig : ScriptableObject
    {
        /*
        [Header("Followed")]
        public Vector3 FollowedPosition;
        public Vector3 FollowedRotateion;

        [Space]
        [Header("Locked")]
        public Vector3 LockedPosition;
        public Vector3 LockedRotateion;
        */

        /*
        [Header("Target")]
        public Vector3 TargetPosition;
        public Quaternion TargetRotation;

        [Space]
        [Header("Value")]
        public float RotateXSpeed;
        public float RotateYSpeed;
        public float ScaleSpeed;
        public float MinSpeed;
        public float MaxSpeed;
        public float MinDistance;
        public float MaxDistance;
        public float MinAngle;
        public float MaxAngle;
        */

        [Header("数值")]
        public float MoveSpeed;
        public float MinDistance;
        public float MaxDistance;
        public float MinAngle;
        public float MaxAngle;
        public float scaleSpeed;

        [Space]
        [Header("边界设置")]
        public Vector3 AreaBottomLeftPosition;
        public Vector3 AreaTopRightPosition;
    }
}