using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] float damagetoPl;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
            Debug.Log($"{damagetoPl}플레이어 맞음");
            PlayerRPG player = collision.GetComponent<PlayerRPG>();
            player.TakeDamage(damagetoPl);
            
            
        }
    }
}
