using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{

    public float rotationSpeed = 30f;
    public float OpenAngle = 90f;
    private bool isOpened = false;

    public void Interact()
    {
        if (isOpened)
        {
            Debug.Log("닫을게요");
            StartCoroutine(CloseDoor()); 
        }
        else
        {
            Debug.Log("열게요");
            StartCoroutine(OpenDoor());
        }
    }

    IEnumerator OpenDoor()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y -= OpenAngle;
        transform.rotation = Quaternion.Euler(rotation);
        isOpened = true;

        yield return null;
    }

    IEnumerator CloseDoor()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.y += OpenAngle;
        transform.rotation = Quaternion.Euler(rotation);
        isOpened = false;

        yield return null;
    }
}
