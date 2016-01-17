using UnityEngine;using System.Collections;public class arrow : MonoBehaviour {    public Rigidbody rb;    public Transform tip;    Collider[] colliders;
    FixedJoint joint;
    bool hit = false;

    // Use this for initialization
    void Start () {        rb = GetComponent<Rigidbody>();        colliders = GetComponents<Collider>();        joint = GetComponent<FixedJoint>();        tip = this.transform.GetChild(1);        Fire();    }

    void Fire() {
        rb.AddForce(transform.up * (4000.0f + (Random.value * 600)));
    }

    void OnCollisionEnter(Collision col) {
        if (hit) return;

        if (col.gameObject.tag == "Unit" && rb.velocity.magnitude > 5) {
            col.gameObject.SendMessage("Hit", rb.velocity.magnitude);
            ArrowStickToUnit(col);
            hit = true;
        }
        else if (col.gameObject.tag == "Shield") {
            if(rb.velocity.magnitude > 3) {
                ArrowStickToShield(col);
                hit = true;
            }
        }else if(col.gameObject.name == "Ground") {
            if(rb.velocity.magnitude > 5) {
                ArrowStickToGround();
                hit = true;
            }
        }
    }

    void ArrowStickToUnit(Collision col) {
        transform.parent = col.transform;
        DisableOnHit();
        transform.Translate(0.5f * Vector3.up);
    }

    void ArrowStickToShield(Collision col) {
        transform.parent = col.transform.parent;
        DisableOnHit();
        transform.Translate(0.3f * Vector3.up);
    }

    void DisableOnHit() {
        Destroy(joint);
        Destroy(rb);
        Destroy(tip.gameObject.GetComponent<Rigidbody>());
        Destroy(tip.gameObject);
    }

    void ArrowStickToGround() {
        foreach (Collider c in colliders) {
            Destroy(c);
        }
        DisableOnHit();
        rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        transform.Translate(0.1f * Vector3.up);
    }

    // Update is called once per frame

    void Update () {	    //	}}