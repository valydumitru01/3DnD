using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 6;
    public int cols = 6;
    public float tileSize = 1 * 5;
    public float boardHeight = 0;
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

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
                    tile = (GameObject)Instantiate(templateBlackTile, transform);
                else
                    tile = (GameObject)Instantiate(templateWhiteTile, transform);

                float xPosition = col * tileSize;
                float yPosition = boardHeight;
                float zPosition = row * -tileSize;

                tile.transform.position = new Vector3(xPosition, yPosition, zPosition);
            }
        }

        Destroy(templateBlackTile);
        Destroy(templateWhiteTile);

        float gridWidth = cols * tileSize;
        float gridHeight = rows * tileSize;

        transform.position = new Vector3(-gridWidth / 2 + tileSize / 2, boardHeight, -gridHeight / 2 + tileSize / 2);
    }
}
