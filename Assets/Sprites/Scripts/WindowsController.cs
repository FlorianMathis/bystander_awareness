using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class WindowsController : MonoBehaviour, ITaskContoller
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
    public GameObject ErrorMessagePin;
    private bool ErrorMessageVisible;
    private bool SecurityQuestionDone;
    private bool loginDone;
    public CanvasGroup SecurityQuestion;
    public CanvasGroup InputFieldsToHide;
    private bool TaskBegan;
     public Text time;
    public Text Date;
    private bool LoginOpen;
    public GameObject LoginCover;
    public GameObject Mask;
    private bool NotificationVisible;
    private bool NotificationLocked;
    private bool introVisible;
    
    void Start(){
        time.text = System.DateTime.Now.ToLocalTime().ToString("HH:mm");
        string rawDate =  System.DateTime.Today.ToString("D");
        Date.text = rawDate.Substring(0, rawDate.LastIndexOf(' '));
        submitted=false;
        LoginOpen = false;
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
            Debug.Log("windows got it");
                StartCoroutine(HideIntro());
                gameManager.StartNotificationCoroutine();
            }
        }
        }

        if(NotificationVisible && !NotificationLocked &&  TaskBegan){
            if(Input.anyKey){
                NotificationVisible = false;
                NotificationLocked = true;
                gameManager.Logger.LogData(this, LogType.NotificationStop, $"User continued task. Stopping Notification" );
                gameManager.StopNotification();
                
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Space) && !LoginOpen && TaskBegan){
            LoginOpen = true;
            KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputFields[inputFieldCounter].transform.parent.gameObject);
            StartCoroutine(MoveCanvas());
        }
        if(TaskBegan && LoginOpen)
        {
        if(Input.GetKeyDown(KeyCode.Tab)){
            if(!loginDone){
                inputFieldCounter++;
                if (inputFieldCounter==inputFields.Length-1) inputFieldCounter = 0;
            }
            KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputFields[inputFieldCounter].transform.parent.gameObject);



        }
        
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Return))  && !loginDone && TaskBegan)
            {
            if (inputFields[0].GetComponent<Text>().text.ToLower().Replace(" ", "").Equals(gameManager.PlayerName)  && inputFields[1].GetComponent<Text>().text.Length !=0)
            {
                gameManager.Logger.LogData(this, LogType.Task, "Correct username and password given" );

                loginDone = true;
                inputFieldCounter = 2;
                if(ErrorMessageVisible)
                {
                    ErrorMessage.GetComponent <Text>().color = new Color(255f,255f,255f,0f);
                    ErrorMessageVisible = false;
                }
                gameManager.OldPassword = inputFields[1].GetComponent<Text>().text.ToLower().Replace(" ", "");
                StartCoroutine(DisplaySecurityQuestion());
                 
            }else{
                //show stuff
                gameManager.Logger.LogData(this, LogType.Task, "Wrong username or password given" );
                ErrorMessage.GetComponent <Text>().color = new Color(255f,255f,255f,255f);
                ErrorMessageVisible = true;
            }
            }else if((Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Return))  && loginDone && !SecurityQuestionDone){
                if(CheckPin()){
                SecurityQuestionDone = true;
                submitButton.onClick.Invoke();
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputFields[inputFieldCounter].transform.parent.gameObject);
        }
        }
    }
    private bool CheckPin()
    {
        InputField parent = inputFields[2].transform.parent.gameObject.GetComponent<InputField>();
        Debug.Log(parent.text);
        if(parent.text.Equals(gameManager.Pin)){
            ErrorMessagePin.GetComponent <Text>().color = new Color(255f,255f,255f,0f);
            gameManager.Logger.LogData(this, LogType.Task, "Pin OK" );
            return true;
        }else{
            ErrorMessagePin.GetComponent <Text>().color = new Color(255f,255f,255f,255f);
            ErrorMessageVisible = true;
            gameManager.Logger.LogData(this, LogType.Task, "Wrong Pin" );
            return false;
        }
                
    }

    IEnumerator MoveCanvas(){
        Vector3 target = new Vector3(0f,450f,0f);
        float secondsSoFar= 0f;
        float secondsToMove = 16f; 
        Image image = LoginCover.GetComponent<Image>();

        float i = 0.95f; 
        while(secondsSoFar < secondsToMove){
        Color c = image.color;
        c.a = i;
        image.color = c;
        LoginCover.transform.position =  Vector3.Lerp(LoginCover.transform.position,target, secondsSoFar/secondsToMove);
        secondsSoFar += Time.deltaTime;
        i-=0.01f;
        yield return null;
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
            gameManager.Logger.LogData(this, LogType.Task, "Enter Pin Visible " );
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
