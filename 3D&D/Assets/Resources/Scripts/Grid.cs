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
    public const int ROWS = 6;
    public const int COLS = 6;
    public GameObject[,] Tiles = new GameObject[ROWS, COLS];

    // Defines the separaction between Tiles
    // 10f No separation, 20f 1 Tile separation
    private const float tileSize = 10f;
    private Vector3 tileScale = new Vector3(0.05f, 0.05f, 0.05f);

    // Defines if actual turn player is Local Player.
    private bool isLocal = true;
    private bool isGazeInput = false;

    private GameController gameController;

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    /**
    * Genera el Tablero (Grid) que contiene las casillas de juego (Tile)
    */
    public void GenerateGrid()
    {
        GameObject templateBlackTile = (GameObject)Instantiate(Resources.Load("Prefabs/BlackTile"));
        GameObject templateWhiteTile = (GameObject)Instantiate(Resources.Load("Prefabs/WhiteTile"));

        for (int row = 0; row < ROWS; row++)
        {
            for (int col = 0; col < COLS; col++)
            {
                bool isEvenTile = (row + col) % 2 == 0;
                GameObject tile;
                if (isEvenTile)
                    tile = Instantiate(templateBlackTile);
                else
                    tile = Instantiate(templateWhiteTile);

                // Set tile parent (the Grid)
                tile.transform.SetParent(gameObject.transform);
                tile.transform.localScale = tileScale;

                tile.name = string.Format("Tile_{0},{1}", row, col);

                // Position inside Parent
                float xPosition = col * tile.transform.localScale.x * tileSize;
                float yPosition = 0;
                float zPosition = row * tile.transform.localScale.z * tileSize;
                tile.transform.localPosition = new Vector3(xPosition, yPosition, zPosition);

                // Send data to the tile
                tile.GetComponent<Tile>().Row = row;
                tile.GetComponent<Tile>().Col = col;
                tile.GetComponent<Tile>().SetGameController(gameController);

                Tiles[row, col] = tile;
            }
        }

        Destroy(templateBlackTile);
        Destroy(templateWhiteTile);
    }
}
