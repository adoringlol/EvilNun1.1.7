using I2.Loc;
using TMPro;
using UnityEngine;

public class MessagesManager : MonoBehaviour
{
	public TextMeshProUGUI label;

	public static MessagesManager instance;

	public Localize messageLocalize;

	public GameObject msgContent;

	private void Awake()
	{
		instance = this;
	}

	public void ShowMessage(string txt)
	{
		messageLocalize.enabled = true;
		messageLocalize.SetTerm(txt);
		msgContent.SetActive(true);
		CancelInvoke("HideMessage");
		Invoke("HideMessage", 5f);
	}

	public void ShowMessage(string txt, float seconds)
	{
		messageLocalize.enabled = true;
		messageLocalize.SetTerm(txt);
		msgContent.SetActive(true);
		CancelInvoke("HideMessage");
		Invoke("HideMessage", seconds);
	}

	public void ShowCustomMessage(string txt)
	{
		messageLocalize.enabled = false;
		label.text = txt;
		msgContent.SetActive(true);
		CancelInvoke("HideMessage");
		Invoke("HideMessage", 5f);
	}

	public void ShowMessage(string txt, string replace)
	{
		messageLocalize.enabled = true;
		messageLocalize.SetTerm(txt);
		label.text = label.text.Replace("{0}", replace);
		msgContent.SetActive(true);
		CancelInvoke("HideMessage");
		Invoke("HideMessage", 5f);
	}

	public void HideMessage()
	{
		msgContent.SetActive(false);
	}
}
