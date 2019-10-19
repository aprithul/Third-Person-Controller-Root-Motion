using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTargetPositionDetectionController : MonoBehaviour
{
    public Transform IkGoalTransform;
    public LayerMask WalkableLayer;
    public float NormalDisplacementMagnitude;
    [HideInInspector] public bool is_ground_uneven;
    public Vector3 TargetPosition
    {
        get
        {
            return lastHitPoint + (lastHitNormal * NormalDisplacementMagnitude);
        }
    }
    public Quaternion TargetRotation
    {
        get
        {
            return Quaternion.LookRotation(IkGoalTransform.forward, lastHitNormal);
        }
    }

    private Vector3 localPosition;
    private Quaternion localRotation;
    private Vector3 lastHitNormal;
    private Vector3 lastHitPoint;
    private float original_distance = -1f;

    // Start is called before the first frame update
    void Awake()
    {
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(transform.position,transform.forward,out hitInfo, 20f, WalkableLayer))
        {
            if (original_distance < 0f)
                original_distance = hitInfo.distance;

            if (Mathf.Abs(original_distance - hitInfo.distance) / original_distance > 0.1f)
                is_ground_uneven = true;
            else
                is_ground_uneven = false;

            lastHitPoint = hitInfo.point;
            lastHitNormal = hitInfo.normal;
            Debug.DrawLine(transform.position, hitInfo.point, Color.cyan);
            Debug.DrawRay(hitInfo.point, hitInfo.normal * 2f, Color.red);
        }
    }



    private void LateUpdate()
    {
        transform.localRotation = localRotation; 
    }
}
