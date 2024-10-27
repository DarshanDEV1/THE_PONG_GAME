using UnityEngine;

public class Player : PlayerBase
{
    [SerializeField] private KeyCode moveUpKey;
    [SerializeField] private KeyCode moveDownKey;
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float boundY = 3.5f;

    private Rigidbody2D playerRigidbody;
    private PhysicsMaterial2D originalMaterial;
    private PhysicsMaterial2D powerUpMaterial;

    protected override void Start()
    {
        base.Start();
        playerRigidbody = GetComponent<Rigidbody2D>();
        originalMaterial = playerRigidbody.sharedMaterial;
        powerUpMaterial = new PhysicsMaterial2D("PowerUpMaterial") { bounciness = 1.5f };

        OnPowerUpActivated += ActivatePowerUp;
    }

    private void Update()
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        float direction = 0;

        if (Input.GetKey(moveUpKey))
        {
            direction = 1;
        }
        else if (Input.GetKey(moveDownKey))
        {
            direction = -1;
        }

        Move(direction);
    }

    private void Move(float direction)
    {
        Vector2 velocity = playerRigidbody.velocity;
        velocity.y = direction * speed;
        playerRigidbody.velocity = velocity;

        Vector2 position = transform.position;
        position.y = Mathf.Clamp(position.y, -boundY, boundY);
        transform.position = position;
    }

    protected override void HandlePowerUp()
    {
        playerRigidbody.sharedMaterial = powerUpMaterial;
        Debug.Log($"{gameObject.name} activated power-up: Increased bounciness!");
    }

    private void OnDisable()
    {
        OnPowerUpActivated -= ActivatePowerUp;
    }
}
