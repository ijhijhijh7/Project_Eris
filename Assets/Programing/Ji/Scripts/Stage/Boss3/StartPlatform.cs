using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlatform : MonoBehaviour
{
    private void OnCollisionExit2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
