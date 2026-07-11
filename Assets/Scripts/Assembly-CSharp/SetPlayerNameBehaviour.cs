using TMPro;
using UnityEngine;

public class SetPlayerNameBehaviour : MonoBehaviour
{
	private void Start()
	{
		GetComponent<TextMeshProUGUI>().text = CarboardController.instance.matchPlayerName;
	}
}
