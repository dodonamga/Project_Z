using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public enum TeleportType { Room0, Room1, Room2 };
    public TeleportType teleportType;

    public Vector3 room0Tp = Vector3.zero;
    public Vector3 room1Tp = Vector3.zero;
    public Vector3 room2TP = Vector3.zero;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Transform player = collision.transform;

        switch (teleportType) {
            case TeleportType.Room0:
                player.position = room0Tp;
                break;
        }
        
    }
}
