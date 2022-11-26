using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateAround : MonoBehaviour
{
    public GameObject generated;
    public int amount = 5;
    public float radius = 5f;
    public float range = 5f;
    public float distance = -5f;
    public float height = -5f;
    private List<GameObject> cards = new List<GameObject>();
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = this.GetComponentInParent<Transform>();
        for (int i = 0; i < amount; i++)
        {
            GameObject newObj = Instantiate(generated, 
            Vector3.zero, 
            Quaternion.identity, 
            this.transform);
            cards.Add(newObj);
        }

    }
    private void Update()
    {
        Vector3 position;
        float angle = 180.0f / amount;
        for (int i = 0; i < amount; i++)
        {
            float x;
            float z;
            float y;
            //Coordinates
            x = playerTransform.position.x + radius * Mathf.Cos(angle * i);
            z = playerTransform.position.z + radius * Mathf.Abs(Mathf.Sin(angle * i));
            y = playerTransform.position.y;

            //Offsets
            z += distance;
            y += height;

            //Create the vector position
            position = new Vector3(x, y, z);

            //Set the rotation
            cards[i].transform.LookAt(playerTransform);

            //Set the position generated
            cards[i].transform.position = position;
        }
    }

}
