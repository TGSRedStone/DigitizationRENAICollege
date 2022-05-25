using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Character
{
    public class FreelookState : CharacterStateBase
    {
        private Vector3 bodyPosition;

        private GameObject building;

        private float mouse_X;
        private float mouse_Y;
        private float mouse_Wheel;

        private float moveMult;

        private float time;

        public override void OnEnter()
        {
            Cursor.lockState = CursorLockMode.Confined;

            //_Owner._CurrAnimator.ChangeAnimatorMode(CharacterAnimatorMode.noAnimation);
            _Owner.ChangeMovementType(CharacterMovementType.freelook);
            _Owner.ChangeLocomotionType(CharacterLocomotionType.stop);

            _Owner._HeadTransform.localRotation = Quaternion.Euler(Vector3.right * 45f);
            _Owner.CanControl = true;

            _Owner._CM_VirtualCamera.Follow = _Owner._HeadTransform;
            _Owner._CM_VirtualCamera.AddCinemachineComponent<CinemachineSameAsFollowTarget>();
            _Owner._CM_VirtualCamera_Body.CameraDistance = 100f;

            bodyPosition = _Owner._BodyTransform.position;
            moveMult = 0.5f + (_Owner._CM_VirtualCamera_Body.CameraDistance - 50f) / 100f;
        }

        public override void OnExit()
        {
            _Owner._BodyTransform.position = bodyPosition;
            _Owner._HeadTransform.localRotation = Quaternion.Euler(Vector3.zero);

            _Owner._CM_VirtualCamera.Follow = _Owner._BodyTransform;
            _Owner._CM_VirtualCamera.DestroyCinemachineComponent<CinemachineSameAsFollowTarget>();
            _Owner._CM_VirtualCamera_Body.CameraDistance = 5f;
        }

        public override void Update()
        {
            if (!_Owner.CanControl) return;

            mouse_X = Input.GetAxis("Mouse X");
            mouse_Y = Input.GetAxis("Mouse Y");
            mouse_Wheel = Input.GetAxis("Mouse ScrollWheel");

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(_Owner._Camera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 1000f, 1 << 6))
                    building = raycastHit.collider.gameObject;
                else
                    building = null;

                time = 0f;
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (building != null)
                {
                    if (time < 0.2f)
                    {
                        _Owner.AdjustPosition(Vector3.Scale(building.transform.position, new Vector3(1f, 0f, 1f)), 0.3f, true);
                    }
                }
            }

            time += Time.deltaTime;
            time = Mathf.Clamp(time, 0f, 10f);

            if (Input.GetMouseButton(0))
            {
                if (mouse_X == 0f && mouse_Y == 0f) return;
                Vector3 cameraForward = Vector3.Scale(_Owner._Camera.transform.forward, new Vector3(1f, 0f, 1f));
                Vector3 moveForward = Vector3.Normalize(cameraForward) * -mouse_Y + _Owner._Camera.transform.right * -mouse_X;
                _Owner._BodyTransform.position += moveForward * 5f * moveMult;
            }
            else if (Input.GetMouseButton(1))
            {
                Vector3 temp = (_Owner._HeadTransform.localRotation * Quaternion.Euler(new Vector3(-mouse_Y, mouse_X, 0f))).eulerAngles;
                _Owner._HeadTransform.localRotation = Quaternion.Euler(new Vector3(Mathf.Clamp(temp.x, 15f, 80f), temp.y, 0f));
            }

            _Owner._CM_VirtualCamera_Body.CameraDistance -= mouse_Wheel * 20f;
            _Owner._CM_VirtualCamera_Body.CameraDistance = Mathf.Clamp(_Owner._CM_VirtualCamera_Body.CameraDistance, 5f, 100f);

            if(mouse_Wheel != 0f)
            {
                moveMult = 0.5f + (_Owner._CM_VirtualCamera_Body.CameraDistance - 50f) / 100f;
            }
        }

        [Obsolete]
        private Vector3 GetPlaneInteractivePoint(Vector3 mousePosition, float ground_Y = 0)
        {
            Ray ray = _Owner._Camera.ScreenPointToRay(mousePosition);
#if UNITY_EDITOR
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, Time.deltaTime);
#endif
            if (ray.origin.y == ground_Y) return ray.origin;
            float mult = (ground_Y - ray.origin.y) / ray.direction.y;
            Vector3 interactivePoint = ray.origin + ray.direction * mult;
            return new Vector3(interactivePoint.x, ground_Y, interactivePoint.z);
        }

        [Obsolete]
        private Vector3 GetMouseInPlaneMove(float ground_Y = 0)
        {
            if (mouse_X == 0f && mouse_Y == 0f) return Vector3.zero;
            Vector3 position = GetPlaneInteractivePoint(Input.mousePosition, ground_Y);
            Vector3 prePosition = GetPlaneInteractivePoint(Input.mousePosition - new Vector3(mouse_X, mouse_Y, 0f), ground_Y);
#if UNITY_EDITOR
            Debug.DrawRay(prePosition, position * 10f, Color.red, Time.deltaTime);
#endif
            return position - prePosition;
        }
    }
}