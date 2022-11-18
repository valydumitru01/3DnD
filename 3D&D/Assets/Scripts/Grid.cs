using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor.Build.Reporting;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Grid : MonoBehaviour
{
    private const int rows = 6;
    private const int cols = 6;


    // Defines the separaction between Tiles
    // 10f No separation, 20f 1 Tile separation
    private const float tileSize = 10f;
    private Vector3 tileScale = new Vector3(0.05f, 0.05f, 0.05f);

    private GameObject[,] tiles = new GameObject[rows, cols];
    // Any negative or above rows/cols value will count as nothing selected
    // Value should not go below -1 or above rows/cols
    private Vector2 selectedTile = new Vector2(0, 0);

    // Defines if actual turn player is Local Player.
    private bool isLocal = true;
    private ControllerControls controls;

    private bool isGazeInput = false;

    void Awake()
    {
        controls = new ControllerControls();

        // If player is not local the Grid for them would be 180 Degrees Rotated, so controls will be inverted
        controls.Gameplay.Left.performed += context => { if (isLocal) MoveLeft(); else MoveRight(); };
        controls.Gameplay.Right.performed += context => { if (isLocal) MoveRight(); else MoveLeft(); };
        controls.Gameplay.Down.performed += context => { if (isLocal) MoveDown(); else MoveUp(); };
        controls.Gameplay.Up.performed += context => { if (isLocal) MoveUp(); else MoveDown(); };
    }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
        GameObject selectedTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
        selectedTileObject.GetComponent<Tile>().Select();
        controls.Gameplay.Enable();

        // TODO Temporal
        ReceiveInput();
    }

    /**
    * Genera el Tablero (Grid) que contiene las casillas de juego (Tile)
    */
    private void GenerateGrid()
    {
        GameObject templateBlackTile = (GameObject)Instantiate(Resources.Load("BlackTile"));
        GameObject templateWhiteTile = (GameObject)Instantiate(Resources.Load("WhiteTile"));

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                bool isEvenTile = (row + col) % 2 == 0;
                GameObject tile;
                if (isEvenTile)
                    tile = Instantiate(templateBlackTile);
                else
                    tile = Instantiate(templateWhiteTile);

                // Set tile parent (the Grid)
                tile.transform.parent = gameObject.transform;
                tile.transform.localScale = tileScale;

                tile.name = string.Format("Tile_{0},{1}", row, col);

                // Position inside Parent
                float xPosition = col * tile.transform.localScale.x * tileSize;
                float yPosition = 0;
                float zPosition = row * tile.transform.localScale.z * tileSize;
                tile.transform.localPosition = new Vector3(xPosition, yPosition, zPosition);

                tile.AddComponent<Tile>();

                tile.AddComponent<PositionCard>();
                PositionCard pc = tile.GetComponent<PositionCard>();
                tile.AddComponent<GvrPointerGraphicRaycaster>();

                tile.AddComponent<EventTrigger>();
                EventTrigger eventTrigger = tile.GetComponent<EventTrigger>();

                EventTrigger.Entry entry = new EventTrigger.Entry();
                // Click en casilla
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((data) => pc.OnPointerClick());
                eventTrigger.triggers.Add(entry);

                // Mirar casilla
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((data) => pc.setIsLooked(true));
                eventTrigger.triggers.Add(entry);

                // Quitar mirada casilla
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((data) => pc.setIsLooked(false));
                eventTrigger.triggers.Add(entry);

                tiles[row, col] = tile;
            }
        }

        Destroy(templateBlackTile);
        Destroy(templateWhiteTile);
    }

    private void MoveLeft()
    {
        // Inside Grid
        if ((int)selectedTile.y > 0 && (int)selectedTile.x >= 0 && (int)selectedTile.x < rows)
        {
            GameObject selectedTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            selectedTile.y += -1;
            GameObject nextTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            selectedTileObject.GetComponent<Tile>().Deselect();
            nextTileObject.GetComponent<Tile>().Select();
        }
    }
    private void MoveRight()
    {
        // Inside Grid
        if ((int)selectedTile.y < cols - 1 && (int)selectedTile.x >= 0 && (int)selectedTile.x < rows)
        {
            GameObject selectedTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            selectedTile.y += 1;
            GameObject nextTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            selectedTileObject.GetComponent<Tile>().Deselect();
            nextTileObject.GetComponent<Tile>().Select();
        }
    }

    // TODO if not local player transfer input changes to top instead of bottom 
    private void MoveDown()
    {
        // Inside Grid
        if ((int)selectedTile.x > 0 && (int)selectedTile.y >= 0 && (int)selectedTile.x < cols)
        {
            GameObject selectedTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            selectedTile.x += -1;
            GameObject nextTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            selectedTileObject.GetComponent<Tile>().Deselect();
            nextTileObject.GetComponent<Tile>().Select();
        }
        // Wehn out of
        else if ((int)selectedTile.x == 0 && (int)selectedTile.y >= 0 && (int)selectedTile.x < cols)
        {
            GameObject selectedTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            selectedTile.x += -1;
            selectedTileObject.GetComponent<Tile>().Deselect();
            // Transfer Input to cards
        }
    }

    // TODO same as above
    private void MoveUp()
    {
        // Inside Grid
        if ((int)selectedTile.x == -1 && (int)selectedTile.y >= 0 && (int)selectedTile.x < cols)
        {
            selectedTile.x += 1;
            GameObject nextTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            nextTileObject.GetComponent<Tile>().Select();
        }
        else if ((int)selectedTile.x < rows - 1 && (int)selectedTile.y >= 0 && (int)selectedTile.x < cols)
        {
            GameObject selectedTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            selectedTile.x += 1;
            GameObject nextTileObject = tiles[(int)selectedTile.x, (int)selectedTile.y];
            selectedTileObject.GetComponent<Tile>().Deselect();
            nextTileObject.GetComponent<Tile>().Select();
        }

    }
    /**
    * Transfers the Input from selecting Tiles from the Grid to selecting Cards from the Hand.
    * TODO Maybe would be better to call this DisableInput and EnableInput?
    */
    private void TransferInput()
    {
        controls.Gameplay.Disable();
        // Cards.ReceiveInput();
    }

    private void ReceiveInput()
    {
        controls.Gameplay.Enable();
        // MoveUp()
    }
}
