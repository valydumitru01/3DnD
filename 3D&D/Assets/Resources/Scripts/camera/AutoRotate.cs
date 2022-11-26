
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class AutoRotate : MonoBehaviour
{
    public float speed = 10f;
    public GameObject goTowards;
    private CinemachineOrbitalTransposer m_orbital;
    private CinemachineVirtualCamera vcam;
    public bool activated = true;
    private bool once = true;
    private GameObject sitPosition;
    public GameObject lookAt;
    public GameObject UI;
    
        
    void Start()
    {
       
        UI = GameObject.FindGameObjectWithTag("UI");
        vcam = GetComponent<CinemachineVirtualCamera>();
        if (vcam != null)
        {
            CinemachineCore.GetInputAxis = GetAxisCustom;
            m_orbital = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }

    }
    private float GetAxisCustom(string axisName)
    {
        return 0;
    }
    void LateUpdate()
    {
        this.transform.LookAt(lookAt.transform);
        if (activated)
        {
            m_orbital.m_XAxis.Value += Time.deltaTime * speed;
        }
        else
        {
            dectivateOrbitalTransposer();
            goSitDown();
        }

    }

    private bool orbitalActivated = true;
    private void activateOrbitalTransposer()
    {
        if (!orbitalActivated)
        {
            vcam.AddCinemachineComponent<CinemachineOrbitalTransposer>();


            orbitalActivated = true;

        }
    }

    private bool orbitalDectivated = false;
    private void dectivateOrbitalTransposer()
    {
        if (!orbitalDectivated)
        {
            vcam.DestroyCinemachineComponent<CinemachineOrbitalTransposer>();
            orbitalDectivated = true;

        }
    }
    private void goSitDown()
    {
        //Not normalized, the further the faster
        Vector3 dir = (goTowards.transform.position - transform.position);
        if (Vector3.Distance(goTowards.transform.position, transform.position) > 0.05)
        {
            transform.Translate(dir * speed * 0.5f * Time.deltaTime);
            if(sitPosition!=null)
                vcam.m_LookAt=sitPosition.transform;
        }
        else ChangeCameraState();
        
    }
    
    //Enable control over the view
    private void ChangeCameraState()
    {
        CinemachineBrain cinemachineBrain=this.GetComponentInChildren<CinemachineBrain>();
        CinemachineVirtualCamera cinemachineVirtualCamera=this.GetComponent<CinemachineVirtualCamera>();
        
        cinemachineVirtualCamera.enabled=false;
        cinemachineBrain.enabled=false;
        transform.position=goTowards.transform.position;
        this.gameObject.GetComponent<Transform>().rotation=Quaternion.identity;
        UI.SetActive(false);
        this.enabled=false;
    }

    public void deactivate(){
        this.activated=false;
    }
}