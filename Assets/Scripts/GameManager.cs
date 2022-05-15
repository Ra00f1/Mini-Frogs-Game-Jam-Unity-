using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public FrogMovement FrogMovementSc;

    public GameObject Winpanel;

    public bool Won = false;

    private void Start()
    {
        Winpanel.active = false;
        FrogMovementSc = GameObject.FindWithTag("Player").GetComponent<FrogMovement>();
    }

    void Update()
    {
        if (Won == true)
        {
            Winpanel.active = true;
            FrogMovementSc.IsAlive = false;
        }
    }
}
