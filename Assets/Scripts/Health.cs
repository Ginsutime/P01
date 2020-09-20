using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] HealthBar healthBar;
    [SerializeField] int _maxHealth = 3;
    int _currentHealth;
    int checkDisabled = 1;

    private void Start()
    {
        _currentHealth = _maxHealth;
        healthBar.SetMaxHealth(_maxHealth);
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        Debug.Log(gameObject.name + " has healed " + amount);
    }

    public void DecreaseHealth(int amount)
    {
        ThirdPersonMovement thirdPersonMovement = GetComponent<ThirdPersonMovement>();

        if (checkDisabled == 0)
        {
            amount = 0;
        }
        else
        {
            _currentHealth -= amount;
            healthBar.SetHealth(_currentHealth);

            Debug.Log("Player's health: " + _currentHealth);
            
            if (_currentHealth <= 0)
            {
                thirdPersonMovement.Invoke("CheckIfDead", 0f);
                Invoke("Kill", 3.17f);
            }
            else if (_currentHealth >= 1)
            {
                thirdPersonMovement.Invoke("CheckIfInjured", 0f);
                thirdPersonMovement.Invoke("CheckIfNotInjured", 1.8f);
            }
        }
    }

    public void Kill()
    {
        if (checkDisabled == 1)
        {
            gameObject.SetActive(false);
        }
    }
}
