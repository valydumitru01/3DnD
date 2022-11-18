using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string cardName;
    public string lifes;
    public string damage;
    public GameObject character;
    public Vector3 offset;
    private TextMesh[] texts;

    // Start is called before the first frame update
    void Start()
    {
        texts = gameObject.GetComponentsInChildren<TextMesh>();
        texts[0].text = cardName;
        texts[1].text = lifes;
        texts[2].text = damage;
    }

    public void InvocateMinion(Transform transform)
    {
        if (character != null && transform.childCount < 1)
        {
            character.tag = cardName;
            character.transform.position = transform.position + offset;
            Instantiate(character, transform);
        }
    }
}
