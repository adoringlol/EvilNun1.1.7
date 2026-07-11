using System;
using UnityEngine;

public class CaptureScreen : MonoBehaviour
{
	public string preString = string.Empty;

	private void Update()
	{
	}

	public void Capture()
	{
		string text = DateTime.Now.ToString();
		text = text.Replace("/", "-");
		text = text.Replace(" ", "_");
		text = text.Replace(":", "-");
		ScreenCapture.CaptureScreenshot("/Users/pabloocasargonzalez/Documents/NunCaptures/" + preString + "_" + text + ".png");
	}
}
