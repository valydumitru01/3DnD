using UnityEngine;

public class CardCharacter : MonoBehaviour
{
    public string cardName;
    public int health;
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
        texts[1].text = health.ToString();
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

    public bool InvocateMinion(Tile tile, int Player)
    {
        // El Tile tiene siempre 3 hijos que son los controladores de particulas
        if (character != null && tile.transform.childCount < 4)
        {
            character.tag = cardName;
            character.transform.localPosition = offset;

            Outline outline = character.GetComponent<Outline>();
            if (Player == 1)
            {
                character.transform.rotation = Quaternion.Euler(0, 0, 0);
                outline.OutlineColor = Color.blue;
            }
            else
            {
                character.transform.rotation = Quaternion.Euler(0, 180, 0);
                outline.OutlineColor = Color.red;
            }

            character.transform.localScale = new Vector3(3f, 3f, 3f);

            MinionCharacter minionCharacter = character.GetComponent<MinionCharacter>();
            minionCharacter.cardName = cardName;
            minionCharacter.maxHealth = health;
            minionCharacter.damage = damage;
            minionCharacter.manaCost = manaCost;
            minionCharacter.MaxMovementDistance = MaxMovementDistance;
            minionCharacter.MinAttackDistance = MinAttackDistance;
            minionCharacter.MaxAttackDistance = MaxAttackDistance;
            minionCharacter.tile = tile;

            minionCharacter.LoadSoundEffects();
            minionCharacter.PlaySummon();

            Instantiate(character, tile.transform);

            return true;
        }
        return false;
    }
}
