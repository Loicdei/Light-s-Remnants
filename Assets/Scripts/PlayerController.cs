using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float maxSpeed = 8f;

    [SerializeField] private float jumpForce = 16f; // Force du saut initial
    [SerializeField] private float variableJumpHeightMultiplier = 0.5f; // Modifie la hauteur du saut si la touche est relâchée
    [SerializeField] private float coyoteTimeDuration = 0.1f; // Temps pendant lequel le joueur peut sauter après avoir quitté une plateforme
    [SerializeField] private float jumpBufferTime = 0.1f; // Temps pendant lequel une pression de saut est "enregistrée" avant d'atterrir
    [SerializeField] private float maxFallSpeed = -15f; // Vitesse de chute maximale

    [SerializeField] private LayerMask groundLayer; // Layer du sol
    [SerializeField] private BoxCollider2D groundCheck; // BoxCollider2D pour la détection du sol
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Animator animator;
    [SerializeField] private UnityEvent onLandEvent;

    private float xInput;
    private bool isGrounded;
    private bool wasGrounded;
    private bool jumpRequest;
    private bool isJumping;
    private float coyoteTimeCounter; // Compteur pour le coyote time
    private float jumpBufferCounter; // Compteur pour le jump buffering

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        isGrounded = groundCheck.IsTouchingLayers(groundLayer);
        //Coyote time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTimeDuration;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffering
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Animation d'atterrissage
        if (isGrounded && !wasGrounded)
        {
            onLandEvent.Invoke();
        }
        wasGrounded = isGrounded;

        // Saut si grounded, coyote time, ou buffered
        if (jumpBufferCounter > 0 && (isGrounded || coyoteTimeCounter > 0))
        {
            jumpRequest = true;
            isJumping = true;
            jumpBufferCounter = 0; // reset buffer
            animator.SetBool("IsJumping", true);
        }

        // Modifie la hauteur du saut si la touche est relâchée
        if (Input.GetButtonUp("Jump") && isJumping)
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
            }
            isJumping = false;
        }

        // Animation de marche
        animator.SetFloat("Speed", Mathf.Abs(xInput * acceleration));

        Flip();
    }

    // Stop animation on landing
    [SerializeField]
    private void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void Flip()
    {
        // Retourne le sprite selon sa direction
        if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Retourner à gauche
        }
        else if (xInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Retourner à droite
        }
    }

    void FixedUpdate()
    {
        // Déplacement horizontal avec interpolation pour un mouvement fluide
        if (xInput != 0)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, xInput * maxSpeed, acceleration * Time.fixedDeltaTime), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, deceleration * Time.fixedDeltaTime), rb.velocity.y);
        }

        // Applique la force de saut si un saut est demandé
        if (jumpRequest)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequest = false; // Réinitialise la demande de saut après l'avoir effectuée
        }

        // Limite la vitesse de chute
        if (rb.velocity.y < maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, maxFallSpeed);
        }
    }
}
