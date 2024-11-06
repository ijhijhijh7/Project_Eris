using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowList : MonoBehaviour
{
    [SerializeField] GameObject UITeamList; // 제작진 리스트 UI
    [SerializeField] GameObject TeamBackground; // 제작진 리스트 출력 시 배경
    [SerializeField] GameObject TeamText; // 제작진 텍스트

    bool isClicked = false;

    private void Update()
    {
        Debug.Log(isClicked);
        if (!isClicked)
        {
            EndShow();
        }
    }

    public void Clicked()
    {
        isClicked = !isClicked;
    }
    public void StartShow()
    {
        UITeamList.SetActive(true); 
        TeamBackground.SetActive(true);
        TeamText.SetActive(true);
    }
    public void EndShow()
    {
        UITeamList.SetActive(false);
        TeamBackground.SetActive(false);
        TeamText.SetActive(false);
    }

}
