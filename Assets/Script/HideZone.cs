using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]

public class HideZone : MonoBehaviour
{

    public static Action<bool, GameObject> ActivateHide = delegate { };

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController objectStatus))
        {

            ActivateHide(true, other.gameObject);

        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out CharacterController objectStatus))
        {

            ActivateHide(false, other.gameObject);

        }

    }

}
