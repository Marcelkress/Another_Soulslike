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

    private Collider[] hitResults = new Collider[10]; // Fixed-size array for overlap results


    private void Start()
    {
        if(GetComponentInParent<Player_Controller>() != null)
            damage = GetComponentInParent<Player_Controller>().weaponDamage;
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
        Debug.Log(colliders.Length);

        if(colliders.Length != 0)
        {
            for(int i = 0; i < colliders.Length; i++)
            {
                IHealth health = hitResults[i].GetComponentInParent<IHealth>();

                if (health != null)
                {
                    Debug.Log("Applying damage to: " + hitResults[i].gameObject.name);
                    health.TakeDamage(damage);
                }
                else
                {
                    Debug.Log("No IHealth component found on: " + hitResults[i].gameObject.name);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(swordTip.position, damageCapsuleRadius);
        Gizmos.DrawWireSphere(swordBottom.position, damageCapsuleRadius);
    }
}
