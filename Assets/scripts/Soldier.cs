using UnityEngine;
using System.Collections;

public class Soldier : MonoBehaviour {
    public Transform goal;

    private int life;
    private bool isAlive;
    private bool hasPlayedDeathAnimation = false;

    NavMeshAgent agent;
    Rigidbody body;

    void Start () {
        life = 100;
        isAlive = true;
        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
    }
	
	void Update () {
        if (isAlive)
        {
            agent.destination = goal.position;
        }

        if(life <= 0)
        {
            isAlive = false;
        }

        if(!isAlive && !hasPlayedDeathAnimation)
        {
            agent.enabled = false;
            body.useGravity = true;
            body.mass = 100f;
            transform.Rotate(30,0,0);
            body.AddForce(Vector3.forward * 30.0f);
            hasPlayedDeathAnimation = true;
        }
    }

    void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            isAlive = false;
        }
    }
}
