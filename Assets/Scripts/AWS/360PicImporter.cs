using UnityEngine;
using System.IO;
using System.Linq;

public class InvertedSphereTextureApplier : MonoBehaviour
{
    public string imagesDirectory = @"D:\TP\models\nepal_jodhpuriVilla\PanoramaImages"; // Path to your images directory
    public GameObject invertedSpherePrefab; // Reference to the inverted sphere prefab
    public float distanceBetweenSpheres = 10f; // Distance between the spheres
    public Vector3 sphereScale = new Vector3(10f, 10f, 10f); // Scale of the inverted spheres

    void Start()
    {
        if (invertedSpherePrefab == null)
        {
            Debug.LogError("Inverted Sphere Prefab is not assigned.");
            return;
        }

        // Load all image files from the directory
        string[] imagePaths = Directory.GetFiles(imagesDirectory, "*.png")
                                       .OrderBy(f => int.Parse(Path.GetFileNameWithoutExtension(f)))
                                       .ToArray();

        if (imagePaths.Length == 0)
        {
            Debug.LogError("No images found in the directory.");
            return;
        }

        for (int i = 0; i < imagePaths.Length; i++)
        {
            // Instantiate the inverted sphere prefab
            GameObject sphere = Instantiate(invertedSpherePrefab, new Vector3(i * distanceBetweenSpheres, 0, 0), Quaternion.identity);

            // Scale the sphere
            sphere.transform.localScale = sphereScale;

            // Load the texture
            Texture2D texture = LoadTexture(imagePaths[i]);
            if (texture == null)
            {
                Debug.LogError($"Failed to load texture from path: {imagePaths[i]}");
                continue;
            }

            // Apply the texture to the sphere using Unlit/Texture shader
            Material material = new Material(Shader.Find("Unlit/Texture"));
            material.mainTexture = texture;
            sphere.GetComponent<Renderer>().material = material;
        }
    }

    Texture2D LoadTexture(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData); // Automatically resizes the texture
        return texture;
    }
}
