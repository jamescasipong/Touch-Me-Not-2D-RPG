using UnityEngine;

public class TargetIndicator : MonoBehaviour
{

    public float hideDistance = 5f;
    public int arrayTarget;

    [SerializeField]WhichObjectstoFind objectsToFind;
    
    // Update is called once per frame
    void Update()
    {
        var targetIndicator = TargetObjectsInstance.instance;
        var targetIndicatorS1House = TargetObjectsS1House.instance;
        var convent = TargetObjectsConvent.instance;

        if (objectsToFind == WhichObjectstoFind.Kabanaata1)
        {
            var dir = targetIndicator.targetNPCS[arrayTarget].position - transform.position;

            if (dir.magnitude < hideDistance)
            {
                //Debug.Log("this one");
                SetChildren(false);
            }
            else
            {

                SetChildren(true);



                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            
        }
        else if (objectsToFind == WhichObjectstoFind.GoToHouse)
        {
            var dir = targetIndicator.targetKapitanHouse[arrayTarget].position - transform.position;

            if (dir.magnitude < hideDistance)
            {
                SetChildren(false);
            }
            else
            {
                SetChildren(true);

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }


        }
        else if (objectsToFind == WhichObjectstoFind.ChefQuest)
        {
            var dir = targetIndicator.targetShop[arrayTarget].position - transform.position;

            if (dir.magnitude < hideDistance)
            {
                SetChildren(false);
            }
            else
            {
                SetChildren(true);

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

        }
        else if (objectsToFind == WhichObjectstoFind.GetGlass)
        {
            var dir = targetIndicatorS1House.targetGlass[arrayTarget].position - transform.position;

            if (dir.magnitude < hideDistance)
            {
                SetChildren(false);
            }
            else
            {
                SetChildren(true);

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

        }
        else if (objectsToFind == WhichObjectstoFind.GoToHotel)
        {
            var dir = targetIndicator.targetHotel[arrayTarget].position - transform.position;

            if (dir.magnitude < hideDistance)
            {
                SetChildren(false);
            }
            else
            {
                SetChildren(true);

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }


        }
        else if (objectsToFind == WhichObjectstoFind.Convent)
        {
            var dir = convent.targetConvent[arrayTarget].position - transform.position;

            if (dir.magnitude < hideDistance)
            {
                SetChildren(false);
            }
            else
            {
                SetChildren(true);

                var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }


        }
    }

    void SetChildren(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }
}


enum WhichObjectstoFind
{
    Kabanaata1,
    GoToHouse,
    GetGlass,
    ChefQuest,
    GoToHotel,
    Convent
}