using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {


    public float movementSpeed = 0.5f;
    public float rotationSpeed = 0.5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * movementSpeed;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back * movementSpeed;
        }

        if(Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left * movementSpeed;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * movementSpeed;
        }
	}
}
