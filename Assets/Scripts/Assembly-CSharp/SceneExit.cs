using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneExit : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKey("escape"))
		{
			SceneManager.LoadScene("SchoolSceneMenu");
		}
	}
}
