using UnityEngine;

public class CardData : MonoBehaviour
{
    public string cardName;
    public string lifes;
    public string damage;
    private TMPro.TextMeshPro[] texts;

    // Start is called before the first frame update
    void Start()
    {
        texts = gameObject.GetComponentsInChildren<TMPro.TextMeshPro>();
        texts[0].text = cardName;
        texts[1].text = lifes;
        texts[2].text = damage;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
