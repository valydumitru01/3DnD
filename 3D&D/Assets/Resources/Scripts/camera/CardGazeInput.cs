using System;
using System.Collections;
using UnityEngine;

///--------------------------------
///   Author: Victor Alvarez, Ph.D.
///   University of Oviedo, Spain
///--------------------------------

public class CardGazeInput : MonoBehaviour
{
    private Vector3 initialPosition;
    private Character character;
    //TIMER
    public float timerDuration = 3f;
    private float lookTimer = 0f;

    public bool IsSelected { get; set; }
    public bool IsLooked { get; set; }
    public bool CanBeFocused { get; set; }
    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }

    void Start()
    {
        // Disable screen dimming
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // InitialPosition = transform.localPosition;
        character = GetComponent<Character>();
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

    public void SetIsLooked(bool looked)
    {
        if (CanBeFocused)
        {
            IsLooked = looked;
            JumpCard(IsLooked);
        }
    }

    public void OnPointerClick()
    {
        if (CanBeFocused)
        {
            if (IsSelected)
            {
                StartCoroutine(Move(InitialPosition));
                IsSelected = false;
            }
            else
            {
                var endPosition = transform.localPosition + new Vector3(0, 1.7f, 1.5f);
                StartCoroutine(Move(endPosition));
                IsSelected = true;
            }
        }
    }

    private void JumpCard(bool isLooked)
    {
        if (!IsSelected)
        {
            if (isLooked)
            {
                var endPosition = transform.localPosition + new Vector3(0, 0.25f, 0);
                StartCoroutine(Move(endPosition));
            }
            else
            {
                StartCoroutine(Move(InitialPosition));
            }
        }
    }
    public void InvocateMinion(Transform transform)
    {
        character.InvocateMinion(transform);
    }

    private IEnumerator Move(Vector3 endPosition)
    {
        while (transform.localPosition != endPosition)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, endPosition, 5 * Time.deltaTime);
            yield return null;
        }
    }
}