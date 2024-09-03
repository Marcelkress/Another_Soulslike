using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseKnight : MonoBehaviour
{
    public GameObject[] playerModels = new GameObject[5];
    public GameObject[] displayModels = new GameObject[5];

    private int currentKnight;

    private void Start()
    {
        currentKnight = 1;

        foreach(GameObject model in playerModels)
        {
            model.SetActive(false);
        }
        foreach(GameObject model in displayModels)
        {
            model.SetActive(false);
        }

        playerModels[currentKnight].SetActive(true);
        displayModels[currentKnight].SetActive(true);
    }

    public void Choose(int value)
    {
        foreach(GameObject model in playerModels)
        {
            model.SetActive(false);
        }
        foreach(GameObject model in displayModels)
        {
            model.SetActive(false);
        }
        
        currentKnight = value - 1; // Minus one because arrays start at 0

        playerModels[currentKnight].SetActive(true);
        displayModels[currentKnight].SetActive(true);
    }
}
