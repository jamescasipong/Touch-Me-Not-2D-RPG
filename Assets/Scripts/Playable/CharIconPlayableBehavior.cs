using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CharIconPlayableBehavior : PlayableBehaviour
{


    public Sprite charIcon;
    public Image charImage;


    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        charImage = playerData as Image;

        if (charIcon != null)
        {
            charImage.sprite = charIcon;
        }
        
    }
}
