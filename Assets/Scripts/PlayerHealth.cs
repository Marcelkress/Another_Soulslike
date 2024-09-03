using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IHealth
{
    private int currentHealth;
    public int maxHealth;
    private Animator anim;

    public UnityEvent takeDamageEvent;
    
    [Button("Take Damage Test")]
    private void DefaultSizedButton()
    {
        TakeDamage(20);
    }

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
        anim.SetTrigger("Take Hit");
        currentHealth -= damage;
        Debug.Log(currentHealth);
        takeDamageEvent?.Invoke();
    }
}
