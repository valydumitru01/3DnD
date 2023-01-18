using System.Linq;
using UnityEngine;

public class TurnSkipClock : MonoBehaviour
{
    private GameObject generator;
    public bool isLooked = false;
    public float timerDuration = 1.5f;
    private float lookTimer = 0f;
    private ChangePosition cam;
    // Start is called before the first frame update
    void Start()
    {
        generator = GameObject.FindWithTag("CardGenerator");
        this.cam = FindObjectOfType<ChangePosition>();
        SetEnabledAnimation(false);
        GetComponentInChildren<ParticleSystem>().enableEmission = false;
    }

    public void EnableAnimation()
    {
        isLooked = true;
        SetEnabledAnimation(true);
        GetComponentInChildren<ParticleSystem>().enableEmission = true;


    }
    public void DisableAnimation()
    {
        isLooked = false;
        SetEnabledAnimation(false);
        GetComponentInChildren<ParticleSystem>().enableEmission = false;
    }
    private void SetEnabledAnimation(bool value)
    {
        foreach (Animator anim in GetComponentsInChildren<Animator>())
        {
            anim.enabled = value;
        }
    }
    private void Update()
    {
        if (isLooked)
        {
            lookTimer += Time.deltaTime;
            if (lookTimer > timerDuration)
            {
                lookTimer = 0f;
                OnPointerClick();
            }
        }
        else
        {
            lookTimer = 0f;
        }
    }

    public void OnPointerClick()
    {
        var characters = FindObjectsOfType<MinionCharacter>();
        if (!characters.Any(character => character.cardsInPlace))
        {
            cam.enabled = true;
            cam.setChangePosition(true);
            generator.GetComponent<GenerateAround>().SetRefill(true);
        }
    }
}
