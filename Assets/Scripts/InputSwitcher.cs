using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSwitcher : MonoBehaviour
{
    private PlayerInput input;
    private TentInteract tent;

    // Start is called before the first frame update
    void Start()
    {
        input = GetComponent<PlayerInput>();
        input.SwitchCurrentActionMap("Player");
        
        tent = FindAnyObjectByType<TentInteract>();
            
        if(tent != null)
        {
            tent.OpenUIEvent += SwitchToUIMap;
        }
    }

    public void SwitchToUIMap()
    {
        input.SwitchCurrentActionMap("UI");
        Cursor.lockState = CursorLockMode.None;
    }

    public void SwitchToPlayerMap()
    {
        input.SwitchCurrentActionMap("Player");
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDestroy()
    {
        if (tent != null)
        {
            tent.OpenUIEvent -= SwitchToUIMap;
        }
    }
}
