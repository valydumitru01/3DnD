using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelectable
{
    public void Execute();
    public void Select();
    public void Unselect();
}

