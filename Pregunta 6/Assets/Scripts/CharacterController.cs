using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float rollSpeed;
    [SerializeField]
    private float jumpMagnitude;
    [SerializeField]
    private float dashMagnitude;
    [SerializeField]
    private float dashTime;
    [SerializeField]
    private float dashGravityRecoverTime;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Rigidbody2D rb2D;

    private float currentSpeed;
    private float originalGravityScale;

    private bool rolling;
    private bool attacking;
    private bool attackButtonPressed;
    private bool attackButtonFirstTime;
    private AttackType previousAttackType;
    private bool dashing;
    private bool grounded;
    

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        originalGravityScale = rb2D.gravityScale;
        ResetAttackFlags();
    }

    private void Update()
    {
        //Cancelar la gravedad si el jugador esta atacando en el aire
        if (attacking && !grounded)
        {
            rb2D.gravityScale = 0;
            rb2D.velocity = new Vector2(rb2D.velocity.x, 0f);
        }
        else
        {
            //Devolver la gravedad solo si el jugador no esta en dashing
            if(!dashing)
                rb2D.gravityScale = originalGravityScale;
        }

        //Moverse solo si no se esta atacando o dashing
        currentSpeed = 0;
        if (!attacking && !dashing && Input.GetKey(KeyCode.A))
        {
            spriteRenderer.flipX = true;
            if (!rolling)
                currentSpeed = runSpeed * -1f;
            else
                currentSpeed = rollSpeed * -1f;
        }
        if (!attacking && !dashing && Input.GetKey(KeyCode.D))
        {
            spriteRenderer.flipX = false;
            if (!rolling)
                currentSpeed = runSpeed;
            else
                currentSpeed = rollSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();
        if (Input.GetKeyDown(KeyCode.J))
            Attack(AttackType.MELEE);
        if (Input.GetKeyDown(KeyCode.I))
            Attack(AttackType.RANGE);
        if (Input.GetKeyDown(KeyCode.L))
            Roll();
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash();
    }

    private void FixedUpdate()
    {
        //Moverse de manera constante solo si no estamos en dashing, si no, aumentar la velocidad para el impulso
        if(!dashing)
            rb2D.velocity = new Vector2(currentSpeed * Time.deltaTime, rb2D.velocity.y);
        else
            rb2D.velocity += new Vector2(currentSpeed * Time.deltaTime, rb2D.velocity.y);

        animator.SetFloat("SpeedX", Mathf.Abs(rb2D.velocity.x));
        if(!dashing)
            animator.SetFloat("SpeedY", rb2D.velocity.y);
        else
            animator.SetFloat("SpeedY", -1f);
    }

    private void Attack(AttackType attackType)
    {
        //Evitar ataque aéreo de rango
        if (!grounded && attackType == AttackType.RANGE)
            return;

        //Activar el flag de botón presionado solo si no es la primera vez (primer ataque)
        if (attackButtonFirstTime)
            attackButtonFirstTime = false;
        else
            attackButtonPressed = true;

        ExecuteAttack(attackType);
    }

    private void ExecuteAttack(AttackType attackType)
    {
        previousAttackType = attackType;

        if (Input.GetKey(KeyCode.A))
            spriteRenderer.flipX = true;
        if (Input.GetKey(KeyCode.D))
            spriteRenderer.flipX = false;

        if (!attacking)
            animator.SetTrigger("Attack");

        if(attackType == AttackType.MELEE)
            animator.SetTrigger("AttackMelee");
        else if (attackType == AttackType.RANGE)
            animator.SetTrigger("AttackRange");
    }

    private void ResetAttackFlags()
    {
        attacking = false;
        animator.SetBool("Attacking", attacking);
        attackButtonPressed = false;
        attackButtonFirstTime = true;
    }

    //Debe ser referenciado de la animación de ataque en el animator al iniciar
    public void OnAttackStart()
    {
        attacking = true;
        animator.SetBool("Attacking", attacking);
    }

    //Debe ser referenciado de la animación de ataque en el animator al finalizar
    public void OnAttackEnd()
    {
        if (attackButtonPressed)
        {
            attackButtonPressed = false;
            ExecuteAttack(previousAttackType);
        }
        else
            ResetAttackFlags();
    }

    private void Roll()
    {
        if (!rolling && grounded)
        {
            ResetAttackFlags();

            animator.SetTrigger("Roll");
            rolling = true;
            animator.SetBool("Rolling", rolling);
        }
    }

    private void Dash()
    {
        if(!dashing && !rolling)
            StartCoroutine(_Dash());
    }

    private IEnumerator _Dash()
    {
        dashing = true;

        //Desactivar la gravedad e incluir un impulso en la dirección actual
        rb2D.velocity = new Vector2(0f, 0f);
        rb2D.gravityScale = 0f;
        Vector2 direction = Vector2.right * (spriteRenderer.flipX ? -1f : 1f);
        rb2D.AddForce(dashMagnitude * direction, ForceMode2D.Impulse);

        //Mantener el impulso en la velocidad por un tiempo determinado
        yield return new WaitForSeconds(dashTime);
        rb2D.velocity = new Vector2(0f, 0f);

        //Devolver la gravedad después de un tiempo al finalizar el dash
        yield return new WaitForSeconds(dashGravityRecoverTime);
        rb2D.gravityScale = originalGravityScale;
        //Incluir una pequeña velocidad hacia abajo para que el animator detecte la caída en salto
        rb2D.velocity = new Vector2(0f, -0.1f);
        dashing = false;
    }

    public void OnRollEnd()
    {
        rolling = false;
        animator.SetBool("Rolling", rolling);
    }

    private void Jump()
    {
        if (!rolling && grounded)
        {
            ResetAttackFlags();

            rb2D.AddForce(Vector2.up * jumpMagnitude, ForceMode2D.Impulse);
            animator.SetBool("Jump", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        grounded = true;
        animator.SetBool("Grounded", grounded);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        grounded = true;
        animator.SetBool("Grounded", grounded);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        grounded = false;
        animator.SetBool("Grounded", grounded);
    }
}
