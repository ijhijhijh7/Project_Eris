using UnityEngine;

/// <summary>
/// ������ �����ӿ� ���� ������ õ��� �ٴ�(����� �̹������� �� ���� �����Ǿ��־����)�� ��ġ��
/// y�� ������ �Ѿ�� ��� ������ ��ġ�� �������ϴ� ��ũ��Ʈ
/// </summary>
public class LenPlatformLoop : MonoBehaviour
{
    // PatternController���� ���� ������ ���, �ϰ����θ� �Ǻ��ϱ� ���� �ʿ�
    [SerializeField] PatternController patternController;
    // �ڽĿ�����Ʈ�� ������ ������ ����(num)�� �����ߴ� ����(space)���� ������Ʈ�� ��ġ�� �����ϱ� ���� �ʿ�
    [SerializeField] CreatePlatform createPlatform;
    [SerializeField] Transform ciling; // õ�� - ����̹������� �� �а� ������ ��
    [SerializeField] Transform dCiling; // �ٴ� - ����̹������� �� �а� ������ ��
    Transform makingPos; // ���� ���ġ�� ���� ���� ��ġ ����
    int num; // ������ ����
    public float space; // ������ ���� 

    private void Start()
    {
        // CreatePlatform���� ������ ��ġ�� �ҷ�����
        makingPos = createPlatform.SetPos;
        num = createPlatform.num - 1; // ������ ������ num �̹Ƿ� �������� ���� num - 1
        space = createPlatform.space;
    }
    private void Update()
    {
        // �����
        if (patternController.isUpMove)
        {
            // õ���� y������ ������Ʈ�� y ���� ��������
            if (gameObject.transform.position.y > ciling.transform.position.y)
            {
                // ���ӿ�����Ʈ�� ��ġ ����
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                            -(makingPos.position.y + space * num));
            }
        }
        // �ϰ���
        if (!patternController.isUpMove)
        {
            // �ٴ��� y������ ������Ʈ�� y���� ��������
            if (gameObject.transform.position.y < dCiling.transform.position.y)
            {
                // ���� ������Ʈ�� ��ġ ����
                gameObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                            makingPos.position.y + space * num);
            }
        }
    }
}
