using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IHealth
{
    public int GetCurrentHealth();
    
    public void TakeDamage(int damage);
}

public interface IInteractable
{
    public void Interact();
}
