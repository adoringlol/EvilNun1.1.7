using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
	public static IAPManager instance;

	private static IStoreController m_StoreController;

	private IExtensionProvider m_StoreExtensionProvider;

	public List<Product> available_products
	{
		get
		{
			return m_StoreController.products.all.ToList();
		}
	}

	public bool IsInitialized
	{
		get
		{
			return m_StoreController != null && m_StoreExtensionProvider != null;
		}
	}

	private void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		// PC build: no store, so skip purchasing initialization.
	}

	public void InitializePurchasing()
	{
		// PC build: in-app purchasing removed. No-op.
	}

	public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
	{
		m_StoreController = controller;
		m_StoreExtensionProvider = extensions;
		Debug.Log("OnInitializeSuccess");
	}

	public void OnInitializeFailed(InitializationFailureReason error)
	{
		Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
	}

	public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
	{
		Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		IAPHelperWindow.instance.OnPurchaseFailed();
		IAPHelperWindow.instance.ShowProcessing(false);
	}

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
	{
		IAPHelperWindow.instance.ShowProcessing(false);
		if (!IsInitialized)
		{
			IAPHelperWindow.instance.OnPurchaseFailed();
			return PurchaseProcessingResult.Complete;
		}
		if (e.purchasedProduct == null)
		{
			RemoveAdsWindow.instance.OnPurchaseFailed();
			Debug.LogWarning("Attempted to process purchasewith unknown product. Ignoring");
			return PurchaseProcessingResult.Complete;
		}
		if (string.IsNullOrEmpty(e.purchasedProduct.receipt))
		{
			IAPHelperWindow.instance.OnPurchaseFailed();
			Debug.LogWarning("Attempted to process purchase with no receipt: ignoring");
			return PurchaseProcessingResult.Complete;
		}
		Debug.Log("Processing transaction: " + e.purchasedProduct.transactionID);
		AdsManager.instance.OnPayRemoveAds();
		AnalyticsController.instance.OnRemoveAds(e.purchasedProduct);
		return PurchaseProcessingResult.Complete;
	}

	public void BuyProductID(string productId)
	{
		if (!IsInitialized)
		{
			throw new Exception("IAP Service is not initialized!");
		}
		IAPHelperWindow.instance.ShowProcessing(true);
		m_StoreController.InitiatePurchase(productId);
	}

	public void RestorePurchases()
	{
		if (!IsInitialized)
		{
			Debug.Log("RestorePurchases FAIL. Not initialized.");
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
		{
			Debug.Log("RestorePurchases started ...");
			IAppleExtensions extension = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
			StartMenuManager.instance.ShowRestoringPurchasesWindow();
			extension.RestoreTransactions(delegate(bool result)
			{
				Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				StartMenuManager.instance.HideRestoringPurchasesWindow();
			});
		}
		else
		{
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
		}
	}
}
