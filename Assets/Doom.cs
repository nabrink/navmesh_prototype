using UnityEngine;
using System.Collections;

public class Doom : MonoBehaviour {

    public GameObject obj;
    public GameObject target;

    private float maxRange = 20;
    // Use this for initialization
    void Start() {        obj = this.gameObject;
        target = FindTarget();
    }

    void LoadAndFire(int qty) {
        if (target == null) return;

        float distance = GetDistance(target);
        if (distance > maxRange) return;

        for (int i = 0; i < qty; i++) {
            GameObject arrow = GameObject.Instantiate(Resources.Load("arrow (4)")) as GameObject;
            arrow.transform.rotation = obj.transform.rotation;
            arrow.transform.Rotate(90, 0, 0);
            arrow.transform.position = new Vector3(this.transform.position.x - 1 + Random.value * 2, this.transform.position.z + 3 + Random.value, this.transform.position.z - 0.5f + Random.value);
            arrow.SendMessage("SetFiringFrom", this.transform);
            arrow.SendMessage("Fire", 3000);
        }
    }

    void OnMouseDown() {
        LoadAndFire(1);
    }

    public float Map(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    float GetDistance(GameObject unit) {
        float dx = unit.transform.position.x - transform.position.x;
        float dz = unit.transform.position.z - transform.position.z;
        float delta = Mathf.Sqrt(dx * dx + dz * dz);
        return delta;
    }

    GameObject FindTarget() {        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");        foreach(GameObject g in units) {
            if(g.GetComponent<NavMeshAgent>() != null && g.GetComponent<NavMeshAgent>().enabled) {
                return g;
            }
        }        return null;    }

    // Update is called once per frame
    void Update() {        if(target != null && target.GetComponent<NavMeshAgent>() != null && target.GetComponent<NavMeshAgent>().enabled) {
            transform.LookAt(target.transform);
            float distance = GetDistance(target);
            float angle = Map(distance, 0, maxRange, 0, 6);
            transform.Rotate(-angle, 0, 0);
        }
        else {
            target = FindTarget();
        }        if (Input.GetKeyDown("space")) {
            LoadAndFire(1);
        }            
    }
}
