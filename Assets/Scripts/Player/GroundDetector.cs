using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    private Collider2D collider2D;  

    private void Awake()
    {
        collider2D = GetComponent<Collider2D>();
    }

    public bool IsGround => collider2D.IsTouchingLayers();
}
