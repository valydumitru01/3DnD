using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey("x"))
        {
            animator.SetBool("isWalking", true);
        }
        else if(Input.GetKey("a"))
        {
            animator.SetBool("isFighting", true);
        }
        else
        {
            animator.SetBool("isFighting", false);
            animator.SetBool("isWalking", false);
        }
    }
}
