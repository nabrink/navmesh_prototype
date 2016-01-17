﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Mouse : MonoBehaviour {

    // Use this for initialization
    public Transform pos;
    private Vector3 lookTowardsPosition;

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
        pos.position = mouseDownPos;
        lookTowardsPosition = mouseUpPos;
        Debug.Log(agents.Length + " units on the scene.");
        float mouseDelta = GetMouseDelta(mouseDownPos, mouseUpPos);
        float rows = Mathf.Floor(Map(mouseDelta, 0, 7, 1, maxRows));
        float cols = agents.Length / rows;
        Debug.Log("rows: " + rows);

        float rowSpacing = 0.6f;
        float colSpacing = 1.0f;

        int loopCounter = 0;
        for (int i = 0; i < cols; i++) {
            for (int j = 0; j < rows; j++) {
                Vector3 v = new Vector3(pos.position.x - (colSpacing * i) + (cols * colSpacing)/2, 0.0f, pos.position.z + (rowSpacing * j) - (rows * rowSpacing)/2);
                CreateWaypoint(agents, loopCounter, v);
                loopCounter++;
            }
        }

        pos.LookAt(GetWorldPosition());
        Debug.Log("pos rotation: " + pos.rotation.ToString());
    }

    void CreateWaypoint(GameObject[] agents, int index, Vector3 position) {
        try {
            GameObject obj = Instantiate(new GameObject("conePrefab")) as GameObject;
            obj.transform.position = new Vector3(position.x, 0.0f, position.z);
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
        Debug.Log("MOUSE DOWN");
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
        pos.GetChild(0).LookAt(GetWorldPosition());
        if (Input.GetMouseButtonDown(1)) {
            //
        }
            

    }
}
