using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pixelate : MonoBehaviour
{
    public Material effectmaterial;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, effectmaterial);
    }
}
