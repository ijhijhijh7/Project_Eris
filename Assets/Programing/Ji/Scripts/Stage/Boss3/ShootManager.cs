using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스 공격 패턴과 연결이 필요
/// </summary>
public class ShootManager : MonoBehaviour
{
    [Header("Balance")]

    [SerializeField] int sameNum; // 동시에 출력할 총알 개수
    [SerializeField] float shootTimer; // 총알이 발사되는 공격패턴이 실행되는 시간
    float nowShootTimer;
    [SerializeField] float spaceTime; // 총알이 발사되는 시간 간격

    List<int> rotationList; // 발사할 총알 회전값을 리스트에 저장한 것

    [SerializeField] public Transform point; // 발사로 생성할 위치
    private BulletPool bulletPool = new BulletPool();

    [SerializeField] float bulletSpeed; // 발사할 총알의 속도
    [SerializeField] public List<BulletObj> fireBulletPool; //기본 리스트

    [Header("BulletObj.cs")]
    [SerializeField] public List<BulletObj> nowBulletPool; // 사용할 총알의 오브젝트 풀
    public BulletObj nowBullet; // 발사할 총알



    private void Start()
    {
        rotationList = new List<int>();
        // 시작하자마자 생성되는 리스트를 가져오기
        fireBulletPool = GameObject.Find("BulletPool").GetComponent<BulletPool>().bulletPoolList;
    }
    /*
    // 보스의 상태가 상태에 따라서 패턴을 출력하도록 수정이 필요
    // 총알을 지정한 개수만큼 발사
    // 단, 회전을 360중 랜덤하게 발사
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            StartCoroutine(timerCounter());
        }
    }
    */

    public void Shooting()
    {
        StartCoroutine(timerCounter());
    }

        // 일정시간(shootTimer)동안 일정간격(spaceTime)으로 출력하기
    IEnumerator timerCounter()
    {
        nowShootTimer = shootTimer; // 시간을 세팅
        while (nowShootTimer >= 0) // 시간이 남아있는 동안
        {
            RandomRotation(); // 랜덤 함수를 통해서 겹치지 않게 출력
            for (int i = 0; i < sameNum; i++) // 동시에 출력되는 개수(sameNum)을 함께 출력하고
            {
                // 총알의 회선은 RandomRotation()에서 만든 리스트에서 출력
                nowBullet = bulletPool.GetBulletObjPool(new Vector2(point.position.x, point.position.y),
                                                            Quaternion.Euler(0, 0, rotationList[i]), fireBulletPool);
                nowBulletPool = fireBulletPool;
                Fire(nowBullet, nowBulletPool);
            }
            yield return new WaitForSeconds(spaceTime); // 일정한 간격만큼 멈춤
            nowShootTimer -= spaceTime;
        }
    }

    /// <summary>
    /// 생성할 전체 총알의 개수에 맞는 회전값을 겹치지 않게 출력하여 리스트에 저장하는 함수
    /// </summary>
    private void RandomRotation()
    {
        rotationList.Clear(); // 매번 실행 시 리스트의 초기화 필요
        for (int i = 0; i < sameNum; i++)
        {
            int num = Random.Range(0, 37) * 10; // 임의의 값을 출력 - 1도씩 나오는 경우 차이가 크지않아 임의로 10도씩 차이를 두기로 설정
            if (rotationList.Contains(num)) // 동일한 수가 나올 경우 다시 숫자를 출력
            {
                i--;
                continue;
            }
            else
            {
                rotationList.Add(num);
            }
        }
    }


    /// <summary>
    /// 총알을 발사하는 함수
    /// </summary>
    /// <param name="bulletObj"></param>
    /// <param name="bulletPoolList"></param>
    public void Fire(BulletObj bulletObj, List<BulletObj> bulletPoolList)
    {
        bulletObj.gameObject.SetActive(true); // 총알 활성화
        bulletPoolList.RemoveAt(bulletPoolList.Count - 1); // 리스트에서 가져온 총알 삭제
        Boss3Bullet bullet = bulletObj.GetComponent<Boss3Bullet>(); // Boss3Bullet의 Bullet 컴포넌트를 가져와서
        bullet.SetSpeed(bulletSpeed); // 총알의 속도를 설정하여 사용
    }
}
