using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;

    public Collider swordCollider;

    private void Start()
    {
        if(GetComponentInParent<Player_Controller>() != null)
            damage = GetComponentInParent<Player_Controller>().weaponDamage;
        
        swordCollider = GetComponent<Collider>();
        swordCollider.enabled = false;
    }
    
    private void OnTriggerEnter(Collider hit)
    {
        hit.GetComponentInParent<IHealth>()?.TakeDamage(damage);
    }

    public void ToggleSword()
    {
        swordCollider.enabled = !swordCollider.enabled;
    }



}
