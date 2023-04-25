using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
using Cinemachine;
using Tool;
using Config;
using static UnityEngine.UI.GridLayoutGroup;

namespace Characters
{
    public enum CharacterMovementType
    {
        freelook = 0,
        normal = 1,
        action = 2,
        sit = 3
    }

    public enum CharacterLocomotionType
    {
        stop = 0,
        walk = 1,
        run = 2,
    }

    public class Character : StateMachine<CharacterStateBase, Character>
    {
        [HideInInspector]
        public CharacterAnimator _CurrAnimator;

        //角色
        [HideInInspector]
        public Transform _BodyTransform;
        [HideInInspector]
        public Transform _HeadTransform;
        //摄像机
        [HideInInspector]
        public Camera _Camera;
        public CinemachineVirtualCamera _CM_VirtualCamera;
        [HideInInspector]
        public Cinemachine3rdPersonFollow _CM_VirtualCamera_Body;

        [HideInInspector]
        public CameraControlConfig _CameraControlConfig;
        private readonly string configPath = "Configs/CameraControlConfig";

        public CharacterMovementType CurrMovementType { get; private set; }
        public CharacterLocomotionType CurrLocomotionType { get; private set; }

        [HideInInspector]
        public bool CanControl;

        private void Awake()
        {
            _BodyTransform = transform;
            _HeadTransform = gameObject.FindGameObjectWithNameInChild("Head").GetComponent<Transform>();

            _Camera = Camera.main;
            _CM_VirtualCamera_Body = _CM_VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();

            _CameraControlConfig = Resources.Load<CameraControlConfig>(configPath);
        }

        private void Start()
        {
            _HeadTransform.localRotation = Quaternion.Euler(Vector3.right * 45f);

            _CM_VirtualCamera.Follow = _BodyTransform;
            _CM_VirtualCamera.LookAt = _HeadTransform;

            CanControl = true;

            ChangeState<FreelookState>(this);
        }

        private void Update()
        {
            State.Update();
        }

        private void LateUpdate()
        {
            State.LateUpdate();
        }

        #region 角色相关

        public void AdjustBodyPosition(Vector3 newPosition, float duration, bool AutoControl = false)
        {
            if (AutoControl) CanControl = false;
            newPosition = new Vector3(
                Mathf.Clamp(newPosition.x, _CameraControlConfig.AreaBottomLeftPosition.x, _CameraControlConfig.AreaTopRightPosition.x),
                _BodyTransform.position.y,
                Mathf.Clamp(newPosition.z, _CameraControlConfig.AreaBottomLeftPosition.z, _CameraControlConfig.AreaTopRightPosition.z));
            StartCoroutine(ChangeBodyPosition(newPosition, duration));

            IEnumerator ChangeBodyPosition(Vector3 newPosition, float duration)
            {
                Vector3 startPosition = _BodyTransform.position;
                float time = 0f;
                while (time < duration)
                {
                    _BodyTransform.position = Vector3.Lerp(startPosition, newPosition, time / duration);
                    time += Time.deltaTime;
                    yield return null;
                }
                _BodyTransform.position = newPosition;

                CanControl = true;
            }
        }

        public void AdjustBodyRotation(Quaternion newRotation, float duration, bool AutoControl = false)
        {
            if (AutoControl) CanControl = false;
            StartCoroutine(ChangeBodyRotation(newRotation, duration));

            IEnumerator ChangeBodyRotation(Quaternion newPosition, float duration)
            {
                Vector3 startRotation = _BodyTransform.rotation.eulerAngles;
                float time = 0f;
                while (time < duration)
                {
                    _BodyTransform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation, newRotation.eulerAngles, time / duration));
                    time += Time.deltaTime;
                    yield return null;
                }
                _BodyTransform.rotation = newPosition;

                CanControl = true;
            }
        }

        public void AdjustHeadRotation(Quaternion newRotation, float duration, bool AutoControl = false)
        {
            if (AutoControl) CanControl = false;
            StartCoroutine(ChangeHeadRotation(newRotation, duration));

            IEnumerator ChangeHeadRotation(Quaternion newPosition, float duration)
            {
                Vector3 startRotation = _HeadTransform.localRotation.eulerAngles;
                float time = 0f;
                while (time < duration)
                {
                    _HeadTransform.localRotation = Quaternion.Euler(Vector3.Lerp(startRotation, newRotation.eulerAngles, time / duration));
                    time += Time.deltaTime;
                    yield return null;
                }
                _HeadTransform.localRotation = newPosition;

                CanControl = true;
            }
        }

        public void AdjustCameraDistance(float newDistance, float duration, bool AutoControl = false)
        {
            if (AutoControl) CanControl = false;
            newDistance = Mathf.Clamp(newDistance, _CameraControlConfig.MinDistance, _CameraControlConfig.MaxDistance);
            StartCoroutine(ChangeCameraDistance(newDistance, duration));

            IEnumerator ChangeCameraDistance(float newDistance, float furation)
            {
                float startDistance = _CM_VirtualCamera_Body.CameraDistance;
                float time = 0f;
                while (time < duration)
                {
                    _CM_VirtualCamera_Body.CameraDistance = Mathf.Lerp(startDistance, newDistance, time / duration);
                    time += Time.deltaTime;
                    yield return null;
                }
                _CM_VirtualCamera_Body.CameraDistance = newDistance;

                CanControl = true;
            }
        }

        #endregion

        #region 枚举相关

        public void ChangeMovementType(CharacterMovementType newMovementType)
        {
            if (CurrMovementType == newMovementType) return;
            CurrMovementType = newMovementType;
        }

        public void ChangeLocomotionType(CharacterLocomotionType newLocomotionType)
        {
            if (CurrLocomotionType == newLocomotionType) return;
            CurrLocomotionType = newLocomotionType;
        }

        #endregion
    }
}