using UnityEngine;

public class MiawaugItemBehaviour : MonoBehaviour
{
	private void Start()
	{
		if (!VariablesGlobales.ownName || !VariablesGlobales.playerOwnName.ToLower().Equals("miawaug"))
		{
			base.gameObject.SetActive(false);
		}
	}
}
