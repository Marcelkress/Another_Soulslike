using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHelper : MonoBehaviour
{
    public void ToggleSword()
    {
        GetComponentInChildren<Weapon>().ToggleSword();
    }
}
