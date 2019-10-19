using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeDetectionController : MonoBehaviour
{
    public LayerMask WalkableLayer;
    [HideInInspector] public Vector3 SurfaceNormal;
    public float SlopeFactor;
    public Vector3 SlopePoint;

    private bool offsetCalculated;
    private float startingSlopeFactor = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 20f, WalkableLayer))
        {
            SurfaceNormal = hitInfo.normal;
            Debug.DrawRay(hitInfo.point, hitInfo.normal, Color.magenta);
            SlopePoint = hitInfo.point;
            var inPlaneNormal = Vector3.ProjectOnPlane(SurfaceNormal, transform.right);
            SlopeFactor = transform.InverseTransformVector(Vector3.Cross(transform.forward, inPlaneNormal.normalized)).x - startingSlopeFactor;

            if (!offsetCalculated)
            {
                startingSlopeFactor = SlopeFactor;
                offsetCalculated = true;
            }
            
            //Debug.Log(Vector3.Cross(transform.forward, inPlaneNormal.normalized));

        }
    }
}
