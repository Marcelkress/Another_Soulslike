using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IHealth
{
    private int currentHealth;
    public int maxHealth;
    public bool isInvincible; // Used for dodging
    private Animator anim;

    public UnityEvent takeDamageEvent;
    public UnityEvent deathEvent;

    
    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int damage)
    {
        if(isInvincible == true)
            return;

        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            anim.SetTrigger("Death");
            deathEvent?.Invoke();
        }
        else
        {
            anim.SetTrigger("Take Hit");
            takeDamageEvent?.Invoke();
        }
    }
}
