using UnityEngine;
using System.Collections;

public class arrow : MonoBehaviour {

    public Rigidbody rb;
    Transform startingPos;

    // Use this for initialization
    void Start () {
        startingPos = this.gameObject.transform;
        Debug.Log("ARROW: " + this.gameObject);
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * (4000.0f + (Random.value * 600)));
	}
	
	// Update is called once per frame

	void Update () {
	
	}
}
