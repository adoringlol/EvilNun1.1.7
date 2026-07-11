using System.Collections.Generic;
using UnityEngine;

public class SceneUtilities : MonoBehaviour
{
	public List<RenderersWithTexture> textures;

	public int id;

	[ContextMenu("GetAllUsedTexturesInScene")]
	public void GetAllMaterialsInScene()
	{
		textures = new List<RenderersWithTexture>();
		Renderer[] array = Object.FindObjectsOfType<Renderer>();
		Renderer[] array2 = array;
		foreach (Renderer r in array2)
		{
			if (r.sharedMaterial != null)
			{
				if (r.sharedMaterial.mainTexture != null)
				{
					if (!textures.Exists((RenderersWithTexture t) => t.tex == r.sharedMaterial.mainTexture))
					{
						textures.Add(new RenderersWithTexture(r.sharedMaterial.mainTexture, r));
						continue;
					}
					textures.Find((RenderersWithTexture t) => t.tex == r.sharedMaterial.mainTexture).AddRenderer(r);
				}
				else
				{
					Debug.Log("Textura no definida en:" + r.gameObject.name);
				}
			}
			else
			{
				Debug.Log("Material no definido en:" + r.gameObject.name);
			}
		}
	}
}
