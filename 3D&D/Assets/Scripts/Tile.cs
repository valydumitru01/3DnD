using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool IsSelectable = true;
    public int Row { get; set; }
    public int Col { get; set; }

    private new ParticleSystem particleSystem;
    private GameObject unit;

    private GameController gameController;

    private bool isLooked;

    // Start is called before the first frame update
    void Awake()
    {
        particleSystem = gameObject.GetComponent<ParticleSystem>();
        particleSystem.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLooked)
        {
            particleSystem.Play();
        }
        else
        {
            particleSystem.Stop();
        }
    }

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void SetIsLooked(bool value)
    {
        if (IsSelectable)
            isLooked = value;
    }

    // TODO Esto es para testing, en el gameplay se arrastara la carta de ataque o movimiento a la casilla
    public void OnPointerClick()
    {
        // Si la casilla tiene una unidad y esta es aliada
        // TODO Check if unit is ally
        // if (unit != null)
        if (IsSelectable)
        {
            if (gameController.IsMoving)
            {
                gameController.PerformMove(this);
                Debug.Log(string.Format("Ended moving/attacking in tile {0},{1}", Row, Col));
            }
            else if (gameController.IsAttacking)
            {
                gameController.PerformAttack(this);
            }
            else
            {
                // TODO CAMBIAR START ATTACK /MOVE PARA TESTING
                gameController.StartAttack(this);
                isLooked = true;
                Debug.Log(string.Format("Started moving/attacking from tile {0},{1}", Row, Col));
            }
        }
    }
}
