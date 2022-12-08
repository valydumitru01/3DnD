using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsSelectable = true;
    public int Row { get; set; }
    public int Col { get; set; }
    public int Player = 1;
    public int TableSeparation { get; set; }

    private new ParticleSystem particleSystem;

    private IEnumerable<CardGazeInput> cardsInput;
    public GameController gameController;
    private ManaManager mana;

    private bool isLooked;
    public float timerDuration = 3f;
    private float lookTimer = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        particleSystem.Stop();
        cardsInput = GameObject.FindGameObjectsWithTag("Card")
                           .Select(card => card.GetComponent<CardGazeInput>());

        mana = GameObject.FindWithTag("Mana").GetComponent<ManaManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSelectable)
            if (isLooked)
            {
                if(Player == 1)
                    particleSystem.startColor = Color.blue;
                else
                    particleSystem.startColor = Color.red;
                particleSystem.Play();

                lookTimer += Time.deltaTime;

                if (lookTimer > timerDuration)
                {
                    lookTimer = 0f;
                    OnPointerClick();
                }
            }
            else
            {
                particleSystem.Stop();
                lookTimer = 0f;
            }
    }

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void SetIsLooked(bool value)
    {
        if (IsSelectable)
            isLooked = value;
    }

    public void OnPointerClick()
    {
        if (IsSelectable)
        {
            IEnumerable<CardGazeInput> selectedCard = cardsInput.Where(card => card.IsSelected && card.gameObject.activeSelf);
            if (selectedCard.Count() > 0)
            {

                if (ThereIsAPieceAlready() && IsYourSideOfTable() && ThereIsEnoughMana(selectedCard))
                {
                    StartCoroutine(UseCard(selectedCard.First(), 0.5f));
                }
            }
            else
            {
                if (gameController.IsMoving)
                {
                    gameController.PerformMove(this);
                }
            }
        }
    }
    private bool ThereIsEnoughMana(IEnumerable<CardGazeInput> selectedCard)
    {
        return mana.CanUpdate(selectedCard.First().GetComponent<CardCharacter>().manaCost);
    }
    
    private bool IsYourSideOfTable()
    {
        if(Player==1)
            return Row >= TableSeparation;
        else
            return Row < TableSeparation;
    }
    private bool ThereIsAPieceAlready()
    {
        return transform.childCount < 1;
    }
    IEnumerator UseCard(CardGazeInput selectedCard, float time)
    {
        GameObject cardGenerator = GameObject.FindGameObjectWithTag("CardGenerator");
        
        selectedCard.CanBeFocused = false;
        var objective = transform.position;
        objective.y += 0.01f;
        while (selectedCard.transform.position != objective)
        {
            selectedCard.transform.position = Vector3.MoveTowards(selectedCard.transform.position, objective, 3 * Time.deltaTime);
            selectedCard.transform.rotation = Quaternion.Lerp(selectedCard.transform.rotation, Quaternion.Euler(-90, 0, 0), 10 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(time);
        selectedCard.gameObject.SetActive(false);
        selectedCard.InvocateMinion(this, Player);
        cardGenerator.GetComponent<GenerateAround>().CardInHands--;
        if (cardGenerator.GetComponent<GenerateAround>().CardInHands == 0)
            cardGenerator.GetComponent<GenerateAround>().RefillHand = true;
    }
}
