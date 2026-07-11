using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;

public class CarboardController : MonoBehaviour
{
	public static CarboardController instance;

	public List<TextMeshProUGUI> centerTexts;

	public TMP_FontAsset normal_font;

	public TMP_FontAsset russian_font;

	public GameObject day1Texts;

	public GameObject day2Texts;

	public GameObject day3Texts;

	public GameObject day4Texts;

	public GameObject day5Texts;

	public bool isDego;

	public bool isItown;

	public bool chivoLife;

	public bool vividPlays;

	public bool isJohnWolfe;

	public bool isGenu;

	public bool febatista;

	public bool windy31;

	public bool kubzscouts;

	public bool isCory;

	public bool isFeromonas;

	public bool isDenis;

	public bool isZbingZ;

	public bool isSpidergaming;

	public bool isfunbabe;

	public bool isLeoGames;

	public bool ispomah;

	public bool isMiauwaug;

	public bool charliecharlie;

	public bool isMomo;

	public bool isGermanSnake;

	public bool isDakblake;

	public string matchPlayerName;

	public GameObject pizzaGenu;

	public GameObject cuadroFrank;

	public GameObject bigoteFrank;

	public TakeableObject crank;

	private int current_day;

	private void Awake()
	{
		instance = this;
		if (VariablesGlobales.randomYoutubeName)
		{
			matchPlayerName = VariablesGlobales.youtuberName;
		}
		else if (VariablesGlobales.ownName)
		{
			matchPlayerName = VariablesGlobales.playerOwnName;
		}
		else
		{
			matchPlayerName = VariablesGlobales.randomName;
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("momo"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isMomo = true;
			PlayersManager.instance.child.momoItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("degoboom"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isDego = true;
			PlayersManager.instance.child.degoItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("itowngameplay"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isItown = true;
			PlayersManager.instance.child.hat.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("chibolife"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			chivoLife = true;
			PlayersManager.instance.child.chivoItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("vividplays"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			vividPlays = true;
			PlayersManager.instance.child.vividPlaysItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("john wolfe"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isJohnWolfe = true;
			PlayersManager.instance.child.johnWolfItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("genuine993"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isGenu = true;
			pizzaGenu.gameObject.SetActive(true);
			PlayersManager.instance.child.genuItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("febatista"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			febatista = true;
			PlayersManager.instance.child.febatistaDoll.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("feromonas"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isFeromonas = true;
			PlayersManager.instance.child.feromonasItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("windy31"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			windy31 = true;
			PlayersManager.instance.child.windyItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("kubzscouts"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			kubzscouts = true;
			PlayersManager.instance.child.kubzscoutsItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("coryxkenshin"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isCory = true;
			PlayersManager.instance.child.coryItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("denis"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isDenis = true;
			PlayersManager.instance.child.denisItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("zbing z."))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isZbingZ = true;
			PlayersManager.instance.child.zbingzItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("spidergaming"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isSpidergaming = true;
			PlayersManager.instance.child.spidergamingItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("funbabe"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isfunbabe = true;
			PlayersManager.instance.child.funbabeItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("leogamesandroidbr"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isLeoGames = true;
			PlayersManager.instance.child.leogamesItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("pomah"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			ispomah = true;
			PlayersManager.instance.child.pomahItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("germansnake"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isGermanSnake = true;
			PlayersManager.instance.child.germansnakeItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("dakblake"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isDakblake = true;
			cuadroFrank.SetActive(true);
			bigoteFrank.SetActive(true);
			crank.labelName = "dakblake_item";
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("miawaug"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			isMiauwaug = true;
			PlayersManager.instance.child.miawaugItem.SetActive(true);
		}
		if ((VariablesGlobales.ownName || VariablesGlobales.randomYoutubeName) && matchPlayerName.ToLower().Equals("charlie charlie"))
		{
			AnalyticsController.instance.OnActivateEasterEgg(matchPlayerName);
			charliecharlie = true;
			PlayersManager.instance.child.charliecharlieItem.SetActive(true);
		}
		ConfigCardboard(1);
	}

	public void ConfigCardboard(int day)
	{
		if (day1Texts != null) day1Texts.SetActive(day >= 1);
		if (day2Texts != null) day2Texts.SetActive(day >= 2);
		if (day3Texts != null) day3Texts.SetActive(day >= 3);
		if (day4Texts != null) day4Texts.SetActive(day >= 4);
		if (day5Texts != null) day5Texts.SetActive(day >= 5);
		current_day = day;
		SetTexts();
	}

	public void SetTexts()
	{
		if (isDego)
		{
			foreach (TextMeshProUGUI centerText in centerTexts)
			{
				if (centerText.gameObject.activeSelf && centerText.transform.parent.gameObject.activeSelf)
				{
					centerText.GetComponent<Localize>().enabled = false;
					if (current_day >= 1 && current_day <= 2)
					{
						centerText.text = "Si hacemos 30.000 likes sería la raja";
						centerText.font = normal_font;
					}
					else if (current_day >= 3 && current_day <= 4)
					{
						centerText.text = "Si hacemos 50.000 likes sería la raja";
						centerText.font = normal_font;
					}
					else if (current_day == 5)
					{
						centerText.text = "Si hacemos 100.000 likes sería la raja";
						centerText.font = normal_font;
					}
				}
			}
			return;
		}
		if (isItown)
		{
			foreach (TextMeshProUGUI centerText2 in centerTexts)
			{
				if (centerText2.gameObject.activeSelf && centerText2.transform.parent.gameObject.activeSelf)
				{
					centerText2.GetComponent<Localize>().enabled = false;
					centerText2.text = "No gusta tanto susto";
					centerText2.font = normal_font;
				}
			}
			return;
		}
		if (isCory)
		{
			foreach (TextMeshProUGUI centerText3 in centerTexts)
			{
				if (centerText3.gameObject.activeSelf && centerText3.transform.parent.gameObject.activeSelf)
				{
					centerText3.GetComponent<Localize>().enabled = false;
					centerText3.text = "it's ok Cory take your pills";
				}
			}
			return;
		}
		if (vividPlays)
		{
			foreach (TextMeshProUGUI centerText4 in centerTexts)
			{
				if (centerText4.gameObject.activeSelf && centerText4.transform.parent.gameObject.activeSelf)
				{
					centerText4.GetComponent<Localize>().enabled = false;
					centerText4.text = "Vividplays is not afraid, Vividplays will leave this school in record time.";
					centerText4.font = normal_font;
				}
			}
			return;
		}
		if (isJohnWolfe)
		{
			foreach (TextMeshProUGUI centerText5 in centerTexts)
			{
				if (centerText5.gameObject.activeSelf && centerText5.transform.parent.gameObject.activeSelf)
				{
					centerText5.GetComponent<Localize>().enabled = false;
					centerText5.text = "Act critically, write critically, run critically, think critically.";
					centerText5.font = normal_font;
				}
			}
			return;
		}
		if (isSpidergaming)
		{
			foreach (TextMeshProUGUI centerText6 in centerTexts)
			{
				if (centerText6.gameObject.activeSelf && centerText6.transform.parent.gameObject.activeSelf)
				{
					centerText6.GetComponent<Localize>().enabled = false;
					centerText6.text = "Hello Xin Chào các bạn và chào mừng các bạn đã quay trở lại với SpiderGaming";
					centerText6.font = normal_font;
				}
			}
			return;
		}
		if (isfunbabe)
		{
			foreach (TextMeshProUGUI centerText7 in centerTexts)
			{
				if (centerText7.gameObject.activeSelf && centerText7.transform.parent.gameObject.activeSelf)
				{
					centerText7.GetComponent<Localize>().enabled = false;
					centerText7.text = "Oie! Eu sou a funBABE e sejam muito bem vindos ao meu canal. Todos, exceto a freira";
					centerText7.font = normal_font;
				}
			}
			return;
		}
		if (isLeoGames)
		{
			foreach (TextMeshProUGUI centerText8 in centerTexts)
			{
				if (centerText8.gameObject.activeSelf && centerText8.transform.parent.gameObject.activeSelf)
				{
					centerText8.GetComponent<Localize>().enabled = false;
					centerText8.text = "Presente da vovó para LeoGamesAndroidBR";
					centerText8.font = normal_font;
				}
			}
			return;
		}
		if (isDakblake)
		{
			foreach (TextMeshProUGUI centerText9 in centerTexts)
			{
				if (centerText9.gameObject.activeSelf && centerText9.transform.parent.gameObject.activeSelf)
				{
					centerText9.GetComponent<Localize>().enabled = false;
					centerText9.text = "Dakblake is playing the game differently, the nun seems more talkative";
					centerText9.font = normal_font;
				}
			}
			return;
		}
		if (ispomah)
		{
			foreach (TextMeshProUGUI centerText10 in centerTexts)
			{
				if (centerText10.gameObject.activeSelf && centerText10.transform.parent.gameObject.activeSelf)
				{
					centerText10.GetComponent<Localize>().enabled = false;
					centerText10.text = "Я уйду из этой школы через какую-то тайную дыру ...";
					centerText10.font = russian_font;
				}
			}
			return;
		}
		if (isGermanSnake)
		{
			foreach (TextMeshProUGUI centerText11 in centerTexts)
			{
				if (centerText11.gameObject.activeSelf && centerText11.transform.parent.gameObject.activeSelf)
				{
					centerText11.GetComponent<Localize>().enabled = false;
					centerText11.text = "#furchtlos #furchtlos #furchtlos #furchtlos #furchtlos #furchtlos";
					centerText11.font = normal_font;
				}
			}
			return;
		}
		if (isGenu)
		{
			string[] array = new string[3] { "¡Hola! muy buenas a todos, muy buenas a todas que tal estáis;", "espero que estéis muy bien", "bienvenidos un dia mas a tu celda, bienvenidos un dias mas a Evil Nun" };
			int num = 0;
			List<TextMeshProUGUI> list = centerTexts.FindAll((TextMeshProUGUI ct) => ct.gameObject.activeSelf);
			{
				foreach (TextMeshProUGUI centerText12 in centerTexts)
				{
					if (centerText12.gameObject.activeSelf && centerText12.transform.parent.gameObject.activeSelf)
					{
						centerText12.GetComponent<Localize>().enabled = false;
						centerText12.text = array[num];
						centerText12.font = normal_font;
						num++;
						if (num > 2)
						{
							num = 0;
						}
					}
					else
					{
						Debug.Log("center text deactive!");
					}
				}
				return;
			}
		}
		if (febatista)
		{
			foreach (TextMeshProUGUI centerText13 in centerTexts)
			{
				if (centerText13.gameObject.activeSelf && centerText13.transform.parent.gameObject.activeSelf)
				{
					centerText13.GetComponent<Localize>().enabled = false;
					centerText13.text = "Eu serei bom. Gutin e eu vamos sair daqui.";
					centerText13.font = normal_font;
				}
			}
			return;
		}
		if (isDenis)
		{
			foreach (TextMeshProUGUI centerText14 in centerTexts)
			{
				if (centerText14.gameObject.activeSelf && centerText14.transform.parent.gameObject.activeSelf)
				{
					centerText14.GetComponent<Localize>().enabled = false;
					centerText14.text = "MEOW MEOW MEOW MEOW MEOW!";
					centerText14.font = normal_font;
				}
			}
			return;
		}
		if (isZbingZ)
		{
			foreach (TextMeshProUGUI centerText15 in centerTexts)
			{
				if (centerText15.gameObject.activeSelf && centerText15.transform.parent.gameObject.activeSelf)
				{
					centerText15.GetComponent<Localize>().enabled = false;
					centerText15.text = "Subscribe to my channel if you want it to survive! Haha";
					centerText15.font = normal_font;
				}
			}
			return;
		}
		if (windy31)
		{
			foreach (TextMeshProUGUI centerText16 in centerTexts)
			{
				if (centerText16.gameObject.activeSelf && centerText16.transform.parent.gameObject.activeSelf)
				{
					centerText16.GetComponent<Localize>().enabled = false;
					centerText16.text = "Я не помню, почему я здесь. Я не помню, почему меня зовут Windy31. Я знаю только, что должен быть хорошим.";
					centerText16.font = russian_font;
				}
			}
			return;
		}
		foreach (TextMeshProUGUI centerText17 in centerTexts)
		{
			if (centerText17.gameObject.activeSelf && centerText17.transform.parent.gameObject.activeSelf)
			{
				centerText17.text = LocalizationManager.GetTranslation("cardboard_center");
				centerText17.text = centerText17.text.Replace("{0}", matchPlayerName);
			}
		}
	}
}
