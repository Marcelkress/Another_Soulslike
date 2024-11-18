using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IHealth
{
    public int maxHealth;
    private int currentHealth;
    private Animator anim;

    // Take damage event and delegate
    public UnityEvent DeathEvent;
    public UnityEvent DamageEvent;

    [SerializeField] private float damageCoolDown; // Time until enemy can take damage again
    private bool canTakeDamage;

    public static int enemyCount;

    private void OnEnable()
    {
        enemyCount = 0;
        anim = GetComponentInChildren<Animator>();
        currentHealth = maxHealth;
        canTakeDamage = true;
        DeathEvent.AddListener(TurnOffComponents);
    }

    void Start()
    {
        enemyCount++;
        //Debug.Log(enemyCount);
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
            enemyCount--;
            return;
        }

        DamageEvent?.Invoke();

        canTakeDamage = false;
        StartCoroutine(DamageCoolDown());
        
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        anim.SetTrigger("Take hit");
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

    private void TurnOffComponents()
    {
        Destroy(GetComponentInChildren<Collider>());
        Destroy(GetComponent<EnemyBehavior>());
        Destroy(GetComponent<NavMeshAgent>());
    }
}