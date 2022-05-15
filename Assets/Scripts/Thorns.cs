using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorns : MonoBehaviour
{
    public FrogMovement FrogMovementSc;

    void Start()
    {
        FrogMovementSc = GameObject.Find("Frog").GetComponent<FrogMovement>();
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            FrogMovementSc.IsAlive = false;
        }
    }
}
