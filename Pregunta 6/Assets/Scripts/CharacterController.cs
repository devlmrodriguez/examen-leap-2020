using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb2D;

    private float speedX;
    private bool rolling;
    private bool grounded;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        speedX = 0;
        if (!rolling && Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            speedX = -500f;
        }
        if (!rolling && Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            speedX = 500f;
        }
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.J))
            AttackMelee();
        if (Input.GetKeyDown(KeyCode.I))
            AttackRange();
        if (Input.GetKeyDown(KeyCode.L))
            Roll();

    }

    private void FixedUpdate()
    {
        Vector3 velocity = rb2D.velocity;
        velocity.x = speedX * Time.deltaTime;
        rb2D.velocity = velocity;

        animator.SetFloat("SpeedX", rb2D.velocity.x);
        animator.SetFloat("SpeedY", rb2D.velocity.y);
        animator.SetBool("Grounded", grounded);
    }

    private void AttackMelee()
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("AttackMelee");
    }

    private void AttackRange()
    {
        animator.SetTrigger("Attack");
        animator.SetTrigger("AttackRange");
    }

    private void Roll()
    {
        StartCoroutine(_Roll());
    }

    private IEnumerator _Roll()
    {
        if (grounded)
        {
            rolling = true;
            animator.SetBool("Rolling", true);
            animator.SetTrigger("Roll");
            yield return new WaitForSeconds(0.2f);
            animator.SetBool("Rolling", false);
            rolling = false;
        }
    }

    private void Jump()
    {
        if (grounded)
        {
            rb2D.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        grounded = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
    }
}
