using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().AgeForward();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
