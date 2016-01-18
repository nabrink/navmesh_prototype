using UnityEngine;
using System.Collections;

public class Formation : MonoBehaviour
{
	const int columns = 20;
	const int rows = 20;

	public float distanceBetweenPositions = 1.0f;
	public Transform positionMarkerPrefab;
	public Transform parentPosition;
	public Transform soldier;

	private Vector3 startPosition;
	private Queue availablePositions;
	private int soldierCount = 0;

    private int[,] formationGrid = new int[columns, rows] {
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 },
		{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }
	};
		
	void Start () {
		availablePositions = new Queue();

		InitializeFormationGrid ();
        SpawnSoldiers();
	}

	public void InitializeFormationGrid()
	{
		Vector3 centerPosition = parentPosition.position;
        startPosition = new Vector3 (centerPosition.x - (distanceBetweenPositions * (columns/2)), 
			centerPosition.y, 
			centerPosition.z - (distanceBetweenPositions * (rows/2)));

		var positionToBePlaced = startPosition;

		for (int y = 0; y < columns; y++) 
		{
			for (int x = 0; x < rows; x++) 
			{
				if (formationGrid [y, x] == 1) 
				{
                    CreateMarker(positionToBePlaced);
                }
				positionToBePlaced = MoveToNextColumn (positionToBePlaced);
			}
			positionToBePlaced = MoveToNextRow (positionToBePlaced);
		}
	}

    private void CreateMarker(Vector3 positionToBePlaced)
    {
        Transform position = (Transform)Instantiate(positionMarkerPrefab, positionToBePlaced, parentPosition.rotation);
        position.parent = parentPosition;
        availablePositions.Enqueue(position);
        soldierCount++;
    }

	private void SpawnSoldiers()
	{
		for (int i = 0; i < soldierCount; i++)
        {
			if (availablePositions.Count > 0)
            {
				Transform spawnPosition = (Transform)availablePositions.Dequeue ();
				Transform spawnedSoldier = (Transform)Instantiate (soldier, spawnPosition.position, parentPosition.rotation);
                //Scriptet Soldier är i detta fall det script som hanter vilken position soldaten vill gå
				spawnedSoldier.GetComponent<Soldier> ().goal = spawnPosition;
			}
		}
	}

	private Vector3 MoveToNextColumn(Vector3 currentPosition) 
	{
		return new Vector3 (currentPosition.x + distanceBetweenPositions, currentPosition.y, currentPosition.z);
	}

	private Vector3 MoveToNextRow(Vector3 currentPosition) 
	{
		return new Vector3 (startPosition.x, currentPosition.y, currentPosition.z + distanceBetweenPositions);
	}
		
	void Update ()
    {
 
	}
}
