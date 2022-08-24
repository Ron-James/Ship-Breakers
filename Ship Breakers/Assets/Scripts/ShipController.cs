using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] float maxZ = 8f;
    [SerializeField] float acc = 20f;

    [Header("Engine Power")]
    [SerializeField] float minVelocity = 2f;
    [SerializeField] float maxVelocity = 7f;
    [SerializeField] UpgradeBar enginePower;
    float velocity;

    [Header("Engine Handling")]
    [SerializeField] float minHandling = 0.7f;
    [SerializeField] float maxHandling = 0;
    [SerializeField] UpgradeBar engineHandling;
    float handling;

    
    
    

    Coroutine reduceVelocity = null;
    bool moving;
    float zInput;
    Rigidbody rb;



    // Start is called before the first frame update
    void Start()
    {
        UpdateVelocity();
        UpdateDrag();
        rb = GetComponent<Rigidbody>();
        reduceVelocity = null;
        moving = false;
    }

    // Update is called once per frame
    void Update()
    {
        zInput = Input.GetAxisRaw("Vertical");
        if(Mathf.Abs(zInput) > 0){
            //rb.velocity = Vector3.forward * zInput * speed;
            if(rb.velocity.magnitude < velocity){
                rb.AddForce(zInput * Vector3.forward * acc ,ForceMode.Acceleration);
            }
            else{
                ChangeVelocity(velocity);
            }
            
        }
        

        if(zInput == 0 && !rb.velocity.Equals(Vector3.zero) && reduceVelocity == null){
            reduceVelocity = StartCoroutine(ReduceVelocity(handling));
        }
    }

    public void UpdateDrag(){
        float differnce = minHandling - maxHandling;
        handling = minHandling - (differnce * engineHandling.Percentage());
    }

    public void UpdateVelocity(){
        float diffrence = maxVelocity - minVelocity;
        velocity = minVelocity + (diffrence * enginePower.Percentage());
    }

    IEnumerator ReduceVelocity(float period){
        float time = 0;
        float rate = rb.velocity.magnitude / (period / Time.deltaTime);
        Debug.Log("Reduce velocity");
        while(true){
            time += Time.deltaTime;
            if(Mathf.Abs(zInput) > 0){
                reduceVelocity = null;
                break;
            }
            if(time >= period || rb.velocity.Equals(Vector3.zero)){
                Debug.Log("Reduce velocity");
                rb.velocity = Vector3.zero;
                reduceVelocity = null;
                break;
            }
            else{
                float velocity = rb.velocity.magnitude;
                ChangeVelocity(velocity - rate);
                yield return null;
            }
        }
    }

    public void ChangeVelocity(float magnitude){
        Vector3 direction = rb.velocity.normalized;
        rb.velocity = direction * magnitude;
    }


}
