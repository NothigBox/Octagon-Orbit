using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CellToWorld : MonoBehaviour
{
    
    public Tilemap tilemap;
    Vector3Int cellPosition;


    void Start()
    {
        tilemap.GetComponent<Tilemap>();

        GridLayout gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        cellPosition = gridLayout.WorldToCell(transform.position);
        transform.position = gridLayout.CellToWorld(cellPosition);
        Debug.Log(cellPosition);
        
    }
    private void Update()
    {
        Debug.Log(cellPosition);

    }
}
