using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerHealth : MonoBehaviour, IHealth
{
    private int currentHealth;
    
    [Title("Health")]
    [MinValue(0)]
    public int minimumHealth;

    [MaxValue(1000)]
    public int maxHealth;

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
}
