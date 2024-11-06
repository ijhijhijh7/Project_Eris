using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float damage;

    private void Start()
    {   
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 playerPos = player.transform.position;

        Vector2 direction = playerPos - transform.position;


        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.Translate(Vector2.right * 30f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            PlayerRPG player = collision.GetComponent<PlayerRPG>();
            player.TakeDamage(damage);


        }
    }
}
