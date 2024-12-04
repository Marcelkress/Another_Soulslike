using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHelper : MonoBehaviour
{
    public void ActivateSword()
    {
        GetComponentInChildren<Weapon>().ActivateDamage();
    }
    public void DeActivateSword()
    {
        GetComponentInChildren<Weapon>().TurnOffDamage();
    }
}
