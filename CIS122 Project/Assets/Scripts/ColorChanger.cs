using UnityEngine;

public class InstantiateWithUniqueMaterial : MonoBehaviour
{
    public GameObject prefab;

    void Start()
    {
        // Instantiate the prefab
        GameObject instance = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        // Modify the 4th material of the instance
        Renderer renderer = instance.GetComponent<Renderer>();
        if (renderer.materials.Length > 3)
        {
            Material uniqueMaterial = new Material(renderer.materials[3]);
            Material[] materials = renderer.materials;
            materials[3] = uniqueMaterial;
            renderer.materials = materials;

            // Set a random color for the unique material
            uniqueMaterial.color = Random.ColorHSV();
        }
    }
}
