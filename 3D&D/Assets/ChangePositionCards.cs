using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePositionCards : MonoBehaviour
{
    private enum positionEnum
    {
        defaultPos,
        otherPos
    }
    private Vector3 defaultPosition;

    public GameObject other;
    private Vector3 otherPosition;

    private positionEnum position = positionEnum.defaultPos;
    
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = transform.position;

        otherPosition = other.transform.position;
    }
    public void changePosition()
    {
        if(position == positionEnum.defaultPos)
        {
            position=positionEnum.otherPos;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            transform.position= otherPosition;
        }
        else
        {
            position = positionEnum.defaultPos;
            transform.rotation= new Quaternion(0, 180, 0,0);
            transform.position = defaultPosition;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
