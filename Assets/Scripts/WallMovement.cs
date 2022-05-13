using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMovement : MonoBehaviour
{
    public float Speed = 3;
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * (Time.deltaTime * Speed), Space.World);
    }
}
