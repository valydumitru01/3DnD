using System.Collections.Generic;
using UnityEngine;

public class GenerateAround : MonoBehaviour
{
    private Dictionary<string, float[]> characters = new Dictionary<string, float[]>() { { "Guerrero", new[] { 5, 3 , 0.1f} },
                                                                                        { "Mago", new[] { 3, 4, 0.1f } },
                                                                                        { "Hada", new[] { 2, 2, 0.25f } } };
    public float radius = 3.94f;
    public float range = 4.93f;
    public float distance = -4.53f;
    public float height = -1.65f;
    private List<GameObject> cards = new List<GameObject>();
    private Transform playerTransform;
    private Transform cardsHand;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GetComponentInParent<Transform>();
        cardsHand = GameObject.FindWithTag("CardsHand").transform;

        foreach (var character in characters)
        {
            GenerateCharacter(character);
        }

        PositionCards();
    }

    private void GenerateCharacter(KeyValuePair<string, float[]> character)
    {
        GameObject newCard = Instantiate(Resources.Load<GameObject>("Prefabs/InvocationCard"), cardsHand);
        var characterData = newCard.GetComponent<Character>();
        characterData.name = $"{character.Key}Card";
        characterData.cardName = character.Key;
        characterData.lifes = (int)character.Value[0];
        characterData.damage = (int)character.Value[1];
        characterData.offset.y = character.Value[2];
        cards.Add(newCard);
    }

    private void PositionCards()
    {
        Vector3 position;
        float angle = 180.0f / characters.Count;
        for (int i = 0; i < characters.Count; i++)
        {
            float x;
            float z;
            float y;
            //Coordinates
            x = radius * Mathf.Cos(angle * i);
            z = radius * Mathf.Abs(Mathf.Sin(angle * i));
            y = playerTransform.position.y;

            //Offsets
            z += distance;
            y += height;

            //Create the vector position
            position = new Vector3(x, y, z);

            //Set the rotation
            cards[i].transform.LookAt(playerTransform);

            //Set the position generated
            cards[i].transform.position = position;
        }
    }
}