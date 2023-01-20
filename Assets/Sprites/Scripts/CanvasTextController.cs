using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class CanvasTextController : MonoBehaviour
{
    public string Name;
    public GameObject inputField;
    public CanvasGroup ObjectsToHide;
    public GameObject[] textToHide;
    public CanvasGroup ObjectsToDisplay;
    public GameObject[] textToDisplay;
    public TMP_Text InsertNameHere;
    public Button submitButton;
    private bool submitted=false;
    private bool submittedNumber=false;
    public GameManager gameManager;
    public CanvasGroup canvas;
    public GameObject KeyboardControls;
    private bool TaskBegan = true;
    public GameObject NumberinputField;
    public GameObject GroupText;
    public GameObject NameinputField;
    private int inputFieldCounter = 0;
    
    void Update(){
        if(TaskBegan)
        {

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if(Input.GetKeyUp(KeyCode.Return)){
                if(!submittedNumber)
                {
                    if(NumberinputField.GetComponent<Text>().text.Length !=0 && Int32.Parse(NumberinputField.GetComponent<Text>().text) < 29){
                        submittedNumber = true;
                        inputFieldCounter = 1;
                        gameManager.SetID(Int32.Parse(NumberinputField.GetComponent<Text>().text));
                        gameManager.Logger.LogData(this, LogType.InputFieldContent, $"Participant ID:  {NumberinputField.GetComponent<Text>().text}");
                        StartCoroutine(ChangeInput());
                    }
                }
                if(!submitted){
                if (inputField.GetComponent<Text>().text.Length !=0)
                {
                    submitted = true;
                    submitButton.onClick.Invoke();
                    TaskBegan = false;

                 
                }
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            if(inputFieldCounter == 0){
                KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(NumberinputField.transform.parent.gameObject);
            }else{
                KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(NameinputField);

            }
        }
        }
    }
    public void SubmitName(){
        Name = inputField.GetComponent<Text>().text.Replace(" ","").ToLower();
        gameManager.PlayerName = Name;
        
        gameManager.Logger.LogData(this, LogType.InputFieldContent, $"Participant's Name:  {Name}");

        InsertNameHere.text = "Hello " +  Name + ", welcome to your virtual office!";
        StartCoroutine(ChangeText());
        Debug.Log("Submitted");
    }


    public void ShowCanvas(){
        StartCoroutine(DisplayCanvas());
    }
    IEnumerator DisplayCanvas()
    {
        for(float f = 0.05f ; f <= 1f; f+=0.05f)
        {
            canvas.alpha = f;
            yield return new WaitForSeconds(0.05f);    
        }
        gameManager.Logger.LogData(this, LogType.ProcedureBegin, "Welcome canvas visible");

    }

     IEnumerator ChangeInput()
    {
        GroupText.GetComponent<RollingTextFade>().FadeOut();

        CanvasGroup input = NumberinputField.transform.parent.gameObject.GetComponent<CanvasGroup>();

        for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                input.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }

            yield return new WaitForSeconds(1f); 

            textToHide[1].GetComponent<RollingTextFade>().FadeIn();


            input = NameinputField.GetComponent<CanvasGroup>();
             for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                input.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
            KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(NameinputField);


    }

    IEnumerator ChangeText()
    {
        if(textToHide.Length != 0){
            foreach (GameObject text in textToHide)
            {
                text.GetComponent<RollingTextFade>().FadeOut();
            }
        }
        if(ObjectsToHide != null){
             for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                ObjectsToHide.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
        }
        yield return new WaitForSeconds(2);    

        if(textToDisplay.Length != 0){
            foreach (GameObject text in textToDisplay)
            {
                text.GetComponent<RollingTextFade>().FadeIn();
            }
        }
        if(ObjectsToDisplay != null){
            for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                ObjectsToDisplay.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
        }
        yield return new WaitForSeconds(5);  

            if(textToDisplay.Length != 0){
            foreach (GameObject text in textToDisplay)
            {
                text.GetComponent<RollingTextFade>().FadeOut();
                Debug.Log("StartedFadeOut");
            }
        }
        if(ObjectsToDisplay != null){
             for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                ObjectsToDisplay.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
        }
        yield return new WaitForSeconds(1);
        for(float f = 1f ; f >= -0.05f; f-=0.05f)
        {
            canvas.alpha = f;
            yield return new WaitForSeconds(0.05f);    
        }
        gameManager.BeginTask();

    }
}
