using UnityEngine;
using System.Collections;

public class FormationPath : MonoBehaviour 
{
	public float reachedThreshold = 2.5f;

	public Transform patrolCheckpoints;

	private Queue checkpoints;
	private Vector3 currentCheckpoint;
	private NavMeshAgent agent;

	void Start ()
	{
		agent = GetComponent<NavMeshAgent>();
		currentCheckpoint = transform.position;

		SetupCheckpointQueue ();
	}

	private void SetupCheckpointQueue()
	{
		checkpoints = new Queue ();

		for(int childIndex = 0; childIndex < patrolCheckpoints.childCount; childIndex++) 
		{
			checkpoints.Enqueue (patrolCheckpoints.GetChild(childIndex));
		}
	}

	void Update () 
	{
		if (IsCheckpointReached() && checkpoints.Count > 0)
		{
			Debug.Log ("Checkpoint reached!");
			Transform checkpoint = (Transform)checkpoints.Dequeue ();
			currentCheckpoint = checkpoint.position;
			agent.destination = currentCheckpoint;
		}
	}

	private bool IsCheckpointReached()
	{
		return Vector3.Distance (currentCheckpoint, transform.position) <= reachedThreshold;
	}
}
