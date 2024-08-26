using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public int GetCurrentHealth();
    
    public void TakeDamage(int damage);
}
