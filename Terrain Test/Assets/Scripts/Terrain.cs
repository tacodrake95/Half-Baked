using UnityEngine;
using System.Collections;

public class Terrain : MonoBehaviour {

	//modifyable settings
	public Vector2 chunkSize;
	
	public Vector2 worldSize;
	
	public int terrainDetailFactor;

	public int seed;

	public Material mat;

    public float scalingFactor;

    //private variables
    private GameObject[] chunk;

	private float[,] heightMap;
	
	private noise noise;

	private mesh chunkMesh;

    // Init Code
    IEnumerator Start () {

        // Generate noise profile
		noise = (noise)gameObject.AddComponent <noise>();
		noise.seed = seed;
		noise.resolution = (int)Mathf.Max((2 * worldSize.x) * chunkSize.x, ( 2 * worldSize.y) * chunkSize.x) / terrainDetailFactor;
		noise.depth = terrainDetailFactor;

        // Generate chunk data
		chunk = new GameObject[(int)worldSize.x * (int)worldSize.y];
        chunkMesh = (mesh)gameObject.AddComponent <mesh>();

        // Generate noise data
		noise.begin ();
        heightMap = new float[(int)chunkSize.x + 1, (int)chunkSize.x + 1];
        for (int wx = 0; wx < worldSize.x * 2; wx++) {
			for (int wy = 0; wy < worldSize.y * 2; wy++) {
				genChunk (wx * (int)chunkSize.x, wy * (int)chunkSize.x);
				yield return 1;
			}
		}

	}

    // For generating a chunk at a given position
	void genChunk(int posX, int posZ){
		for (int x = 0; x < chunkSize.x + 1; x++) {
			for (int z = 0; z < chunkSize.x + 1; z++) {
                // Loop through and create each vertex
				int t = 12;
				float valueNoise = 0;
				for(int i = 1; i <= t; i++){
					valueNoise += noise.ValueNoise((int)((x + posX) / (i * 0.25)), (int)((z + posZ) / (i * 0.25)));
				}
                heightMap [x, z] = (valueNoise / t) * chunkSize.y - chunkSize.y / 4;
			}
		}
        // Generate the actual chunk
		chunkMesh.newChunk(posX - (int)(worldSize.x * chunkSize.x), posZ - (int)(worldSize.y * chunkSize.x), (int)chunkSize.x + 1, heightMap, mat, scalingFactor);
	}
}
