<FinalScene 사용법>

1. Sample 폴더
- Sample 폴더에는 BossD와 BossS Scene가 있습니다.
   : 두 씬은 각각 대화Scene(BossD), 스테이지Scene(BossS)의 가장 기초적인 뼈대를 제작해 둔 Scene파일입니다.
   : BossD는 대화에서 필요한 UI와 컴포넌트와 스크립트를 배치해두었으며, 필요한 파일의 연결 후 사용 가능합니다.
     1) DatabaseManager에 Csv Data 파일을 연결하기
     2) DialogueManager에 DatabaseManager와 각종 UI와 Text, Image 오브젝트를 연결하고 CSV파일에서 사용한
        올바른 Unit파일을 연결하기
   : BossS는 전투 장면에서 필요한 기본 카메라 움직임을 배치에두었으며, 필요한 파일 연결 후 사용 가능합니다.
     1) Player에 PlayerController.sc에 GameObjectRoatation, PlayerAinmator, GFX, AttatackTest를 연결하고
        StageCam을 CameraController에 연결하기
     2) StageCam 안에 있는 LPlayCam과 RPlayCam의 CinemachinConfiner2D를 삭제하여 초기화하고
        CinemachineVirualCamera컨트롤러의 AddExtension으로 새로 CinemachinConfiner2D를 추가하기
     3) StageCam 안에 있는 CamSize의 Polygon Collider2D를 조절하여 새로운 배경의 이미지와 일치시키고
        2)에서 제작한 CinemachinConfiner2D에 추가하여 연결하기

2. 각 Boss폴더 안에 대화Scene 3개(Start, Phase, End)와 전투Scene(Phase1, Phase2)
   그리고 제작에 사용된 Data파일이 정리되어 있습니다.
- 각 전투Scene는 바로 재생하는 경우, 작동은 제대로 하지만 NullReferenceException 에러가 발생합니다.
  아마 Test를 위한 Player의 설정이 완전하지 않아서 발생하는 에러이므로 추후 Scene를 완성하면 발생하지 않을 것으로
  예상합니다.
- Boss폴더 안의 Prefab 폴더에는 각 Scene에서 사용된 것들을 혹시 필요할까 싶어 설정 후 프리팹화하여 저장한 것입니다.
- Boss2 폴더
   : 전투Scene(Phase1, Phase2)의 경우 SideWall오브젝트에 플레이어를 연결하여 사용해야합니다.
- 각 폴더 안에는 각 보스의 장면을 제작할때 사용한 데이터와 프리팹이 있습니다.
   : 보스의 장면들에 사용된 프리팹이므로 원하면 폴더내의 프리팹에서 수정하여 사용할 수 있습니다.
   : 단, 스크립트는 개별 스크립트로 적용되어있으므로 스크립트의 수정은 불가합니다.
