using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidePlatform : MonoBehaviour
{
    float velocity;
    Rigidbody2D playerRigidbody;
    bool isLeft; // �÷��̾ ���ִ� ������ �Ǻ�
    public bool isCollison = false; // �÷��̾ �÷����� �浹���θ� Ȯ��

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            // �÷��̾��� ������ �ٵ� ��������
            playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            // �浹�� ��ü�� x ��ġ���� ���� ��ü�� x������ ũ�ų� �������� ���Ͽ� bool���� ����
            isLeft = collision.gameObject.transform.position.x < transform.position.x; 
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isCollison = true;
            if (isLeft) // �÷��̾ ��ü�� ���غ��� ���ʿ� �ִ� ���
            {
                // �÷��̾��� �ӷ��� �־ �з������� ����
                playerRigidbody.velocity = new Vector2(velocity, playerRigidbody.velocity.y);
            }
            else // �÷��̾ ��ü�� ���غ��� �����ʿ� �ִ� ���
            {
                // �÷��̾��� �ӷ��� �־ �з������� ���� - �ݴ�������
                playerRigidbody.velocity = new Vector2(-velocity, playerRigidbody.velocity.y);
            }
        }
    }
}