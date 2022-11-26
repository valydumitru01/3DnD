using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionQuit : MonoBehaviour, MenuOption
{
    public void Execute()
    {
        Application.Quit();
    }
}
