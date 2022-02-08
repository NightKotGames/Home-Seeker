using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]

public class HideZone : MonoBehaviour
{

    public static Action<bool, GameObject> Hide = delegate { };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController objectStatus))
        {

            Hide(true, other.gameObject);

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController objectStatus))
        {

            Hide(false, other.gameObject);

        }

    }

}
