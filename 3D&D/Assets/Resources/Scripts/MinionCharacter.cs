using System.Collections;
using System.Linq;
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
    public bool moveCards = true;
    private bool cardsInPlace = false;

    public int MaxMovementDistance;
    public int MinAttackDistance;
    public int MaxAttackDistance;

    public HealthBar healthBar;
    public GameObject cardsHand;
    public GameObject actionCards;
    private Vector3 handInitialPosition;
    private Vector3 actionInitialPosition;

    //Sonido
    private AudioClip attackClip;
    private AudioClip hitClip;
    private AudioClip summonClip;
    private AudioClip deathClip;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        cardsHand = GameObject.FindWithTag("CardsHand");
        handInitialPosition = cardsHand.transform.position;
        // cardsHand.GetComponent<CardsManagement>().CanInteract();

        actionCards = GameObject.FindWithTag("ActionCards");
        actionInitialPosition = actionCards.transform.position;
        actionCards.GetComponentsInChildren<CardGazeInput>().ToList().ForEach(card => card.InitialPosition = card.transform.localPosition);
        InitAudioSource();
        PlaySummon();
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

    public void SetIsLooked(bool looked)
    {
        IsLooked = looked;
    }

    public void OnPointerClick()
    {
        StartCoroutine(MoveCards());
        if (cardsInPlace && tile.gameController.IsAttacking)
            PerformAction();
    }

    public void PerformAction()
    {
        if (!isSelected)
        {
            Debug.Log(tile.gameController.selectedMinion);
            if (tile.gameController.selectedMinion == null || tile.gameController.selectedMinion == this)
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
                    actionCards.GetComponentsInChildren<ActionCard>().ToList().ForEach(card =>
                    {
                        if (card.IsSelected)
                        {
                            card.OnPointerClick();
                        }
                    });
                }
            }

        }
        else
        {
            isSelected = false;
            tile.gameController.ResetTiles();
        }
    }

    private IEnumerator MoveCards()
    {
        Vector3 handEndPosition;
        Vector3 actionEndPosition;

        if (cardsHand.GetComponent<CardsManagement>().CardSelected())
            moveCards = false;

        if (moveCards)
        {
            handEndPosition = handInitialPosition - new Vector3(3, 0, 0);
            actionEndPosition = actionInitialPosition + new Vector3(0, 0, 3);
            actionCards.GetComponentsInChildren<ActionCard>().ToList().ForEach(card => { if (card.minion == null) card.minion = gameObject; });
            if (!tile.gameController.IsAttacking && !tile.gameController.IsMoving)
                moveCards = false;

            cardsHand.GetComponent<CardsManagement>().CantInteract();
            actionCards.GetComponent<CardsManagement>().CanInteract();
            cardsInPlace = true;
        }
        else
        {
            handEndPosition = handInitialPosition;
            actionEndPosition = actionInitialPosition;
            moveCards = true;

            cardsHand.GetComponent<CardsManagement>().CanInteract();
            actionCards.GetComponent<CardsManagement>().CantInteract();
            actionCards.GetComponentsInChildren<ActionCard>().ToList().ForEach(card => card.minion = null);
            cardsInPlace = false;
        }
        while (cardsHand.transform.localPosition != handEndPosition)
        {
            cardsHand.transform.localPosition = Vector3.MoveTowards(cardsHand.transform.localPosition, handEndPosition, 2.5f * Time.deltaTime);
            actionCards.transform.localPosition = Vector3.MoveTowards(actionCards.transform.localPosition, actionEndPosition, 2.5f * Time.deltaTime);
            yield return null;
        }
    }

    public void DamageMinion(int damage)
    {
        currentHealth -= damage;

        GameObject minion = gameObject;
        Animator animator = minion.GetComponent<Animator>();
        if (currentHealth <= 0)
        {
            if (animator != null)
            {
                animator.SetBool("isDieing", true);
                StartCoroutine(PlayDeathSound());
            }
            StartCoroutine(DestroyMinion());
        }
        else
        {
            if (animator != null)
            {
                animator.SetBool("isGettingHit", true);
                StartCoroutine(ReturnToIdle());
            }
        }
        healthBar.UpdateHealthBar();
    }

    private IEnumerator DestroyMinion()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject, 1f);
    }

    public IEnumerator PlayDeathSound()
    {
        yield return new WaitForSeconds(2);
        PlayDeath();
    }

    public IEnumerator ReturnToIdle()
    {
        yield return new WaitForSeconds(5);
        GameObject minion = gameObject;
        Animator animator = minion.GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isFighting", false);
            animator.SetBool("isGettingHit", false);
            animator.SetBool("isDieing", false);
        }
    }

    public void InitAudioSource()
    {
        var playerEffects = gameObject.AddComponent<AudioSource>();
        playerEffects.enabled = true;
        playerEffects.playOnAwake = false;

        attackClip = Resources.Load<AudioClip>("Characters/Sounds/" + cardName + "/Attack");
        hitClip = Resources.Load<AudioClip>("Characters/Sounds/" + cardName + "/GettingHit");
        summonClip = Resources.Load<AudioClip>("Characters/Sounds/" + cardName + "/Summoning");
        deathClip = Resources.Load<AudioClip>("Characters/Sounds/" + cardName + "/Death");
    }

    public void PlayAttack()
    {
        var playerEffects = gameObject.GetComponent<AudioSource>();
        playerEffects.PlayOneShot(attackClip);
    }

    public void PlayHit()
    {
        var playerEffects = gameObject.GetComponent<AudioSource>();
        playerEffects.PlayOneShot(hitClip);
    }

    public void PlaySummon()
    {
        var playerEffects = gameObject.GetComponent<AudioSource>();
        playerEffects.PlayOneShot(summonClip);
    }

    public void PlayDeath()
    {
        var playerEffects = gameObject.GetComponent<AudioSource>();
        playerEffects.PlayOneShot(deathClip);
    }
}
