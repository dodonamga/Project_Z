using UnityEngine;

public class Cuser : MonoBehaviour
{
    // 커서 이미지 변경을 위한 변수
    [SerializeField] Texture2D cuserImg;
    // 커서 위치를 갖기위해 변수 선언
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
