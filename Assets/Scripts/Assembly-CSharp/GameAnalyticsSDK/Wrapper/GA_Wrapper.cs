using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK.State;
using GameAnalyticsSDK.Utilities;
using UnityEngine;

namespace GameAnalyticsSDK.Wrapper
{
	public class GA_Wrapper
	{
		private static readonly AndroidJavaClass GA = new AndroidJavaClass("com.gameanalytics.sdk.GameAnalytics");

		private static readonly AndroidJavaClass UNITY_GA = new AndroidJavaClass("com.gameanalytics.sdk.unity.UnityGameAnalytics");

		private static void configureAvailableCustomDimensions01(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object item in list2)
			{
				arrayList.Add(item);
			}
			GA.CallStatic("configureAvailableCustomDimensions01", arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions02(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object item in list2)
			{
				arrayList.Add(item);
			}
			GA.CallStatic("configureAvailableCustomDimensions02", arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableCustomDimensions03(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object item in list2)
			{
				arrayList.Add(item);
			}
			GA.CallStatic("configureAvailableCustomDimensions03", arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceCurrencies(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object item in list2)
			{
				arrayList.Add(item);
			}
			GA.CallStatic("configureAvailableResourceCurrencies", arrayList.ToArray(typeof(string)));
		}

		private static void configureAvailableResourceItemTypes(string list)
		{
			IList<object> list2 = GA_MiniJSON.Deserialize(list) as IList<object>;
			ArrayList arrayList = new ArrayList();
			foreach (object item in list2)
			{
				arrayList.Add(item);
			}
			GA.CallStatic("configureAvailableResourceItemTypes", arrayList.ToArray(typeof(string)));
		}

		private static void configureSdkGameEngineVersion(string unitySdkVersion)
		{
			GA.CallStatic("configureSdkGameEngineVersion", unitySdkVersion);
		}

		private static void configureGameEngineVersion(string unityEngineVersion)
		{
			GA.CallStatic("configureGameEngineVersion", unityEngineVersion);
		}

		private static void configureBuild(string build)
		{
			GA.CallStatic("configureBuild", build);
		}

		private static void configureUserId(string userId)
		{
			GA.CallStatic("configureUserId", userId);
		}

		private static void initialize(string gamekey, string gamesecret)
		{
			UNITY_GA.CallStatic("initialize");
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject androidJavaObject = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.gameanalytics.sdk.GAPlatform");
			androidJavaClass2.CallStatic("initializeWithActivity", androidJavaObject);
			GA.CallStatic("initializeWithGameKey", gamekey, gamesecret);
		}

		private static void setCustomDimension01(string customDimension)
		{
			GA.CallStatic("setCustomDimension01", customDimension);
		}

		private static void setCustomDimension02(string customDimension)
		{
			GA.CallStatic("setCustomDimension02", customDimension);
		}

		private static void setCustomDimension03(string customDimension)
		{
			GA.CallStatic("setCustomDimension03", customDimension);
		}

		private static void addBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, string fields)
		{
			GA.CallStatic("addBusinessEventWithCurrency", currency, amount, itemType, itemId, cartType);
		}

		private static void addBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature, string fields)
		{
			GA.CallStatic("addBusinessEventWithCurrency", currency, amount, itemType, itemId, cartType, receipt, store, signature);
		}

		private static void addResourceEvent(int flowType, string currency, float amount, string itemType, string itemId, string fields)
		{
			GA.CallStatic("addResourceEventWithFlowType", flowType, currency, amount, itemType, itemId);
		}

		private static void addProgressionEvent(int progressionStatus, string progression01, string progression02, string progression03, string fields)
		{
			GA.CallStatic("addProgressionEventWithProgressionStatus", progressionStatus, progression01, progression02, progression03);
		}

		private static void addProgressionEventWithScore(int progressionStatus, string progression01, string progression02, string progression03, int score, string fields)
		{
			GA.CallStatic("addProgressionEventWithProgressionStatus", progressionStatus, progression01, progression02, progression03, (double)score);
		}

		private static void addDesignEvent(string eventId, string fields)
		{
			GA.CallStatic("addDesignEventWithEventId", eventId);
		}

		private static void addDesignEventWithValue(string eventId, float value, string fields)
		{
			GA.CallStatic("addDesignEventWithEventId", eventId, (double)value);
		}

		private static void addErrorEvent(int severity, string message, string fields)
		{
			GA.CallStatic("addErrorEventWithSeverity", severity, message);
		}

		private static void setEnabledInfoLog(bool enabled)
		{
			GA.CallStatic("setEnabledInfoLog", enabled);
		}

		private static void setEnabledVerboseLog(bool enabled)
		{
			GA.CallStatic("setEnabledVerboseLog", enabled);
		}

		private static void setFacebookId(string facebookId)
		{
			GA.CallStatic("setFacebookId", facebookId);
		}

		private static void setGender(string gender)
		{
			switch (gender)
			{
			case "male":
				GA.CallStatic("setGender", 1);
				break;
			case "female":
				GA.CallStatic("setGender", 2);
				break;
			}
		}

		private static void setBirthYear(int birthYear)
		{
			GA.CallStatic("setBirthYear", birthYear);
		}

		private static void setManualSessionHandling(bool enabled)
		{
			GA.CallStatic("setEnabledManualSessionHandling", enabled);
		}

		private static void gameAnalyticsStartSession()
		{
			GA.CallStatic("startSession");
		}

		private static void gameAnalyticsEndSession()
		{
			GA.CallStatic("endSession");
		}

		private static string getCommandCenterValueAsString(string key, string defaultValue)
		{
			return GA.CallStatic<string>("getCommandCenterValueAsString", new object[2] { key, defaultValue });
		}

