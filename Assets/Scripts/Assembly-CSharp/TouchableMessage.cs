using DarkTonic.MasterAudio;

public class TouchableMessage : TouchableElement
{
	public string key;

	public bool playSound;

	[SoundGroup]
	public string sound;

	public float seconds = 5f;

	public override void Touched(bool byPlayer = true, bool trapPlayer = false)
	{
		if (key != null)
		{
			MessagesManager.instance.ShowMessage(key, seconds);
		}
		if (playSound)
		{
			MasterAudio.PlaySound3DAtVector3(sound, base.transform.position);
		}
	}
}
