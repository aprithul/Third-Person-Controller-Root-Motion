using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class IKController : MonoBehaviour
{

    public bool IkOn;
    public Transform SpineJoint;
    public float lookAtWeight;
    public float MaxSlopeLeanFactor;
    public IKTargetPositionDetectionController RightTargetPositionDetector;
    public IKTargetPositionDetectionController LeftTargetPositionDetector;
    public SlopeDetectionController SlopeDetector;

    private Vector3 rightTargetPosition;
    private Vector3 leftTargetPosition;
    private Animator animator;
    private Rigidbody thisRigidbody;
    private float leaningWeight;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        thisRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var vel = thisRigidbody.velocity;
        //thisRigidbody.velocity = new Vector3(vel.x, vel.y - 0.5f, vel.z);
    }

    private void OnAnimatorIK(int layerIndex)
    {

        if (IkOn)
        {
            // no need to apply ik if ground is even
            if (!RightTargetPositionDetector.is_ground_uneven || !LeftTargetPositionDetector.is_ground_uneven)
                return;

            //get the animation state
            AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
            var animationTime = animStateInfo.normalizedTime % 1;

            animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("LeftFootIkWeight"));
            animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, animator.GetFloat("RightFootIkWeight"));

            leftTargetPosition = (animationTime > 0.15f && animationTime < 0.25f) ? LeftTargetPositionDetector.TargetPosition: leftTargetPosition;
            rightTargetPosition =(animationTime > 0.60f && animationTime < 0.70f)? RightTargetPositionDetector.TargetPosition: rightTargetPosition;

            animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftTargetPosition);
            animator.SetIKPosition(AvatarIKGoal.RightFoot, rightTargetPosition);

            animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, animator.GetFloat("LeftFootIkWeight"));
            animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, animator.GetFloat("RightFootIkWeight"));

            animator.SetIKRotation(AvatarIKGoal.LeftFoot,LeftTargetPositionDetector.TargetRotation);
            animator.SetIKRotation(AvatarIKGoal.RightFoot,RightTargetPositionDetector.TargetRotation);

            leaningWeight = Mathf.Lerp(leaningWeight, SlopeDetector.SlopeFactor, Time.deltaTime * 3f);
            animator.SetLookAtWeight(leaningWeight, leaningWeight, leaningWeight * 0.3f);
            animator.SetLookAtPosition(SlopeDetector.SlopePoint);



            // lean
            //var slope = SlopeDetector.SurfaceNormal;
            //Quaternion.FromToRotation(SpineJoint.rotation, Quaternion.AngleAxis())
            //SpineJoint.rotation = Quaternion.AngleAxis(90f, Vector3.right);

        }

    }
}
