using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class ChangePasswordController : MonoBehaviour, ITaskContoller
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
    public GameObject ChangePassword;
    public GameObject currentInputField;
    public int inputFieldCounter;
    public GameObject ErrorMessage;
    public GameObject ErrorMessage1;
    public GameObject ErrorMessage2;
    public GameObject ErrorMessagePin;
    private bool ErrorMessageVisible;
    private bool loginDone;
    private bool TaskBegan;
    private Image[] parents;
    private Sprite originalInputImage;
    public Sprite redInputField;
    public CanvasGroup SecurityQuestion;
    private bool SecurityQuestionDone;
    public Image CoverImage;
    private bool NotificationVisible;
    private bool NotificationLocked;
     private bool introVisible;


    
    void Start(){
        submitted=false;
        //StartCoroutine(BeginTask());
        inputFieldCounter = 0;
        ErrorMessageVisible = false;
        loginDone = false;
        TaskBegan = false;
        SecurityQuestionDone = false;
        NotificationVisible = false;
        NotificationLocked = false;
        introVisible = false;
        parents = new Image[inputFields.Length];
        for (int i = 0; i < parents.Length; i++)
        {
            parents[i] = inputFields[i].transform.parent.gameObject.GetComponent<Image>();
        }
        

        originalInputImage = Instantiate(parents[0].sprite);

    }
    void Update(){
        if(introVisible){
         if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
         if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Return)){
            Debug.Log("Change got it");
                StartCoroutine(HideIntro());
                gameManager.StartNotificationCoroutine();
            }
        }
        }
        if(TaskBegan)
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
            if(Input.GetKeyUp(KeyCode.Return)  && !loginDone){

                //Change
            if (CheckInputFields())
            {
                gameManager.Logger.LogData(this, LogType.Task, "Correct passwords given" );

                loginDone = true;
                inputFieldCounter = 3;
                if(ErrorMessageVisible)
                {
                    ErrorMessage.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
                    ErrorMessage1.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
                    ErrorMessage2.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
                    ErrorMessageVisible = false;
                }
                gameManager.NewPassword = inputFields[1].transform.parent.gameObject.GetComponent<InputField>().text;
                StartCoroutine(DisplayPin());
             
            }
            }else if(Input.GetKeyUp(KeyCode.Return)  && loginDone && !SecurityQuestionDone){
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

    IEnumerator DisplayPin(){
        //Hide email and password inputfields 
        for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                Color c = CoverImage.color; 
                c.a = f;
                CoverImage.color = c; 
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

    private bool CheckPin()
    {
        if(inputFields[3].transform.parent.gameObject.GetComponent<InputField>().text.Equals(gameManager.Pin)){
            parents[3].sprite = originalInputImage; 
            ErrorMessagePin.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
            gameManager.Logger.LogData(this, LogType.Task, "Pin OK" );

            return true;
        }else{
            parents[3].sprite = redInputField; 
            ErrorMessagePin.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,255f);
            ErrorMessageVisible = true;
            gameManager.Logger.LogData(this, LogType.Task, "Wrong Pin" );

            return false;
        }
                
    }

    private bool CheckInputFields()
    {
        bool allDone = true;
        for (int i = 0; i < parents.Length; i++)
        {
            switch (i)
            {
                case 0:
                    if(inputFields[i].GetComponent<Text>().text.ToLower().Replace(" ", "").Equals(gameManager.OldPassword) || inputFields[i].GetComponent<Text>().text.Equals(gameManager.backupPassword)){
                        parents[i].sprite = originalInputImage; 
                        allDone = allDone && true;
                        ErrorMessage.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
                    }else{
                        parents[i].sprite = redInputField; 
                        allDone = allDone && false;  
                        ErrorMessage.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,255f);
                        gameManager.Logger.LogData(this, LogType.Task, "Old password wrong" );
                        ErrorMessageVisible = true;
                    }
                    break;
                case 1:
                    if(inputFields[i].GetComponent<Text>().text.Length > 7){
                        parents[i].sprite = originalInputImage; 
                        allDone = allDone && true;
                        ErrorMessage1.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
                    }else{
                        parents[i].sprite = redInputField; 
                        allDone = allDone && false; 
                        ErrorMessage1.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,255f);
                        gameManager.Logger.LogData(this, LogType.Task, "New Password doesn't match criterias" );
                        ErrorMessageVisible = true;
                    }
                    break;
                case 2: 
                    if(inputFields[i].GetComponent<Text>().text.Equals(inputFields[i-1].GetComponent<Text>().text)){
                        parents[i].sprite = originalInputImage; 
                        allDone = allDone && true;
                        ErrorMessage2.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
                    }else{
                        parents[i].sprite = redInputField; 
                        allDone = allDone && false;
                        ErrorMessage2.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,255f);
                        gameManager.Logger.LogData(this, LogType.Task, "Password Repeat doesn;'t match password" );
                        ErrorMessageVisible = true;
                    }
                    break;
            
        
            }
        }
        return allDone;
    }

    private void ChangeSprite(Image image, bool toRed){
        image.sprite = toRed ? redInputField : originalInputImage; 
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
            gameManager.Logger.LogData(this, LogType.Task, "Task visible" );


        KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputFields[inputFieldCounter].transform.parent.gameObject);
        TaskBegan = true;

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
        ChangePassword.SetActive(false);

    }

     public void SetNotificationVisible(bool isVisible){
        NotificationVisible = isVisible;
        NotificationLocked = isVisible;
        gameManager.Logger.LogData(this, LogType.Task, $"Notification Lock set to {isVisible}" );
        if(isVisible) StartCoroutine(RemoveNotificationLocked());
    }

    IEnumerator RemoveNotificationLocked(){
        yield return new WaitForSecondsRealtime(3f);
        gameManager.Logger.LogData(this, LogType.Task, "Notification Lock removed" );
        NotificationLocked = false;

    }
}
