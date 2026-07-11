using System.Collections.Generic;
using DarkTonic.MasterAudio;
using UnityEngine;
using UnityEngine.Purchasing;

public class RemoveAdsWindow : MonoBehaviour
{
	public static RemoveAdsWindow instance;

	public GameObject processingPanel;

	public GameObject purchaseDonePanel;

	public GameObject purchaseFailedPanel;

	public GameObject window;

	public GameObject currentPopUp;

	public List<IAPItemBehaviour> iaps;

	private void Awake()
	{
		instance = this;
		currentPopUp = null;
	}

	public void ShowWindow(bool t)
	{
		MasterAudio.PlaySound("button");
		if (!window.activeSelf)
		{
			if (LoadPrices())
			{
				window.SetActive(t);
			}
		}
		else
		{
			window.SetActive(t);
		}
	}

	public void Later()
	{
		CloseCurrentPopUp();
		ShowWindow(false);
	}

	public bool LoadPrices()
	{
		if (IAPManager.instance.IsInitialized)
		{
			foreach (Product p in IAPManager.instance.available_products)
			{
				IAPItemBehaviour iAPItemBehaviour = null;
				iAPItemBehaviour = iaps.Find((IAPItemBehaviour iap) => iap.android_sku == p.definition.id);
				if (iAPItemBehaviour != null && !string.IsNullOrEmpty(p.metadata.localizedPriceString))
				{
					iAPItemBehaviour.Init(p.metadata.localizedPriceString);
				}
				else
				{
					iAPItemBehaviour.Init(string.Empty);
				}
			}
			return true;
		}
		return false;
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
			if (currentPopUp == purchaseDonePanel)
			{
				ShowWindow(false);
			}
			currentPopUp.gameObject.SetActive(false);
			currentPopUp = null;
		}
	}
}
