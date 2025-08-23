using UnityEngine;

public class Follow : MonoBehaviour
{
    RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        screenPos.y -= 30f; // y√‡¿∏∑Œ -30
        rect.position = screenPos;
    }
}
