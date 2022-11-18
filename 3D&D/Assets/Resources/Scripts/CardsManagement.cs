using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardsManagement : MonoBehaviour
{
    private IEnumerable<CardGazeInput> cardsInput;
    private IEnumerable<CardGazeInput> notSelectedCards;

    // Start is called before the first frame update
    void Start()
    {
        cardsInput = GameObject.FindGameObjectsWithTag("Card")
                               .Select(card => card.GetComponent<CardGazeInput>());
    }

    // Update is called once per frame
    void Update()
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
}