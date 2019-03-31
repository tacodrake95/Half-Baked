using UnityEngine;
using System.Collections;

public class noise : MonoBehaviour {

	public Texture2D source;
	public int resolution;
	public int depth;
	public int seed;

	public void begin (){
		//set seed for randomization
		Random.InitState(seed);
		//define the new textures
		source = new Texture2D (resolution, resolution);
		//set texture modes
		source.filterMode = FilterMode.Point;
		//apply the textures to the materials
		
		//create the random pixel array
		for(int x = 0; x < resolution; x++) {
			for(int y = 0; y < resolution; y++) {
				float cValue = (float)Random.Range(0f, 1f);
				source.SetPixel(x, y, new Color(cValue, cValue, cValue));
			}
		}
	}

	public float PixelNoise(int x, int y){
		return source.GetPixel (x, y).grayscale;
	}

	public float ValueNoise (int tx, int ty){
		//get localized coordinates
		int x = tx % depth;
		int y = ty % depth;
		//get source coordinates
		int posX = Mathf.FloorToInt(tx / depth);
		int posY = Mathf.FloorToInt(ty / depth);
		//get grayscale pixel data
		float UR = source.GetPixel(posX, posY).grayscale / 2;
		float UL = source.GetPixel(posX + 1, posY).grayscale / 2;
		float LR = source.GetPixel(posX, posY + 1).grayscale / 2;
		float LL = source.GetPixel(posX + 1, posY + 1).grayscale / 2;
		//blend vertically
		float rvLerp = Mathf.Lerp (UR, LR, (float)y / ((float)depth - 1));
		float lvLerp = Mathf.Lerp (UL, LL, (float)y / ((float)depth - 1));
		//blend horizontally
		float uhLerp = Mathf.Lerp (UR, UL, (float)x / ((float)depth - 1));
		float lhLerp = Mathf.Lerp (LR, LL, (float)x / ((float)depth - 1));
		//blend horizontals vertically and verticals horizontally
		float vLerp = Mathf.Lerp (uhLerp, lhLerp, (float)y / ((float)depth - 1));
		float hLerp = Mathf.Lerp (rvLerp, lvLerp, (float)x / ((float)depth - 1));
		//final blend operation
		float fLerp = Mathf.Lerp (vLerp, hLerp, (float)x * (float)y / Mathf.Pow (depth, 2));
		return fLerp;
	}
}