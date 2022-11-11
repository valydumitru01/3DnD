
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
    void Start()
    {
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
        Vector3 dir = (goTowards.transform.position - transform.position);
        if (Vector3.Distance(goTowards.transform.position, transform.position) > 0.05)
        {
            transform.Translate(dir * speed * 0.5f * Time.deltaTime);
            vcam.m_LookAt=sitPosition.transform;
        }
        else
            ChangeScene();
    }
    private void ChangeScene()
    {
        SceneManager.LoadScene("3DnD Playable");
    }

    public void deactivate(){
        this.activated=false;
    }
}