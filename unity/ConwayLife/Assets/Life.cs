using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Life : MonoBehaviour {
    public GameObject cellPrefab;
    public float secondsPerCycle = 3f;
    public float neighborDistance = 1.0f;
    public int loneliness = 1;
    public int overcrowded = 4;
    public int birthLow = 3;
    public int birthHigh = 5;
    public float startingProbablility = 0.3f;
    public int maxCells = 800;
    public int NumberOfCells = 0;
    public int CellsBorn = 0;
    public int CellsDied = 0;
    private float lifeTime = 0;
    private int totalCycles;
	// Use this for initialization
	void Start () {
        makeLife (Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
        lifeTime += Time.deltaTime;
        int neighbors = 0;
        if (lifeTime > secondsPerCycle) {
            CellsBorn = 0;
            CellsDied = 0;

            lifeTime = 0;
            GameObject[] cells = GameObject.FindGameObjectsWithTag ("cell");
            NumberOfCells = cells.Length;
            //check deaths
            foreach (GameObject cellGO in cells) {
                Cell cell = cellGO.GetComponent<Cell> ();
                neighbors = countNeighbors (cellGO);
                if (neighbors <= loneliness || neighbors >= overcrowded)
                    killCell (cell);
            }
            //check births
            foreach (GameObject cellGO in cells) {
                Cell cell = cellGO.GetComponent<Cell> ();
                List<Vector3> spaces = FindNeighboringSpace (cell);
                foreach (Vector3 space in spaces) {
                    neighbors = countNeighbors (space);
                    if (neighbors >= birthLow && neighbors <= birthHigh)
                        makeCell (space, cell);
                }
            }
            if (cells.Length == 0)
                makeLife (Vector3.zero);
            totalCycles++;
        }
	}

    void makeLife(Vector3 center){
        Debug.Log ("Previous extinction after: " + totalCycles + " cycles.");
        totalCycles = 0;
        CellsBorn = 0;
        CellsDied = 0;
        for (int x = -4; x <= 4; x++) {
            for (int y = -4; y <= 4; y++) {
                for (int z = -4; z <= 4; z++) {
                    if(Random.value < startingProbablility)
                        makeCell(new Vector3(x, y, z) + center);
                }
            }
        }
        CellsBorn = 0;
        CellsDied = 0;
    }

    void makeCell(Vector3 location, Cell mother = null){
        Collider[] occupied = Physics.OverlapSphere (location, .01f);
        if (occupied.Length == 0 && NumberOfCells < maxCells) {
            GameObject newCell = GameObject.Instantiate (cellPrefab) as GameObject;
            Cell cell = newCell.GetComponent<Cell> ();
            if (mother) {
                cell.target = location;
                cell.transform.position = mother.transform.position;
            } else {
                cell.transform.position = location;
                cell.target = location;
            }
            newCell.transform.SetParent (this.transform);
            CellsBorn++;
        }
    }
    void killCell(Cell cell){
        if(cell)
            cell.isLiving = false;
        CellsDied++;
    }

    int countNeighbors(Cell cell){
        return countNeighbors (cell.gameObject.transform.position);
    }

    int countNeighbors(GameObject cell){
        return countNeighbors (cell.transform.position);
    }

    int countNeighbors(Vector3 location){
        Collider[] neighbors = Physics.OverlapSphere (location, neighborDistance);
        return neighbors.Length - 1;
    }

    List<Vector3> FindNeighboringSpace(Cell cell){
        if (cell)
            return FindNeighboringSpace (cell.gameObject);
        else
            return new List<Vector3> ();
    }
        
    List<Vector3> FindNeighboringSpace(GameObject cell){
        List<Vector3> spaces = new List<Vector3>();
        for (int x = -1; x < 2; x++) {
            for (int y = -1; y < 2; y++) {
                for (int z = -1; z < 2; z++) {
                    Vector3 location = cell.transform.position + new Vector3(x, y, z);
                    Collider[] occupied = Physics.OverlapSphere (location, .01f);
                    if(occupied.Length == 0){
                        spaces.Add (location);
                    }
                }
            }
        }
        return spaces;
    }

}
