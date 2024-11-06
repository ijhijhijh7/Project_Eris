using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bashscript : MonoBehaviour
{
    public GameObject[] Colliders;

    private void Start()
    {
        Destroy(gameObject, 1.7f);
        StartCoroutine(Controller());
    }

    private IEnumerator Controller()
    {
        yield return new WaitForSeconds(0.5f);      
        Colliders[1].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        Colliders[0].SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Colliders[2].SetActive(true);
        Colliders[1].SetActive(false);
    }    
}
