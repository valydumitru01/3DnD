using UnityEngine;

public class CardCharacter : MonoBehaviour
{
    public string cardName;
    public int lifes;
    public int damage;
    public int manaCost;
    private GameObject character;
    public Vector3 offset;

    public int MaxMovementDistance { get; set; }
    public int MinAttackDistance { get; set; }
    public int MaxAttackDistance { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        SetTexts();
        SetSprite();
        SetPrefab();
    }

    private void SetTexts()
    {
        var texts = gameObject.GetComponentsInChildren<TextMesh>();
        texts[0].text = cardName;
        texts[1].text = lifes.ToString();
        texts[2].text = damage.ToString();
    }

    private void SetSprite()
    {
        var cardPath = $"Images/Cards/carta_front_{cardName.ToLower()}";
        var cardFront = gameObject.GetComponentInChildren<SpriteRenderer>();
        cardFront.sprite = Resources.Load<Sprite>(cardPath);
    }

    private void SetPrefab()
    {
        var prefabPath = $"Characters/Prefabs/{cardName}/{cardName}";
        character = Resources.Load<GameObject>(prefabPath);
    }

    public void InvocateMinion(Tile tile, int Player)
    {
        // El Tile tiene siempre 2 hijos que son los controladores de particulas
        if (character != null && tile.transform.childCount < 3)
        {
            character.tag = cardName;
            character.transform.localPosition = offset;
            if (Player == 1)
                character.transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                character.transform.rotation = Quaternion.Euler(0, 180, 0);
            character.transform.localScale = new Vector3(3f, 3f, 3f);

            MinionCharacter minionCharacter = character.GetComponent<MinionCharacter>();
            minionCharacter.cardName = cardName;
            minionCharacter.lifes = lifes;
            minionCharacter.damage = damage;
            minionCharacter.manaCost = manaCost;
            minionCharacter.MaxMovementDistance = MaxMovementDistance;
            minionCharacter.MinAttackDistance = MinAttackDistance;
            minionCharacter.MaxAttackDistance = MaxAttackDistance;
            minionCharacter.tile = tile;

            Outline outline = character.GetComponent<Outline>();
            // outline.OutlineColor = Color.blue;
            outline.OutlineColor = Color.red;

            Instantiate(character, tile.transform);
        }
    }
}
