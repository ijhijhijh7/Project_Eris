using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BulletPool : MonoBehaviour
{
    [SerializeField] public List<BulletObj> bulletPoolList = new List<BulletObj>();
    [SerializeField] int size; // 총알이 저장될 오브젝트 풀의 사이즈
    [SerializeField] BulletObj prefab; // 생성할 총알 프리펩을 인스턴스 창에서 설정
    [SerializeField] Transform point; // 총알이 생성되는 위치

    private void Awake()
    {
        // 사이즈 만큼의 총알을 생성해서 리스트에 저장
        for(int i = 0; i < size; i++)
        {
            BulletObj bullet = Instantiate(prefab);
            bullet.gameObject.SetActive(false); // 생성 후 비활성화 상태로 저장할 것
            bullet.transform.parent = transform;
            bulletPoolList.Add(bullet);
        }
        point = GameObject.Find("ShootManager").GetComponent<ShootManager>().point; // StootManager에서 지정한 발사지점 불러오기
    }

    /// <summary>
    /// ShootManager에서 사용하며
    /// 리스트에 저장한 총알 객체를 원하는 위치와 회전에 생성하는 함수
    /// </summary>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="bullets"></param>
    /// <returns></returns>
    public BulletObj GetBulletObjPool(Vector2 position, Quaternion rotation, List<BulletObj> bullets)
    {
        // 리스트의 용량에 자리가 있을 경우
        if (bullets.Count > 0)
        {
            BulletObj fireBullet = bullets[bullets.Count - 1];
            fireBullet.transform.position = position;
            fireBullet.transform.rotation = rotation;
            fireBullet.transform.parent = null;
            fireBullet.bulletPool = this; // 반환할 오브젝트를 설정해야함
                                          // BulletObj.cs파일에 returnPool을 정의하여 사용
            return fireBullet;
        }
        else
        {
            // 저장된 탄이 없는 경우
            BulletObj bullet = Instantiate(prefab,position,rotation);
            /*bullet.transform.position = position;
            bullet.transform.rotation = rotation;
            bullet.transform.parent = null;*/
            return bullet; // 새로 생성해서 발사
        }
    }

    public void ReturnBulletObjPull(BulletObj bullet, List<BulletObj> bulletPool)
    {
        bullet.gameObject.SetActive(false); // 총알을 비활성화하고
        bullet.transform.parent = point; // 기존의 위치로 이동시켜
        bulletPool.Add(bullet); // 원래의 리스트에 새로 추가
    }

}
