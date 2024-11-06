using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChara : MonoBehaviour
{
    [SerializeField] float moveSpeed;
   
    [SerializeField] float Atk;

    [SerializeField] bool firstStage;
    [SerializeField] bool secondStage;
    [SerializeField] bool thridStage;

    [SerializeField] bool ondash;
    [SerializeField] bool onhover;



    [Range(0, 1)]
    [SerializeField] float BgmVol;


    // 데이터 저장 및 불러오기 예시 , 데이터를 저장 및 불러오기를 하려면 LoadGameData() , SaveGameData();  가 선행 되어야 함 
    // 저장이 필요한 타이밍에 아래와 비슷하게 작성해서 저장 및 불러오기를 해야함 
    private void Start()
    {
        DataManager.Instance.LoadGameData(); // 불러오기
        DataManager.Instance.data.isUnlock[0] = firstStage;
        DataManager.Instance.data.isUnlock[1] = secondStage;
        DataManager.Instance.data.isUnlock[2] = thridStage;
        DataManager.Instance.SaveGameData();
    }

    private void OnApplicationQuit()
    {
      //  Saving();
      //  DataManager.Instance.SaveGameData(); // 저장하기 
    }

    public void Saving() // 씬에 있는 인스턴스로 데이터 전달 
    {
        DataManager.Instance.data.isUnlock[0] = firstStage;
        DataManager.Instance.data.isUnlock[1] = secondStage;
        DataManager.Instance.data.isUnlock[2] = thridStage;


        DataManager.Instance.data.vol = BgmVol;
   

        DataManager.Instance.data.dash = ondash;
        DataManager.Instance.data.hover = onhover;
    }
    public void Loading() // json 데이터 파일에서 불러온 데이터를 뽑아옴 
    {
        firstStage = DataManager.Instance.data.isUnlock[0];
        secondStage = DataManager.Instance.data.isUnlock[1];
        thridStage = DataManager.Instance.data.isUnlock[2];


        BgmVol = DataManager.Instance.data.vol;
     

        ondash = DataManager.Instance.data.dash;
        onhover = DataManager.Instance.data.hover;
    }

}
