using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Mouse : MonoBehaviour {

    // Use this for initialization
    public Transform pos;

    GameObject[] agents;

    public Vector3 mouseDownPos;
    private float maxRows;

    private float rowSpacing = 1.0f;
    private float colSpacing = 1.2f;

    private float rows = 0;
    private float cols = 0;

    private Vector3 lookAtPosition;

    void Start () {
        if (agents != null) {
            rows = 2;
            cols = agents.Length / rows;
        }
        lookAtPosition = new Vector3(0.0f, 0.0f, 0.0f);
    }

    void UpdateUnitCount() {
        agents = GameObject.FindGameObjectsWithTag("Unit");
        maxRows = Mathf.Sqrt(Mathf.Round(agents.Length));
    }

    void OnMouseUp() {
        UpdateUnitCount();
        Vector3 mouseUpPos = GetWorldPosition();
        pos.position = mouseDownPos;
        lookAtPosition = mouseUpPos;

        float mouseDelta = GetMouseDelta(mouseDownPos, mouseUpPos);
        rows = Mathf.Floor(Map(mouseDelta, 0, 7, 1, maxRows));
        cols = agents.Length / rows;
        SetFormation(rows, cols, mouseUpPos);
    }

    void SetFormation(float rows, float cols, Vector3 lookAt) {
        KillCones();
        pos.rotation = new Quaternion(0, 0, 0, 0);

        int loopCounter = 0;
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                if (loopCounter <= agents.Length) {
                    Vector3 v = new Vector3(pos.position.x - (colSpacing * i) + (cols * colSpacing) / 2, 0.0f, pos.position.z + (rowSpacing * j) - (rows * rowSpacing) / 2);
                    CreateWaypoint(agents, loopCounter++, v);
                }
            }
        }
        pos.LookAt(lookAt);
    }

    void KillCones()
    {
        GameObject[] childrenOfPos = GameObject.FindGameObjectsWithTag("Cone");
        foreach (GameObject o in childrenOfPos)
        {
            Destroy(o);
        }
    }

    void CreateWaypoint(GameObject[] agents, int index, Vector3 position) {
        try {
            if (agents.Length == 0) return;

            GameObject obj = GameObject.Instantiate(Resources.Load("cone01")) as GameObject;
            obj.transform.position = new Vector3(position.x, -0.5f, position.z);
            obj.transform.parent = pos.transform;
            agents[index].SendMessage("SetGoal", obj.transform);
        }
        catch (Exception e) {
            Debug.Log(e.Message);
        }
    }

    public float Map(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    void OnMouseDown() { 
        mouseDownPos = GetWorldPosition();
    }

    float GetMouseDelta(Vector3 mouseDownPos, Vector3 mouseUpPos) {
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
            worldPos.y = 0.0f;
            worldPos.z = Mathf.Round(worldPos.z);
            return worldPos;
        }
        return new Vector3();
    }

// Update is called once per frame
    void Update () {
        if (Input.GetKeyDown("1")) {
            rowSpacing += 0.05f;
            colSpacing += 0.05f;
            SetFormation(rows, cols, lookAtPosition);
        }
        if (Input.GetKeyDown("2")) {
            rowSpacing -= 0.05f;
            colSpacing -= 0.05f;
            SetFormation(rows, cols, lookAtPosition);
        }
    }
}
