using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionGoBack : MenuOption
{
    private GameObject ArenaPanel;
    private GameObject InitialMenu;
    private void Start()
    {
        ArenaPanel = GameObject.FindGameObjectWithTag("ArenaPanel");
        InitialMenu = GameObject.FindGameObjectWithTag("InitialMenu");
        ArenaPanel.SetActive(true);
    }
    public override void Execute()
    {
        ArenaPanel.SetActive(false);
        InitialMenu.SetActive(true);
    }
}
