using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PaypalController : MonoBehaviour, ITaskContoller
{
    public GameObject[] inputFields;
    public GameObject[] introText;
    public GameObject[] TaskEndText;
    public CanvasGroup ObjectsToDisplay;
    public CanvasGroup TaskCanvas;
    public CanvasGroup IntroCanvas;
    public CanvasGroup EndCanvas;
    public Button submitButton;
    private bool submitted;
    public GameManager gameManager; 
    public GameObject KeyboardControls;
    public GameObject Paypal;
    public GameObject currentInputField;
    public int inputFieldCounter;
    public GameObject ErrorMessage;
    private bool ErrorMessageVisible;
    private bool SecurityQuestionDone;
    private bool loginDone;
    public CanvasGroup SecurityQuestion;
    public CanvasGroup InputFieldsToHide;
    private bool TaskBegan;
    private bool NotificationVisible;
    private bool NotificationLocked;
    private bool introVisible;


    
    void Start(){
        submitted=false;
        //StartCoroutine(BeginTask());
        inputFieldCounter = 0;
        ErrorMessageVisible = false;
        loginDone = false;
        SecurityQuestionDone = false;
        TaskBegan = false;
        NotificationVisible = false;
        NotificationLocked = false;
        introVisible = false;

    }
    void Update(){
        if(introVisible){
         if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
         if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Return)){
            Debug.Log("Paypal got it");
                StartCoroutine(HideIntro());
                gameManager.StartNotificationCoroutine();
            }
        }
        }
        if (TaskBegan)
        {
            if(NotificationVisible && !NotificationLocked){
            if(Input.anyKey){
                NotificationVisible = false;
                NotificationLocked = true;
                gameManager.Logger.LogData(this, LogType.NotificationStop, $"User continued task. Stopping Notification" );
                gameManager.StopNotification();
                
            }
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            if(!loginDone){
                inputFieldCounter++;
                if (inputFieldCounter==inputFields.Length-1) inputFieldCounter = 0;
            }
            KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputFields[inputFieldCounter].transform.parent.gameObject);



        }

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if(Input.GetKeyUp(KeyCode.Return)  && !loginDone && TaskBegan){
            if (inputFields[0].GetComponent<Text>().text.ToLower().Contains($"{gameManager.PlayerName}@") && inputFields[0].GetComponent<Text>().text.Contains(".") && (inputFields[1].transform.parent.gameObject.GetComponent<InputField>().text.Equals(gameManager.NewPassword)|| inputFields[1].transform.parent.gameObject.GetComponent<InputField>().text.Equals(gameManager.backupPassword)))
            {
                gameManager.Logger.LogData(this, LogType.Task, "Correct email and password given" );
                loginDone = true;
                inputFieldCounter = 2;
                if(ErrorMessageVisible)
                {
                    ErrorMessage.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
                    ErrorMessageVisible = false;
                }
                StartCoroutine(DisplaySecurityQuestion());
                 
            }else{
                //show stuff
                gameManager.Logger.LogData(this, LogType.Task, "Wrong email or password given" );
                ErrorMessage.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,255f);
                ErrorMessageVisible = true;
            }
            }else if(Input.GetKeyUp(KeyCode.Return)  && loginDone && !SecurityQuestionDone){
                 gameManager.Logger.LogData(this, LogType.Task, "Security Question submitted" );
                SecurityQuestionDone = true;
                submitButton.onClick.Invoke();
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputFields[inputFieldCounter].transform.parent.gameObject);
        }
        }
        
    }


    IEnumerator DisplaySecurityQuestion(){
        //Hide email and password inputfields 
        for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                InputFieldsToHide.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }

            //Display Security Question
            for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                SecurityQuestion.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
            gameManager.Logger.LogData(this, LogType.Task, "Security Question Visible " );
             KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputFields[inputFieldCounter].transform.parent.gameObject);


    }

    public void StartTask(){
       
        BeginTask();
    }
    
    public void EndTaskButton(){
        //PlaySound
        StartCoroutine(EndTask());
    }

    private void BeginTask()
    {

        introVisible = true;
        Debug.Log("Task Began " + gameObject.name);

        //Debug.Log("Wait Ended");
        //Display IntroText
        if(introText.Length != 0){
            foreach (GameObject text in introText)
            {
                text.GetComponent<RollingTextFade>().FadeIn();
               // Debug.Log("Began Fade in for " + text.name);
            }
        }
        
        gameManager.Logger.LogData(this, LogType.Task, "Task Intro visible" );
        //Display Canvas

        
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


        KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputFields[inputFieldCounter].transform.parent.gameObject);
        TaskBegan = true;
        gameManager.Logger.LogData(this, LogType.Task, "Task visible" );

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
        Paypal.SetActive(false);

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
