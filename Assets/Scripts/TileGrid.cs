using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public List<CastlePiece> allPieces;
    public GameEvent startGame;
    public GameEvent endGame;
    public CastlePiece keep;
    CastlePiece keepReference;
    public CastlePiece tree;
    public TreeSpawnPoint treeSpawnPoint;
    public List<TreeSpawnPoint> treeSpawnPoints;
    public int keepStartingRow;
    public int keepStartingColumn;
    public InputEvent touchDownEvent;
    public InputEvent releaseEvent;
    public InputEvent dragEvent;
    public bool interactable;
    public float minimumTouchThreshold;
    public float minimumDropThreshold;
    public Grid grid;
    public CastleLeveller castleLeveller;
    public AudioSource audioSource;
    public int numColumns;
    public int numRows;
    [NonSerialized] public Vector2 MaxGridDimensions;
    [NonSerialized] public Vector2 MinGridDimensions;

    Vector2 offset = Vector2.zero;
    CastlePiece pieceSelected = null;

    void StartGame()
    {
        SetBaseGrid();
        interactable = true;
    }

    void OnEnable()
    {
        allPieces = new List<CastlePiece>();
        treeSpawnPoints = new List<TreeSpawnPoint>();
        startGame.RegisterListener(StartGame);
        touchDownEvent.RegisterListener(OnTouchDown);
        releaseEvent.RegisterListener(OnRelease);
        dragEvent.RegisterListener(OnDrag);
        MaxGridDimensions = GetMaxGridDimensions();
        MinGridDimensions = GetMinGridDimensions();
    }

    void OnDisable()
    {
        touchDownEvent.UnregisterListener(OnTouchDown);
        releaseEvent.UnregisterListener(OnRelease);
        dragEvent.UnregisterListener(OnDrag);
        startGame.UnregisterListener(StartGame);
    }



    void SetBaseGrid()
    {
        for(int x = 0; x < numRows; x++)
        {
            for(int y = 0; y < numColumns; y++)
            {
                if(x == keepStartingRow && y == keepStartingColumn)
                {
                    var k = Instantiate(keep);
                    allPieces.Add(k);
                    keepReference = k;
                    k.currentColumn = keepStartingColumn;
                    k.currentRow = keepStartingRow;
                    k.tileGrid = this;
                    k.transform.position = grid.CellToWorld(new Vector3Int(y, x, 0));
                }
                else if(x == 2 && y == 1)
                {
                    AddTree(2, 1);
                }
                else if(x == 1 && y == 3)
                {
                    AddTree(1, 3);
                }
                else //treespawner
                {
                    CreateTreeSpawnPoint(x, y);
                }

            }
        }
    }

    public void CreateTreeSpawnPoint(int row, int column)
    {
        var t = Instantiate(treeSpawnPoint);
        treeSpawnPoints.Add(t);
        t.currentColumn = column;
        t.currentRow = row; 
        t.tileGrid = this;
        t.transform.position = grid.CellToWorld(new Vector3Int(column, row, 0));
    }

    public void AddPiece(CastlePiece piece)
    {
        allPieces.Add(piece);
        piece.tileGrid = this;
    }

    public void AddTree(int row, int column)
    {
        CastlePiece p = Instantiate(tree);
        p.currentColumn = column;
        p.currentRow = row;
        p.transform.position = grid.CellToWorld(new Vector3Int(column, row, 0));
        AddPiece(p);
    }

    public void RemovePiece(CastlePiece piece)
    {
        allPieces.Remove(piece);
    }
    
    public void OnTouchDown(TouchData touchData)
    {
        if(interactable)
        {
            //Check if any tiles touched
            foreach(CastlePiece p in allPieces)
            {
                if(Vector2.Distance(touchData.startPosition, p.transform.position) < minimumTouchThreshold && p.mergeRank < 9) //not the keep
                {
                    offset = touchData.startPosition - (Vector2)p.transform.position;
                    pieceSelected = p;
                    p.PieceSelected();
                    audioSource.Play();
                }
            }
        }
    }

    public void OnDrag(TouchData touchData)
    {
        if(pieceSelected != null)
        {
            pieceSelected.transform.position = touchData.endPosition - offset;
        }
    }

    public void OnRelease(TouchData touchData)
    {
        if(pieceSelected != null)
        {
            //if dropped on another tile...
            foreach(CastlePiece p in allPieces)
            {
                if(p.transform != pieceSelected.transform) //except itself
                {
                    if(Vector2.Distance(touchData.endPosition, p.transform.position) < minimumDropThreshold)
                    {
                        //if same rank and colour
                        if(p.mergeRank == pieceSelected.mergeRank && p.castleType == pieceSelected.castleType)
                        {
                            //merge tiles!
                            castleLeveller.Merge(p, pieceSelected);
                            pieceSelected = null;
                            return;
                        }
                    }
                }
            }

            //not merged, so back to original position
            pieceSelected.transform.position = grid.CellToWorld(new Vector3Int(pieceSelected.currentColumn, pieceSelected.currentRow, 0));
            pieceSelected.PieceDeselected();
            pieceSelected = null;
            
        }   
    }

    //returns the first piece in range, or if nothing is in range, the closest piece out of range
    public CastlePiece GetClosestCastlePiece(Vector2 toLocation, float range)
    {
        float closestDistance = 0f;
        CastlePiece closestPiece = null;
        foreach(CastlePiece p in allPieces)
        {
            if(pieceSelected == null || p != pieceSelected ) 
            {
                if(p.mergeRank != 0) //don't find trees
                {
                    float d = Vector2.Distance(p.transform.position, toLocation);
                    if(d < range) return p;
                    else if(d < closestDistance)
                    {
                        closestDistance = d;
                        closestPiece = p;
                    }
                }
            }
        }
        return closestPiece;
    }

    public Vector2 GetMaxGridDimensions()
    {
        return grid.CellToWorld(new Vector3Int(numColumns, numRows, 0));
    }

    public Vector2 GetMinGridDimensions()
    {
        return grid.CellToWorld(new Vector3Int(0, 0, 0));
    }

    void LateUpdate()
    {
        if(interactable && keepReference.health <= 0) EndGame();
    }

    void EndGame()
    {
        interactable = false;
        endGame.Raise();
        Debug.Log("The game is over");
    }

}
