using System;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour, IPlayerActions
{
    public event Action OnLifeLost;
    public event Action OnPowerUpActivated;

    private int lives = 3;
    private float health = 100f;
    private bool powerUpActive = false;
    private const float healthLossPerHit = 25f;

    protected virtual void Start()
    {
        health = 100f;
        lives = 3;
        powerUpActive = false;
    }

    public void TakeDamage()
    {
        health -= healthLossPerHit;

        if (health <= 0)
        {
            lives--;
            OnLifeLost?.Invoke();
            ResetHealth();
        }

        CheckPowerUpActivation();
    }

    public void ResetHealth()
    {
        health = 100f;
    }

    public void ActivatePowerUp()
    {
        if (!powerUpActive && (health <= 25 || (lives == 1 && health <= 50)))
        {
            powerUpActive = true;
            OnPowerUpActivated?.Invoke();
            HandlePowerUp();
        }
    }

    private void CheckPowerUpActivation()
    {
        if (!powerUpActive && (health <= 25 || (lives == 1 && health <= 50)))
        {
            ActivatePowerUp();
        }
    }

    protected abstract void HandlePowerUp();

    internal int GetLives() => lives;
    internal float GetHealth() => health;
}
