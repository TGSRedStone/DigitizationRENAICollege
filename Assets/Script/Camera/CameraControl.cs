using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    private Transform _follow;
    [SerializeField]
    private Transform _lookAt;

    [SerializeField]
    private float distance = 100f;
    [SerializeField]
    private float mouse_Move_Multiple = 1f;
    [SerializeField]
    private float mouse_Rotate_Multiple = 1f;
    [SerializeField]
    private float mouse_Wheel_Multiple = 10f;

    private float mouse_X;
    private float mouse_Y;
    private float mouse_Wheel;

    private void Awake()
    {
        _follow.rotation = Quaternion.Euler(Vector3.zero);
        _lookAt.localRotation = Quaternion.Euler(new Vector3(45f, 0f, 0f));
    }

    private void Update()
    {
        mouse_X = Input.GetAxis("Mouse X");
        mouse_Y = Input.GetAxis("Mouse Y");
        mouse_Wheel = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetMouseButton(0))
        {
            _follow.position += (_follow.forward * -mouse_Y + _follow.right * -mouse_X) * mouse_Move_Multiple;
        }
        if (Input.GetMouseButton(1))
        {
            _follow.rotation *= Quaternion.Euler(0f, Mathf.Clamp(mouse_X * mouse_Rotate_Multiple, -360f / Time.deltaTime, 360f / Time.deltaTime), 0f);
        }

        distance += -mouse_Wheel * mouse_Wheel_Multiple;
        distance = Mathf.Clamp(distance, 20f, 100f);
    }

    private void LateUpdate()
    {
        transform.LookAt(_lookAt.position);
        transform.position = _lookAt.position + _lookAt.forward * -1 * distance;
    }
}
