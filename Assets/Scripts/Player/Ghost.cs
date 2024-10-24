using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    private Transform target;
    private float moveSpeed = 3f;
    private Vector3 originScale;
    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        originScale = transform.localScale;
    }

    private void Update()
    {
        transform.Translate((target.position - transform.position).normalized * moveSpeed * Time.deltaTime);
        if (target.position.x > transform.position.x) transform.localScale = originScale;
        else if (target.position.x < transform.position.x) transform.localScale = new Vector3(-originScale.x, originScale.y, originScale.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EventManager.GameRestartEvent();
            Debug.Log("Remake");
        }
    }
}
