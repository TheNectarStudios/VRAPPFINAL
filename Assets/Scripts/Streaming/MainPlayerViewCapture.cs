using UnityEngine;

public class MainPlayerViewCapture : MonoBehaviour
{
    public RenderTexture mainPlayerViewTexture; // The RenderTexture to capture the main player's view

    void Update()
    {
        // Check if the mainPlayerViewTexture is assigned
        if (mainPlayerViewTexture != null)
        {
            // Ensure the RenderTexture is the same size as the screen
            if (mainPlayerViewTexture.width != Screen.width || mainPlayerViewTexture.height != Screen.height)
            {
                mainPlayerViewTexture.Release(); // Release the current RenderTexture
                mainPlayerViewTexture.width = Screen.width; // Set the width to match the screen
                mainPlayerViewTexture.height = Screen.height; // Set the height to match the screen
                mainPlayerViewTexture.Create(); // Recreate the RenderTexture
            }

            // Render the main player's view onto the RenderTexture
            Camera.main.targetTexture = mainPlayerViewTexture;
            Camera.main.Render();
            Camera.main.targetTexture = null; // Reset the target texture to prevent rendering issues
        }
        else
        {
            Debug.LogError("MainPlayerViewTexture is not assigned!");
        }
    }
}
