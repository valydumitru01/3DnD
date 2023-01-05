using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaPanel : MonoBehaviour
{
    public OptionMatchFound matchFound;
    private GameObject option;
    private int NUMBER_FOUND_MATCHES = 3;
    // Start is called before the first frame update
    void Start()
    {
        matchFound = GetComponentInChildren<OptionMatchFound>();
        option = matchFound.gameObject;
        foreach (Component comp in option.GetComponents<Component>())
        {
            Debug.Log(comp.name);
        }

        GenerateFoundMatches();

    }
    private void GenerateFoundMatches()
    {
        for(int i = 0; i < NUMBER_FOUND_MATCHES; i++)
        {
            RectTransform oldTransform = option.GetComponent<RectTransform>();
            GameObject copy=Instantiate(option);
            copy.GetComponent<RectTransform>().SetParent(gameObject.transform);
            copy.GetComponent<RectTransform>().localScale= new Vector3(0.5f, 0.5f, 0.02f);
            copy.GetComponent<RectTransform>().localPosition = new Vector3(2, 2-i, 0);
            copy.GetComponent<RectTransform>().rotation= new Quaternion(oldTransform.rotation.x, oldTransform.rotation.y, oldTransform.rotation.z, oldTransform.rotation.w);

            copy.GetComponent<ParticleSystem>().enableEmission = false;
            copy.GetComponentInChildren<TMPro.TextMeshPro>().text = "ID: "+ (uint)Random.Range(0, 100);

            Debug.Log(copy.GetComponent<RectTransform>().localScale);

        }
        option.active = false;
        GetComponentInParent<Menu>().UpdateSelectables();
    }
}
