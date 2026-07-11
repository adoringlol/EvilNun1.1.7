using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
	public string SceneName;

	public void ButtonClick()
	{
		SceneManager.LoadScene(SceneName);
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKey("escape"))
		{
			Application.Quit();
		}
	}
}
