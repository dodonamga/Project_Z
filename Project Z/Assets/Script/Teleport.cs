using Unity.VisualScripting;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public enum TeleportType { Boss, Room0, Room1, Room2, Room3, Room4, Room5 };
    public TeleportType teleportType;

    public Vector3 Boss = Vector3.zero;
    public Vector3 room0Tp = Vector3.zero;
    public Vector3 room1Tp = Vector3.zero;
    public Vector3 room2Tp = Vector3.zero;
    public Vector3 room3Tp = Vector3.zero;
    public Vector3 room4Tp = Vector3.zero;
    public Vector3 room5Tp = Vector3.zero;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        Transform player = collision.transform;

        switch (teleportType) {
            case TeleportType.Room0:
                player.position = room0Tp;
                break;
            case TeleportType.Room1:
                player.position = room1Tp;
                break;
            case TeleportType.Room2:
                player.position = room2Tp;
                break;
            case TeleportType.Room3:
                player.position = room3Tp;
                break;
            case TeleportType.Room4:
                player.position = room4Tp;
                break;
            case TeleportType.Room5:
                player.position = room5Tp;
                break;
            case TeleportType.Boss:
                player.position = Boss;
                break;
        }
        
    }
}
