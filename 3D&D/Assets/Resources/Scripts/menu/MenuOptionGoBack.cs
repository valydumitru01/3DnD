using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionGoBack : MenuOption
{
    public GameObject ArenaPanel;
    public GameObject InitialMenu;
    private void Start()
    {
        /*
        ArenaPanel = GameObject.FindGameObjectWithTag("ArenaPanel");
        InitialMenu = GameObject.FindGameObjectWithTag("InitialMenu");
        */
        ArenaPanel.SetActive(true);
        Debug.Log(ArenaPanel.name);
        Debug.Log(InitialMenu.name);
    }
    public override void Execute()
    {
        ArenaPanel.SetActive(false);
        InitialMenu.SetActive(true);
    }
}
