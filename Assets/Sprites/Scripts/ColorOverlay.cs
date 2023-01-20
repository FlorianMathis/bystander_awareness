using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorOverlay : MonoBehaviour
{
    Shader originalShader;
    Shader shader;
    public bool Blink { get; set; }
    
    void Start(){
        Blink = false;
        originalShader = Shader.Find("Standard");
        shader = Shader.Find("Example/Tint Final Color");

        
    }

    public void StartOverlay(){
        Debug.Log("Started Overlay");
        Blink = true;
        StartCoroutine(Execute());
    }

    public void StopOverlay(){
        Debug.Log("Stoped Overlay");
        Blink = false;
        Material[] materials = GetComponent<Renderer>().materials;
        foreach (Material material in materials)
            {  
                material.shader = originalShader;
            }
    }

    private IEnumerator Execute(){
        Debug.Log("Started Overlay Coroutine");
        yield return new WaitForSecondsRealtime(1);

        
        Material[] materials = GetComponent<Renderer>().materials;

        while(Blink){
            foreach (Material material in materials)
            {  
                material.shader = shader;
            }
            
            yield return new WaitForSecondsRealtime(1);
            foreach (Material material in materials)
            {  
                material.shader = originalShader;
            }
            yield return new WaitForSecondsRealtime(1);
        }



    }

}