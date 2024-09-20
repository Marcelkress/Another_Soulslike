using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class PlayerHealthbar : MonoBehaviour
{
    private Slider slider;
    private PlayerHealth ph;

    void Start()
    {
        slider = GetComponent<Slider>();  
        ph = GetComponentInParent<PlayerHealth>();

        slider.maxValue = ph.GetCurrentHealth();
        slider.value = ph.GetCurrentHealth();
        ph.takeDamageEvent.AddListener(AdjustHealthBar);
    }

    private void AdjustHealthBar()
    {
        slider.value = ph.GetCurrentHealth();
    }
}
