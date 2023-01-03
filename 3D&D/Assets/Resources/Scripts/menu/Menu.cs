using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    int menuItemCount;
    int index=0;
    SelectableMenuOption[] selectableOptions;
    
    void Start()
    {
        selectableOptions=GetComponentsInChildren<SelectableMenuOption>();
        menuItemCount=selectableOptions.Length;
    }
    [System.Obsolete]
    void Update()
    {
        float axis=Input.GetAxis("Horizontal");
        if (axis>0) index++;
        else if(axis < 0) index--;
        int selectedItem=index%menuItemCount;
        Debug.Log(selectedItem);
        transform.GetChild(selectedItem).gameObject.GetComponent<ParticleSystem>().enableEmission=true;
        for (int i = 0; i < menuItemCount; i++)
        {  
            if(i!=selectedItem)
                transform.GetChild(i).gameObject.GetComponent<ParticleSystem>().enableEmission=false;
        }
        if(Input.GetAxis("Fire1")!=0){

            selectOption(Mathf.Abs(selectedItem));
        }
    }
    //IMPORTANT: for this to work the options need to be in the same order from right to left as the children
    //of UI object from up to down
    void selectOption(int i){
        Transform menuTransform=transform.GetChild(i);
        menuTransform.gameObject.GetComponent<SelectableMenuOption>().Execute();
        selectableOptions=GetComponentsInChildren<SelectableMenuOption>();
    }
    
}
