using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Data  // 데이터 클래스 
{
    // 각 챕터의 잠금여부를 저장할 배열
    public bool[] isUnlock = new bool[5];

    // 저장할 데이터 자유롭게 추가 가능
    public float vol;
    public bool dash;
    public bool hover;


}
