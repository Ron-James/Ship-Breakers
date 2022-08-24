using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CollectableType
{
    upgrade = 0,
    ammo = 1,
    health = 2
}
public class Collectable : MonoBehaviour
{
    bool isStopped;
    Vector3 target;
    [SerializeField] CollectableType type;
    [SerializeField] float speed = 1f;
    [SerializeField] Transform actives;
    [SerializeField] Transform inactives;
    [SerializeField] Material[] materials;

    // Start is called before the first frame update
    void Start()
    {
        Deactivate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStopped)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "End":
                Deactivate();
                break;
            case "Ship":
                switch ((int)type)
                {
                    case 0:
                        GameManager.instance.AddUpgradePoint(1);
                        break;
                    case 1:
                        GameManager.instance.Ship.GetComponent<Weapon>().AddAmmo(4);
                        break;
                    case 2:
                        GameManager.instance.AddHealth(1);
                        break;
                }

                Deactivate();
                break;
        }
    }

    public void UpdateType()
    {
        GetComponentInChildren<MeshRenderer>().material = materials[(int)type];
    }
    public void Activate(Vector3 position, int Type)
    {

        GetComponentInChildren<Collider>().enabled = true;
        GetComponentInChildren<MeshRenderer>().enabled = true;
        transform.position = position;
        target = position;
        target.x -= 1000;
        transform.SetParent(actives);
        isStopped = false;
        if (Type > 2 || Type < 0)
        {
            type = (CollectableType)0;
        }
        else
        {
            type = (CollectableType)Type;
        }
        UpdateType();
    }

    public void Deactivate()
    {
        GetComponentInChildren<Collider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        transform.position = Vector3.zero;
        target = Vector3.zero;
        isStopped = true;
        transform.SetParent(inactives);
    }

    IEnumerator DoEveryXseconds(float x, float duration){
        float overallTime = 0;
        float repeatTime = 0;

        while(true){
            if(overallTime >= duration){
                break;//end here
            }
            else if(repeatTime >= x){
                //run your code here
                repeatTime = 0;
                overallTime += Time.deltaTime;
                yield return null;
            }
            else{
                repeatTime += Time.deltaTime;
                overallTime += Time.deltaTime;
                yield return null;
            }
        }
    }


}
