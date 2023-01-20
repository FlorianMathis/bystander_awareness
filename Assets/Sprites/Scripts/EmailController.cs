using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EmailController : MonoBehaviour, ITaskContoller
{
    public GameObject inputField;
    public GameObject[] introText;
    public GameObject[] TaskEndText;
    public CanvasGroup ObjectsToDisplay;
    public GameObject[] fromToSubject;
    public CanvasGroup TaskCanvas;
    public CanvasGroup IntroCanvas;
    public CanvasGroup EndCanvas;
    public Button submitButton;
    private bool submitted;
    public GameManager gameManager; 
    public GameObject KeyboardControls;
    public GameObject Email;
    private bool TaskBegan;
    public Text counter;
    private bool NotificationVisible;
    private bool NotificationLocked;
    private bool introVisible;

    
    void Start(){
        submitted=false;
        TaskBegan = false;
        NotificationVisible = false;
        NotificationLocked = false;
        introVisible = false;
        //StartCoroutine(BeginTask());

    }
    void Update(){
        counter.text = $"{inputField.GetComponent<Text>().text.Length}/30";
        if(introVisible){
         if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
         if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Return)){    
            Debug.Log("Email got it");
                StartCoroutine(HideIntro());
                gameManager.StartNotificationCoroutine();
            }
        }
        }
        if(TaskBegan){
            if(NotificationVisible && !NotificationLocked){
            if(Input.anyKey){
                NotificationVisible = false;
                NotificationLocked = true;
                gameManager.Logger.LogData(this, LogType.NotificationStop, $"User continued task. Stopping Notification" );
                gameManager.StopNotification();
                
            }
        }

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if(Input.GetKeyUp(KeyCode.Return)  && !submitted){
            Debug.Log(inputField.GetComponent<Text>().text);
            if (inputField.GetComponent<Text>().text.Length > 30)
            {
                gameManager.Logger.LogData(this, LogType.Task, $"Email length is OK. Length: {counter}" );
                if (inputField.GetComponent<Text>().text.Contains(gameManager.Pin))
                {
                    gameManager.Logger.LogData(this, LogType.Task, "Input fulfills requirements. Email sent" );
                    Debug.Log("Length is more than 30");
                    submitted = true;
                    submitButton.onClick.Invoke();
                }else{
                    gameManager.Logger.LogData(this, LogType.Task, $"Email doesn't contain pin" );
                }

                 
            }else{
                //show stuff
                gameManager.Logger.LogData(this, LogType.Task, $"Email length is too short. Length: {counter}" );
                Debug.Log("Length is less than 30");
            }
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputField.transform.parent.gameObject);
        }
        }
        
    }
    public void StartTask(){
       
        fromToSubject[0].GetComponent<TMP_Text>().text = gameManager.PlayerName + "@metadefenders.com";
        BeginTask();
    
    }
    
    public void SendMessage(){
        //PlaySound
        StartCoroutine(EndTask());
    }

    private void BeginTask()
    {
        introVisible = true;
        //to be removed
        //yield return new WaitForSecondsRealtime(2);
        //Debug.Log("Wait Ended");
        fromToSubject[0].GetComponent<TMP_Text>().text = gameManager.PlayerName + "@metadefenders.com";
        Debug.Log(fromToSubject[0].GetComponent<TMP_Text>().text);
        //Display IntroText
        if(introText.Length != 0){
            foreach (GameObject text in introText)
            {
                text.GetComponent<RollingTextFade>().FadeIn();
               // Debug.Log("Began Fade in for " + text.name);
            }
        }
        

        gameManager.Logger.LogData(this, LogType.Task, "Task Intro visible" );
    }
    IEnumerator HideIntro()
    {
        introVisible = false;
        //Hide Intro
        for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                IntroCanvas.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }



        //Display Email and Elements
        for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                //TaskCanvas.alpha = f;
                ObjectsToDisplay.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }


        //Display FromToSubject
        if(fromToSubject.Length != 0){
            foreach (GameObject text in fromToSubject)
            {
                text.GetComponent<RollingTextFade>().FadeIn();
            }
            gameManager.Logger.LogData(this, LogType.Task, "Task visible" );
            KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputField.transform.parent.gameObject);
            TaskBegan = true;
        }

    }

    IEnumerator EndTask()
    {
        Debug.Log("Task Ended");
        //Hide Task Canvas
        
        for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                TaskCanvas.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }

        //Display TaskEndCanvas
        for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                EndCanvas.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
        //Display TaskEndText

        if(TaskEndText.Length != 0){
            foreach (GameObject text in TaskEndText)
            {
                text.GetComponent<RollingTextFade>().FadeIn();
            }
        }

        gameManager.Logger.LogData(this, LogType.Task, "Task End Canvas visible" );
        yield return new WaitForSeconds(10); 

        //Hide Task End Canvas
        for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                EndCanvas.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }

        gameManager.EndTask();
        TaskBegan = false;
        Email.SetActive(false);
        

    }

     public void SetNotificationVisible(bool isVisible){
        NotificationVisible = isVisible;
        NotificationLocked = isVisible;
        gameManager.Logger.LogData(this, LogType.Task, "Notification Lock set" );
        if(isVisible) StartCoroutine(RemoveNotificationLocked());
    }

    IEnumerator RemoveNotificationLocked(){
        yield return new WaitForSecondsRealtime(3f);
        gameManager.Logger.LogData(this, LogType.Task, "Notification Lock removed" );
        NotificationLocked = false;

    }
}
