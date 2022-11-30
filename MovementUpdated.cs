using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementUpdated : MonoBehaviour
{
    public float MoveSpeed, HorizontalMovement, JumpForce;
    public bool IsGrounded, facingRight = true, IsMoving, IsChargingJump = false;
    public Rigidbody2D rb;
    private Vector2 PlayerPosition;
    public Animator playerAni;

    void Start()
    {
        MoveSpeed = 2.0f;
        rb = GetComponent<Rigidbody2D>();
    }

 
    void Update()
    {
        HorizontalMovement = Input.GetAxisRaw("Horizontal");
        PlayerPosition = transform.position;
        if (Input.GetKeyUp(KeyCode.Space) && IsGrounded && facingRight && (JumpForce > 10)) //Jumping Right
        {
            rb.AddForce(Vector2.up * (JumpForce * 0.20f), ForceMode2D.Impulse);
            IsGrounded = false;
            JumpForce = 0;
            rb.AddForce(Vector2.right * 2.5f, ForceMode2D.Impulse);
        }

        if (Input.GetKeyUp(KeyCode.Space) && IsGrounded && !facingRight && (JumpForce > 10)) //Jumping Left
        {
            rb.AddForce(Vector2.up * (JumpForce * 0.20f), ForceMode2D.Impulse);
            IsGrounded = false;
            JumpForce = 0;
            rb.AddForce(Vector2.right * -2.5f, ForceMode2D.Impulse);
        }

        if(Input.GetKeyUp(KeyCode.Space)) //Resets JumpForce
        {
            JumpForce = 0;
            playerAni.SetBool("IsChargingJump", false);
            IsChargingJump = false;
        }

        if(HorizontalMovement < 0 && IsGrounded == true || HorizontalMovement > 0 && IsGrounded == true)
        {
            playerAni.SetBool("IsMoving", true);
        }
        else
        {
            playerAni.SetBool("IsMoving", false);
        }
      
    }

    private void FixedUpdate()
    {


        if (Input.GetKey(KeyCode.Space) && IsGrounded)     //JumpForce Charge, and character stationary
        {
            JumpForce++;
            rb.velocity = new Vector2(0, rb.velocity.y);
            playerAni.SetBool("IsChargingJump", true);
            IsChargingJump = true;
        }


        if(JumpForce > 50)     //Stops JumpForce from going to far
        {
            JumpForce = 49;
        }

        if (JumpForce < 10 && IsGrounded)
        {
            rb.velocity = new Vector2(HorizontalMovement * MoveSpeed, rb.velocity.y);   
        }

        if (HorizontalMovement > 0 && !facingRight && IsGrounded && !IsChargingJump) //flip right
        {
            Flip();
        }

        if (HorizontalMovement < 0 && facingRight && IsGrounded && !IsChargingJump) //flip left
        {
            Flip();
        }


    }

    private void Flip()
    {

        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight; //means if one way, when action happens change 
    }

    private void OnCollisionEnter2D(Collision2D Col)
    {

        if (Col.gameObject.tag == "Floor")
        {
            IsGrounded = true;
        }

        if (Col.gameObject.tag == "Wall")
        { 

            Flip();
            
            if(facingRight)
            {
                rb.AddForce(Vector2.right * 2.5f, ForceMode2D.Impulse);
            }

            if(!facingRight)
            {
                rb.AddForce(Vector2.right * -2.5f, ForceMode2D.Impulse);
            }
        }
    }
}
