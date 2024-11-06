using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static PatternController;

public class Boss3Controller : MonoBehaviour
{
    [SerializeField] Transform[] shootingPoints;
    [SerializeField] GameObject[] lineRender;
    [SerializeField] GameObject player;

    [SerializeField] GameObject dealTimePlatform;


    public UnityEvent bulletShooting;
    public UnityEvent movePlatform;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;

    // 발사용 프리팹
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject missilePrefab;
   



    [SerializeField] float bossHp;  // 보스 체력
    [SerializeField] float patternTerm; // 보스 패턴간 시간 
    [SerializeField] int patternCount = 0;
    [SerializeField] bool phasech;

    
    private int patternSelectNum;

    [SerializeField] float laserTerm = 0.7f; // 레이저 발사 간격
    [SerializeField] int missileQty = 3; // 미사일 갯수(최대 4)
   // [SerializeField] bool onPhaseChange = false; // 페이즈 바뀌면 true
    Coroutine curCoroutine;
    private void Start()
    {
        
        curCoroutine = StartCoroutine(BossDo());

    }
    private void Update()
    {
       
        if (bossHp <= 0) 
        {
            Debug.Log("보스 사망");
            StopCoroutine("BossDo");
            StartCoroutine(Died());
        }
    }
    IEnumerator BossDo() // 보스의 행동. 패턴 4개 중앙 총알발사 , 레이저발사 , 미사일 발사 , 발판 이동  
                        // 일정 시간(조절 가능하게)마다 패턴 1개씩만 수행, 패턴을 몇번 수행하고 나면 중앙 발판 생성(딜타임)
    {
        WaitForSeconds time = new WaitForSeconds(patternTerm);

        while (bossHp > 0)  // 체력이 0보다 크면 반복
        {
            
            if (patternCount >= 3)
            {
                //  패턴을 3번 수행하면 딜타임(중앙 발판 생성)
                Debug.Log("딜타임");
                dealTimePlatform.SetActive(true);
                patternCount = 0;
            }
            else
            {
                patternSelectNum = Random.Range(0, 4);// 0 ~ 3
                switch (patternSelectNum) // 패턴 랜덤 수행 
                {

                    case 0:
                        Debug.Log("중앙 총알");
                        audioSource.clip = audioClips[2];
                        audioSource.Play();
                        bulletShooting?.Invoke();
                        break;
                    case 1:
                        Debug.Log("발판 이동");
                        audioSource.clip = audioClips[0];
                        audioSource.Play();
                        movePlatform?.Invoke();
                        yield return time;
                        audioSource.clip = audioClips[1];
                        audioSource.Play();
                        break;
                    case 2:
                        Debug.Log("미사일");
                        StartCoroutine(ShootingMissile());
                        break;
                    case 3:
                        Debug.Log("레이저");
                        StartCoroutine(ShootingLaser());
                        break;
                }
                patternCount++;
            }

            yield return time;
        }



        yield return new WaitForSeconds(1f);
    }

    IEnumerator ShootingLaser() //레이저 패턴, 1발씩 6번씩 쏨 
    {
        for (int i = 0; i < 6; i++)
        {
            int rand = Random.Range(0, 6);
            lineRender[rand].SetActive(true);
            yield return new WaitForSeconds(1f);
            lineRender[rand].SetActive(false);
            audioSource.clip = audioClips[5];
            audioSource.Play();
            GameObject obj = Instantiate(laserPrefab, shootingPoints[rand].position, shootingPoints[rand].rotation);
            Destroy(obj,20f);
            yield return new WaitForSeconds(laserTerm);
        }

        yield return null;
    }
    IEnumerator ShootingMissile() // 미사일 패턴 , 한번에 3발을 쏨 
    {
        List<int> randNums = new List<int> { 0, 1, 2, 3, 4, 5 };  //  중복 안되는 n개 뽑는 기능 
        List<int> pickNum = new List<int>();
        while (pickNum.Count < missileQty)
        {
            int rand = Random.Range(0, randNums.Count);
            if (randNums.Contains(rand) == true)
            {
                pickNum.Add(rand);
                randNums.Remove(rand);
            }
            if (pickNum.Count == missileQty)
            {
                
                break;

            }
        }
        for (int i = 0; i < pickNum.Count; i++) 
        {
            lineRender[pickNum[i]].SetActive(true);
        }
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < pickNum.Count; i++)
        {
            lineRender[pickNum[i]].SetActive(false);
        }
        audioSource.clip = audioClips[4];
        audioSource.Play();
        for (int i = 0; i < pickNum.Count; i++) 
        {   

            Instantiate(missilePrefab, shootingPoints[pickNum[i]].position, shootingPoints[pickNum[i]].rotation);
        }
    }
    //private void PhaseChange() // 일정 체력 이하면 페이즈 변경 , 보스 패턴변경 및 이미지 변경 
    //{
    //    if (onPhaseChange == false) 
    //    {
    //        Debug.Log("페이즈 변경");
    //        patternTerm = 4f; // 기본 6초 
    //        laserTerm = 0.3f; // 기본 1초 
    //        missileQty = 4;  // 기본 3개 
    //        onPhaseChange =true;

    //        boss.sprite = bossPhase2img;
    //        backGroundimg.sprite = bossPhase2backimg;
    //        mid.color = new Color(1f, 0.698f, 0.698f, 1f);
    //    }
    //}

 


    public void TakeDamage(float damage) // 업데이트나 이벤트로 처리하면 될듯
    {
        bossHp -= damage;

        // 보스의 체력이 0 이하가 되면 상태를 Die로 변경
        if (bossHp <= 0)
        {
            StartCoroutine(Died());
        }
    }
    private void Die() 
    {
        // 보스 사망 

        if (phasech == false)
        {
            //1페이즈 끝 
            GameManager.Instance.LoadSceneByName("Boss3DPhase");
        }
        else if (phasech == true) // 2페이즈 끝 
        {
            DataManager.Instance.LoadGameData();
            DataManager.Instance.data.isUnlock[2] = true;
            DataManager.Instance.SaveGameData();
            GameManager.Instance.LoadSceneByName("Boss3DEnd");
        }
    }
    IEnumerator Died()
    {
        if (phasech == false)
        {
            //1페이즈 끝 
            yield return new WaitForSeconds(0.5f);
            GameManager.Instance.LoadSceneByName("Boss3DPhase");
        }
        else if (phasech == true) // 2페이즈 끝 
        {
            yield return new WaitForSeconds(0.5f);
            DataManager.Instance.LoadGameData();
            DataManager.Instance.data.isUnlock[2] = true;
            DataManager.Instance.SaveGameData();
            GameManager.Instance.LoadSceneByName("Boss3DEnd");
        }
    }
}
