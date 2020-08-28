using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class DialogueCameraMasker : MonoBehaviour
{
    [SerializeField] private Material cameraMaskMat;
    [SerializeField] private RenderTexture targetTexture;

    public void UpdateMask(Vector2Int windowSize)
    {
        targetTexture = new RenderTexture(windowSize.x, windowSize.y, targetTexture.depth);

        cameraMaskMat.SetVector("_ScreenDims", new Vector4(windowSize.x, windowSize.y, 0 ,0));

        DialogueBoxController.Instance.camImage.texture = targetTexture;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, targetTexture, cameraMaskMat);
    }
}
