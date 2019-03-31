using UnityEngine;
using UnityEditor;
using System.Collections;



public class mesh : MonoBehaviour {
	public GameObject newChunk(int posX, int posZ, int width, float[,] heightMap, Material mat, float scalingFactor)
	{
        // Create mesh
		Mesh m = new Mesh();
		Vector3[] verts = new Vector3[width * width];
		for (int x = 0; x < width; x++) {
			for (int z = 0; z < width; z++) {
				int i = x + z * width;
				verts [i] = new Vector3 (x, heightMap[x, z] * scalingFactor, z);
			}
		}
		m.name = "ChunkMesh";
		m.vertices = verts;

        // Set UVs
		Vector2[] uv = new Vector2[verts.Length];
		for(int i = 0; i < verts.Length; i++){
			uv[i] = new Vector2(verts[i].x, verts[i].z);
		}
		m.uv = uv;

        // Generate triangles
		int[] tris = new int[width * width * 6];
		int index = 0;
		for (int x = 0; x < width - 1; x++) {
			for (int z = 0; z < width - 1; z++) {
				index = z + x * width;
				tris[index * 6] = index;
				tris[index * 6 + 1] = index + width;
				tris[index * 6 + 2] = index + 1;
				tris[index * 6 + 3] = index + 1;
				tris[index * 6 + 4] = index + width;
				tris[index * 6 + 5] = index + 1 + width;
			}
		}
		m.triangles = tris;
		m.RecalculateNormals();

        // Set up chunk object
		GameObject plane = new GameObject ("chunk");
		MeshFilter meshFilter = (MeshFilter)plane.AddComponent (typeof(MeshFilter));
		meshFilter.mesh = m;
		MeshRenderer renderer = plane.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material = mat;
		plane.transform.Translate(posX, 0, posZ);
		MeshCollider collider = (MeshCollider)plane.AddComponent (typeof(MeshCollider));
		collider.sharedMesh = m;
		return plane;
	}
}
