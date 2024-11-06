using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class delete : MonoBehaviour
{
    [SerializeField] float destorytime;

    private void Start()
    {
        Destroy(gameObject, destorytime);
    }
}
