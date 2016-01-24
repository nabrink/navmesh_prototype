using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    float movementSpeed = 10.0f;

    void Update()
    { 
        CheckMovement();
        CheckRotation();
    }

    private void CheckMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += (new Quaternion(0, transform.rotation.y, 0, transform.rotation.w) * Vector3.forward) * movementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += (new Quaternion(0, transform.rotation.y, 0, transform.rotation.w) * Vector3.back) * movementSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.position += (transform.rotation * Vector3.left) * movementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += (transform.rotation * Vector3.right) * movementSpeed * Time.deltaTime;
        }
    }

    private void CheckRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.RotateAround(GetRotationCenter(), Vector3.down, 1);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.RotateAround(GetRotationCenter(), Vector3.up, 1);
        }
    }

    private Vector3 GetRotationCenter()
    {
        RaycastHit hit;
        Vector3 rotationCenter = new Vector3();

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            rotationCenter = hit.point;
        }

        return rotationCenter;
    }
}
