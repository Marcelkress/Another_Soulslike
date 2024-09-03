using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TentInteract : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject customKnightCanvas;
    public bool isActive;
    
    public delegate void tentDelegate();
    public tentDelegate OpenUIEvent;

    private void Start()
    {
        customKnightCanvas.SetActive(false);
        isActive = false;   
    }

    public void Interact()
    {
        if(isActive == true)
        {
            // Close the menu
            customKnightCanvas.SetActive(false);
            isActive = false;
        }
        else
        {
            // Open the menu
            customKnightCanvas.SetActive(true);
            isActive = true;
            OpenUIEvent?.Invoke();
        }
    }




}
