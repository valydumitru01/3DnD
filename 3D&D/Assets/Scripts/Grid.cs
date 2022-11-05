using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private const int rows = 6;
    private const int cols = 6;
    // Define la separación entre Tiles
    // 10f No hay separación, 20f Hay 1 Tile de separación
    private const float tileSize = 10f;
    private Vector3 tileScale = new Vector3(1f, 1f, 1f);
    private Vector3 boardPosition = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

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
                    tile = (GameObject)Instantiate(templateBlackTile);
                else
                    tile = (GameObject)Instantiate(templateWhiteTile);

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
            }
        }

        Destroy(templateBlackTile);
        Destroy(templateWhiteTile);

        // Grid Position
        transform.position = boardPosition;
    }
}
