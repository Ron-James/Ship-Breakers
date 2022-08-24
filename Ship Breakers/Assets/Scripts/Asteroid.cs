using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Asteroid : MonoBehaviour
{
    bool isStopped;
    Vector3 target;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float iniAcc = 0.1f;
    [SerializeField] Transform actives;
    [SerializeField] Transform inactives;
    [SerializeField] int dropRate = 5;


    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        Deactivate();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStopped)
        {
            time += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target, iniAcc + (acceleration * time));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "End":
                Deactivate();
                break;
            case "Bullet":
                int rand = Random.Range(0, dropRate);
                if (GameManager.instance.Ship.GetComponent<Weapon>().Ammo < 2)
                {
                    if (CollectableSpawner.instance.NumActive() < 2)
                    {
                        CollectableSpawner.instance.SpawnCollectable(transform.position, 1);
                    }
                }
                else if (GameManager.instance.currentHealth == 1)
                {
                    if (CollectableSpawner.instance.NumActive() < 2)
                    {
                        CollectableSpawner.instance.SpawnCollectable(transform.position, 2);
                    }
                }
                else if (GameManager.instance.UpgradePoints == 0)
                {
                    if (CollectableSpawner.instance.NumActive() < 2)
                    {
                        CollectableSpawner.instance.SpawnCollectable(transform.position, 0);
                    }
                }

                else
                {
                    
                    int type = Random.Range(0, 3);
                    if(type == 2 && GameManager.instance.currentHealth == GameManager.instance.MaxHealth){
                        type = Random.Range(0, 1);
                    }
                    
                    if (rand == 0)
                    {
                        CollectableSpawner.instance.SpawnCollectable(transform.position, type);
                    }
                }
                Deactivate();
                other.gameObject.GetComponent<Bullet>().Deactivate();
                break;
            case "Ship":
                GameManager.instance.AddHealth(-1);
                Deactivate();
                break;
        }
    }
    public void Activate(Vector3 position, float acc)
    {
        time = 0;
        acceleration = acc;
        GetComponentInChildren<Collider>().enabled = true;
        GetComponentInChildren<MeshRenderer>().enabled = true;
        Vector3 angles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        Quaternion rot = Quaternion.Euler(angles);
        transform.rotation = rot;
        transform.position = position;
        target = position;
        target.x -= 1000;
        transform.SetParent(actives);
        isStopped = false;

    }

    public void Deactivate()
    {
        time = 0;
        GetComponentInChildren<Collider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        transform.position = Vector3.zero;
        target = Vector3.zero;
        isStopped = true;
        transform.SetParent(inactives);
    }


}
