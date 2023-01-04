using System.ComponentModel;
using UnityEngine;

public class MinionCharacter : MonoBehaviour
{
    public string cardName;
    public int currentHealth;
    public int maxHealth;
    public int damage;
    public int manaCost;
    public Tile tile;

    public bool IsLooked { get; set; }
    //TIMER
    public float timerDuration = 3f;
    private float lookTimer = 0f;

    public bool isSelected;

    public int MaxMovementDistance;
    public int MinAttackDistance;
    public int MaxAttackDistance;

    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            DamageMinion(1);
        if (IsLooked)
        {
            lookTimer += Time.deltaTime;

            if (lookTimer > timerDuration)
            {
                lookTimer = 0f;
                OnPointerClick();
            }
        }
        else
        {
            lookTimer = 0f;
        }
    }

    public void OnPointerClick()
    {
        // TODO quitar, es para pruebas, solo puede haber 1 activo en pruebas
        // tile.gameController.IsAttacking = true;
        tile.gameController.IsMoving = true;
        // end todo
        if (!isSelected)
        {
            if (tile.gameController.selectedMinion.Equals(cardName) || tile.gameController.selectedMinion.Equals(""))
            {
                if (tile.gameController.IsMoving)
                {
                    isSelected = true;
                    tile.gameController.StartMove(tile, this);
                    Debug.Log(string.Format("Started moving from tile {0},{1}", tile.Row, tile.Col));
                }
                else if (tile.gameController.IsAttacking)
                {
                    isSelected = true;
                    tile.gameController.StartAttack(tile, this);
                    Debug.Log(string.Format("Started attacking from tile {0},{1}", tile.Row, tile.Col));
                }
            }
            else
            {
                if (tile.gameController.IsAttacking)
                {
                    //recibir daño
                    tile.gameController.PerformAttack(this);
                }
            }

        }
        else
        {
            isSelected = false;
            tile.gameController.ResetTiles();
        }
    }

    public void DamageMinion(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject, 1f);
        }
        healthBar.UpdateHealthBar();
    }
}
