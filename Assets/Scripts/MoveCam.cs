using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCam : MonoBehaviour
{
    public GameObject target;

    public float lerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(target != null)
        {
            //transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * lerpSpeed);     
            transform.position = Vector3.Lerp(transform.position, 
                new Vector3(transform.position.x, 0f, target.transform.position.z),
                Time.deltaTime * lerpSpeed);     
        }
    }
}
