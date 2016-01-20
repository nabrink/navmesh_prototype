using UnityEngine;
using System.Collections;

public class FormationMoveControl : MonoBehaviour
{
    private Vector3 endPoint;
    private float yAxis;

    void Start()
    {
        yAxis = gameObject.transform.position.y;
    }

    void Update()
    {
        HandleMousePosition();
        HandleRotation();
    }

    private void HandleMousePosition()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray;

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                gameObject.transform.position = hit.point;
            }
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, 100.0f * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.down, 100.0f * Time.deltaTime, Space.World);
        }
    }
}