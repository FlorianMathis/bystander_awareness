using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class QuestionnaireController : MonoBehaviour
{
    public GameObject[] Questions;
    public GameObject[] introText;
    public GameObject[] TaskEndText;
    public CanvasGroup QuestionsCanvas;
    public CanvasGroup IntroCanvas;
    public CanvasGroup EndCanvas;
    public GameManager gameManager; 
    public GameObject Questionnaire;
    private GameObject currentQuestion;
    public int questionsCounter= 0;
    private bool QuestionnaireBegan = false;
    private bool allQuestionsDone = false;
    private Toggle[] currentToggles;
    private int[] answers;    
    private bool answerChosen = false;
    private bool ErrorMessageVisible = false;
    public GameObject ErrorMessage;
    public CanvasGroup SubmitButton;
    public Text counterDisplay;
    private bool introVisible = false;
    private bool isLast = false;
    void Start(){
        answers = new int[Questions.Length];
        currentQuestion = Questions[questionsCounter];

        SetCurrentToggles();

    }


    void Update(){
        if(introVisible){
         if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            Debug.Log("shift");
         if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyDown(KeyCode.Return)){
                Debug.Log("Got Input");
                StartCoroutine(HideIntro());
            }
        }
        }

        if(QuestionnaireBegan)
        {

            if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            if(Input.GetKeyUp(KeyCode.Return)){
            if (answerChosen)
            {
                gameManager.Logger.LogData(this, LogType.Questionnaire, $"Answer for Question {questionsCounter+1}: {answers[questionsCounter]}" );
                questionsCounter++;
                if(ErrorMessageVisible)
                {
                    ErrorMessage.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,0f);
                    ErrorMessageVisible = false;
                }
                if(questionsCounter > Questions.Length-1)
                {
                    allQuestionsDone = true;
                    StartCoroutine(EndQuestionnaire());
                }else{
                    StartCoroutine(ChangeQuestion());
                }
            
                 
            }else{
                //show stuff
                gameManager.Logger.LogData(this, LogType.Questionnaire, "Tried submitting without choosing answer" );
                ErrorMessage.GetComponent <TextMeshProUGUI>().color = new Color(255f,255f,255f,255f);
                ErrorMessageVisible = true;
            }
            }
        }

            if(Input.GetKeyDown(KeyCode.Alpha1)){
                ChooseAnswer(1);
            }
            if(Input.GetKeyDown(KeyCode.Alpha2)){
                ChooseAnswer(2);
            } 
            if(Input.GetKeyDown(KeyCode.Alpha3)){
                ChooseAnswer(3);
            } 
            if(Input.GetKeyDown(KeyCode.Alpha4)){
                ChooseAnswer(4);
            }  
            if(Input.GetKeyDown(KeyCode.Alpha5)){
                ChooseAnswer(5);
            } 
            if(Input.GetKeyDown(KeyCode.Alpha6) && questionsCounter < 17){
                ChooseAnswer(6);
            } 
            if(Input.GetKeyDown(KeyCode.Alpha7) && questionsCounter < 17){
                ChooseAnswer(7);
            } 
        }
    }




    public void StartQuestionnaire(bool islast){
       // TODO reset everything to original
       if(islast){
        isLast = islast ;
        TaskEndText[1].GetComponent<TMP_Text>().text = "You've successfully completed the simulation. Please take off the real headset and inform your tester";
       }
       
        ShowIntro();
    }
    

    IEnumerator ChangeQuestion()
    {
        answerChosen = false;
        currentQuestion = Questions[questionsCounter];

        CanvasGroup previousQuestion = Questions[questionsCounter-1].GetComponent<CanvasGroup>();

        for(float f = 1f ; f >= -0.1f; f-=0.15f)
        {
            previousQuestion.alpha = f;
            SubmitButton.alpha = f;
            Color c = counterDisplay.color;
            c.a = f;
            counterDisplay.color = c;
            yield return new WaitForSeconds(0.02f);    
        }
        
        SetCurrentToggles();
        counterDisplay.text = $"Question {questionsCounter+1}/{Questions.Length}";

        CanvasGroup canvasgroup = currentQuestion.GetComponent<CanvasGroup>();

        for(float f = 0.1f ; f <= 1f; f+=0.15f)
        {
            canvasgroup.alpha = f;
            SubmitButton.alpha = f;
            Color c = counterDisplay.color;
            c.a = f;
            counterDisplay.color = c;
            yield return new WaitForSeconds(0.02f);    
        }


        


    }

    IEnumerator DisplayCanvas()
    {
        
        CanvasGroup canvasGroup = Questionnaire.GetComponent<CanvasGroup>();
        for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                canvasGroup.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
    }
    private void ShowIntro(){
        //Display IntroText
        introVisible = true;
        StartCoroutine(DisplayCanvas());
        if(introText.Length != 0){
            foreach (GameObject text in introText)
            {
               text.GetComponent<RollingTextFade>().FadeIn();
            }
        }
        Debug.Log("Show 2 introVisible = " + introVisible) ;
        gameManager.Logger.LogData(this, LogType.Questionnaire, "Questionnaire Intro visible" );
        
        //Display Canvas

        
    }
    IEnumerator HideIntro(){
        Debug.Log("hideintro" ) ;
        //Hide Intro
        introVisible = false;
        for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                IntroCanvas.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }




        //Display First Question
        for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                QuestionsCanvas.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
            QuestionnaireBegan = true;
            gameManager.Logger.LogData(this, LogType.Questionnaire, "Questionnaire visible" );

    }

    IEnumerator EndQuestionnaire()
    {
        //Hide Task Canvas
        for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                QuestionsCanvas.alpha = f;
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
        gameManager.Logger.LogData(this, LogType.Questionnaire, "Questionnaire End Canvas visible" );

        string msg = "Answers:";
        for (int i = 0; i < answers.Length; i++)
        {
            msg += $"\n ;;;Q{i+1}: {answers[i]}";
        }
        gameManager.Logger.LogData(this, LogType.Questionnaire, msg );

        if(!isLast){        
        yield return new WaitForSeconds(6); 

        //Hide Task End Canvas
         CanvasGroup canvasGroup = Questionnaire.GetComponent<CanvasGroup>();
        for(float f = 1f ; f >= -0.05f; f-=0.05f)
            {
                canvasGroup.alpha = f;
                yield return new WaitForSeconds(0.05f);   
            }
        }


       
        //Reset();
         QuestionnaireBegan = false;
        gameManager.EndQuestionnaire();
       if(!isLast){  
        Questionnaire.SetActive(false);
       }
    }

    private void SetCurrentToggles(){
        
        Transform Toggles = currentQuestion.transform.Find("Toggles");
        currentToggles = new Toggle[Toggles.childCount];
        for (int i = 0; i < Toggles.childCount; i++)
        {
            Transform child = Toggles.GetChild(i);
            currentToggles[i] = child.gameObject.GetComponent<Toggle>();
            }
    }
    private void ChooseAnswer(int input){
        //remove previous answers
        foreach (Toggle toggle in currentToggles)
        {
            toggle.isOn = false;
        }

        answers[questionsCounter] = input;
        currentToggles[input-1].isOn = true;
        
        answerChosen = true;

    }


    private void Reset()
    {
        // all text colors
        //all canvas groups
        Questions[0].GetComponent<CanvasGroup>().alpha = 1f;
        for (int i = 1; i < Questions.Length; i++)
        {
             Questions[i].GetComponent<CanvasGroup>().alpha = 0f;
        }
        

                    // roll back intro visibility
            if(introText.Length != 0){
            foreach (GameObject text in introText)
            {
                Color oColor = text.GetComponent <TextMeshProUGUI>().color;
                text.GetComponent <TextMeshProUGUI>().color = new Color(oColor.r,oColor.g,oColor.b,0f);
            }
        }
    }
}
