using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]

public class AlarmZoneDetector : MonoBehaviour
{

    public static event Action<bool, GameObject> AlarmTriggered = delegate { };


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController objectStatus))
        {
            AlarmTriggered(true, other.gameObject);
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController objectStatus))
        {
            AlarmTriggered(false, other.gameObject);
        }

    }


}
