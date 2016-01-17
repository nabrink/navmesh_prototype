// MoveTo.cs
using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

    public Transform goal = null;
    NavMeshAgent agent;
    public Rigidbody rb;
    public FixedJoint shieldJoint;
    public Rigidbody shield;
    private bool shieldUp = false;

    bool isAlive;

    void Start () {
        isAlive = true;
		agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        shieldJoint = GetComponent<FixedJoint>();
        rb.useGravity = false;
        shield.useGravity = false;
    }

    void ToggleShielUp(float val) {
        if (!isAlive) return;
        if (shieldUp) {
            val = -val;
        }
        shieldJoint.connectedBody = null;
        Transform s = transform.FindChild("Shield");
        Vector3 newPos = s.localPosition;
        s.localPosition = new Vector3(newPos.x + val, newPos.y += val, newPos.z);
        shieldJoint.connectedBody = shield;
        shieldUp = (shieldUp) ? false : true;
    }

    void Kill() {
        if(agent != null)
            agent.enabled = false;
        Destroy(agent);
        rb.useGravity = true;
        rb.isKinematic = false;
        shield.useGravity = true;
        Destroy(shieldJoint);
        rb.AddForce(transform.forward * (50 + (Random.value * 40)));
        isAlive = false;
    }

    void Hit(float velocity) {
        if(velocity > 5) {
            Kill();
        }
        else {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    void OnMouseDown() {
        Kill();
    }

    void SetGoal(Transform g) {
        goal = g;
    }

    void Update() {
        if (isAlive) {
            if (goal != null && agent != null) {
                agent.destination = goal.position;
                Quaternion v = new Quaternion(goal.rotation.x, goal.rotation.y, goal.rotation.z, goal.rotation.w);
                this.transform.rotation = v;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            ToggleShielUp(0.2f);
        }

    }
}