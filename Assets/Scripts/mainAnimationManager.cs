using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class mainAnimationManager : MonoBehaviour {

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        animator.SetFloat("Speed", gameObject.GetComponentInParent<RigidbodyFirstPersonController>().movementSettings.CurrentTargetSpeed);
        animator.SetBool("IsJumping", gameObject.GetComponentInParent<RigidbodyFirstPersonController>().Jumping);
        animator.SetBool("IsGrounded", gameObject.GetComponentInParent<RigidbodyFirstPersonController>().Grounded);
    }

}
