using UnityEngine;

public class Follow : MonoBehaviour {
    public enum FollowType { Player, Monster }
    public FollowType fw;

    public Transform target;      // ���� ��� (���ͳ� �÷��̾� Transform)
    public Vector3 offset;        // ��ġ ������ (�Ӹ� ���� �ø��� ��)
    public Canvas canvas;         // ����ٴ� ĵ���� (WorldSpace/ScreenSpace)

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        Transform unitRoot = transform.parent.parent.Find("UnitRoot");
        target = unitRoot;
        canvas.sortingOrder = 10;
    }

    private void LateUpdate() // ī�޶� �̵� �� ���󰡵��� LateUpdate ��� ��õ
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

        // --- ĵ���� ��庰 ó�� ---
        if (canvas.renderMode == RenderMode.WorldSpace) {
            // ���� ���� �� �״�� ��ġ ����
            rect.position = worldPos;
        }
        else {
            // ���� ��ǥ �� ��ũ�� ��ǥ
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            // ī�޶� ���ʿ� ������ ���� (z < 0)
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
