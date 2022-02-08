using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]

public class AlarmZoneDetector : MonoBehaviour
{

    public static event Action<bool, GameObject> Alarm = delegate { };
    public static event Action<bool, GameObject> EatingProcess = delegate { };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController objectStatus))
        {
            Alarm(true, other.gameObject);
        }
        else if (other.gameObject.TryGetComponent(out NPC eatStatus))
        {
            EatingProcess(true, other.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController objectStatus))
        {
            Alarm(false, other.gameObject);
        }
        else if (other.gameObject.TryGetComponent(out NPC eatStatus))
        {
            EatingProcess(false, null);
        }

    }


}
