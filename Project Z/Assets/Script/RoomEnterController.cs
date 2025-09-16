using Unity.VisualScripting;
using UnityEngine;

public class RoomEnterController : MonoBehaviour
{
    EnterRoom[] enterRooms;
    public GameObject[] titles;
    private void Awake()
    {
        enterRooms = GetComponentsInChildren<EnterRoom>();
    }

    private void Update()
    {
        if (titles[0].activeSelf || titles[0].activeSelf) {
            for(int index = 0; index < enterRooms.Length; index++) {
                enterRooms[index].gameObject.SetActive(false);
            }
        }
        else {
            for (int index = 0; index < enterRooms.Length; index++) {
                enterRooms[index].gameObject.SetActive(true);
            }
        }
    }
}
