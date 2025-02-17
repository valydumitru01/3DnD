using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class CardsManagement : MonoBehaviour
{
    public string cardTag;
    private IEnumerable<CardGazeInput> cardsInput;
    private IEnumerable<CardGazeInput> notSelectedCards;
    private bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        cardsInput = GameObject.FindGameObjectsWithTag(cardTag)
                               .Select(card => card.GetComponent<CardGazeInput>());
    }

    // Update is called once per frame
    void Update()
    {
        if (canInteract)
        {
            IEnumerable<CardGazeInput> selectedCard = cardsInput.Where(card => card.IsSelected && card.gameObject.activeInHierarchy);
            if (selectedCard.Count() > 0)
            {
                notSelectedCards = cardsInput.Where(card => card.gameObject.name != selectedCard.First().gameObject.name);
                foreach (CardGazeInput card in notSelectedCards)
                {
                    card.CanBeFocused = false;
                }
            }
            else
            {
                notSelectedCards = cardsInput.Where(card => !card.CanBeFocused && card.gameObject.activeInHierarchy);
                foreach (CardGazeInput card in notSelectedCards)
                {
                    card.CanBeFocused = true;
                }
            }
        }
        DestroyDisabled();
    }

    void DestroyDisabled()
    {
        IEnumerable<CardGazeInput> disabledCards = cardsInput.Where(card => !card.gameObject.activeInHierarchy);
        foreach (var disable in disabledCards)
        {
            Destroy(disable.gameObject, 1f);
        }
        cardsInput = GameObject.FindGameObjectsWithTag(cardTag)
                               .Select(card => card.GetComponent<CardGazeInput>());
    }

    public bool CardSelected()
    {
        return cardsInput.Any(card => card.IsSelected && card.gameObject.activeInHierarchy);
    }

    public void CanInteract()
    {
        canInteract = true;
        cardsInput = GameObject.FindGameObjectsWithTag(cardTag)
                            .Select(card => card.GetComponent<CardGazeInput>());
        foreach (var card in cardsInput)
        {
            card.CanBeFocused = true;
        }
    }

    public void CantInteract()
    {
        canInteract = false;
        cardsInput = GameObject.FindGameObjectsWithTag(cardTag)
                            .Select(card => card.GetComponent<CardGazeInput>());
        foreach (var card in cardsInput)
        {
            card.CanBeFocused = false;
        }
    }

    public void NextTurn()
    {
        cardsInput = GameObject.FindGameObjectsWithTag(cardTag)
                               .Select(card => card.GetComponent<CardGazeInput>());

        foreach (var card in cardsInput)
        {
            card.CanBeFocused = true;
            card.IsSelected = false;
        }
    }
}