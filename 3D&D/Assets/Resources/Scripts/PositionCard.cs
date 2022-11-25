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
        int row = gameObject.name[5] - '0';
        if (selectedCard.Count() > 0 && transform.childCount < 1 && row < 3)
        {
            StartCoroutine(UseCard(selectedCard.First(), 0.5f));
        }
    }

    public void setIsLooked(bool looked)
    {
        IsLooked = looked;
    }

    IEnumerator UseCard(CardGazeInput selectedCard, float time)
    {
        selectedCard.CanBeFocused = false;
        var objective = transform.position;
        objective.y += 0.01f;
        while (selectedCard.transform.position != objective)
        {
            selectedCard.transform.position = Vector3.MoveTowards(selectedCard.transform.position, objective, 3 * Time.deltaTime);
            selectedCard.transform.rotation = Quaternion.Lerp(selectedCard.transform.rotation, Quaternion.Euler(90, 180, 0), 10 * Time.deltaTime);
            yield return null;
        }
        yield return new WaitForSeconds(time);
        selectedCard.gameObject.SetActive(false);
        selectedCard.InvocateMinion(transform);
    }
}