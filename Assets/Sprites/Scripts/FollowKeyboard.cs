using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FollowKeyboard : MonoBehaviour
{
    public GameObject Keyboard;
    private Renderer renderer;
    private OVRTrackedKeyboard keyboardScript;
    private Transform keyboardTransform;
    public bool flash;
    private bool flashing;
    public SpriteRenderer[] sectors;

    void Start(){
        renderer = gameObject.GetComponent<Renderer>();
        ChangeColor(0f);
        
        keyboardScript = Keyboard.GetComponent<OVRTrackedKeyboard>();
        keyboardTransform = keyboardScript.ActiveKeyboardTransform;
        flash = false;
      //  StartCoroutine(StartFlash());
    }
     void Update()
     {
        if(keyboardScript.GetKeyboardVisibility()){
            transform.position = keyboardTransform.transform.position;
            transform.localEulerAngles= new Vector3(keyboardTransform.localEulerAngles.x-90f,keyboardTransform.localEulerAngles.y,keyboardTransform.localEulerAngles.z+90f) ;
            if(!flashing && flash){
                StartCoroutine(Flash());
                flashing = true;
            }
            //ChangeColor(1f);
        }else{
            flash = false;
//            ChangeColor(0f);
        }
     }

    IEnumerator Flash()
    {
        StartCoroutine(StartSectors());
        while(flash){
            flashing = true;
            for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {     
                if(!flash) break; 
                Color c = renderer.material.color;
                c.a = f;
                renderer.material.color = c;    
                yield return new WaitForSeconds(0.02f);
            }

            for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {  
                if(!flash) break;  
                Color c = renderer.material.color;
                c.a = f;
                renderer.material.color = c;    
                yield return new WaitForSeconds(0.02f);
            }
               
        }
        flashing = false;
        ChangeColor(0f);
    }

    IEnumerator StartSectors()
    {
        System.Random rnd = new System.Random();

        while(flash){
            int i = rnd.Next(21); 
            
            for(float f = 0f ; f <= 0.2f; f+=0.01f)
            {
                Color c = sectors[i].color;
                c.a = f;
                sectors[i].color = c;  
                yield return new WaitForSeconds(0.03f);
            } 
            yield return new WaitForSeconds(1f); 
            for(float f = 0.2f ; f >= -0.01f; f-=0.01f)
            {
                Color c = sectors[i].color;
                c.a = f;
                sectors[i].color = c;  
                yield return new WaitForSeconds(0.03f);
            } 
        }
    }
    
    public void StartSytem(){
        flash  = true;
    }

    public void StopSytem(){
        flash = false;
    }
    IEnumerator StartFlash()
    {
        yield return new WaitForSeconds(10);
        flash = true;
        yield return new WaitForSeconds(20f);
        flash = true;

    }

    private void ChangeColor(float f){
        Color c = renderer.material.color;
        c.a = f;
        renderer.material.color = c;
     }
}
