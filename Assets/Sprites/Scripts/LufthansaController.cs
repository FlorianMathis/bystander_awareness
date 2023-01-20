using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text.RegularExpressions;

public class LufthansaController : MonoBehaviour, ITaskContoller
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
    public GameObject Lufthansa;
    public GameObject currentInputField;
    public int inputFieldCounter;
    public GameObject ErrorMessage;
    private bool ErrorMessageVisible;
    private bool loginDone;
    private bool TaskBegan;
    private Image[] parents;
    private Sprite originalInputImage;
    public Sprite redInputField;
    private bool NotificationVisible;
    private bool NotificationLocked;
    private string lastValidInput = string.Empty;
    private bool introVisible;
    public Text counterDisplay;
    


    
    void Start(){
        submitted=false;
        //StartCoroutine(BeginTask());
        inputFieldCounter = 0;
        ErrorMessageVisible = false;
        loginDone = false;
        TaskBegan = false;
        NotificationVisible = false;
        NotificationLocked = false;
        introVisible = false;
        parents = new Image[inputFields.Length];
        for (int i = 0; i < parents.Length; i++)
        {
            parents[i] = inputFields[i].transform.parent.gameObject.GetComponent<Image>();
        }
        

        originalInputImage = Instantiate(parents[0].sprite);
        //inputFields[0].GetComponent<InputField>().onValueChanged.AddListener(OnValueChanged);

    }
    void Update(){
       
        if(introVisible){
         if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
         if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Return)){
            Debug.Log("Lufthansa got it");
                StartCoroutine(HideIntro());
                gameManager.StartNotificationCoroutine();
            }
        }
        }

        if(TaskBegan)
        {
            counterDisplay.text = $"{inputFields[0].GetComponent<Text>().text.Length}/16";
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
                if (inputFieldCounter==inputFields.Length) inputFieldCounter = 0;
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
                inputFieldCounter = 2;
                if(ErrorMessageVisible)
                {
                    ErrorMessage.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
                    ErrorMessageVisible = false;
                }
                submitButton.onClick.Invoke();
                 
            }else{
                //show stuff
                ErrorMessage.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,255f);
                ErrorMessageVisible = true;
            }
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            KeyboardControls.GetComponent<TrackedKeyboardControls>().SetFocus(inputFields[inputFieldCounter].transform.parent.gameObject);
        }
        }
    }

  //  private void OnValueChanged( string input ) // Supposing input = "9534"
  //   {
    //     string format = "{0}{1}{2}{3} {4}{5}{6}{7} {8}{9}{10}{11} {12}{13}{14}{15}" ;
      //   string[] array = new string[input.Length];
        // int index = format.IndexOf("{" + (input.Length) + "}");
//         if ( input.Length == 0 )
 //        {
  //           inputFields[0].GetComponent<Text>().text = string.Empty;
   //          return;
    //     }
      //   if ( index >= 0 )
       //  {
        //     format = format.Substring( 0, index);
        // }
       //  else
       //  {
          //   inputFields[0].GetComponent<Text>().text = lastValidInput;
        //     return;
        // }
   //      for ( index = 0 ; index < input.Length ; ++index )
  //           array[index] = string.Empty + input[index];
  //       lastValidInput = input;
  //       inputFields[0].GetComponent<Text>().text = string.Format( format, array ); // Will output 95/34
//
 //   }
    private bool CheckInputFields()
    {
        bool allDone = true;
        for (int i = 0; i < parents.Length; i++)
        {
            switch (i)
            {
                case 0:
                    if(Regex.IsMatch(inputFields[i].GetComponent<Text>().text.Replace(" ", ""), @"^\d{16}$")){
                        parents[i].sprite = originalInputImage; 
                        allDone = allDone && true;
                    }else{
                        parents[i].sprite = redInputField; 
                        allDone = allDone && false;  
                        gameManager.Logger.LogData(this, LogType.Task, "Incorrect Card Number submitted" );
                    }
                    break;
                case 1:
                    if(Regex.IsMatch(inputFields[i].GetComponent<Text>().text, @"^\+?\d+$")){
                        parents[i].sprite = originalInputImage; 
                        allDone = allDone && true;
                    }else{
                        parents[i].sprite = redInputField; 
                        allDone = allDone && false; 
                        gameManager.Logger.LogData(this, LogType.Task, "Incorrect Month submitted" );
                    }
                    break;
                case 2: 
                    if(Regex.IsMatch(inputFields[i].GetComponent<Text>().text, @"^20\d{2}$") && Int32.Parse(inputFields[i].GetComponent<Text>().text) > 2021){
                        parents[i].sprite = originalInputImage; 
                        allDone = allDone && true;
                    }else{
                        parents[i].sprite = redInputField; 
                        allDone = allDone && false;
                        gameManager.Logger.LogData(this, LogType.Task, "Incorrect Year submitted" );
                    }
                    break;
                case 3:
                    if(inputFields[i].transform.parent.gameObject.GetComponent<InputField>().text.Equals("456")){
                        parents[i].sprite = originalInputImage; 
                        allDone = allDone && true;
                    }else{
                        parents[i].sprite = redInputField; 
                        allDone = allDone && false;
                        gameManager.Logger.LogData(this, LogType.Task, "Incorrect Pin submitted" );
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
        Lufthansa.SetActive(false);

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
