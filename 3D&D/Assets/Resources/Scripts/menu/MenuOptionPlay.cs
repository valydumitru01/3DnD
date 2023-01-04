using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionPlay : MenuOption
{
    GameObject ArenaPanel;
    GameObject InitialMenu;
    private void Start()
    {
        ArenaPanel = GameObject.FindGameObjectWithTag("ArenaPanel");
        InitialMenu = GameObject.FindGameObjectWithTag("InitialMenu");
        ArenaPanel.SetActive(false);
    }
    public override void Execute()
    {
        ArenaPanel.SetActive(true);
        InitialMenu.SetActive(false);
    }
}
