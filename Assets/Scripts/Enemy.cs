using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;

public class Enemy : MonoBehaviour, IHealth
{

    [Range(1, 10)]
    [HideLabel, InfoBox("How fast the enemy moves")]
    public int walkSpeed;

    [Title("Health")]
    [MinValue(0)]
    public int minimumHealth;

    [MaxValue(1000)]
    public int maxHealth;

    [Button("Take Damage")]
    private void NamedButton()
    {
        TakeDamage(10);
    }
    private int currentHealth;
    private Animator anim;

    // Take damage event and delegate
    public delegate void damageTaken();
    public damageTaken damageEvent;
    public damageTaken deathEvent;

    private void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("here");

        if(currentHealth <= 0)
        {
            anim.SetTrigger("Death");  
            deathEvent?.Invoke();

            GetComponentInChildren<CapsuleCollider>().enabled = false;

            return;
        }

        damageEvent?.Invoke();
        anim.SetTrigger("Take hit");
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
    
    public void LockOn()
    {
        
    }

}
