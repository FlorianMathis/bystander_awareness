using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{

    private bool _fadeIn;
    private bool _fadeOut;
    public bool SingleFadeIn { get; set; }
    public bool SingleFadeOut { get; set; }
    private bool _singleFadeOut;
    private bool _deactivateMesh;
    private Renderer renderer;
    private List<Material> materials;
    public GameManager Manager;
    private bool isHeadset;
    private bool passthrough;
    private bool background;
    

    // Start is called before the first frame update
    void Start()
    {
        _fadeIn = false;
        _fadeOut = false;
        SingleFadeOut = false;
        SingleFadeOut = false;
        _singleFadeOut = false;
        passthrough = false;
        _deactivateMesh =
            gameObject.name.Equals("laptop") ||
            gameObject.name.Equals("TI_Seat_Number_02") ||
            gameObject.name.Equals("TI_Seat_Number_01") ||
            gameObject.name.Equals("smartphone");
        isHeadset= gameObject.name.Equals("Oculus Headset");
        background = gameObject.name.Contains("TI_Background");

        renderer = gameObject.GetComponent<Renderer>();

        if (!_deactivateMesh)
        {
            if (renderer != null){
                materials = new List<Material>(renderer.materials);
            }
            for (
                int childIndex = 0;
                childIndex < gameObject.transform.childCount;
                childIndex++
            )
            {
                Transform child = gameObject.transform.GetChild(childIndex);
                if(!child.gameObject.name.Contains("Questionnaire")){
                child.gameObject.AddComponent<Fader>();
                child.gameObject.GetComponent<Fader>().Manager = Manager;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.FadeOut && !Manager.Passthrough && renderer != null)
        {
            if(!_fadeOut){
            _fadeOut = true;
            StartCoroutine(FadeOut());
            }
        }
        if (Manager.FadeIn && !Manager.Passthrough && renderer != null)
        {
            if(!_fadeIn){
            _fadeIn = true;
            StartCoroutine(FadeIn());
            }
        }

        if (Manager.FadeOut && Manager.Passthrough && renderer != null)
        {
            if(!_fadeOut){
            _fadeOut = true;
            passthrough = true;
            StartCoroutine(FadeOut());
            }
        }
        if (Manager.FadeIn && Manager.Passthrough && renderer != null)
        {
            if(!_fadeIn){
            _fadeIn = true;
            passthrough = true;
            StartCoroutine(FadeIn());
            }
        }
        
         if (SingleFadeOut)
        {
            for (int childIndex = 0;
                childIndex < gameObject.transform.childCount;
                childIndex++
            )
            {
                Transform child = gameObject.transform.GetChild(childIndex);

                child.gameObject.GetComponent<Fader>().SingleFadeOut = true;
            }
            
            if(!_fadeOut && renderer != null){
            _fadeOut = true;
            StartCoroutine(FadeOut());
            
            }
            SingleFadeOut = false;
            
        }
        if (SingleFadeIn)
        {
                        for (int childIndex = 0;
                childIndex < gameObject.transform.childCount;
                childIndex++
            )
            {
                Transform child = gameObject.transform.GetChild(childIndex);

                child.gameObject.GetComponent<Fader>().SingleFadeIn = true;
            }
            
            if(!_fadeIn && renderer != null){
            _fadeIn = true;
            StartCoroutine(FadeIn());
            } 
            
        }
        SingleFadeIn = false;
        
    }

    IEnumerator FadeOut()
    {
        // Debug.Log("Started FadeOut");
        // yield return new WaitForSeconds(3);
        if (_fadeOut)
        {
            if (!(passthrough &&isHeadset))
            {
            if (!_deactivateMesh)
            {
                foreach (Material material in materials)
                {
                    MyMaterialHelper
                            .SetMaterialRenderingMode(material,
                            MyMaterialHelper.BlendMode.Fade);
                }
                for(float f = 1f ; f >= -0.05f; f-=0.05f)
                {
                    foreach (Material material in materials)
                    {
                        Color c = material.color;
                        c.a = f;
                        material.color = c;
                    }
                    yield return new WaitForSeconds(0.05f);    
                }
            } else
            {
                gameObject.GetComponent<Renderer>().enabled = false;
            }
        }
            _fadeOut = false;
            passthrough = false;
            if (_singleFadeOut)
            {
               yield return new WaitForSeconds(0.05f);
                foreach (Material material in materials)
                {
                    MyMaterialHelper
                            .SetMaterialRenderingMode(material,
                            MyMaterialHelper.BlendMode.Opaque);
                } 
                _singleFadeOut = false;
            }
            
        }
    }
    

    IEnumerator FadeIn()
    {
        // Debug.Log("Started FadeIn");
        // yield return new WaitForSeconds(6);

        if (_fadeIn)
        {
            if (!(passthrough && isHeadset))
            {
            if (!_deactivateMesh)
            {
                foreach (Material material in materials)
                {
                    MyMaterialHelper
                            .SetMaterialRenderingMode(material,
                            MyMaterialHelper.BlendMode.Fade);
                    Color c = material.color;
                    c.a = 0f;
                    material.color = c;
                }

                

                for(float f = 0.05f ; f <= 1f; f+=0.05f)
                {
                    foreach (Material material in materials)
                    {       
                        Color c = material.color;             
                        c.a = f;
                        material.color = c;      
                    }
                    yield return new WaitForSeconds(0.05f);
                }
                if(!background){
                foreach (Material material in materials)
                {
                    MyMaterialHelper
                            .SetMaterialRenderingMode(material,
                            MyMaterialHelper.BlendMode.Opaque);
                }
                }
            } else
            {
                gameObject.GetComponent<Renderer>().enabled = false;
            }
            }
            _fadeIn = false;
            passthrough = false;
        }


        
    }



    public void StartFadeIn(){


        for (int childIndex = 0;
                childIndex < gameObject.transform.childCount;
                childIndex++
            )
        {
            Transform child = gameObject.transform.GetChild(childIndex);
            child.gameObject.GetComponent<Fader>().StartFadeIn();
        }
            foreach (Material material in materials)
                {
                    MyMaterialHelper
                            .SetMaterialRenderingMode(material,
                            MyMaterialHelper.BlendMode.Opaque);
                    Color c = material.color;
                    c.a = 1f;
                    material.color = c;
                }

                
        // if(renderer != null){
        //     _fadeIn = true;
        //     StartCoroutine(FadeIn());
        //     }

    }

    public void StartFadeOut(){
        for (int childIndex = 0;
                childIndex < gameObject.transform.childCount;
                childIndex++
            )
        {
            Transform child = gameObject.transform.GetChild(childIndex);
            child.gameObject.GetComponent<Fader>().StartFadeOut();
        }
            foreach (Material material in materials)
                {
                    MyMaterialHelper
                            .SetMaterialRenderingMode(material,
                            MyMaterialHelper.BlendMode.Opaque);
                    Color c = material.color;
                    c.a = 1f;
                    material.color = c;
                }

        // if(renderer != null){
        //     _fadeOut= true;
        //     _singleFadeOut = true;
        //     StartCoroutine(FadeOut());
        //     }
    }
}
