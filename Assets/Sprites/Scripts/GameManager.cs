using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public bool FadeIn { get; set; }
    public bool FadeOut { get; set; }
    public bool FadeInVR { get; set; }
    public bool FadeOutVR { get; set; }
    public bool Passthrough { get; set; }
    public bool RemoveCullingMaskSR { get; set; }
    public bool ActivateVR { get; set; }
    public bool QuestionnaireMode { get; set; }
    public bool QuestionsCompleted{ get; set; }
    public string PlayerName { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public bool NotificationVisible { get; set; }
    public string Pin { get; set; }
    public string backupPassword { get; set; }
    public int ID { get; set;}
    public bool TasksCompleted { get; set; }
    public Camera camera;
    public GameObject vr;
    public GameObject keyboard;
    public GameObject UITextNotification;
    public GameObject AttentionMarker;
    public GameObject Welcome;
    [SerializeField] private CanvasGroup _radarNotification;
    [SerializeField] private CanvasGroup _avatarNotificationCanvasGroup;
    [SerializeField] private GameObject _avatarNotification;
    [SerializeField] private GameObject _controls;
    [SerializeField] private GameObject _attacker;
    [SerializeField] private GameObject _headset;
    [SerializeField] private GameObject[] Tasks;
    private List<int> completedTasks;
    private int currentTaskNo;
    private bool radarOn;
    private bool threeDScanOn;
    
    private int currentNotification;
    private int[] notifications;
    GameObject currentTask;
    public InputLogger Logger;
    public GameObject Questionnaire;
    public AudioMixer audioMixer;



    private bool startedCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        FadeIn = false;
        FadeOut = false;
        FadeInVR = false;
        FadeOutVR = false;
        Passthrough = false;
        RemoveCullingMaskSR = false;
        startedCoroutine = false;
        currentTaskNo = 0;
        PlayerName = "test";
        QuestionnaireMode = false;
        completedTasks = new List<int>();
        radarOn = false;
        threeDScanOn = false;
        TasksCompleted = false;
        QuestionsCompleted = false;
        Pin = "4567";
        currentNotification = 0;
        NotificationVisible = false; 
        backupPassword = "mypassword"; 
        }

    void Update()
    {
        if(RemoveCullingMaskSR){
            RemoveCullingMaskSR = false;
            hide("SR");
            hide("People");
            hide("Attacker");
            hide("Oculus");
            hide("Questionnaire");
            
        }
        if(ActivateVR){
            ActivateVR = false;
            _controls.GetComponent<TrackedKeyboardControls>().SetFocus();
            if(QuestionnaireMode && QuestionsCompleted){
                QuestionsCompleted = false;
                BeginTask();
            }else if(!Passthrough){
                BeginProcedure();
            }
            
        }


    }

    private void show(string layer){
        camera.cullingMask |= 1 << LayerMask.NameToLayer(layer);
        Debug.Log("Showed " + layer);
    }

    private void hide(string layer){
        camera.cullingMask &=  ~(1 << LayerMask.NameToLayer(layer));
    }

    public void StartTextNotification(){
   //     UITextNotification.GetComponent<FlashingEffect>().NoOfFlickers = noOfFlickers;
        UITextNotification.GetComponent<FlashingEffect>().StartFlicker();
    }

    public void StopTextNotification(){
   //     UITextNotification.GetComponent<FlashingEffect>().NoOfFlickers = noOfFlickers;
        UITextNotification.GetComponent<FlashingEffect>().StopFlicker();
    }

    public void StartRadar(){
        _radarNotification.alpha =1f;
        SoundManager.PlayPingSound();
    }

    public void StopRadar(){
        _radarNotification.alpha =0f;
    }
    
    private IEnumerator Start3DScan(){
        SoundManager.PlayPingSound(); 
        Debug.Log("Started Start3DScan");   
        StartFadeInAttacker();
        GameObject attackerChild = _attacker.transform.Find("Ch12").gameObject;
        attackerChild.GetComponent<ColorOverlay>().StartOverlay();
        yield return null;
         
        
        
    }
    private IEnumerator Stop3DScan(){
        Debug.Log("Started Stop3DScan");
        GameObject attackerChild = _attacker.transform.Find("Ch12").gameObject;
        attackerChild.GetComponent<ColorOverlay>().StopOverlay();  
        StartFadeOutAttacker();
        yield return new WaitForSecondsRealtime(1);
        hide("Attacker");
        
        
    }
        public void StartFadeOutAttacker(){
        Debug.Log("Started StartFadeOutAttacker");
        GameObject attackerChild = _attacker.transform.Find("Ch12").gameObject;
        Debug.Log(attackerChild.name);
        attackerChild.GetComponent<Fader>().StartFadeOut(); 
        //_attacker.GetComponent<Outline>().enabled = true;
    
    }
    public void StartFadeInAttacker(){
        Debug.Log("Started StartFadeInAttacker");
        GameObject attackerChild = _attacker.transform.Find("Ch12").gameObject;
        attackerChild.GetComponent<Fader>().StartFadeIn(); 
        show("Attacker");    
    }
    public void StartFadeInSR(bool passthrough){
        show("SR");
        show("People");
        show("Attacker");
        if(!passthrough){
             show("Oculus");
             show("Questionnaire");
        }
        Passthrough = passthrough;
        FadeIn = true;
        StartCoroutine(DeactivateFadeIn()); 
    }

    public void StartFadeOutSR(bool passthrough){
        Passthrough = passthrough;
        FadeOut = true;
        StartCoroutine(DeactivateFadeOut());
        StartCoroutine(ActivateRemoveCullingMaskSR());
    }
    public void StartFadeInVR(){
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "trainVolume", 3, 0f));
        show("VR");
        show("Task");
        FadeInVR = true;
        StartCoroutine(DeactivateFadeInVR()); 
    }

    public void StartFadeOutVR(){
        FadeOutVR = true;
        StartCoroutine(DeactivateFadeOutVR());
        StartCoroutine(ActivateRemoveCullingMaskVR());
    
    }


    public void StartAvatarNotification(int direction, int size){
        _avatarNotification.GetComponent<Avatar>().SelectAvatar(direction, size);
        _avatarNotificationCanvasGroup.alpha =1f;
        StartCoroutine(PlayPing(1));

        
    }

    public void StopAvatarNotification(){
        _avatarNotificationCanvasGroup.alpha =0f;
    }

    public void StartPassthrough(){
        StartFadeInSR(true);
        StartFadeOutVR();

    }
    public void StopPassthrough(){
        StartFadeOutSR(true);
        StartFadeInVR();
    }

    public void BackToSR(){
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, "trainVolume", 4, 1f));
        StartFadeInSR(false);
        StartFadeOutVR();
        _headset.GetComponent<LaunchVR>().StartReturnHeadsetToTable();

    }
    private void StartAttentionMarker(){
        AttentionMarker.GetComponent<FollowKeyboard>().StartSytem();
    }

    private void StopAttentionMarker(){
        AttentionMarker.GetComponent<FollowKeyboard>().StopSytem();
    }

    private void BeginProcedure(){
        Welcome.GetComponent<CanvasTextController>().ShowCanvas();

    }

    public void BeginTask(){
        
        QuestionnaireMode = false;
        currentTask = Tasks[currentTaskNo];
        Logger.LogData(this, LogType.Task, $"Beginning Task {currentTaskNo+1}: " + currentTask.name );
        currentTask.GetComponent<TaskManager>().BeginTask();
        currentNotification = currentTaskNo;
        //Attach Notification Sytem

        //StartCoroutine(TriggerNotificationStart());

    }

    public void EndTask(){
        Logger.LogData(this, LogType.Task, $"Task \"{currentTaskNo+1}\" ended");

        
        if(NotificationVisible) StopNotification();
        BackToSR();
        if(!TasksCompleted){
            QuestionnaireMode = true;
            BeginQuestionnaire(currentTaskNo == Tasks.Length-1);
        }
        
    }

    private void BeginQuestionnaire(bool isLast){
        Instantiate(Questionnaire, Questionnaire.transform.parent).transform.Find("QuestionnaireController").gameObject.GetComponent<QuestionnaireController>().StartQuestionnaire(isLast);
    }

    public void EndQuestionnaire(){
        QuestionsCompleted = true;
        if (currentTaskNo >= Tasks.Length) TasksCompleted = true;
        currentTaskNo++;
    }



    private IEnumerator DeactivateFadeIn(){
        yield return new WaitForSecondsRealtime(1);
        FadeIn = false;
    }
    private IEnumerator DeactivateFadeOut(){
        yield return new WaitForSecondsRealtime(1);
        FadeOut = false;
    }

    private IEnumerator DeactivateFadeInVR(){
        yield return new WaitForSecondsRealtime(1);
        FadeInVR = false;
    }
    private IEnumerator DeactivateFadeOutVR(){
        yield return new WaitForSecondsRealtime(1);
        FadeOutVR = false;
    }

    private IEnumerator ActivateRemoveCullingMaskSR(){
        yield return new WaitForSecondsRealtime(2);
        RemoveCullingMaskSR = true;
    }
    private IEnumerator ActivateRemoveCullingMaskVR(){
        yield return new WaitForSecondsRealtime(2);
        hide("VR");

    }
    // private IEnumerator HideAttacker(){
    //     yield return new WaitForSecondsRealtime(2);
    //     hide("Attacker");
    // }

    private IEnumerator PlayPing(int repeats){
        
        for (int i = 0; i < repeats; i++)
        {
            SoundManager.PlayPingSound();
            yield return new WaitForSecondsRealtime(1);
        }
    }

    private int[] balancedLatinSquare(int participantId) {
    int[] array = Enumerable.Range(0, 7).ToArray(); 

	List<int> result = new List<int>();
    int j = 0;
    int h = 0;
	for (int i = 0 ; i < array.Length; ++i) {
		int val = 0;
		if (i < 2 || i % 2 != 0) {
			val = j++;
		} else {
			val = array.Length - h - 1;
			++h;
		}

		var idx = (val + participantId) % array.Length;
		result.Add(array[idx]);
	}

	if (array.Length % 2 != 0 && participantId % 2 != 0) {
		result.Reverse();
	}
    

	return result.ToArray();
}
    public void SetID(int id)
    {
        ID = id;
        notifications = balancedLatinSquare(id);
        Logger.LogData(this, LogType.VariableChange, $"Assigned notifications order: [{String.Join(", ", notifications)}]");

    }

    public void StartNotification(){
        if(!NotificationVisible){
            NotificationVisible = true;
        switch(notifications[currentNotification]) 
        {
            case 0:
                Logger.LogData(this, LogType.NotificationStart, "UITextNotification");
                 StartTextNotification();
                 break;
            case 1:
                Logger.LogData(this, LogType.NotificationStart, "Radar Notification");
                radarOn = true;
                 StartRadar(); //StopRadar?
                 break;
            case 2:
                Logger.LogData(this, LogType.NotificationStart, "Avatar Notification");
                 StartAvatarNotification(0,0); //change sizes in test?
                 break;
            case 3:
                Logger.LogData(this, LogType.NotificationStart, "3D Scan");
                threeDScanOn = true;
                 StartCoroutine(Start3DScan());
                 break;
            case 4:
                Logger.LogData(this, LogType.NotificationStart, "Passthrough");
                 StartCoroutine(PlayPing(1));
                 StartPassthrough(); 
                 break;
            case 5:
                Logger.LogData(this, LogType.NotificationStart, "Attention Marker");
                    StartCoroutine(PlayPing(1));
                 StartAttentionMarker();
                 break;
            case 6:
            Logger.LogData(this, LogType.NotificationStart, "Baseline");
                 //BASELINE
                 break;
        } 
        currentTask.GetComponent<TaskManager>().SetNotificationVisible(true);
        }
    }

    public void StopNotification(){
        if (NotificationVisible)
        {
        switch(notifications[currentNotification]) 
        {
            case 0:
                Logger.LogData(this, LogType.NotificationStop, "UITextNotification");
                 StopTextNotification(); // Stop
                 break;
            case 1:
                Logger.LogData(this, LogType.NotificationStop, "Radar Notification");
                 StopRadar(); 
                 radarOn = true;
                 break;
            case 2:
                Logger.LogData(this, LogType.NotificationStop, "Avatar Notification");
                 StopAvatarNotification();
                 break;
            case 3:
                Logger.LogData(this, LogType.NotificationStop, "3D Scan");
                threeDScanOn = false;
                StartCoroutine(Stop3DScan());
                break;
            case 4:
                Logger.LogData(this, LogType.NotificationStop, "Passthrough");
                StopPassthrough();
                break;
            case 5:
                Logger.LogData(this, LogType.NotificationStop, "AttentionMarker");
                 StopAttentionMarker();
                 break;
            case 6:
                 Logger.LogData(this, LogType.NotificationStop, "Baseline");
                 //BASELINE
                 break;


        }
        currentTask.GetComponent<TaskManager>().SetNotificationVisible(false);
        NotificationVisible = false;
    }
        
}

public void StartNotificationCoroutine(){
     StartCoroutine(TriggerNotificationStart());
}
private IEnumerator TriggerNotificationStart(){
    yield return new WaitForSecondsRealtime(5f);
    StartCoroutine(TriggerNotificationStop());
    StartNotification();
    
}

private IEnumerator TriggerNotificationStop(){
    yield return new WaitForSecondsRealtime(10f);
    if(NotificationVisible){
        Logger.LogData(this, LogType.NotificationStop, $"Notification stopped by Time trigger" );
        StopNotification();
    } 
}

}
