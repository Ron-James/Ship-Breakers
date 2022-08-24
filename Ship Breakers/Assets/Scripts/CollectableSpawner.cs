using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : Singleton<CollectableSpawner>
{
    [SerializeField] Transform inactives;
    [SerializeField] Transform actives;

    Collectable[] collectables;
    // Start is called before the first frame update
    void Start()
    {
        RefreshArray();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshArray(){
        collectables = inactives.GetComponentsInChildren<Collectable>();
    }

    public void SpawnCollectable(Vector3 pos, int type){
        collectables[0].Activate(pos, type);
        RefreshArray();
    }

    public bool IsActiveCollectables(){
        if(actives.GetComponentsInChildren<Collectable>().Length > 0){
            return true;
        }
        else{
            return false;
        }
    }

    public int NumActive(){
        return actives.GetComponentsInChildren<Collectable>().Length;
    }
}
