using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBranch : MonoBehaviour
{
    private Animator Animator;

    void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(TimeCounter());
            Animator.SetBool("IsFrogOn", true);
        }
    }
    private IEnumerator TimeCounter()
    {
        float countdown = 2f;
        yield return new WaitForSeconds(countdown);
        Destroy(gameObject);
    }
}
