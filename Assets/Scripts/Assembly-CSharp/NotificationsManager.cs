using System;
using Assets.SimpleAndroidNotifications;
using Assets.SimpleAndroidNotifications.Data;
using Assets.SimpleAndroidNotifications.Enums;
using Assets.SimpleAndroidNotifications.Helpers;
using I2.Loc;
using UnityEngine;

public class NotificationsManager : MonoBehaviour
{
	public static NotificationsManager instance;

	public bool notificationsEnabled;

	public void Awake()
	{
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		CheckOpenedFromNotification();
	}

	public void ScheduleSimple(int seconds)
	{
		// PC build: local notifications are Android-native (AndroidJNI). Skip on Standalone.
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		NotificationManager.Send(TimeSpan.FromSeconds(seconds), LocalizationManager.GetTranslation("notification_01_title"), LocalizationManager.GetTranslation("notification_01_message"), new Color(1f, 0.3f, 0.15f));
	}

	public void CheckOpenedFromNotification()
	{
		// PC build: no Android notification intent to read.
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (NotificationManager.GetNotificationCallback() != null)
		{
			int pictures_unlocked = PlayerPrefs.GetInt("museum_lights", 0);
			AnalyticsController.instance.OnOpenGameFromNotification("00-story-unlock", "out-game", pictures_unlocked);
		}
	}

	public void OnApplicationPause(bool pause)
	{
		// PC build: no Android notification intent to read.
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (!pause && NotificationManager.GetNotificationCallback() != null)
		{
			int pictures_unlocked = PlayerPrefs.GetInt("museum_lights", 0);
			AnalyticsController.instance.OnOpenGameFromNotification("00-story-unlock", "in-game", pictures_unlocked);
		}
	}

	public void CancelAll()
	{
		// PC build: local notifications are Android-native (AndroidJNI). Skip on Standalone.
		if (Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		NotificationManager.CancelAll();
	}

	public void ScheduleNormal()
	{
		NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(5.0), "Notification", "Notification with app icon", new Color(0f, 0.6f, 1f), NotificationIcon.Message);
	}

	public void ScheduleRepeated()
	{
		NotificationParams notificationParams = new NotificationParams();
		notificationParams.Id = NotificationIdHandler.GetNotificationId();
		notificationParams.Delay = TimeSpan.FromSeconds(5.0);
		notificationParams.Title = "Repeated notification";
		notificationParams.Message = "Please rate the asset on the Asset Store!";
		notificationParams.Ticker = "This is repeated message ticker!";
		notificationParams.Sound = true;
		notificationParams.Vibrate = true;
		notificationParams.Vibration = new int[6] { 500, 500, 500, 500, 500, 500 };
		notificationParams.Light = true;
		notificationParams.LightOnMs = 1000;
		notificationParams.LightOffMs = 1000;
		notificationParams.LightColor = Color.magenta;
		notificationParams.SmallIcon = NotificationIcon.Skull;
		notificationParams.SmallIconColor = new Color(0f, 0.5f, 0f);
		notificationParams.LargeIcon = "app_icon";
		notificationParams.ExecuteMode = NotificationExecuteMode.Inexact;
		notificationParams.Repeat = true;
		notificationParams.RepeatInterval = TimeSpan.FromSeconds(30.0);
		NotificationParams notificationParams2 = notificationParams;
		NotificationManager.SendCustom(notificationParams2);
	}

	public void ScheduleMultiline()
	{
		NotificationParams notificationParams = new NotificationParams();
		notificationParams.Id = NotificationIdHandler.GetNotificationId();
		notificationParams.Delay = TimeSpan.FromSeconds(5.0);
		notificationParams.Title = "Multiline notification";
		notificationParams.Message = "Line#1\nLine#2\nLine#3\nLine#4";
		notificationParams.Ticker = "This is multiline message ticker!";
		notificationParams.Multiline = true;
		NotificationParams notificationParams2 = notificationParams;
		NotificationManager.SendCustom(notificationParams2);
	}

	public void ScheduleGrouped()
	{
		int notificationId = NotificationIdHandler.GetNotificationId();
		NotificationParams notificationParams = new NotificationParams();
		notificationParams.Id = notificationId;
		notificationParams.GroupName = "Group";
		notificationParams.GroupSummary = "{0} new messages";
		notificationParams.Delay = TimeSpan.FromSeconds(5.0);
		notificationParams.Title = "Grouped notification";
		notificationParams.Message = "Message " + notificationId;
		notificationParams.Ticker = "Please rate the asset on the Asset Store!";
		NotificationParams notificationParams2 = notificationParams;
		NotificationManager.SendCustom(notificationParams2);
	}

	public void ScheduleCustom()
	{
		NotificationParams notificationParams = new NotificationParams();
		notificationParams.Id = NotificationIdHandler.GetNotificationId();
		notificationParams.Delay = TimeSpan.FromSeconds(5.0);
		notificationParams.Title = "Notification with callback";
		notificationParams.Message = "Open app and check the checkbox!";
		notificationParams.Ticker = "Notification with callback";
		notificationParams.Sound = true;
		notificationParams.Vibrate = true;
		notificationParams.Vibration = new int[6] { 500, 500, 500, 500, 500, 500 };
		notificationParams.Light = true;
		notificationParams.LightOnMs = 1000;
		notificationParams.LightOffMs = 1000;
		notificationParams.LightColor = Color.red;
		notificationParams.SmallIcon = NotificationIcon.Sync;
		notificationParams.SmallIconColor = new Color(0f, 0.5f, 0f);
		notificationParams.LargeIcon = "app_icon";
		notificationParams.ExecuteMode = NotificationExecuteMode.Inexact;
		notificationParams.Importance = NotificationImportance.Max;
		notificationParams.CallbackData = "notification created at " + DateTime.Now;
		NotificationParams notificationParams2 = notificationParams;
		NotificationManager.SendCustom(notificationParams2);
	}

	public void ScheduleWithChannel()
	{
		NotificationParams notificationParams = new NotificationParams();
		notificationParams.Id = NotificationIdHandler.GetNotificationId();
		notificationParams.Delay = TimeSpan.FromSeconds(5.0);
		notificationParams.Title = "Notification with news channel";
		notificationParams.Message = "Check the channel in your app settings!";
		notificationParams.Ticker = "Notification with news channel";
		notificationParams.ChannelId = "com.company.app.news";
		notificationParams.ChannelName = "News";
		NotificationParams notificationParams2 = notificationParams;
		NotificationManager.SendCustom(notificationParams2);
	}
}
