using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitDownPosition : MonoBehaviour
{
    public bool occupied=false;
    private GameObject cam;
    public void setCamera(GameObject camera)
    {
        this.cam = camera;
    }
    public GameObject getCamera()
    {
        return cam;
    }
}
