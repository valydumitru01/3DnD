using UnityEngine;

///--------------------------------
///   Author: Victor Alvarez, Ph.D.
///   University of Oviedo, Spain
///--------------------------------

public class CardGazeInput : MonoBehaviour
{
    private Vector3 initialPosition;

    //TIMER
    public float timerDuration = 3f;
    private float lookTimer = 0f;

    public bool IsClicked { get; set; }
    public bool IsLooked { get; set; }
    public bool CanBeFocused { get; set; }

    void Start()
    {
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        initialPosition = transform.position;
    }

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

    public void setIsLooked(bool looked)
    {
        if (CanBeFocused)
        {
            IsLooked = looked;
            MoveCard(IsLooked);
        }
    }

    public void OnPointerClick()
    {
        if (CanBeFocused)
        {
            if (IsClicked)
            {
                transform.position = initialPosition;
                IsClicked = false;
            }
            else
            {
                transform.position += new Vector3(0, 0.5f, -2.5f);
                IsClicked = true;
            }
        }
    }

    private void MoveCard(bool isLooked)
    {
        if (!IsClicked)
        {
            if (isLooked)
                transform.position += new Vector3(0, 0.25f, 0);
            else
                transform.position = initialPosition;
        }
    }

}