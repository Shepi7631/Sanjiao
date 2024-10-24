using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialAreaDetector : MonoBehaviour
{
    private new Collider2D collider2D;
    public bool flag;
    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    public bool InSpecialArea => collider2D.IsTouchingLayers();

    private void Update()
    {
        flag = InSpecialArea;
    }
}
