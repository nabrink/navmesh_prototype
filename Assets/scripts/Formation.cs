using UnityEngine;
using System.Collections;
using Assets;

public class Formation : MonoBehaviour
{
	public float distanceBetweenPositions = 1.0f;

	public Transform positionMarkerPrefab;
    public Transform parentMarker;

    public Transform defaultSoldier;
    public Transform shieldedSoldier;

    private Vector3 startPosition;
    private int columns;
    private int rows;

    private int[,] currentFormation;

    private IList formationList;

    private IList army;
    private IList spawnedMarkers;
		
    private void InitializeGrids()
    {
        formationList = new ArrayList();

        formationList.Add(new int[10, 10] {
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,2,1,1,2,0,0,0},
            {0,0,0,1,1,1,1,0,0,0},
            {0,0,0,1,1,1,1,0,0,0},
            {0,0,0,1,1,1,1,0,0,0},
            {0,0,0,2,1,1,2,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0}});

        formationList.Add(new int[10, 10] {
            {0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,1,0,0,0,0},
            {0,0,2,0,1,0,0,2,0,0},
            {0,0,0,1,0,1,0,0,0,0},
            {0,1,0,0,1,0,2,0,1,0},
            {0,1,0,1,0,1,0,0,0,0},
            {0,0,0,0,1,0,1,0,0,0},
            {0,0,2,0,1,0,0,0,1,0},
            {0,0,0,0,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0}});

        formationList.Add(new int[10, 10] {
            {0,2,0,1,0,1,0,2,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,1,0,1,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,1,0,1,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,1,0,1,0,1,0,1,0,0},
            {0,0,0,0,0,0,0,0,0,0},
            {0,2,0,1,0,1,0,2,0,0},
            {0,0,0,0,0,0,0,0,0,0}});

        formationList.Add(new int[10, 10] {
            {0,0,0,0,0,0,0,0,0,0},
            {2,0,0,1,0,0,1,0,0,2},
            {0,0,0,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,1,0,0,1},
            {0,0,0,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,1,0,0,1},
            {0,0,0,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,1,0,0,1},
            {0,0,0,0,0,0,0,0,0,0},
            {2,0,0,1,0,0,1,0,0,2}});

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

        IList initializedPositions = InitializeFormationGrid();
        SpawnSoldiers(initializedPositions);
    }

	public IList InitializeFormationGrid()
	{
        spawnedMarkers = new ArrayList();
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
                    spawnedMarkers.Add(new Position(marker, currentFormation[y,x]));
                }
				positionToBePlaced = MoveToNextColumn (positionToBePlaced);
			}
			positionToBePlaced = MoveToNextRow (positionToBePlaced);
		}
        return spawnedMarkers;
	}

    private Transform CreateMarker(Vector3 positionToBePlaced)
    {
        Transform position = (Transform)Instantiate(positionMarkerPrefab, positionToBePlaced, parentMarker.rotation);
        position.parent = parentMarker;
        return position;  
    }

	private void SpawnSoldiers(IList positions)
	{
        var soldiersNeeded = positions.Count;
        army = new ArrayList();

		for (int i = 0; i < soldiersNeeded; i++)
        {
			if (positions.Count > 0)
            {
				Position spawnPosition = (Position)positions[i];
                Transform soldierPrefab = GetSoldierPrefabByType(spawnPosition.SoldierType);
				Transform spawnedSoldier = (Transform)Instantiate (soldierPrefab, spawnPosition.Marker.position, parentMarker.rotation);
				spawnedSoldier.GetComponent<Soldier> ().goal = spawnPosition.Marker;
                army.Add(spawnedSoldier);
			}
		}
	}

    private Transform GetSoldierPrefabByType(int soldierType)
    {
        var soldierPrefab = defaultSoldier;

        if (soldierType == 2)
        {
            soldierPrefab = shieldedSoldier;
        }

        return soldierPrefab;
    }

    private void AssingSolidiers(IList markers)
    {
        if (markers == null || markers.Count == 0) return;

        int index = 0;
        foreach (Transform s in army)
        {
            Position position = (Position)markers[index];
            s.GetComponent<Soldier>().goal = position.Marker;
            index++;
        }               
    }

    public void ChangeFormation(int[,] newFormation)
    {
        DestroySpawnedMarkers();
        IList markers = new ArrayList();
        currentFormation = newFormation;
        markers = InitializeFormationGrid();
        AssingSolidiers(markers);
    }

    private void DestroySpawnedMarkers()
    {
        foreach(Position pos in spawnedMarkers)
        {
            Destroy(pos.Marker.gameObject);
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
        HandleUserInput();
    }

    private void HandleUserInput()
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
