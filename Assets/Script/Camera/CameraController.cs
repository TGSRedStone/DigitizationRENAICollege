using System.Collections;
using System.Collections.Generic;
using Tool;
using UnityEngine;
using Config;
using System;

namespace CameraController
{
    internal struct Limit
    {
        public float min;
        public float max;
    }

    [Obsolete]
    public class CameraController : MonoBehaviour
    {
        #region
        /// <summary>
        /// 摄像机目标点
        /// </summary>
        private Transform _targetTrans;

        private CameraControlConfig _cameraControllerConfig;
        private readonly string configPath = "Configs/CameraControllerConfig";

        private float moveSpeed;
        private float rotateXSpeed;
        private float rotateYSpeed;
        private float scaleSpeed;
        private float distance;
        private Limit speedLimit;
        /// <summary>
        /// 与_followedGo的距离限制
        /// </summary>
        private Limit distanceLimit;
        /// <summary>
        /// _lockedGo的角度限制
        /// </summary>
        private Limit rotateLimit;

        private float mouse_X;
        private float mouse_Y;
        private float mouse_Wheel;
        #endregion

        private void Awake()
        {
            _targetTrans = GameObject.Find("Target").GetComponent<Transform>();

            _cameraControllerConfig = Resources.Load<CameraControlConfig>(configPath);
        }

        private void Start()
        {
            ValueInit();

            //_targetTrans.position = _cameraControllerConfig.TargetPosition;
            //_targetTrans.rotation = _cameraControllerConfig.TargetRotation;
        }

        private void Update()
        {
            mouse_X = Input.GetAxis("Mouse X");
            mouse_Y = Input.GetAxis("Mouse Y");
            mouse_Wheel = Input.GetAxis("Mouse ScrollWheel");

            /*
            if (Input.GetMouseButton(0))
            {
                _followed.position += (_followed.forward * -mouse_Y + _followed.right * -mouse_X) * mouse_Move_Multiple;
            }
            if (Input.GetMouseButton(1))
            {
                _follow.rotation *= Quaternion.Euler(0f, Mathf.Clamp(mouse_X * mouse_Rotate_Multiple, -360f / Time.deltaTime, 360f / Time.deltaTime), 0f);
            }

            distance += -mouse_Wheel * mouse_Wheel_Multiple;
            distance = Mathf.Clamp(distance, 20f, 100f);
            */

            if (Input.GetMouseButton(0))
            {
                if (mouse_X != 0f && mouse_Y != 0f)
                {
                    //Vector3 moveDirection = Vector3.ProjectOnPlane(_targetTrans.forward, Vector3.up).normalized * -1 * mouse_Y
                    //    + _targetTrans.right * -1 * mouse_X;
                    //_targetTrans.position = moveDirection * moveSpeed;
                }
            }
            else if (Input.GetMouseButton(1))
            {
                
            }
            else if(mouse_Wheel != 0f)
            {
                distance += -mouse_Wheel * scaleSpeed;
                distance = Mathf.Clamp(distance, distanceLimit.min, distanceLimit.max);
            }
        }

        private void LateUpdate()
        {
            /*
            transform.LookAt(_lookAt.position);
            transform.position = _lookAt.position + _lookAt.forward * -1 * distance;
            */

            transform.position = _targetTrans.position + _targetTrans.forward * -1 * distance;
            transform.LookAt(_targetTrans);
        }

        private void ValueInit()
        {
            //moveSpeed = _cameraControllerConfig.MaxSpeed;
            //rotateXSpeed = _cameraControllerConfig.RotateXSpeed;
            //rotateYSpeed = _cameraControllerConfig.RotateYSpeed;
            //scaleSpeed = _cameraControllerConfig.ScaleSpeed;
            //distance = _cameraControllerConfig.MaxDistance;

            //speedLimit.min = _cameraControllerConfig.MinSpeed;
            //speedLimit.max = _cameraControllerConfig.MaxSpeed;
            //distanceLimit.min = _cameraControllerConfig.MinDistance;
            //distanceLimit.max = _cameraControllerConfig.MaxDistance;
            //rotateLimit.min = _cameraControllerConfig.MinAngle;
            //rotateLimit.max = _cameraControllerConfig.MaxAngle;
        }
    }
}