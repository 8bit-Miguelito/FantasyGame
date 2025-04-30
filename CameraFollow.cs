using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform followTarget; //Transform target that can be added in inspector
    public float sensitivity = 5f;
    public float distance = 5;

    public float pitchMin = -30f;
    public float pitchMax = 60f;

    float yaw;
    float pitch;

    //bools for inversion
    public bool invertX;
    public bool invertY;

    private float inversionX;
    private float inversionY;

    [SerializeField] Vector2 FramingOffset;

    
    private void Update()
    {   

        //if invert is true, turn yaw and pitch to negative values
        inversionX = (invertX) ? -1 : 1;
        inversionY = (invertY) ? -1 : 1;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        yaw += mouseX * sensitivity * inversionX;
        pitch += mouseY * sensitivity * inversionY;

        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        var targetRotation = Quaternion.Euler(pitch, yaw, 0);

        var focusPosition = followTarget.position + new Vector3(FramingOffset.x, FramingOffset.y);

        transform.position = focusPosition - targetRotation * new Vector3(0,0,distance);
        transform.rotation = targetRotation;
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, yaw, 0);
}
