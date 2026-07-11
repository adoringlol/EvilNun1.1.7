using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
	public Camera mainCamera;

	private void Start()
	{
		if (mainCamera == null)
		{
			mainCamera = Camera.main;
		}
		if (mainCamera == null)
		{
			return;
		}
		if (VariablesGlobales.difficultyMode == 3)
		{
			mainCamera.farClipPlane = 35f;
			if (ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.distanceToSeePlayer = 15f;
			}
			VariablesGlobales.distanceToInteract = 1.6f;
			RenderSettings.fog = false;
		}
		else if (VariablesGlobales.difficultyMode == 2)
		{
			mainCamera.farClipPlane = 5f;
			if (ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.distanceToSeePlayer = 4.5f;
			}
			VariablesGlobales.distanceToInteract = 1.6f;
		}
		else if (VariablesGlobales.difficultyMode == 1)
		{
			mainCamera.farClipPlane = 35f;
			if (ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.distanceToSeePlayer = 15f;
			}
			VariablesGlobales.distanceToInteract = 1.6f;
			RenderSettings.fog = false;
		}
		else if (VariablesGlobales.difficultyMode == 0)
		{
			mainCamera.farClipPlane = 35f;
			RenderSettings.fog = false;
			if (ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.distanceToSeePlayer = 15f;
			}
			VariablesGlobales.distanceToInteract = 1.6f;
		}
	}

	public void EnableFog()
	{
	}
}
