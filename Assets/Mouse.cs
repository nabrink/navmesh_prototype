using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mouse : MonoBehaviour {

    // Use this for initialization
    public Transform pos;

    GameObject[] agents;

    public Vector3 mouseDownPos;
    private float maxRows;

    void Start () {

    }

    void UpdateUnitCount() {
        agents = GameObject.FindGameObjectsWithTag("Unit");
        maxRows = Mathf.Sqrt(Mathf.Round(agents.Length));
    }

    void OnMouseUp() {
        UpdateUnitCount();
        Vector3 mouseUpPos = GetWorldPosition();
        Debug.Log(agents.Length + " units on the scene.");
        float mouseDelta = GetMouseDelta(mouseUpPos);
        Debug.Log("Mouse delta: " + mouseDelta);

        float rows = Mathf.Floor(Map(mouseDelta, 0, 7, 1, maxRows));
        float cols = agents.Length / rows;
        Debug.Log("rows: " + rows);

        float rowSpacing = 0.8f;
        float colSpacing = 1.0f;
        pos.position = mouseDownPos;

        int loopCounter = 0;
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                Vector3 v = new Vector3(mouseDownPos.x - (colSpacing * i), 0.0f, mouseDownPos.z + (rowSpacing * j));
                CreateWaypoint(agents[loopCounter], v);
                loopCounter++;
            }
        }

        Debug.Log("radius: " + GetMouseAngle(mouseUpPos));
        pos.LookAt(GetWorldPosition());
        Debug.Log("pos rotation: " + pos.rotation.ToString());
    }

    void CreateWaypoint(GameObject agent, Vector3 position) {
        GameObject obj = Instantiate(new GameObject("conePrefab")) as GameObject;
        obj.transform.position = new Vector3(position.x, 0.0f, position.z);
        obj.transform.parent = pos.transform;
        agent.SendMessage("SetGoal", obj.transform);
    }

    public float Map(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void OnMouseDown() {
        Debug.Log("MOUSE DOWN");
        mouseDownPos = GetWorldPosition();
    }

    float GetMouseDelta(Vector3 mouseUpPos) {
        float dx = mouseDownPos.x - mouseUpPos.x;
        float dz = mouseDownPos.z - mouseUpPos.z;
        float delta = Mathf.Sqrt(dx * dx + dz * dz);
        return delta;
    }

    float GetMouseAngle(Vector3 mouseUpPos) {
        float dx = mouseDownPos.x - mouseUpPos.x;
        float dz = mouseDownPos.z - mouseUpPos.z;
        float radius = Mathf.Atan2(dz, dx);
        return radius * (180 / Mathf.PI);
    }

    Vector3 GetWorldPosition() {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit)) {
            Vector3 worldPos = hit.point;
            worldPos.x = Mathf.Round(worldPos.x);
            worldPos.z = Mathf.Round(worldPos.z);
            return worldPos;
        }
        return new Vector3();
    }

// Update is called once per frame
void Update () {

    }
}
