using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentFlag : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
