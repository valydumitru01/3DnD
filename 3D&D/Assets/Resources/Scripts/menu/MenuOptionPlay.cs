using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptionPlay : MonoBehaviour,MenuOption
{
    public void Execute()
    {
        GameObject orb=GameObject.FindGameObjectWithTag("Orbit");
        orb.GetComponent<AutoRotate>().deactivate();
    }
}
