using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionQuit : MonoBehaviour, SelectableMenuOption
{
    public void Execute()
    {
        Application.Quit();
    }
}
