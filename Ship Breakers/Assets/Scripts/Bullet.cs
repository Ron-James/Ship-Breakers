using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    bool isStopped;
    [SerializeField] float speed = 5f;
    Vector3 target;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Collider coll;
    [SerializeField] Transform inactives;
    [SerializeField] Transform actives;
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
            case "Out":
                Deactivate();
                break;
        }
    }
    public void Activate(Vector3 position, Vector3 direction, float Speed)
    {
        speed = Speed;
        transform.SetParent(actives);
        meshRenderer.enabled = true;
        coll.enabled = true;
        transform.position = position;
        target = position + direction * 1000;
        //target.x += 1000;
        transform.LookAt(target, Vector3.up);
        isStopped = false;
    }

    public void Deactivate()
    {
        transform.SetParent(inactives);
        isStopped = true;
        meshRenderer.enabled = false;
        coll.enabled = false;
        target = Vector3.zero;
    }
}
