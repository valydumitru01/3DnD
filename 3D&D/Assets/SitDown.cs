using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SitDown : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private GameObject sitPosition;
    public bool activated;
    private bool once = true;
    // Start is called before the first frame update
    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
        {


        }
    }

    // Update is called once per frame
    void Update()
    {

        if (activated)
        {
            if (once)
            {
                vcam.AddCinemachineComponent<CinemachineTransposer>();
                once = false;
                
            }
            transform.Translate(sitPosition.transform.position);
        }else{
            if(!once){
                vcam.AddCinemachineComponent<CinemachineOrbitalTransposer>();
                once=true;
            }
        }

    }
}
