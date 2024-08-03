using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    public float moveSpeed;
    private Animator anim;

    private Rigidbody2D rb2d;
    float moveHorizontal;

    public bool facingRight;

    public float jumpForce;
    public bool isGrounded;
    public bool canDoubleJump;

    PlayerCombat playercombat;

    public bool characterattack;

    public float charactertimer;
    
    void Start()
    {
        moveSpeed = 5.0f;
        moveHorizontal = Input.GetAxis("Horizontal");
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        playercombat = GetComponent<PlayerCombat>();
        charactertimer = 1.0f;
    }

    
    void Update()
    {
        CharacterMovement();
        CharacterAnimation();
        CharacterAttack();
        CharacterRunAttack2();
        CharacterJump();
        CharacterAttackSpacing();
    }


    void CharacterMovement() 
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        rb2d.velocity = new Vector2 (moveHorizontal*moveSpeed, rb2d.velocity.y);
    }


    void CharacterAnimation()
    {
        if (moveHorizontal > 0) 
        {
            anim.SetBool("isRunning", true);
        }
        if (moveHorizontal == 0) 
        {
            anim.SetBool("isRunning", false);
        }
        if (moveHorizontal < 0) 
        {
            anim.SetBool("isRunning", true);
        }
        if (facingRight == false && moveHorizontal > 0) 
        {
            CharacterFlip();
        }
        if (facingRight == true && moveHorizontal < 0)
        {
            CharacterFlip();
        }


        void CharacterFlip() 
        {
            facingRight =!facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

    }

    void CharacterAttack()
    {
        if (Input.GetKeyDown(KeyCode.E) && moveHorizontal == 0)
        {
            
            if (characterattack) 
            {
                anim.SetTrigger("isAttack");
                FindObjectOfType<AudioManager>().Play("mixkitidle");
                playercombat.DamageEnemy();
                characterattack = false;
            }

            


            
        }

    }

    void CharacterRunAttack2() 
    {
        if (Input.GetKeyDown(KeyCode.E) && moveHorizontal > 0 || Input.GetKeyDown(KeyCode.E) && moveHorizontal < 0) 
        {
            
            if (characterattack)
            {
                anim.SetTrigger("isRunAttack2");
                FindObjectOfType<AudioManager>().Play("mixkitrun");
                playercombat.DamageEnemy();
                characterattack = false;
            }

            
        }
    }

    void CharacterJump () 
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            anim.SetBool("isJumping", true);

            if (isGrounded)
            {
                rb2d.velocity = Vector2.up * jumpForce;
                canDoubleJump = true;
            }

            else if (canDoubleJump) 
            {
                jumpForce = jumpForce / 1.5f;
                rb2d.velocity = Vector2.up * jumpForce;

                canDoubleJump = false;
                jumpForce = jumpForce * 1.5f;
            }
        }
    }


    void CharacterAttackSpacing() 
    {
        if (characterattack == false) 
        {
            charactertimer -= Time.deltaTime;
        }
        if (charactertimer < 0) 
        {
            charactertimer = 0f;
        }
        if (charactertimer == 0f) 
        {
            characterattack = true;
            charactertimer = 1.0f;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        anim.SetBool("isJumping", false);

        if (col.gameObject.tag == "Grounded") 
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        anim.SetBool("isJumping", false);
        if (col.gameObject.tag == "Grounded") 
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        anim.SetBool("isJumping", true);
        if (col.gameObject.tag == "Grounded") 
        {
            isGrounded = false;
        }
    }
}
