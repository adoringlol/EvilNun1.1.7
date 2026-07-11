using DarkTonic.MasterAudio;
using UnityEngine;

public class LaundryAnimationEvents : MonoBehaviour
{
	public Transform camPosition;

	public void EnableMask()
	{
		CinematicsManager.instance.mask.SetActive(true);
		CinematicsManager.instance.nunMask.SetActive(false);
	}

	public void HitGround()
	{
		MasterAudio.PlaySound3DAtVector3("fallbody1", camPosition.position);
	}
}
