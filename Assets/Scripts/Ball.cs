using System;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public event Action<string> OnScore;

    private Rigidbody2D rb;
    private Vector2 initialPosition;
    private float respawnDelay = 2f;
    private float speed = 5f;

    // Boundary values for checking if the ball goes out of bounds vertically
    private float upperBoundary = 6f;
    private float lowerBoundary = -6f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
        ResetBall();
    }

    private void Update()
    {
        CheckBounds();
    }

    private void CheckBounds()
    {
        // Check if the ball goes beyond the upper or lower boundaries
        if (transform.position.y > upperBoundary || transform.position.y < lowerBoundary)
        {
            ResetBall();
        }
    }

    private void ResetBall()
    {
        // Reset position and velocity
        transform.position = initialPosition;
        rb.velocity = Vector2.zero;

        // Relaunch the ball after a delay
        Invoke(nameof(LaunchBall), respawnDelay);
    }

    private void LaunchBall()
    {
        // Choose a random angle for the initial launch direction
        float randomAngle = UnityEngine.Random.Range(-45f, 45f);
        Vector2 direction = Quaternion.Euler(0, 0, randomAngle) * (UnityEngine.Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right);

        // Add force in the random direction
        rb.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Ensure the ball bounces at an angle based on the collision point
            Vector2 normal = collision.contacts[0].normal;
            Vector2 newDirection = Vector2.Reflect(rb.velocity.normalized, normal);

            // Apply new velocity
            rb.velocity = newDirection * ((speed * speed) * -1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1Goal"))
        {
            Debug.Log("Player 1");
            OnScore?.Invoke("Player1");
            ResetBall();
        }
        else if (collision.CompareTag("Player2Goal"))
        {
            Debug.Log("Player 2");
            OnScore?.Invoke("Player2");
            ResetBall();
        }
    }
}
