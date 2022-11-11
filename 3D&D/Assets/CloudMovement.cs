using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float speed=10;
    public float amplitude=10;

    private Vector3 initTransform;
    
    private float ticks;
    // Start is called before the first frame update
    void Start()
    {
        initTransform=transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ticks+=Time.deltaTime*speed;
        transform.position=initTransform+Vector3.forward*Mathf.Sin(ticks)*amplitude/1000;
    }
}
