using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    int menuItemCount;

    int indexCounter=0;
    int index=0;
    float menuChangeInput;

    bool waitChangeSelectedEnded=true;
    float timeBetweenChanges = 0.0f;

    bool waitExecuteEnded = true;

    int selected=0;
    int lastSelected=0;
    MenuOption[] selectableOptions;

    [System.Obsolete]
    void Start()
    {
        selectableOptions=GetComponentsInChildren<MenuOption>();
        menuItemCount =selectableOptions.Length;
        ClearSelected();
    }
    [System.Obsolete]
    void Update()
    {

        menuChangeInput = Input.GetAxis("Horizontal");
        if(menuChangeInput != 0 && waitChangeSelectedEnded) {
            waitChangeSelectedEnded = false;
            Invoke(nameof(ChangeSelected), timeBetweenChanges);
        }
        else
        {
            timeBetweenChanges=0f;
        }
        lastSelected = selected;
        selected = index%menuItemCount;
        if (selected != lastSelected)
        {
            selectableOptions[selected].Select();
            selectableOptions[lastSelected].Unselect();
        }

        if (Input.GetAxis("Fire1")!=0 && waitExecuteEnded)
        {
            waitExecuteEnded = false;
            Invoke(nameof(WaitExecute), 0.3f);
            Execute();
        }

    }
    [System.Obsolete]
    private void ClearSelected()
    {
        //Clear selected to all selectable
        foreach (var option in selectableOptions)
        {
            option.Unselect();
        }
        //Select first
        selectableOptions[0].Select();
    }
    private void ChangeSelected()
    {
        if (menuChangeInput > 0) indexCounter++;
        else if (menuChangeInput < 0) indexCounter--;
        index=Mathf.Abs(indexCounter);
        timeBetweenChanges = 0.3f;
        waitChangeSelectedEnded = true;
    }
    [System.Obsolete]
    void Execute()
    {
        selectableOptions[selected].Execute();
        selectableOptions = GetComponentsInChildren<MenuOption>();
        menuItemCount = selectableOptions.Length;
        selected = 0;
        lastSelected = 0;
        ClearSelected();
    }
    void WaitExecute()
    {
        waitExecuteEnded = true;
    }

}
