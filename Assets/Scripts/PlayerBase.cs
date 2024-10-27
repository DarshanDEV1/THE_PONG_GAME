using System;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour, IPlayerActions
{
    public event Action OnLifeLost;
    public event Action OnPowerUpActivated;
    public event Action OnPowerDeactivated;

    private float health = 100f;
    private bool powerUpActive = false;
    private const float healthLossPerHit = 25f;

    protected virtual void Start()
    {
        health = 100f;
        powerUpActive = false;
    }

    public void TakeDamage()
    {
        health -= healthLossPerHit;

        if (health <= 0)
        {
            OnLifeLost?.Invoke();
        }

        CheckPowerUpActivation();
    }

    public void ResetHealth()
    {
        health = 100f;
    }

    public void ActivatePowerUp()
    {
        if (!powerUpActive && (health <= 25))
        {
            powerUpActive = true;
            OnPowerUpActivated?.Invoke();
            HandlePowerUp(true);
        }
    }

    public void DeactivatePowerUp()
    {
        powerUpActive = false;
        HandlePowerUp(false);
    }

    private void CheckPowerUpActivation()
    {
        if (!powerUpActive && (health <= 25))
        {
            ActivatePowerUp();
        }
        else
        {
            DeactivatePowerUp();
        }
    }

    protected abstract void HandlePowerUp(bool m_powerup);

    internal float GetHealth() => health;
}
