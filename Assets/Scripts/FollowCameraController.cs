using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraController : MonoBehaviour
{
    public Transform TargetTransform;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - TargetTransform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = TargetTransform.position + offset;
    }
}
