using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RenderersWithTexture
{
	public Texture tex;

	[SerializeField]
	public List<Renderer> renderers;

	public Material mat;

	public RenderersWithTexture(Texture t, Renderer r)
	{
		tex = t;
		renderers = new List<Renderer>();
		renderers.Add(r);
	}

	public void AddRenderer(Renderer r)
	{
		renderers.Add(r);
	}
}
