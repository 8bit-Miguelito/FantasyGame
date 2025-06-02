using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class Targeter : MonoBehaviour
{

    [field: SerializeField] public float radius { get; private set; }

    [field: SerializeField] public float MaxAngle { get; private set; }

    [field: SerializeField] public float MaxDistance { get; private set; }

    public Target currentTarget { get; private set; }
    
    private Camera MainCam;

    public void Awake()
    {
        MainCam = Camera.main;
    }

    public bool targetInRange()
    {
        Vector3 distanceFromTarget = currentTarget.GetTargetTransform().position - transform.position;

        if (distanceFromTarget.sqrMagnitude > MaxDistance * MaxDistance)
        {
            CancelTarget();
            return false;
        }

        return true;
    }

    public void CancelTarget()
    {
        currentTarget = null;
    }

    public void ScanForEnemies()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, radius, 7);
        float closestAngle = Mathf.Infinity;
        float currentAngle = 0f;

        foreach (var enemy in enemies)
        {
            Target target = enemy.GetComponent<Target>();

            if (!target)
            {
                continue;
            }
            if (!IsTargetVisible(target, ref currentAngle)) continue;

            if (currentAngle < closestAngle)
            {
                currentTarget = target;
                closestAngle = currentAngle;
            }
        }
    }

    private bool IsTargetVisible(in Target target, ref float angle)
    {
        Transform targetTransform = target.GetTargetTransform();

        //Gets the coordinate of the target relative to the screen 
        Vector3 TargetViewport = MainCam.WorldToViewportPoint(targetTransform.position);

        //Checks if target is current in the scene
        if (TargetViewport.z < 0) return false;
        if (TargetViewport.x < 0 || TargetViewport.x > 1) return false;
        if (TargetViewport.y < 0 || TargetViewport.y > 1) return false;

        //Calculate distance Vector from player position to target position
        Vector3 DistanceToTarget = targetTransform.position - transform.position;
        DistanceToTarget.Normalize();

        angle = Vector3.Angle(transform.forward, DistanceToTarget);

        if (angle > MaxAngle) return false;

        return true;
    }

}
