using UnityEngine;



public class CardGazeInput : MonoBehaviour
{
    private Vector3 initialPosition;
    private CardCharacter character;
    //TIMER
    public float timerDuration = 3f;
    public float lookTimer = 0f;

    public bool IsSelected { get; set; }
    public bool IsLooked { get; set; }
    public bool CanBeFocused { get; set; }
    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public GameObject loadingCircle;

    public virtual void Start()
    {
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // InitialPosition = transform.localPosition;
        character = GetComponent<CardCharacter>();
        loadingCircle = GameObject.FindGameObjectWithTag("LoadingSelectingCircle");
        gameObject.GetComponentInParent<CardsManagement>().CanInteract();
    }

    public virtual void Update()
    {
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
            StopLoading();
            lookTimer = 0f;
        }
    }

    public void SetIsLooked(bool looked)
    {
        if (lookTimer <= timerDuration)
            StartLoading();
        if (CanBeFocused)
        {
            IsLooked = looked;
            JumpCard(IsLooked);
        }
    }


    public void StartLoading()
    {
        loadingCircle.GetComponent<SelectLoading>().StartLoading(timerDuration);
    }
    public void StopLoading()
    {
        loadingCircle.GetComponent<SelectLoading>().stopLoading();
    }













    public virtual void OnPointerClick()
    {
        if (CanBeFocused)
        {
            if (IsSelected)
            {
                // StartCoroutine(Move(InitialPosition));
                Move(InitialPosition);
                IsSelected = false;
            }
            else
            {
                var endPosition = transform.localPosition + new Vector3(0, 1.7f, 1.5f);
                // StartCoroutine(Move(endPosition));
                Move(endPosition);
                IsSelected = true;
            }
        }
    }

    public void JumpCard(bool isLooked)
    {
        if (!IsSelected)
        {
            if (isLooked)
            {
                var endPosition = transform.localPosition + new Vector3(0, 0.25f, 0);
                // StartCoroutine(Move(endPosition));
                Move(endPosition);
            }
            else
            {
                // StartCoroutine(Move(InitialPosition));
                Move(InitialPosition);
            }
        }
    }

    public bool InvocateMinion(Tile tile, int Player)
    {
        if (character.InvocateMinion(tile, Player))
        {
            IsSelected = false;

            GenerateAround cardGenerator = GameObject.FindGameObjectWithTag("CardGenerator").GetComponent<GenerateAround>();
            cardGenerator.hand.Remove(character.cardName);
            cardGenerator.cards.Remove(gameObject);
            if (cardGenerator.hand.Count == 0)
                cardGenerator.SetRefill(true);
            return true;
        }
        return false;
    }

    public void /*IEnumerator*/ Move(Vector3 endPosition)
    {
        while (transform.localPosition != endPosition)
        {
            // Debug.Log(endPosition);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPosition, 5 * Time.deltaTime);
            // yield return null;
        }
    }
}