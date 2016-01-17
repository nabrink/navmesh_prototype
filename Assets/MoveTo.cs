﻿// MoveTo.cs
using UnityEngine;
using System.Collections;

public class MoveTo : MonoBehaviour {

    public Transform goal = null;
    NavMeshAgent agent;

    void Start () {
		agent = GetComponent<NavMeshAgent>();
	}

    void SetDestination(Vector3 pos) {

    }

    void SetGoal(Transform g) {
        goal = g;
    }

    void Update() {
        if (goal != null)
            agent.destination = goal.position;

    }
}