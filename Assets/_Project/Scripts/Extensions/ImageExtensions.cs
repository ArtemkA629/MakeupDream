using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class ImageExtensions
{
    public static IEnumerator ChangeImageTo(this Image currentImage, Image newImage, Sprite newSprite, float duration)
    {
        newImage.sprite = newSprite;
        newImage.color = new Color(1f, 1f, 1f, 0f);
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
        
            if (currentImage.sprite != null)
                currentImage.color = new Color(1f, 1f, 1f, 1f - t);
            
            newImage.color = new Color(1f, 1f, 1f, t);
         
            yield return null;
        }
    
        currentImage.sprite = newSprite;
        currentImage.color = Color.white;
    }
}