using UnityEngine;

public class Boss3Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed; // 탄환의 이동속도
    AudioSource bulletSound; // 총알 발사음

    private void Awake()
    {
        bulletSound = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        bulletSound.Play();
    }
    private void OnDisable()
    {
        bulletSound.Stop();
    }
    private void Update()
    {
        // 총알의 방향에 맞게 직진
        gameObject.transform.Translate(Vector2.up * bulletSpeed * Time.deltaTime, Space.Self);

    }

    public void SetSpeed(float speed)
    {
        this.bulletSpeed = speed;
    }
}
