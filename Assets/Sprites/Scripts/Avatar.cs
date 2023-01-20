using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Avatar : MonoBehaviour
{
    private Image image;
    [SerializeField] private Sprite[,] sprites;
    // Start is called before the first frame update
    void Start()
    {
      string path = "";
      image = gameObject.GetComponent<Image>(); 
      sprites = new Sprite[4,3];
      for (int i = 0; i < sprites.GetLength(0); i++)
      {
        for (int j = 0; j < sprites.GetLength(1); j++)
        {
          switch (i)
          {
            case 0:
            path = $"Sprites/avatar-up-{j+1}";
            break;
            case 1:
            path = $"Sprites/avatar-right-{j+1}";
            break;
            case 2:
            path = $"Sprites/avatar-down-{j+1}";
            break;
            case 3:
            path = $"Sprites/avatar-left-{j+1}";
            break;
            
          }
          sprites[i,j] = Resources.Load<Sprite>(path);
        }
      }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    // Directions: 0 = Up, 1 = Right, 2 = Down, 3 = Left 
    // Sizes: 0 = Small, 1 = Medium, 2 = Big
    public void SelectAvatar(int direction, int size){
      SoundManager.PlayPingSound();
      image.sprite = sprites[direction,size];
    }
}
