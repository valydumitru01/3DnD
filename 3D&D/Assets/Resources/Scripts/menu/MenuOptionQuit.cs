using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionQuit :MenuOption
{
    public override void Execute()
    {
        Application.Quit();
    }
}
