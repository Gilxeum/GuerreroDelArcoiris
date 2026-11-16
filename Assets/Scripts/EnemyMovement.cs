using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;

    [SerializeField] float wallCheckDistance = 0.5f;
    [SerializeField] float groundCheckDistance = 0.1f;
    [SerializeField] float groundCheckOffset = 0.1f;

    [SerializeField] float enemyCheckDistance = 0.05f;

    [SerializeField]
    [Tooltip("If true, the enemy will start moving to the right. If false, it will start moving to the left.")]
    bool startingDirection = false;

    
    Rigidbody2D rb;

    int direction = 1; // 1 for right, -1 for left
    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        TurnAround(startingDirection ? 1 : -1);
    }

    void FixedUpdate()
    {
        if(IsTouchingWall() || IsAtEdge() || IsTouchingEnemy())
        {
            TurnAround(null);
        }

        rb.linearVelocity = new Vector2(direction * moveSpeed, 0f);

    }

    private bool IsTouchingWall()
    {
        Vector2 origin = rb.position;
        Vector2 rayDirection = Vector2.right * direction;

        Debug.DrawRay(origin, rayDirection * wallCheckDistance, Color.cyan);

        return Physics2D.Raycast(origin, rayDirection, wallCheckDistance, LayerMask.GetMask("Ground"));
    }

    private bool IsAtEdge()
    {
        Vector2 origin = rb.position + new Vector2(groundCheckOffset * direction, 0);
        
        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, Color.blue);
        return !Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
    }

    private bool IsTouchingEnemy()
    {
        Vector2 origin = rb.position + new Vector2(0.5f * direction, 0);
        Vector2 rayDirection = Vector2.right * direction;
        Debug.DrawRay(origin, rayDirection * enemyCheckDistance, Color.red);
        return Physics2D.Raycast(origin, rayDirection, enemyCheckDistance, LayerMask.GetMask("Enemy"));
    }

    private void TurnAround(int? direccionEspecifica)
    {
        direction = direccionEspecifica.HasValue ? direccionEspecifica.Value : -direction;
        transform.localScale = new Vector2(direction, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().TakeDamage();
        }
    }
}
