using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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

                // Particles scale
                ParticleSystem cursorParticleSystem = tile.transform.Find("CursorParticle").GetComponent<ParticleSystem>();
                cursorParticleSystem.scalingMode = ParticleSystemScalingMode.Hierarchy;
                ParticleSystem areaParticleSystem = tile.transform.Find("AreaParticle").GetComponent<ParticleSystem>();
                areaParticleSystem.scalingMode = ParticleSystemScalingMode.Hierarchy;

                // Send data to the tile
                tile.GetComponent<Tile>().Row = row;
                tile.GetComponent<Tile>().Col = col;
                tile.GetComponent<Tile>().TableSeparation = ROWS / 2;
                tile.GetComponent<Tile>().SetGameController(gameController);

                // r 5 c 2 KnightWarrior
                if (row == 5 && col == 2)
                {
                    var knight = Resources.Load<GameObject>("Characters/Prefabs/KnightWarrior/KnightWarrior");
                    knight.GetComponent<MinionCharacter>().tile = tile.GetComponent<Tile>();
                    Instantiate(knight, tile.transform);
                }
                // r 0 c 3 DemonicMage
                if (row == 0 && col == 3)
                {
                    var demonicMage = Resources.Load<GameObject>("Characters/Prefabs/DemonicMage/DemonicMage");
                    demonicMage.GetComponent<MinionCharacter>().tile = tile.GetComponent<Tile>();
                    Instantiate(demonicMage, tile.transform);
                }

                Tiles[row, col] = tile;
            }
        }

        Destroy(templateBlackTile);
        Destroy(templateWhiteTile);
    }
}

