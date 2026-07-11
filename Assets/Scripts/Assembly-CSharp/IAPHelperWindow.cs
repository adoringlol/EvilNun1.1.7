using UnityEngine;

public class IAPHelperWindow : MonoBehaviour
{
	public static IAPHelperWindow instance;

	public GameObject processingPanel;

	public GameObject purchaseDonePanel;

	public GameObject purchaseFailedPanel;

	private GameObject currentPopUp;

	private void Awake()
	{
		instance = this;
		currentPopUp = null;
		Object.DontDestroyOnLoad(base.gameObject);
	}

	public void MakePurchase(string sku)
	{
		IAPManager.instance.BuyProductID(sku);
	}

	public void ShowProcessing(bool t)
	{
		processingPanel.SetActive(t);
	}

	public void OnPurchaseFailed()
	{
		purchaseFailedPanel.gameObject.SetActive(true);
		currentPopUp = purchaseFailedPanel;
	}

	public void OnPurchaseCompleted()
	{
		purchaseDonePanel.gameObject.SetActive(true);
		currentPopUp = purchaseDonePanel;
	}

	public void CloseCurrentPopUp()
	{
		if (currentPopUp != null)
		{
			currentPopUp.gameObject.SetActive(false);
			currentPopUp = null;
		}
	}
}
