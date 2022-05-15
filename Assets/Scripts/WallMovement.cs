using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    public float Speed = 3;
    public float SlowedSpeed = 3;

    public bool WallIsSlowed = false;
    void Update()
    {
        if (WallIsSlowed == false)
        {
            transform.Translate(Vector2.right * (Time.deltaTime * Speed), Space.World);
        }
        if (WallIsSlowed == true)
        {
            transform.Translate(Vector2.right * (Time.deltaTime * SlowedSpeed), Space.World);
        }
    }
}
