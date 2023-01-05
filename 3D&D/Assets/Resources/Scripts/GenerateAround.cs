using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateAround : MonoBehaviour
{
    /*
     * Valores de los arrays en orden
     * {health, damage, offset.y, manaCost, MaxMovementDistance, MinAttackDistance, MaxAttackDistance}
     * */
    private Dictionary<string, float[]> characters = new Dictionary<string, float[]>()
    {
        { "Guerrero", new[] {5, 3, 0.1f, 40, 2, 1, 2} },
        { "Mago", new[] {3, 4, 0.1f, 40, 2, 2, 3} },
        { "Hada", new[] {3, 1, 5f, 5, 30, 1, 1} },
        { "Troll", new[] {15, 2, 0.1f, 60, 2, 1, 3} },
        { "Esqueleto", new[] {2, 2, 0.1f, 30, 2, 1, 2} },
        { "Perro", new[] {10, 6, 0.1f, 100, 3, 2, 3} }
    };

    public List<GameObject> cards = new List<GameObject>();
    public List<string> hand = new List<string>();
    private Transform cardsHand;

    public float radius = 3.94f;
    public float range = 4.93f;
    public float distance = -4.53f;
    public float height = -1.65f;
    private bool refillHand = false;

    private Transform playerTransform;

    private Vector3 initialPosition = new Vector3(-6.2f, 1.14f, -0.61f);
    private Quaternion initialRotation = Quaternion.Euler(-90, 0, 0);
    private Vector3 finalPosition = new Vector3(-1f, -.6f, 1.6f);
    private Quaternion finalRotation = Quaternion.Euler(-30, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponentInParent<Transform>();
        cardsHand = GameObject.FindWithTag("CardsHand").transform;

        GenerateHand();
    }

    void GenerateHand()
    {
        var rnd = new System.Random();
        bool generated = false;
        while (hand.Count < 3)
        {
            var character = characters.ElementAt(rnd.Next(0, 6));
            hand.Add(character.Key);
            GenerateCharacter(character);
            generated = true;
        }

        GameObject.FindWithTag("Mana").GetComponent<ManaManager>().nextTurn();

        if (generated)
        {
            PositionCards();
            generated = false;
        }
    }

    private void GenerateCharacter(KeyValuePair<string, float[]> character)
    {
        GameObject newCard = Instantiate(Resources.Load<GameObject>("Prefabs/InvocationCard"), cardsHand);

        newCard.transform.localPosition = initialPosition;
        newCard.transform.localRotation = initialRotation;
        newCard.SetActive(false);
        // finalPosition.x -= 1.5f;

        var characterData = newCard.GetComponent<CardCharacter>();
        characterData.name = $"{character.Key}Card";
        characterData.cardName = character.Key;
        characterData.health = (int)character.Value[0];
        characterData.damage = (int)character.Value[1];
        characterData.offset.y = character.Value[2];
        characterData.manaCost = (int)character.Value[3];

        characterData.MaxMovementDistance = (int)character.Value[4];
        characterData.MinAttackDistance = (int)character.Value[5];
        characterData.MaxAttackDistance = (int)character.Value[6];

        cards.Add(newCard);
    }

    private void PositionCards()
    {
        foreach (var card in cards)
        {
            card.SetActive(true);
            StartCoroutine(Move(card, finalPosition));
            finalPosition.x -= 1.5f;
        }
    }

    private void Update()
    {
        if (refillHand == true)
        {
            finalPosition = new Vector3(-1f, -.6f, 1.6f);
            GenerateHand();
            refillHand = false;
        }
    }

    private IEnumerator Move(GameObject card, Vector3 endPosition)
    {
        card.GetComponent<CardGazeInput>().InitialPosition = endPosition;
        card.transform.localPosition = initialPosition;
        card.transform.localRotation = initialRotation;
        while (card.transform.localPosition != endPosition)
        {
            card.transform.localPosition = Vector3.MoveTowards(card.transform.localPosition, endPosition, 2.5f * Time.deltaTime);
            card.transform.localRotation = Quaternion.Lerp(card.transform.localRotation, finalRotation, 5 * Time.deltaTime);
            yield return null;
        }

    }

    public void SetRefill(bool refill)
    {
        refillHand = refill;
    }
}
