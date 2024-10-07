using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float maxSpeed = 8f;

    [SerializeField] private float jumpForce = 12f; // Force du saut
    [SerializeField] private LayerMask groundLayer; // Layer du sol
    [SerializeField] private BoxCollider2D groundCheck; // BoxCollider2D pour la détection du sol
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Animator animator;
    [SerializeField] private UnityEvent onLandEvent;

    private float xInput;
    private bool isGrounded;
    private bool wasGrounded; // New variable to track previous grounded state
    private bool jumpRequest;

    void Update()
    {
        // Récupère l'input horizontal (gauche/droite)
        xInput = Input.GetAxisRaw("Horizontal");

        // Vérification si le joueur est au sol
        isGrounded = groundCheck.IsTouchingLayers(groundLayer);


        //Savoir si le joueur atterit
        if (isGrounded && !wasGrounded)
        {
            onLandEvent.Invoke();
        }
        wasGrounded = isGrounded;

        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequest = true;
            animator.SetBool("IsJumping", true);
        }

        //Animation stuff
        animator.SetFloat("Speed", Mathf.Abs(xInput * acceleration));


        Flip();
    }

    //Stop animation on landing
    [SerializeField]
    private void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    void Flip()
        // Retourne le sprite selon sa direction
    {
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
        
        if (xInput != 0)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, xInput * maxSpeed, acceleration * Time.fixedDeltaTime), rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, deceleration * Time.fixedDeltaTime), rb.velocity.y);
        }

        // Si un saut est demandé, applique la force de saut
        if (jumpRequest)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequest = false; // Reset le saut après l'avoir effectué
        }
    }
}
