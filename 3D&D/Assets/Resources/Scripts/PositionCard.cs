using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PositionCard : MonoBehaviour
{
    private IEnumerable<CardGazeInput> cardsInput;
    //TIMER
    public float timerDuration = 3f;
    private float lookTimer = 0f;
    public bool IsLooked { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        cardsInput = GameObject.FindGameObjectsWithTag("Card")
                           .Select(card => card.GetComponent<CardGazeInput>());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLooked)
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
        IEnumerable<CardGazeInput> selectedCard = cardsInput.Where(card => card.IsSelected && card.gameObject.activeSelf);
        if (selectedCard.Count() > 0)
        {
            StartCoroutine(UseCard(selectedCard.First(), 1));
        }
    }

    public void setIsLooked(bool looked)
    {
        IsLooked = looked;
    }

    IEnumerator UseCard(CardGazeInput selectedCard, float time)
    {
        selectedCard.CanBeFocused = false;
        while (selectedCard.transform.position != transform.position)
        {
            selectedCard.transform.position = Vector3.MoveTowards(selectedCard.transform.position, transform.position, 15 * Time.deltaTime);
            selectedCard.transform.rotation = Quaternion.Lerp(selectedCard.transform.rotation, Quaternion.Euler(90, 0, 0), 10 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(time);
        selectedCard.gameObject.SetActive(false);
        invocateMinion(transform.position);
    }

    private void invocateMinion(Vector3 position)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = position + new Vector3(0f, 0.2f, 0f);
        sphere.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        Color brown = new Color(139f/255f, 69f/255f, 19f/255f, 1f);
        sphere.GetComponent<Renderer>().material.color = brown;
    }
}