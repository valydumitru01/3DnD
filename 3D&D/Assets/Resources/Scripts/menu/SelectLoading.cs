using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLoading : MonoBehaviour
{

    [SerializeField][Range(0, 1)] float progress = 0f;


    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().fillAmount = progress;
    }
    private bool once=false;
    public void StartLoading(float timer)
    {
        if (once == false) { 
            progress = 1f;
            once=true;
        }

        progress -= Time.deltaTime * 1 / timer;


    }


    public void stopLoading()
    {
        progress = 0f;
    }
}