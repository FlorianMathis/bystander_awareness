using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name : MonoBehaviour
{
    public GameObject oculus;
    // Start is called before the first frame update
    void Start()
    {
      oculus.name = "Oculus";
      oculus.tag = "Oculus";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
