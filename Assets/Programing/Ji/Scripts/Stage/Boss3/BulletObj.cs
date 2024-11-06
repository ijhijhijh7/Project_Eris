using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletObj : MonoBehaviour
{
    public BulletPool bulletPool;

    [SerializeField] public List<BulletObj> shootBulletPool; // 총알의 오브젝트 풀 선언

    [SerializeField] float returnTime; // 총알의 반환시간
    [SerializeField] public float curTime; // 총알이 생성된 현재 시간으로 총알이 활성화되면 회수시간과 동일하게 설정

    // ShootManger.cs에서 반환하기 위해 사용
    List<BulletObj> returnPool; // 반환할 총알의 리스트
    BulletObj returnObj; // 반환할 BulletObj

    /// <summary>
    /// 객체 활성화시 각각 shootManager.cs에 의해 발사된 각 BulletObj 객체와 리스트를 저장하고
    /// 현재 시간을 returnTime으로 타이머를 설정
    /// </summary>
    private void OnEnable()
    {
        curTime = returnTime;
        // 총알의 발사 시 ShootManager에서 생성되는 오브젝트와 리스트 가져오기
        returnPool = GameObject.Find("ShootManager").GetComponent<ShootManager>().nowBulletPool;
        returnObj = GameObject.Find("ShootManager").GetComponent<ShootManager>().nowBullet;
    }

    private void Update()
    {
        curTime -= Time.deltaTime; // 시간 타이머 시작

        if (curTime < 0)
        {
            ReturnBullet(returnObj, returnPool); // 회수를 시작
        }
    }

    // 총알이 플레이어와 충돌 시
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            ReturnBullet(returnObj, returnPool); // 회수를 시작
        }
    }

    // 반활할 총알과 반활할 리스트를 받아서 회수를 시작
    public void ReturnBullet(BulletObj returnObj, List<BulletObj> bulletPools)
    {
        bulletPool.ReturnBulletObjPull(returnObj, bulletPools); // returnPool.cs에 있는 ReturnBulletPool()을 사용하여
        // 반환을 시작
    }

}
