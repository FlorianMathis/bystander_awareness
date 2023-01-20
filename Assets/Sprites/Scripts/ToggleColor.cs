using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleColor : MonoBehaviour
{
        private Toggle toggle;
 
     void Start()
     {
         toggle = GetComponent<Toggle>();
         toggle.onValueChanged.AddListener(OnToggleValueChanged);
     }
 
     void OnToggleValueChanged(bool isOn)
     {
         ColorBlock cb = toggle.colors;
         if (isOn)
         {
             cb.normalColor = Color.green;
             cb.highlightedColor = Color.green;
         }
         else
         {
             cb.normalColor = Color.black;
             cb.highlightedColor = Color.black;
         }
         toggle.colors = cb;
     }
}
