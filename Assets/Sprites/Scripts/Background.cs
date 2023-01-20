using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Texture[] frames; 
    private Renderer renderer;
    int framesPerSecond = 20;

 void Start(){
    renderer = GetComponent<Renderer>();
 }
 void Update() { 
    int index = (int)(Time.time * framesPerSecond) % frames.Length; 
    renderer.material.mainTexture = frames[index]; 
    }
}
