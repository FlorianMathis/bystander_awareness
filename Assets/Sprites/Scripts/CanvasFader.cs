using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    private bool _fadeIn;
    private bool _fadeOut;
    private float secondsToMove;
    private float secondsSoFar;
    private Vector3 VRPosition;
    private Vector3 SRPosition;
    private Vector3 SRScale;
    private Vector3 VRScale;
    private CanvasGroup canvasGroup;
    [SerializeField] private GameManager Manager;
    public bool AsFader;


    // Start is called before the first frame update
    void Start()
    {
        _fadeIn = false;
        _fadeOut = false;
        secondsSoFar = 0f;
        secondsToMove = 8f;
        SRScale = new Vector3 (0.6578647f, 0.6578647f, 0.6578647f);
        VRScale = new Vector3 (1,1,1);
        VRPosition = gameObject.transform.position;
        SRPosition = new Vector3(0f,1.1f, -1.1f);
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        // canvasGroup.alpha = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.FadeOutVR && !Manager.Passthrough)
        {
            if(!_fadeOut){
             _fadeOut = true;
                if(AsFader) StartCoroutine(FadeOut());
            }
        }
        if (Manager.FadeInVR && !Manager.Passthrough)
        {
            if(!_fadeIn){
                _fadeIn = true;
               if(AsFader) StartCoroutine(FadeIn());
            }
        }
        if (Manager.FadeOutVR && Manager.Passthrough)
        {
            float t = secondsSoFar / secondsToMove;
            Vector3 target = SRPosition;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,target,t);
            transform.localScale = Vector3.Lerp (transform.localScale, SRScale ,t);
            secondsSoFar += Time.deltaTime;
            if (secondsSoFar >= 1){
                secondsSoFar = 0f;
            } 
        }
        if (Manager.FadeInVR && Manager.Passthrough)
        {
            float t = secondsSoFar / secondsToMove;
            Vector3 target = VRPosition;
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position,target,t);
            gameObject.transform.localScale = Vector3.Lerp (gameObject.transform.localScale, VRScale ,t);
            secondsSoFar += Time.deltaTime;
            if (secondsSoFar >= 1){
                secondsSoFar = 0f;
            } 
        }
    }


    IEnumerator FadeOut()
    {
        if (_fadeOut)
        {
            for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                gameObject.GetComponent<CanvasGroup>().alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
            _fadeOut = false;
        }
    }

        IEnumerator FadeIn()
    {
        if (_fadeIn)
        {
            for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                gameObject.GetComponent<CanvasGroup>().alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
        }
        _fadeIn = false;
    }


}
