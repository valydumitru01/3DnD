using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

/**
* Clase que controla todos los eventos y el gameplay
*
*/
public class GameController : MonoBehaviour
{
    public Grid Grid;
    private GameObject[] tilesAtDistance, tilesOutsideDistance;
    private Tile activatedTile;
    public MinionCharacter selectedMinion;

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
    public void StartMove(Tile tile, MinionCharacter minionCharacter)
    {
        selectedMinion = minionCharacter;
        (tilesAtDistance, tilesOutsideDistance) = GetTilesAtDistance(tile, 0, minionCharacter.MaxMovementDistance, DistanceType.MANHATTAN);

        foreach (GameObject eachTile in tilesAtDistance)
        {
            // El Tile tiene siempre 3 hijos que son los controladores de particulas
            if (eachTile.transform.childCount == 3)
            {
                eachTile.GetComponent<Tile>().IsSelectable = true;
            }
            else
            {
                eachTile.GetComponent<Tile>().IsSelectable = false;
            }
        }
        foreach (GameObject eachTile in tilesOutsideDistance)
        {
            eachTile.GetComponent<Tile>().IsSelectable = false;
        }
        // No podemos movernos a nosotros mismos
        activatedTile = tile;
    }

    public void PerformMove(Tile end)
    {
        // TODO Ejecutar animación en el gameObject de TP
        // TODO Particulas alrededor del gameObject
        Tile start = activatedTile;

        // Mover minion de una casilla a otra
        // TODO ARREGLAR, AHORA LA CASILLA TIENE MAS DE 1 HIJO
        GameObject minion = start.transform.GetChild(3).gameObject;
        Vector3 position = minion.transform.localPosition;
        minion.transform.SetParent(end.transform);
        minion.transform.localPosition = position;
        minion.GetComponent<MinionCharacter>().tile = end;

        // TP GameObject
        minion.GetComponent<MinionCharacter>().isSelected = false;
        ResetTiles();

        ParticleSystem teleportParticleSystem = end.GetParticleSystem("teleport");
        teleportParticleSystem.Play();

        Animator animator = minion.GetComponent<Animator>();
        if(animator != null){
            animator.SetBool("isWalking", true);
            StartCoroutine(ReturnToIdle(minion));
        }
    }

    public void StartAttack(Tile tile, MinionCharacter minionCharacter)
    {
        selectedMinion = minionCharacter;
        MinionCharacter character = tile.GetComponentInChildren<MinionCharacter>();

        (tilesAtDistance, tilesOutsideDistance) = GetTilesAtDistance(tile, character.MinAttackDistance, character.MaxAttackDistance, DistanceType.EUCLIDEAN);

        foreach (GameObject eachTile in tilesAtDistance)
        {
            //TODO check is enemy the minion on the tile to attack
            // El Tile tiene siempre 3 hijos que son los controladores de particulas
            if (eachTile.transform.childCount > 3 /*and is enemy*/)
            {
                eachTile.GetComponent<Tile>().IsSelectable = true;
            }
            else
            {
                eachTile.GetComponent<Tile>().IsSelectable = false;
            }
        }
        foreach (GameObject eachTile in tilesOutsideDistance)
        {
            eachTile.GetComponent<Tile>().IsSelectable = false;
        }
        // No podemos movernos a nosotros mismos
        tile.IsSelectable = false;
        activatedTile = tile;
    }

    // TODO quitar vida, mana, animacinoes
    public void PerformAttack(MinionCharacter minionCharacter)
    {
        // Ejecutar animación en el gameObject
        Tile tileAttack = selectedMinion.GetTile();
        GameObject minion = Grid.Tiles[tileAttack.Row, tileAttack.Col].transform.GetChild(3).gameObject;
        Animator animator = selectedMinion.GetComponent<Animator>();
        if(animator != null){
            animator.SetBool("isFighting", true);
            StartCoroutine(ReturnToIdle(minion));
        }

        Tile tileHit = minionCharacter.GetTile();
        minion = Grid.Tiles[tileHit.Row, tileHit.Col].transform.GetChild(3).gameObject;
        animator = minion.GetComponent<Animator>();
        if(animator != null){
            animator.SetBool("isGettingHit", true);
            StartCoroutine(ReturnToIdle(minion));
        }

        // Bajar vida al enemigo
        Debug.Log(selectedMinion.cardName + " atacando a: " + minionCharacter.cardName);
        ResetTiles();
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

                if (distance >= minDistance && distance <= maxDistance)
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

    public void ResetTiles()
    {
        selectedMinion = null;
        foreach (GameObject eachTile in Grid.Tiles)
        {
            eachTile.GetComponent<Tile>().IsSelectable = true;
            eachTile.GetComponent<Tile>().SetIsLooked(false);
        }
        activatedTile = null;
        IsMoving = false;
        IsAttacking = false;
    }

    private IEnumerator ReturnToIdle(GameObject minion){
        yield return new WaitForSeconds(5);
        Animator animator = minion.GetComponent<Animator>();
            if(animator != null){
                animator.SetBool("isWalking", false);
                animator.SetBool("isFighting", false);
                animator.SetBool("isGettingHit", false);
                animator.SetBool("isDieing", false);
            }
    }
}

enum DistanceType
{
    MANHATTAN,
    EUCLIDEAN
}