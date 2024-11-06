using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int targetFramerate;

    void Start()
    {
        Application.targetFrameRate = targetFramerate;
    }

}
