using UnityEngine;

public class Follow : MonoBehaviour {
    public enum FollowType { Player, Monster }
    public FollowType fw;

    public Transform target;      // 따라갈 대상 (몬스터나 플레이어 Transform)
    public Vector3 offset;        // 위치 보정값 (머리 위로 올리기 등)
    public Canvas canvas;         // 따라다닐 캔버스 (WorldSpace/ScreenSpace)

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        Transform unitRoot = transform.parent.parent.Find("UnitRoot");
        target = unitRoot;
        canvas.sortingOrder = 10;
    }

    private void LateUpdate() // 카메라 이동 후 따라가도록 LateUpdate 사용 추천
    {
        if (canvas == null) return;

        switch (fw) {
            case FollowType.Player:
                if (GameManager.instance == null || GameManager.instance.player == null) return;
                FollowTarget(GameManager.instance.player.transform);
                break;

            case FollowType.Monster:
                if (target == null) return;
                FollowTarget(target);
                break;
        }
    }

    private void FollowTarget(Transform followTarget)
    {
        Vector3 worldPos = followTarget.position + offset;

        // --- 캔버스 모드별 처리 ---
        if (canvas.renderMode == RenderMode.WorldSpace) {
            // 월드 공간 → 그대로 위치 지정
            rect.position = worldPos;
        }
        else {
            // 월드 좌표 → 스크린 좌표
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            // 카메라 뒤쪽에 있으면 무시 (z < 0)
            if (screenPos.z < 0) return;

            if (canvas.renderMode == RenderMode.ScreenSpaceOverlay) {
                rect.position = screenPos;
            }
            else // ScreenSpace - Camera
            {
                Vector2 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.GetComponent<RectTransform>(),
                    screenPos,
                    canvas.worldCamera,
                    out localPos
                );
                rect.localPosition = localPos;
            }
        }
    }
}
