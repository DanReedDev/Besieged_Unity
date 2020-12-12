using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour { 

    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;

    float horizontalMove = 0f;

    public bool canMove = true;
    bool jump = false;
    //bool crouch = false;
    bool attack = false;

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
        /*
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
        } 

        if (Input.GetButtonDown("Attack"))
        {
            attack = true;
        } else if (Input.GetButtonUp("Attack"))
        {
            attack = false;
        }
        */
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
            jump = false;
        }
    }

    public void CanMove(int canPlayerMove)
    {
        if (canPlayerMove == 1)
            canMove = true;

        if (canPlayerMove == 0)
            canMove = false;
    }
}
