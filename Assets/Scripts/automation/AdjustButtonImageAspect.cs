using UnityEngine;
using UnityEngine.UI;

public class AdjustButtonImageAspect : MonoBehaviour
{
    public void SetImage(Texture2D texture)
    {
        Image buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            Rect rect = new Rect(0, 0, texture.width, texture.height);
            Vector2 pivot = new Vector2(0.5f, 0.5f);
            Sprite sprite = Sprite.Create(texture, rect, pivot);
            buttonImage.sprite = sprite;

            // Adjust aspect ratio
            float aspectRatio = (float)texture.width / (float)texture.height;
            RectTransform rt = buttonImage.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.x / aspectRatio);
            }
        }
        else
        {
            Debug.LogError("Image component not found on the button.");
        }
    }
}
