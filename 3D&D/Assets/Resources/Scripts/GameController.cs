using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/**
* Clase que controla todos los eventos y el gameplay
*
*/
public class GameController : MonoBehaviour
{
    public Grid Grid;
    private GameObject[] tilesAtDistance, tilesOutsideDistance;

    public bool IsMoving = false;
    public bool IsAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        Grid.SetGameController(this);
        Grid.GenerateGrid();
    }

    // Block controls for all tiles except area to be selected
    // Block particles for all tiles except area to be selected
    // Change click interaction to move interaction
    public void StartMove(Tile tile)
    {
        // UnitComponents unitComponents = unit.GetComponent<UnitComponents>();
        // TODO TEMPORAL TESTING
        UnitComponents unitComponents = new UnitComponents
        {
            MaxMovementDistance = 3,
            MinAttackDistance = 2,
            MaxAttackDistance = 2
        };

        (tilesAtDistance, tilesOutsideDistance) = GetTilesAtDistance(tile, 0, unitComponents.MaxMovementDistance, DistanceType.MANHATTAN);

        IsMoving = true;
        foreach (GameObject eachTile in tilesAtDistance)
        {
            eachTile.GetComponent<Tile>().IsSelectable = true;
            // TODO Particulas alrededor del area de movimiento
        }
        foreach (GameObject eachTile in tilesOutsideDistance)
        {
            eachTile.GetComponent<Tile>().IsSelectable = false;
        }
        // No podemos movernos a nosotros mismos
        tile.IsSelectable = false;
    }

    public void PerformMove(Tile tile)
    {
        // Ejecutar animación en el gameObject
        // TP GameObject
        // Particulas alrededor del gameObject
        IsMoving = false;
        foreach (GameObject eachTile in Grid.Tiles)
        {
            eachTile.GetComponent<Tile>().IsSelectable = true;
            eachTile.GetComponent<Tile>().SetIsLooked(false);
            // TODO Quitar particulas alrededor del area de movimiento
        }
    }

    public void StartAttack(Tile tile)
    {
        // UnitComponents unitComponents = unit.GetComponent<UnitComponents>();
        // TODO TEMPORAL TESTING
        UnitComponents unitComponents = new UnitComponents
        {
            MaxMovementDistance = 3,
            MinAttackDistance = 2,
            MaxAttackDistance = 3
        };

        (tilesAtDistance, tilesOutsideDistance) = GetTilesAtDistance(tile, unitComponents.MinAttackDistance, unitComponents.MaxAttackDistance, DistanceType.EUCLIDEAN);

        IsAttacking = true;
        foreach (GameObject eachTile in tilesAtDistance)
        {
            eachTile.GetComponent<Tile>().IsSelectable = true;
            // TODO Particulas alrededor del area de ataque
        }
        foreach (GameObject eachTile in tilesOutsideDistance)
        {
            eachTile.GetComponent<Tile>().IsSelectable = false;
        }
        // No podemos atacar a nosotros mismos
        tile.IsSelectable = false;
    }

    public void PerformAttack(Tile tile)
    {
        // Ejecutar animación en el gameObject
        // Bajar vida al enemigo
        IsAttacking = false;
        foreach (GameObject eachTile in Grid.Tiles)
        {
            eachTile.GetComponent<Tile>().IsSelectable = true;
            eachTile.GetComponent<Tile>().SetIsLooked(false);
            // TODO Quitar particulas alrededor del area de ataque
        }
    }
    /**
    * min included, max excluded
    */
    private (GameObject[], GameObject[]) GetTilesAtDistance(Tile tile, int minDistance, int maxDistance, DistanceType distanceType)
    {
        ArrayList tilesAtDistance = new ArrayList();
        ArrayList tilesOutsideDistance = new ArrayList();
        for (int i = 0; i < Grid.ROWS; i++)
        {
            for (int j = 0; j < Grid.COLS; j++)
            {
                var distance = distanceType switch
                {
                    DistanceType.MANHATTAN => Math.Abs(tile.Row - i) + Math.Abs(tile.Col - j),
                    DistanceType.EUCLIDEAN => (int)Math.Sqrt(Math.Pow(tile.Row - i, 2) + Math.Pow(tile.Col - j, 2)),
                    _ => 0,
                };

                if (distance >= minDistance && distance < maxDistance)
                {
                    tilesAtDistance.Add(Grid.Tiles[i, j]);
                }
                else
                {
                    tilesOutsideDistance.Add(Grid.Tiles[i, j]);
                }
            }
        }
        return (
            (GameObject[])tilesAtDistance.ToArray(typeof(GameObject)),
            (GameObject[])tilesOutsideDistance.ToArray(typeof(GameObject))
        );
    }
}

enum DistanceType
{
    MANHATTAN,
    EUCLIDEAN
}