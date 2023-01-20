using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchVR : MonoBehaviour
{
    public Transform oculus;
    public Transform head;
    public Camera camera;
    public GameObject sr;
    public GameObject vr;
    public GameObject keyboard;
    private bool startUpdate;
    private bool collided;
    private bool returnToTable;
    private bool srHidden;
    private float secondsToRotate;
    private float secondsToReturn;
    private float secondsSoFar;
    private float secondsSoFarReturn;
    private Renderer myRenderer;
    public GameManager Manager;
    private Material material;
    private Vector3 initialPosition;
    




    // Start is called before the first frame update
    void Start()
    {
        startUpdate = false;
        initialPosition = new Vector3(oculus.position.x-1.4f,oculus.position.y-0.8f, oculus.position.z-1.2f);
        secondsToRotate = 8.0f;
        secondsToReturn = 16.0f;
        secondsSoFar = 0.0f;
        secondsSoFarReturn = 0.0f;
        returnToTable = false;
        collided = false;
        material = gameObject.GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        if(collided){
            float t = secondsSoFar / secondsToRotate;
            Vector3 target = new Vector3(head.position.x,head.position.y, head.position.z-0.1f);
            Quaternion targetRotation = new Quaternion(head.rotation.x,head.rotation.y+90, head.rotation.z+90,1);
            oculus.position = Vector3.Lerp(oculus.position,target,t);
            oculus.rotation = Quaternion.Lerp(oculus.rotation, targetRotation,t);
            secondsSoFar += Time.deltaTime;
            if (secondsSoFar >= 1 && !srHidden){
                Manager.FadeOut = true;
                Manager.StartFadeInVR();
                StartCoroutine(DeactivateFadeOut());
                Manager.ActivateVR = true;
                Manager.RemoveCullingMaskSR = true;
                camera.clearFlags = CameraClearFlags.Skybox;
                srHidden = true;
                secondsSoFar = 0f;
            } 

        }
        if(returnToTable){
            float t = secondsSoFarReturn / secondsToReturn;
            Vector3 target = initialPosition;
            Quaternion targetRotation = new Quaternion(0,90,90,1);
            oculus.position = Vector3.Lerp(oculus.position,target,t);
            oculus.rotation = Quaternion.Lerp(oculus.rotation, targetRotation,t);
            secondsSoFarReturn += Time.deltaTime;
            if (secondsSoFarReturn >= 1){
                returnToTable = false;
                secondsSoFarReturn = 0f;
            } 

        }
    }

void OnTriggerEnter(Collider collider){
    // Debug.Log("triggered ");
        if(!collided){
        // Debug.Log("Collided");
        // Debug.Log(collider.gameObject.name);
        // Debug.Log(collider.gameObject.tag);
        if(collider.gameObject.tag == "Player" && !Manager.TasksCompleted){
            if (!Manager.QuestionnaireMode || (Manager.QuestionnaireMode && Manager.QuestionsCompleted))
            {
                collided = true;
                srHidden = false;
                secondsSoFar = 0f;
            }

           // sr.SetActive(false);
            //vr.SetActive(true);
            //keyboard.SetActive(true);
        }
        }
    }
    IEnumerator DeactivateFadeOut()
    {
        yield return new WaitForSeconds(4f);
        Manager.FadeOut = false;


    }
    public void HideHeadset(){
         MyMaterialHelper
                            .SetMaterialRenderingMode(material,
                            MyMaterialHelper.BlendMode.Fade);
                    Color c = material.color;
                    c.a = 0f;
                    material.color = c;
    }
    public void ShowHeadset(){
        Color c = material.color;
        c.a = 1f;
        material.color = c;
        MyMaterialHelper
                .SetMaterialRenderingMode(material,
                 MyMaterialHelper.BlendMode.Opaque);
    }

    public void StartReturnHeadsetToTable(){
        collided = false;
        returnToTable = true;
        
    }

    
}
