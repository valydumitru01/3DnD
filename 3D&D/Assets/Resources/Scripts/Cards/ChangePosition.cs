using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChangePosition : MonoBehaviour
{

    private bool changingPosition = false;
    private SitDownPosition positionKnight;
    private SitDownDemon positionDemon;
    private ChangePositionCards[] cards;
    public float speed = 10f;
    private void Start()
    {
        this.cards = FindObjectsOfType<ChangePositionCards>();
        positionKnight = GameObject.FindObjectOfType<SitDownPosition>();
        positionDemon = GameObject.FindObjectOfType<SitDownDemon>();
    }
    public void setChangePosition(bool val)
    {
        this.changingPosition = val;
    }
    public void changePosition()
    {
        if (positionKnight.occupied == true)
        {
            goHere(positionKnight, positionDemon.gameObject);
        }
        else
        {
            goHere(positionKnight, positionKnight.gameObject);
        }
    }
    private void goHere(SitDownPosition positionKnight, GameObject where)
    {
        //Not normalized, the further the faster
        Vector3 dir = (where.transform.position - transform.position);
        if (Vector3.Distance(where.transform.position, transform.position) > 0.05)
        {
            transform.Translate(dir * speed * 0.5f * Time.deltaTime);
        }
        else
        {
            if (positionKnight.occupied == true)
                positionKnight.occupied = false;
            else
                positionKnight.occupied = true;
            changingPosition = false;
            foreach (ChangePositionCards card in this.cards)
                card.changePosition();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (changingPosition == true)
        {
            changePosition();
            FindObjectsOfType<MinionCharacter>().ToList().ForEach(minion =>
            {
                //if (minion.player == 2)
                    minion.UpdateInitialPosition();
            });
        }
    }
}
