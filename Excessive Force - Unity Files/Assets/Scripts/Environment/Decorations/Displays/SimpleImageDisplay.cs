using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleImageDisplay : MonoBehaviour
{
    [SerializeField]
    private DisplayController targetDisplay;

    [SerializeField]
    private Texture2D imageToDisplay;

    // Start is called before the first frame update
    void Start()
    {
        targetDisplay.SetScreenImage(imageToDisplay);
    }
}
