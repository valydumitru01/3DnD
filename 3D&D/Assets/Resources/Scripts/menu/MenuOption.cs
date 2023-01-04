using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MenuOption : MonoBehaviour,ISelectable
{
    public abstract void Execute();

    [System.Obsolete]
    public void Select()
    {
        GetComponent<ParticleSystem>().enableEmission = true;
    }
    [System.Obsolete]
    public void Unselect()
    {
        GetComponent<ParticleSystem>().enableEmission = false;
    }

    void ISelectable.Execute()
    {
        this.Execute();
    }
}
