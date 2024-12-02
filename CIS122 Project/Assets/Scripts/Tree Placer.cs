using UnityEngine;
using UnityEngine.UI; // Include this for UI elements

public class TreePlacer : MonoBehaviour
{
    public Terrain terrain; // Assign your terrain in the Inspector
    public GameObject treePrefab; // Assign your tree prefab in the Inspector
    public int treeCount = 10; // Number of trees to place
    public float minSlope = 0.01f;
    public float maxSlope = 45f;

    // Replace with the indices of your cliff textures
    private int[] cliffTextureIndices = { 3 };

    // This method will be called when the button is pressed
    public void PlaceTrees()
    {
        TerrainData terrainData = terrain.terrainData;

        for (int i = 0; i < treeCount; i++) // check
        {
            // Generate a random position on the terrain
            float randomX = Random.Range(0f, 1f);
            float randomZ = Random.Range(0f, 1f);
            float terrainHeight = terrainData.GetHeight((int)(randomX * terrainData.heightmapResolution),
                                                        (int)(randomZ * terrainData.heightmapResolution));
            Vector3 position = new Vector3(randomX * terrainData.size.x, terrainHeight, randomZ * terrainData.size.z);

            // Check the slope at this position
            float slope = terrainData.GetSteepness(randomX, randomZ);
            if (slope < minSlope || slope > maxSlope) continue;

            // Check if the texture at this position is not a cliff
            int terrainX = Mathf.FloorToInt(randomX * terrainData.alphamapWidth);
            int terrainZ = Mathf.FloorToInt(randomZ * terrainData.alphamapHeight);
            float[,,] alphaMap = terrainData.GetAlphamaps(terrainX, terrainZ, 1, 1);

            bool isCliff = false;
            for (int j = 0; j < cliffTextureIndices.Length; j++)
            {
                if (alphaMap[0, 0, cliffTextureIndices[j]] > 0.5f) // Check if any cliff texture weight is high
                {
                    isCliff = true;
                    break; // Exit loop if we find a cliff texture
                }
            }

            if (isCliff) continue; // Skip placing trees on cliff textures

            // Create a new TreeInstance
            TreeInstance treeInstance = new TreeInstance
            {
                position = new Vector3(randomX, terrainHeight / terrainData.size.y, randomZ), // Normalized position
                prototypeIndex = 0, // Ensure this matches the index of the tree prototype
                widthScale = 1,
                heightScale = 1,
                color = Color.white,
                lightmapColor = Color.white
            };

            // Add the tree instance to the terrain
            terrain.AddTreeInstance(treeInstance);
        }
    }
}