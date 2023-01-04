using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCard : MonoBehaviour
{
    public GameObject minion;
    // Start is called before the first frame update
    void Start()
    {
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
    void Update()
    {

    }
}
