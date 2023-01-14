using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCard : CardGazeInput
{
    public GameObject minion;

    // Start is called before the first frame update
    public override void Start()
    {
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        loadingCircle = GameObject.FindGameObjectWithTag("LoadingSelectingCircle");
        gameObject.GetComponentInParent<CardsManagement>().CantInteract();

        if (name.Equals("Ataque"))
        {
            var cardPath = $"Images/Cards/carta_front_{name.ToLower()}";
            var cardFront = gameObject.GetComponentInChildren<SpriteRenderer>();
            cardFront.sprite = Resources.Load<Sprite>(cardPath);
        }
        else if (name.Equals("Moverse"))
        {
            var cardPath = $"Images/Cards/carta_front_{name.ToLower()}";
            var cardFront = gameObject.GetComponentInChildren<SpriteRenderer>();
            cardFront.sprite = Resources.Load<Sprite>(cardPath);
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        if (Input.GetAxis("Fire1") > 0 && IsLooked)
        {
            OnPointerClick();
        }
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

    public override void OnPointerClick()
    {
        if (CanBeFocused)
        {
            if (IsSelected)
            {
                Move(InitialPosition);
                PerformAction();
                minion.GetComponent<MinionCharacter>().moveCards = false;
                IsSelected = false;
            }
            else
            {
                var endPosition = transform.localPosition + new Vector3(0, 1.2f, 1.5f);
                Move(endPosition);
                PerformAction();
                IsSelected = true;
            }
        }
    }

    private void PerformAction()
    {
        if (IsSelected)
        {
            minion.GetComponent<MinionCharacter>().tile.gameController.IsMoving = false;
            minion.GetComponent<MinionCharacter>().tile.gameController.IsAttacking = false;
            minion.GetComponent<MinionCharacter>().tile.gameController.ResetTiles();
        }
        else if (name.Equals("Ataque"))
        {
            minion.GetComponent<MinionCharacter>().tile.gameController.IsMoving = false;
            minion.GetComponent<MinionCharacter>().tile.gameController.IsAttacking = true;
            minion.GetComponent<MinionCharacter>().moveCards = true;
            minion.GetComponent<MinionCharacter>().PerformAction();
        }
        else if (name.Equals("Moverse"))
        {
            minion.GetComponent<MinionCharacter>().tile.gameController.IsMoving = true;
            minion.GetComponent<MinionCharacter>().tile.gameController.IsAttacking = false;
            minion.GetComponent<MinionCharacter>().moveCards = true;
            minion.GetComponent<MinionCharacter>().PerformAction();
        }
    }
}
