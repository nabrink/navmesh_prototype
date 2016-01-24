using UnityEngine;
using System.Collections;

public class Formation : MonoBehaviour
{
	public float distanceBetweenPositions = 1.0f;

	public Transform positionMarkerPrefab;
    public Transform soldierPrefab;
    public Transform parentMarker;
	
	private Vector3 startPosition;
    private int columns;
    private int rows;

    private int[,] currentFormation;

    private IList formationList;

    private ArrayList army;
    private ArrayList spawnedMarkers;
		
    private void InitializeGrids()
    {
        formationList = new ArrayList();

        formationList.Add(new int[10, 10] {
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,1,1,1,0,0,0},
            {0,0,0,1,1,1,1,0,0,0},
            {0,0,0,1,1,1,1,0,0,0},
            {0,0,0,1,1,1,1,0,0,0},
            {0,0,0,1,1,1,1,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0}});

        formationList.Add(new int[10, 10] {
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,1,0,0,0,0},
            {0,0,1,0,1,0,0,1,0,0},
            {0,0,0,1,0,1,0,0,0,0},
            {0,1,0,0,1,0,1,0,1,0},
            {0,1,0,1,0,1,0,0,0,0},
            {0,0,0,0,1,0,1,0,0,0},
            {0,0,1,0,1,0,0,0,1,0},
            {0,0,0,0,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0}});

        formationList.Add(new int[10, 10] {
            {0,1,0,1,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,1,0,1,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,1,0,1,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,1,0,1,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,1,0,1,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0}});

        formationList.Add(new int[10, 10] {
            {0,0,0,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,1,0,0,1},
            {0,0,0,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,1,0,0,1},
            {0,0,0,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,1,0,0,1},
            {0,0,0,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,1,0,0,1},
            {0,0,0,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,1,0,0,1}});

        currentFormation = (int[,])formationList[0];
    }

	void Start ()
    {
        InitializeGrids();
        CreateArmy(currentFormation, 10, 10);
	}

    public void CreateArmy(int[,] formation,int columns, int rows)
    {
        this.columns = columns;
        this.rows = rows;
        currentFormation = formation;

        Queue initializedPositions = InitializeFormationGrid();
        SpawnSoldiers(initializedPositions);
    }

	public Queue InitializeFormationGrid()
	{
        spawnedMarkers = new ArrayList();
        Queue availableMarkers = new Queue();
        Vector3 centerPosition = parentMarker.position;
        startPosition = new Vector3 (centerPosition.x - (distanceBetweenPositions * (columns / 2)), 
			                            centerPosition.y, 
			                            centerPosition.z - (distanceBetweenPositions * (rows/2)));

		var positionToBePlaced = startPosition;

		for (int y = 0; y < columns; y++) 
		{
			for (int x = 0; x < rows; x++) 
			{
				if (currentFormation[y, x] != 0) 
				{
                    var marker = CreateMarker(positionToBePlaced);
                    spawnedMarkers.Add(marker);
                    availableMarkers.Enqueue(marker);
                }
				positionToBePlaced = MoveToNextColumn (positionToBePlaced);
			}
			positionToBePlaced = MoveToNextRow (positionToBePlaced);
		}
        return availableMarkers;
	}

    private Transform CreateMarker(Vector3 positionToBePlaced)
    {
        Transform position = (Transform)Instantiate(positionMarkerPrefab, positionToBePlaced, parentMarker.rotation);
        position.parent = parentMarker;
        return position;  
    }

	private void SpawnSoldiers(Queue positions)
	{
        var soldiersNeeded = positions.Count;
        army = new ArrayList();

		for (int i = 0; i < soldiersNeeded; i++)
        {
			if (positions.Count > 0)
            {
				Transform spawnPosition = (Transform)positions.Dequeue ();
				Transform spawnedSoldier = (Transform)Instantiate (soldierPrefab, spawnPosition.position, parentMarker.rotation);
				spawnedSoldier.GetComponent<Soldier> ().goal = spawnPosition;
                army.Add(spawnedSoldier);
			}
		}
	}

    private void AssingSolidiers(Queue markers)
    {
        if (markers == null || markers.Count == 0) return;

        foreach (Transform s in army)
        {
            Transform marker = (Transform)markers.Dequeue();
            s.GetComponent<Soldier>().goal = marker;
        }               
    }

    public void ChangeFormation(int[,] newFormation)
    {
        DestroySpawnedMarkers();
        Queue markers = new Queue();
        currentFormation = newFormation;
        markers = InitializeFormationGrid();
        AssingSolidiers(markers);
    }

    private void DestroySpawnedMarkers()
    {
        foreach(Transform m in spawnedMarkers)
        {
            Destroy(m.gameObject);
        }

        spawnedMarkers = null;
    }

	private Vector3 MoveToNextColumn(Vector3 currentPosition) 
	{
		return new Vector3 (currentPosition.x + distanceBetweenPositions, currentPosition.y, currentPosition.z);
	}

	private Vector3 MoveToNextRow(Vector3 currentPosition) 
	{
		return new Vector3 (startPosition.x, currentPosition.y, currentPosition.z + distanceBetweenPositions);
	}

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeFormation((int[,])formationList[0]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeFormation((int[,])formationList[1]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeFormation((int[,])formationList[2]);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeFormation((int[,])formationList[3]);
        }
    }
}
