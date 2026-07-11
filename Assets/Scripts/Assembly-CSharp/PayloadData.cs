using UnityEngine;

public class PayloadData
{
	public JsonData JsonData;

	public string signature;

	public string json;

	public static PayloadData FromJson(string json)
	{
		PayloadData payloadData = JsonUtility.FromJson<PayloadData>(json);
		payloadData.JsonData = JsonUtility.FromJson<JsonData>(payloadData.json);
		return payloadData;
	}
}
