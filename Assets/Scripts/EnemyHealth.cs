using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public int walkSpeed;

    [Title("Health")]
    [MinValue(0)]
    public int minimumHealth;

    [MaxValue(1000)]
    public int maxHealth;
    private int currentHealth;
    private Animator anim;

    // Take damage event and delegate
    public UnityEvent deathEvent;

    public UnityEvent damageEvent;

    [SerializeField] private float damageCoolDown; // Time until enemy can take damage again
    private bool canTakeDamage;

    private void OnEnable()
    {
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        canTakeDamage = true;
    }

    public void TakeDamage(int damage)
    {
        if(canTakeDamage == false)
            return;

        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            anim.SetTrigger("Death");  
            deathEvent?.Invoke();

            GetComponentInChildren<CapsuleCollider>().enabled = false;

            return;
        }

        damageEvent?.Invoke();
        anim.SetTrigger("Take hit");

        canTakeDamage = false;
        StartCoroutine(DamageCoolDown());
    }

    private IEnumerator DamageCoolDown()
    {
        yield return new WaitForSeconds(damageCoolDown);
        canTakeDamage = true;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
