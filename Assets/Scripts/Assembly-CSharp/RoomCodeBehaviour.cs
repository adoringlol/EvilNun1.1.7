using System.Collections.Generic;
using DarkTonic.MasterAudio;
using TMPro;
using UnityEngine;

public class RoomCodeBehaviour : MonoBehaviour
{
	public List<int> pressed;

	public List<TextMeshProUGUI> text;

	public OpenCloseBehaviour door;

	private string currentCode;

	private void Start()
	{
		pressed = new List<int>();
		currentCode = "XXXX";
		UpdateDisplay();
	}

	public void OnPressButton(int p)
	{
		MasterAudio.PlaySound3DAtVector3("code_button_press", base.transform.position);
		if (pressed.Count < 4)
		{
			pressed.Insert(0, p);
			UpdateDisplay();
		}
	}

	public string GetCodeString()
	{
		string text = string.Empty;
		for (int num = pressed.Count - 1; num >= 0; num--)
		{
			text += pressed[num];
		}
		return text;
	}

	public void UpdateDisplay()
	{
		int num = text.Count - 1;
		foreach (int item in pressed)
		{
			text[num].text = item.ToString();
			num--;
		}
		while (num >= 0)
		{
			text[num].text = string.Empty;
			num--;
		}
	}

	public void OnPressRemoveButton()
	{
		MasterAudio.PlaySound3DAtVector3("code_button_press", base.transform.position);
		ClearDisplay();
		UpdateDisplay();
	}

	public void ClearDisplay()
	{
		pressed.Clear();
	}

	public void OnPressAcceptButton()
	{
		MasterAudio.PlaySound3DAtVector3("code_button_press", base.transform.position);
		switch (CompareWithRoomCode(GetCodeString()))
		{
		case 1:
			MessagesManager.instance.ShowMessage("correct_code");
			door.state = OpenCloseBehaviour.openState.CLOSED;
			door.Touched();
			break;
		case 0:
			MasterAudio.PlaySound3DAtVector3("Alarm_Fail", base.transform.position);
			if (ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.AddNoise(base.transform.position, 80f, string.Empty);
			}
			Debug.Log("The code is not correct message");
			MessagesManager.instance.ShowMessage("incorrect_code");
			ClearDisplay();
			UpdateDisplay();
			break;
		case 2:
			if (ZombieBehaviour.instance != null)
			{
				ZombieBehaviour.instance.AddNoise(base.transform.position, 80f, string.Empty);
			}
			MasterAudio.PlaySound3DAtVector3("Alarm_Fail", base.transform.position);
			Debug.Log("The code is not finished...");
			MessagesManager.instance.ShowMessage("incomplete_code");
			break;
		}
	}

	public int CompareWithRoomCode(string compareCode)
	{
		if (compareCode == currentCode)
		{
			return 1;
		}
		if (compareCode.Length < 4)
		{
			return 2;
		}
		return 0;
	}

	public void SetCode(string c)
	{
		currentCode = c;
	}
}
