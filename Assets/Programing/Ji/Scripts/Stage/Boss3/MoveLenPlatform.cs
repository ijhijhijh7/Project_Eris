using UnityEngine;

public class MoveLenPlatform : MonoBehaviour
{
    // PatternController.cs에서 선언하고 가져오기
    PatternController patternController;
    public float moveSpeed;
   
    // patternController.cs에서 선언하고 MoveLenPlatform.cs을 거쳐 LenPlatformLoop.cs에서 사용
    public Transform pCiling; // 천장 - 배경이미지보다 더 넓게 설정할 것
    public Transform pGround; // 바닥 - 배경이미지보다 더 넓게 설정할 것
    // 방향별로 이동
    public bool isUpMove;

    private void Awake()
    {
        patternController = transform.parent.GetComponent<PatternController>();
        pCiling = patternController.pCiling;
        pGround = patternController.pGround;
    }
    private void Start()
    {
        moveSpeed = patternController.moveSpeed;
        int num = Random.Range(0, 2);
        switch (num)
        {
            case 0:
                isUpMove = false;
                Debug.Log("다운");
                break;
            case 1:
                isUpMove = true;
                Debug.Log("업");
                break;
        }

    }

    private void Update()
    {
        if (isUpMove)
        {
            MoveUp();
        }
        else if (!isUpMove)
        {
            MoveDown();
        }
    }

    /// <summary>
    /// 발판을 일정한 속도로 위로 계속 이동
    /// </summary>
    public void MoveUp()
    {
        transform.Translate(Vector2.up * moveSpeed * Time.deltaTime);
    }

    public void MoveDown()
    {
        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
    }

}
