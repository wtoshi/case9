using System.Collections.Generic;
using UnityEngine;

public class TowerSlotController : MonoBehaviour
{
    public List<GameEntries.Slot> slots;

    public Vector3 GetSocketPosition(GameEntries.SOCKET_TYPE socketType)
    {
        var slot = slots.Find(s => s.slotType == socketType);

        return slot.slotTransform.position;
    }
}
