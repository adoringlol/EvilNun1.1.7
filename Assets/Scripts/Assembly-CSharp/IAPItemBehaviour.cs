using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IAPItemBehaviour : MonoBehaviour
{
	public string android_sku;

	public string ios_sku;

	public TextMeshProUGUI priceTxt;

	public Button buyButton;

	public void OnClickBuyItem()
	{
		RemoveAdsWindow.instance.MakePurchase(android_sku);
	}

	public void Init(string price)
	{
		priceTxt.text = price;
		if (string.IsNullOrEmpty(price))
		{
			buyButton.interactable = false;
		}
		else
		{
			buyButton.interactable = true;
		}
	}
}
