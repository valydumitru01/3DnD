using System;
using System.Collections;
using UnityEngine;

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

    private PlayerEnum player = PlayerEnum.KNIGHT;

    public int manaMovement = 10;
    public int manaAttack = 20;

    private GameObject currentMana;
    private ManaManager manaManager;

    // Start is called before the first frame update
    void Start()
    {
        Grid.SetGameController(this);
        Grid.GenerateGrid();
        updateManaCharacter();
    }
    internal void changePlayer()
    {
        if(player == PlayerEnum.KNIGHT)
            player = PlayerEnum.DEMON;
        else
            player = PlayerEnum.KNIGHT;
    }
    public void updateManaCharacter()
    {
        GameObject[] manas = GameObject.FindGameObjectsWithTag("Mana");
        foreach (GameObject mana in manas) 
        { 
            if (mana.GetComponent<ManaManager>().Player == player) 
            { 
                currentMana = mana;
                manaManager = mana.GetComponent<ManaManager>(); 
                return;
            }
        }
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
        
        if (currentMana.GetComponent<ManaManager>().CanUpdate(manaMovement)){
            Tile start = activatedTile;

            // Mover minion de una casilla a otra
            GameObject minion = start.transform.GetChild(3).gameObject;
            Vector3 position = minion.transform.localPosition;
            minion.transform.SetParent(end.transform);
            minion.transform.localPosition = position;
            minion.GetComponent<MinionCharacter>().tile = end;


            // TP GameObject
            minion.GetComponent<MinionCharacter>().isSelected = false;

            // Particulas alrededor del gameObject
            ParticleSystem teleportParticleSystem = end.GetParticleSystem("teleport");
            teleportParticleSystem.Play();

            // Ejecutar animación en el gameObject de TP
            Animator animator = minion.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("isWalking", true);
                StartCoroutine(minion.GetComponent<MinionCharacter>().ReturnToIdle());
            }
        }
        ResetTiles();
    }

    public void StartAttack(Tile tile, MinionCharacter minionCharacter)
    {
        Debug.Log("Start attack");
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

    public void PerformAttack(MinionCharacter minionCharacter)
    {
        if (minionCharacter.Equals(selectedMinion))
            return;
        
        if(currentMana.GetComponent<ManaManager>().CanUpdate(manaAttack)){
            // Ejecutar animación en el gameObject
            Animator animator = selectedMinion.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("isFighting", true);
                StartCoroutine(minionCharacter.ReturnToIdle());
            }
            selectedMinion.PlayAttack();
            StartCoroutine(PlayHitSound(minionCharacter));

            // Bajar vida al enemigo
            minionCharacter.DamageMinion(selectedMinion.damage);
            Debug.Log(selectedMinion.cardName + " atacando a: " + minionCharacter.cardName);
        }
        ResetTiles();
    }

    internal ManaManager getMana()
    {
        return manaManager;
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

    private IEnumerator PlayHitSound(MinionCharacter character)
    {
        yield return new WaitForSeconds(2);
        character.PlayHit();
    }
}

enum DistanceType
{
    MANHATTAN,
    EUCLIDEAN
}