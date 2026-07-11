using DarkTonic.MasterAudio;
using UnityEngine;

public class GenuItemBehaviour : TouchableMessage
{
	public GameObject fullPizza;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		MasterAudio.PlaySound("hide");
		base.gameObject.SetActive(false);
	}

	private void OnCollisionEnter(Collision col)
	{
		MasterAudio.PlaySound3DAtVector3("caer_objeto_03", base.transform.position);
	}
}
