using UnityEngine;

public class Cuser : MonoBehaviour
{
    // Ŀ�� �̹��� ������ ���� ����
    [SerializeField] Texture2D cuserImg;
    // Ŀ�� ��ġ�� �������� ���� ����
    [SerializeField] Camera mainCamera;

    private void Awake()
    {
        GameManager.instance.cuser = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;

        Vector2 hotspot = new Vector2(cuserImg.width / 2f, cuserImg.height / 2f);
        Cursor.SetCursor(cuserImg, hotspot, CursorMode.ForceSoftware);
    }

    public Vector3 GetmousePoint()
    {
        Vector3 worldMousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 0;
        return worldMousePos;
    }
}
