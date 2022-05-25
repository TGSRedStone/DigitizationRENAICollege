using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using State;
using Cinemachine;
using Tool;

namespace Character
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
        }

        private void Start()
        {
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

        public void AdjustPosition(Vector3 newPosition, float duration, bool AutoContorl = false)
        {
            if (AutoContorl) CanControl = false;
            StartCoroutine(ChangePosition(newPosition, duration));
            if (AutoContorl) CanControl = true;
        }

        private IEnumerator ChangePosition(Vector3 newPosition, float duration)
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