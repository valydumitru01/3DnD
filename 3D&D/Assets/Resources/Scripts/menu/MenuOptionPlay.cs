using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionPlay : MenuOption
{
    public GameObject ArenaPanel;
    public GameObject InitialMenu;
    private void Start()
    {
        ArenaPanel.SetActive(false);
    }
    public override void Execute()
    {
        ArenaPanel.SetActive(true);
        InitialMenu.SetActive(false);
    }
}
