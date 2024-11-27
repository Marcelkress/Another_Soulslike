using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;
    public Transform swordTip, swordBottom;
    public float damageCapsuleRadius;
    private bool damageActive;

    private void Start()
    {

        // Logic depending on parent objects
        if(GetComponentInParent<Player_Controller>() != null)
            damage = GetComponentInParent<Player_Controller>().weaponDamage;

        if(GetComponentInParent<EnemyHealth>() != null)
            GetComponentInParent<EnemyHealth>().DeathEvent.AddListener(TurnOffDamage);

            
    }

    public void ToggleSword()
    {
        damageActive = !damageActive;
    }

    void Update()
    {
        if(damageActive)
        {
            CheckForHit();
        }
    }

    private void CheckForHit()
    {
        Collider[] colliders = Physics.OverlapCapsule(swordTip.position, swordBottom.position, damageCapsuleRadius);

        if(colliders.Length != 0)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                IHealth health = colliders[i].gameObject.GetComponentInParent<IHealth>();

                health?.TakeDamage(damage);
            }
        }
    }

    public void TurnOffDamage()
    {
        damageActive = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(swordTip.position, damageCapsuleRadius);
        Gizmos.DrawWireSphere(swordBottom.position, damageCapsuleRadius);
    }
}
