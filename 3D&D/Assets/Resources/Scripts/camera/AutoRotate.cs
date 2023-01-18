
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class AutoRotate : MonoBehaviour
{
    private GameObject goTowards;
    private GameObject lookAt;
    public float speed = 10f;
    private float rotationWhenSit=0;
    public bool activated = true;

    private GameObject UI;
    private CinemachineOrbitalTransposer m_orbital;
    private CinemachineVirtualCamera vcam;
    private GameObject sitPositionKnight;
    private GameObject sitPositionDemon;
    private SitDownPosition positionKnight;
    private SitDownDemon positionDemon;

    void Start()
    {
        positionKnight = GameObject.FindObjectOfType<SitDownPosition>();
        positionDemon= GameObject.FindObjectOfType<SitDownDemon>();
        positionKnight.occupied = true;
        positionKnight.setCamera(this.gameObject);
        sitPositionDemon = positionDemon.gameObject;
        sitPositionKnight = positionKnight.gameObject;

        goTowards = sitPositionKnight;
        lookAt = sitPositionKnight;
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
        this.gameObject.GetComponent<Transform>().rotation=new Quaternion(
            Quaternion.identity.x,
            Quaternion.identity.y + rotationWhenSit,
            Quaternion.identity.z, Quaternion.identity.w
            );
        UI.SetActive(false);
        this.enabled=false;
    }

    public void deactivate(){
        this.activated=false;
    }
}