using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlashingEffect : MonoBehaviour
{
    [SerializeField] private float _flickerDelay =1f;
    [SerializeField] private CanvasGroup _flickerGroup;
    [SerializeField] private float _duration;
    private bool _isFlickering;
    public int NoOfFlickers { get; set; }


//    private void Update(){
  //      if(_flickerGroup)
    //        StartFlicker(noOfFlickers);
      //  else
        //    enabled = false;
  //  }

    private IEnumerator ContinuousFlickering(){

          SoundManager.PlayPingSound();
            _flickerGroup.alpha =1f;
            yield return new WaitForSecondsRealtime(_flickerDelay);
            _flickerGroup.alpha =0f;  
            yield return new WaitForSecondsRealtime(_flickerDelay);
        
        _flickerGroup.alpha =1f; 
    }


    public void StopFlicker(){
      _flickerGroup.alpha =0f;  
      _isFlickering = false;
    }
    public void StartFlicker(){
        _isFlickering = true;
        StartCoroutine(ContinuousFlickering());
    }

}
