using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionPlay : MonoBehaviour, SelectableMenuOption
{
    public void Execute()
    {
        ShowArenaPanel();
        StartCoroutine(ChangeMusic());
        SitOnChair();
    }

    // every 2 seconds perform the print()
    private void SitOnChair()
    {
    }
    private void ShowArenaPanel()
    {
        GameObject.FindGameObjectWithTag("ArenaPanel").SetActive(true);
    }
    
}
