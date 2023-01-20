using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderVR : MonoBehaviour
{

    private bool _fadeIn;
    private bool _fadeOut;
    private bool _isGlass;
    public bool SingleFadeIn { get; set; }
    public bool SingleFadeOut { get; set; }
    private bool _deactivateMesh;
    private Renderer renderer;
    private List<Material> materials;
    public GameManager Manager;
    private float glassAlpha;
    private bool _isGlassTable;

    

    // Start is called before the first frame update
    void Start()
    {
        _fadeIn = false;
        _fadeOut = false;
        SingleFadeOut = false;
        SingleFadeOut = false;
        glassAlpha = 1f;
        _deactivateMesh =
            gameObject.name.ToLower().Contains("water") ||
            gameObject.name.ToLower().Contains("foam") ||
            gameObject.name.ToLower().Contains("shore");
        _isGlass = gameObject.name.Contains("Neo_Window_04_snaps001");
        _isGlassTable = gameObject.name.Contains("Desktop_4P_01");

        renderer = gameObject.GetComponent<Renderer>();

        if (!_deactivateMesh)
        {
            if (renderer != null){
                materials = new List<Material>(renderer.materials);
                for (int i = 0; i < materials.Count; i++)
                {
                    Material material =  materials[i];
                    MyMaterialHelper
                            .SetMaterialRenderingMode(material,
                            MyMaterialHelper.BlendMode.Fade);
                    Color c = material.color;
                    if(_isGlass && i==1) glassAlpha = c.a;
                    if(_isGlassTable && i==2) glassAlpha = c.a;
                    c.a = 0f;
                    material.color = c;
                }
            }
        }else if (renderer != null)
        {
            renderer.enabled = false;
        }

            for (
            int childIndex = 0;
            childIndex < gameObject.transform.childCount;
            childIndex++
            )
            {
                Transform child = gameObject.transform.GetChild(childIndex);
                if (!child.gameObject.name.Equals("__Light")||
                gameObject.name.Equals("RayInteractableCanvas")  )
                {
                child.gameObject.AddComponent<FaderVR>();
                child.gameObject.GetComponent<FaderVR>().Manager = Manager;  
                }


            }
        
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Manager.FadeOutVR  && renderer != null)
        {
            if(!_fadeOut){
            _fadeOut = true;
            StartCoroutine(FadeOut());
            }
        }
        if (Manager.FadeInVR && renderer != null)
        {
            if(!_fadeIn){
            _fadeIn = true;
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
            if (!_deactivateMesh )
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
                renderer.enabled = false;
            }
            _fadeOut = false;
        }
    }
    

    IEnumerator FadeIn()
    {
        // Debug.Log("Started FadeIn");
        // yield return new WaitForSeconds(6);

        if (_fadeIn)
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

                

                for(float f = 0.05f ; f <= 1f ; f+=0.05f)
                {
                    for (int i = 0; i < materials.Count; i++)
                {
                    Material material =  materials[i];      
                        Color c = material.color;
                        if(_isGlass && i == 1){
                            c.a = f/2;
                        }else if (_isGlassTable && i == 2){
                            c.a = f/50;
                        }else{
                            c.a =  f ;
                        }      
                        
                        material.color = c;      
                    }
                    yield return new WaitForSeconds(0.05f);
                }
                    for (int i = 0; i < materials.Count; i++)
                {
                    Material material =  materials[i];
                    if(!(_isGlass && i == 1) && ! (_isGlassTable && i == 2)){
                    MyMaterialHelper
                            .SetMaterialRenderingMode(material,
                            MyMaterialHelper.BlendMode.Opaque);
                    }
                }
            } else if (_deactivateMesh)
            {
                renderer.enabled = true;
            }
            
            _fadeIn = false;
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
            
        if(renderer != null){
            _fadeIn = true;
            StartCoroutine(FadeIn());
            }

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
            
        if(renderer != null){
            _fadeOut= true;
            StartCoroutine(FadeOut());
            }
    }
}
