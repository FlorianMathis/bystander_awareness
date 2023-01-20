using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public GameObject controller;
    private ITaskContoller taskController;

    void Start(){

    }
    public void BeginTask(){
        taskController = controller.GetComponent(typeof(ITaskContoller)) as ITaskContoller ;
        StartCoroutine(StartTask());
    }

    IEnumerator StartTask(){
        CanvasGroup canvas = gameObject.GetComponent<CanvasGroup>();
        for(float f = 0.05f ; f <= 1f; f+=0.05f)
            {
                canvas.alpha = f;
                yield return new WaitForSeconds(0.05f);    
            }
        taskController.StartTask();
    }
    public void SetNotificationVisible(bool isVisible){
        taskController.SetNotificationVisible(isVisible);
    }
}
