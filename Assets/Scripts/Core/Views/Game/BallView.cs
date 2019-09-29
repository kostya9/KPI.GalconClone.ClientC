using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallView : MonoBehaviour
{
    public System.Action onWrongObjectHit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("untouchable"))
        {
            if (onWrongObjectHit != null)
            {
                onWrongObjectHit();
            }
        }
    }
}
