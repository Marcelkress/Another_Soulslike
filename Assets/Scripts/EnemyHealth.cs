using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public int walkSpeed;
    public int minimumHealth;

    public int maxHealth;
    private int currentHealth;
    private Animator anim;

    // Take damage event and delegate
    public UnityEvent DeathEvent;
    public UnityEvent DamageEvent;

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
            DeathEvent?.Invoke();

            GetComponentInChildren<CapsuleCollider>().enabled = false;

            return;
        }

        DamageEvent?.Invoke();
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
