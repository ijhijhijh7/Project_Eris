Prefab 폴더

- Stage
  : Boss2Stage에서 사용할 수 있도록 제작한 프리팹 폴더
    1. Boss1 / Boss2 / Player 프리팹
       - 전체 비율을 맞추어 둔 사이즈로 저장되어 있으므로 프리팹으로 배치 후 원하는 컴포넌트를 이용하여 기능을 추가하여 사용
    2. StageCam 프리팹의 경우 Boss2Stage에서 바로 사용 가능한 카메라이며 다른 스테이지에서도 프리팹을 복사하여 
       내부의 CamSize 오브젝트의 PolygonCollider2D의 범위를 맵의 크기에 맞게 조절 후
       각 카메라 오브젝트의 Cinemachine Confiner2D에 PolygonCollider2D를 다시 적용하여 사용 가능

- TextUI
  : UISample에서 사용하고 있는 프리팹 폴더로 다른 씬에서 설정 후 바로 사용 가능
    1. UIDialogue 프리팹
       - Canvas 오브젝트를 생성한 후 Canvas 오브젝트의 하위 오브젝트로 설정하여야하며 각 씬에서의 UI들의 배치 프리팹
    2. DialogueManager 프리팹
       - 플레이어와 보스의 이미지 오브젝트(ImgPlayer / ImgBoss)를 포함
       - DatabaseManager를 포함하고 있음
       - DatabaseManager에 씬에서 사용하고자하는 스크립트 데이터 파일을 형식에 맞추어 .csv파일로 저장한 후 설정하여 사용
         : 이때 DialogueManager.cs에  ShowImgName()함수에서 사용하는 캐릭터ID 형식이 .csv파일의 형식과 
       - 각 보스와 플레이어의 이미지는 수정하여 사용
       - UIDialogue와 함께 사용하여 설정 가능
       - 인스펙터창에서 UnitId를 정확하게 csv파일과 동일하게 기재하여 사용할 것
    2. StageUICam 프리팹은 대화 시스템 전반에서 사용가능한 카메라로 별다른 특이사항은 없음
       - CamSize가 적용되어 있으므로 혹시라도 맵의 비율이 변경되어야하는 경우 수정하여 사용할 것