		private static bool isCommandCenterReady()
		{
			return GA.CallStatic<bool>("isCommandCenterReady", new object[0]);
		}

		private static string getConfigurationsContentAsString()
		{
			return GA.CallStatic<string>("getConfigurationsContentAsString", new object[0]);
		}

		public static void SetAvailableCustomDimensions01(string list)
		{
			configureAvailableCustomDimensions01(list);
		}

		public static void SetAvailableCustomDimensions02(string list)
		{
			configureAvailableCustomDimensions02(list);
		}

		public static void SetAvailableCustomDimensions03(string list)
		{
			configureAvailableCustomDimensions03(list);
		}

		public static void SetAvailableResourceCurrencies(string list)
		{
			configureAvailableResourceCurrencies(list);
		}

		public static void SetAvailableResourceItemTypes(string list)
		{
			configureAvailableResourceItemTypes(list);
		}

		public static void SetUnitySdkVersion(string unitySdkVersion)
		{
			configureSdkGameEngineVersion(unitySdkVersion);
		}

		public static void SetUnityEngineVersion(string unityEngineVersion)
		{
			configureGameEngineVersion(unityEngineVersion);
		}

		public static void SetBuild(string build)
		{
			configureBuild(build);
		}

		public static void SetCustomUserId(string userId)
		{
			configureUserId(userId);
		}

		public static void SetEnabledManualSessionHandling(bool enabled)
		{
			setManualSessionHandling(enabled);
		}

		public static void StartSession()
		{
			if (GAState.IsManualSessionHandlingEnabled())
			{
				gameAnalyticsStartSession();
			}
			else
			{
				Debug.Log("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		public static void EndSession()
		{
			if (GAState.IsManualSessionHandlingEnabled())
			{
				gameAnalyticsEndSession();
			}
			else
			{
				Debug.Log("Manual session handling is not enabled. \nPlease check the \"Use manual session handling\" option in the \"Advanced\" section of the Settings object.");
			}
		}

		public static void Initialize(string gamekey, string gamesecret)
		{
			initialize(gamekey, gamesecret);
		}

		public static void SetCustomDimension01(string customDimension)
		{
			setCustomDimension01(customDimension);
		}

		public static void SetCustomDimension02(string customDimension)
		{
			setCustomDimension02(customDimension);
		}

		public static void SetCustomDimension03(string customDimension)
		{
			setCustomDimension03(customDimension);
		}

		public static void AddBusinessEventWithReceipt(string currency, int amount, string itemType, string itemId, string cartType, string receipt, string store, string signature, IDictionary<string, object> fields)
		{
			string fields2 = DictionaryToJsonString(fields);
			addBusinessEventWithReceipt(currency, amount, itemType, itemId, cartType, receipt, store, signature, fields2);
		}

		public static void AddBusinessEvent(string currency, int amount, string itemType, string itemId, string cartType, IDictionary<string, object> fields)
		{
			string fields2 = DictionaryToJsonString(fields);
			addBusinessEvent(currency, amount, itemType, itemId, cartType, fields2);
		}

		public static void AddResourceEvent(GAResourceFlowType flowType, string currency, float amount, string itemType, string itemId, IDictionary<string, object> fields)
		{
			string fields2 = DictionaryToJsonString(fields);
			addResourceEvent((int)flowType, currency, amount, itemType, itemId, fields2);
		}

		public static void AddProgressionEvent(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, IDictionary<string, object> fields)
		{
			string fields2 = DictionaryToJsonString(fields);
			addProgressionEvent((int)progressionStatus, progression01, progression02, progression03, fields2);
		}

		public static void AddProgressionEventWithScore(GAProgressionStatus progressionStatus, string progression01, string progression02, string progression03, int score, IDictionary<string, object> fields)
		{
			string fields2 = DictionaryToJsonString(fields);
			addProgressionEventWithScore((int)progressionStatus, progression01, progression02, progression03, score, fields2);
		}

		public static void AddDesignEvent(string eventID, float eventValue, IDictionary<string, object> fields)
		{
			string fields2 = DictionaryToJsonString(fields);
			addDesignEventWithValue(eventID, eventValue, fields2);
		}

		public static void AddDesignEvent(string eventID, IDictionary<string, object> fields)
		{
			string fields2 = DictionaryToJsonString(fields);
			addDesignEvent(eventID, fields2);
		}

		public static void AddErrorEvent(GAErrorSeverity severity, string message, IDictionary<string, object> fields)
		{
			string fields2 = DictionaryToJsonString(fields);
			addErrorEvent((int)severity, message, fields2);
		}

		public static void SetInfoLog(bool enabled)
		{
			setEnabledInfoLog(enabled);
		}

		public static void SetVerboseLog(bool enabled)
		{
			setEnabledVerboseLog(enabled);
		}

		public static void SetFacebookId(string facebookId)
		{
			setFacebookId(facebookId);
		}

		public static void SetGender(string gender)
		{
			setGender(gender);
		}

		public static void SetBirthYear(int birthYear)
		{
			setBirthYear(birthYear);
		}

		public static string GetCommandCenterValueAsString(string key, string defaultValue)
		{
			return getCommandCenterValueAsString(key, defaultValue);
		}

		public static bool IsCommandCenterReady()
		{
			return isCommandCenterReady();
		}

		public static string GetConfigurationsContentAsString()
		{
			return getConfigurationsContentAsString();
		}

		private static string DictionaryToJsonString(IDictionary<string, object> dict)
		{
			Hashtable hashtable = new Hashtable();
			if (dict != null)
			{
				foreach (KeyValuePair<string, object> item in dict)
				{
					hashtable.Add(item.Key, item.Value);
				}
			}
			return GA_MiniJSON.Serialize(hashtable);
		}
	}
}
