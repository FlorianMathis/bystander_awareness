using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TrackedKeyboardControls : MonoBehaviour
{
    public OVRTrackedKeyboard trackedKeyboard;
    public InputField StartingFocusField;
    public InputLogger logger;
    // Start is called before the first frame update
    void Start()
    {
        StartingFocusField.Select();
        StartingFocusField.ActivateInputField(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown){
            if (Input.GetKeyDown(KeyCode.Space)){
                logger.LogData(this, LogType.Input ,"Space");
            }else if(Input.GetKeyDown(KeyCode.Backspace)){
                logger.LogData(this, LogType.Input ,"Backspace");
            }else if(Input.GetKeyDown(KeyCode.Delete)){
                logger.LogData(this, LogType.Input ,"Delete");
            }else if(Input.GetKeyDown(KeyCode.Clear)){
                logger.LogData(this, LogType.Input ,"Clear");
            }else if(Input.GetKeyDown(KeyCode.Return)){
                logger.LogData(this, LogType.Input ,"Return");
            }else if(Input.GetKeyDown(KeyCode.Escape)){
                logger.LogData(this, LogType.Input ,"Escape");
            }else if(Input.GetKeyDown(KeyCode.UpArrow)){
                logger.LogData(this, LogType.Input ,"UpArrow");
            }else if(Input.GetKeyDown(KeyCode.DownArrow)){
                logger.LogData(this, LogType.Input ,"DownArrow");
            }else if(Input.GetKeyDown(KeyCode.RightArrow)){
                logger.LogData(this, LogType.Input ,"RightArrow");
            }else if(Input.GetKeyDown(KeyCode.LeftArrow)){
                logger.LogData(this, LogType.Input ,"LeftArrow");
            }else if(Input.GetKeyDown(KeyCode.RightShift)){
                logger.LogData(this, LogType.Input ,"RightShift");
            }else if(Input.GetKeyDown(KeyCode.LeftShift)){
                logger.LogData(this, LogType.Input ,"LeftShift");
            }else if(Input.GetKeyDown(KeyCode.LeftControl)){
                logger.LogData(this, LogType.Input ,"LeftControl");
            }else if(Input.GetKeyDown(KeyCode.RightControl)){
                logger.LogData(this, LogType.Input ,"RightControl");
            }else if(Input.GetKeyDown(KeyCode.Tab)){
                logger.LogData(this, LogType.Input ,"Tab");
            }else if(Input.GetKeyDown(KeyCode.RightAlt)){
                logger.LogData(this, LogType.Input ,"RightAlt");
            }else if(Input.GetKeyDown(KeyCode.LeftAlt)){
                logger.LogData(this, LogType.Input ,"LeftAlt");
            }else{
                logger.LogData(this, LogType.Input ,Input.inputString);
            }
        }
    }

    public void SetFocus(GameObject o){
        InputField inputField = o.GetComponent<InputField>();
        inputField.Select();
        inputField.ActivateInputField(); 
        logger.LogData(this, LogType.Focus, "Set Focus to " + o.name);

    }

        public void SetFocus(){
        StartingFocusField.Select();
        StartingFocusField.ActivateInputField(); 
    }
}
