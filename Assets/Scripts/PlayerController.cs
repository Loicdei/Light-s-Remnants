using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float maxSpeed = 8f;

    [SerializeField] private float jumpForce = 12f; // Force du saut
    [SerializeField] private LayerMask groundLayer; // Layer du sol
    [SerializeField] private BoxCollider2D groundCheck; // BoxCollider2D pour la d�tection du sol
    [SerializeField] private Rigidbody2D rb;

    private float xInput;
    private bool isGrounded;
    private bool jumpRequest;

    void Update()
    {
        // R�cup�re l'input horizontal (gauche/droite)
        xInput = Input.GetAxisRaw("Horizontal");

        // V�rification si le joueur est au sol
        isGrounded = groundCheck.IsTouchingLayers(groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            jumpRequest = true;
        }

        Flip();
    }

    void Flip()
        // Retourne le sprite selon sa direction
    {
        if (xInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Retourner � gauche
        }
        else if (xInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Retourner � droite
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

        // Si un saut est demand�, applique la force de saut
        if (jumpRequest)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequest = false; // Reset le saut apr�s l'avoir effectu�
        }
    }
}
