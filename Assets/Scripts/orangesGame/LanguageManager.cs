using UnityEngine;

namespace orangesGame
{
	public class LanguageManager : Singleton<LanguageManager>
	{
		public void Init()
		{
			Language language = GetLanguage();
			LanguageConfig.MAX = language.GetTxt(44);
			LanguageConfig.DIALOG_OFFLINE = language.GetTxt(74);
			LanguageConfig.DIALOG_TASK = language.GetTxt(75);
			LanguageConfig.BATTLE_PROGRESS = language.GetTxt(17);
			LanguageConfig.UPGRED_LIMITED = language.GetTxt(46);
			LanguageConfig.MAIN_PROPERTY_1 = language.GetTxt(3);
			LanguageConfig.MAIN_PROPERTY_2 = language.GetTxt(4);
			LanguageConfig.COIN_PROPERTY_1 = language.GetTxt(5);
			LanguageConfig.COIN_PROPERTY_2 = language.GetTxt(6);
			LanguageConfig.FREE = language.GetTxt(40);
			LanguageConfig.ADS = language.GetTxt(45);
			LanguageConfig.UNLOCK = language.GetTxt(76);
			string[] targets = new string[9]
			{
				language.GetTxt(22),
				language.GetTxt(23),
				language.GetTxt(24),
				language.GetTxt(25),
				language.GetTxt(26),
				language.GetTxt(27),
				language.GetTxt(28),
				language.GetTxt(29),
				language.GetTxt(30)
			};
			SetArry(ref LanguageConfig.TIPS, targets);
			LanguageConfig.FaceBookShareTitle = language.GetTxt(47);
			LanguageConfig.FaceBookShareContext = language.GetTxt(48);
			LanguageConfig.MessageShareTitle = language.GetTxt(49);
			LanguageConfig.MessageShareContext = language.GetTxt(50);
			LanguageConfig.TUTORIAL_UPGARDEVIEW = language.GetTxt(77);
			LanguageConfig.TUTORIAL_UPGRADE_POWER = language.GetTxt(78);
			string[] targets2 = new string[8]
			{
				language.GetTxt(32),
				language.GetTxt(33),
				language.GetTxt(34),
				language.GetTxt(35),
				language.GetTxt(36),
				language.GetTxt(37),
				language.GetTxt(38),
				language.GetTxt(39)
			};
			SetArry(ref LanguageConfig.TASK_TITLE, targets2);
			LanguageConfig.TASK_COMPLETE = language.GetTxt(51);
			LanguageConfig.CONVERSION_USE_DIAMOND = language.GetTxt(42);
			LanguageConfig.CONVERSION_GET_COIN = language.GetTxt(79);
			LanguageConfig.GOOLGE_PAY_FAIL = language.GetTxt(80);
			LanguageConfig.DIAMOND_NOT_ENOUGH = language.GetTxt(81);
			LanguageConfig.REVIVE = language.GetTxt(82);
			LanguageConfig.NO_THANKS = language.GetTxt(21);
			LanguageConfig.COLLECT = language.GetTxt(2);
			LanguageConfig.DIALY_TASK_TITLE = language.GetTxt(31);
			LanguageConfig.DIALY_TASK_TIME_TITLE = language.GetTxt(83);
			LanguageConfig.CONVERSION_TITLE = language.GetTxt(41);
			LanguageConfig.CONVERT = language.GetTxt(43);
			LanguageConfig.FINAL_WAVE = language.GetTxt(20);
			LanguageConfig.BOSS_TIP = language.GetTxt(19);
			LanguageConfig.RESULT_TITLE = language.GetTxt(1);
			LanguageConfig.RESULT_100_CLEAR = language.GetTxt(18);
			LanguageConfig.Shop_Title = language.GetTxt(52);
			LanguageConfig.Shop_bestOrder = language.GetTxt(92);
			LanguageConfig.Shop_bestOrder1_title = language.GetTxt(65);
			LanguageConfig.Shop_bestOrder2_title = language.GetTxt(66);
			LanguageConfig.Shop_bestOrder3_title = language.GetTxt(67);
			LanguageConfig.Shop_bestOrder4_title = language.GetTxt(68);
			LanguageConfig.Shop_bestOrder5_title = language.GetTxt(69);
			LanguageConfig.Shop_bestOrder6_title = language.GetTxt(94);
			LanguageConfig.Shop_bestOrder7_title = language.GetTxt(95);
			LanguageConfig.Shop_bestOrder8_title = language.GetTxt(96);
			LanguageConfig.Shop_bestOrder_Item = language.GetTxt(93);
			LanguageConfig.Shop_recommend = language.GetTxt(53);
			LanguageConfig.Shop_diamondMembership = language.GetTxt(54);
			LanguageConfig.Shop_detail = language.GetTxt(56);
			LanguageConfig.Shop_vip4pri = language.GetTxt(55);
			LanguageConfig.Shop_moveAds = language.GetTxt(57);
			LanguageConfig.Shop_diamondTitle = language.GetTxt(58);
			LanguageConfig.Main_Tip_Shop = language.GetTxt(97);
			LanguageConfig.NO_ADS_IS_BUY = language.GetTxt(89);
			LanguageConfig.SOLD_OUT = language.GetTxt(59);
			LanguageConfig.LUCKYBOX_CONTENT = language.GetTxt(73);
			LanguageConfig.FULL = language.GetTxt(91);
			LanguageConfig.NOT_ENOUGH_DIAMONDS = language.GetTxt(105);
			LanguageConfig.NO_HIGHER_THAN_MAIN_WEAPON = language.GetTxt(106);
			LanguageConfig.ValidUntil = language.GetTxt(72);
			LanguageConfig.DAILYBENIFIT = language.GetTxt(98);
			LanguageConfig.CHAIM = language.GetTxt(70);
			LanguageConfig.HASCHAIM = language.GetTxt(71);
			LanguageConfig.TRYFORFREE = language.GetTxt(64);
			LanguageConfig.FREE_LITTLE = language.GetTxt(99);
			LanguageConfig.PAYBIG = language.GetTxt(100);
			LanguageConfig.PAYLITTLE = language.GetTxt(101);
			string[] targets3 = new string[4]
			{
				language.GetTxt(60),
				language.GetTxt(61),
				language.GetTxt(62),
				language.GetTxt(63)
			};
			SetVipDetail(LanguageConfig.STARS_STRINGS, targets3);
			LanguageConfig.LUCKYDRAW = language.GetTxt(107);
			LanguageConfig.LUCKYDRAW_CONFIRM = language.GetTxt(108);
			LanguageConfig.Share = language.GetTxt(102);
			LanguageConfig.BATTLE_ITEM_LIMIT_ITEM_TITLE = language.GetTxt(103);
			LanguageConfig.BATTLE_ITEM_NO_ITEM_TITLE = language.GetTxt(104);
			string[][] targets4 = new string[5][]
			{
				new string[2]
				{
					language.GetTxt(7),
					language.GetTxt(8)
				},
				new string[2]
				{
					language.GetTxt(9),
					language.GetTxt(10)
				},
				new string[2]
				{
					language.GetTxt(11),
					language.GetTxt(12)
				},
				new string[2]
				{
					language.GetTxt(13),
					language.GetTxt(14)
				},
				new string[2]
				{
					language.GetTxt(15),
					language.GetTxt(16)
				}
			};
			SetArry(ref LanguageConfig.WEAPON_PROPTERY_NAMES, targets4);
			LanguageConfig.item_sale_title1 = language.GetTxt(84);
			LanguageConfig.item_sale_title2 = language.GetTxt(85);
			LanguageConfig.item_sale_title3 = language.GetTxt(86);
			LanguageConfig.item_sale_title4 = language.GetTxt(87);
			LanguageConfig.item_sale_title5 = language.GetTxt(88);
			LanguageConfig.item_sale_purchase = language.GetTxt(110);
			LanguageConfig.item_sale_use = language.GetTxt(113);
			LanguageConfig.item_sale_get = language.GetTxt(112);
			LanguageConfig.item_sale_getmore = language.GetTxt(109);
			LanguageConfig.item_sale_owned = language.GetTxt(114);
			LanguageConfig.shareLink = language.GetTxt(115);
			LanguageConfig.unlock_success = language.GetTxt(155);
			LanguageConfig.unlock_content = language.GetTxt(156);
			string[] array = new string[LanguageConfig.constellation_names.Length];
			int num = array.Length - 1;
			for (int i = 0; i < num; i++)
			{
				array[i] = language.GetTxt(143 + i);
			}
			array[num] = language.GetTxt(162);
			LanguageConfig.constellation_names = array;
			LanguageConfig.level_unlock_weapon = language.GetTxt(163);
			LanguageConfig.level_click_tip = language.GetTxt(164);
			LanguageConfig.level_start_npc = language.GetTxt(165);
			LanguageConfig.clickAndContinues = language.GetTxt(166);
			LanguageConfig.story = language.GetTxt(167);
		}

		/// <summary>
		/// 抖音版只发简体中文包，这里强制返回中文。
		/// 原实现第一行写死了 return new LanguageEn();，导致下面所有语言判断都成了不可达代码，
		/// 界面恒为英文（中文文本即使写了也永远取不到）。
		/// 如需恢复多语言，按 IsCn()/IsKorean()/IsPortuguese()/IsSpanish()/IsTraditional()/IsRu()/IsDe()
		/// 的顺序把分支接回来即可，判断方法都保留在本类下方。
		/// </summary>
		public Language GetLanguage()
		{
			return new LanguageCn();
		}

		public void InitOld()
		{
		}

		public bool IsTraditional()
		{
			return Application.systemLanguage == SystemLanguage.ChineseTraditional;
		}

		public bool IsCn()
		{
			return Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified;
		}

		public bool IsSpanish()
		{
			return Application.systemLanguage == SystemLanguage.Spanish;
		}

		public bool IsPortuguese()
		{
			return Application.systemLanguage == SystemLanguage.Portuguese;
		}

		public bool IsKorean()
		{
			return Application.systemLanguage == SystemLanguage.Korean;
		}

		public bool IsRu()
		{
			return false;
		}

		public bool IsDe()
		{
			return false;
		}

		private void SetVipDetail(string[] language, string[] targets)
		{
			for (int i = 0; i < language.Length; i++)
			{
				language[i] = targets[i];
			}
		}

		private void SetArry(ref string[] language, string[] targets)
		{
			language = new string[targets.Length];
			for (int i = 0; i < targets.Length; i++)
			{
				language[i] = targets[i];
			}
		}

		private void SetArry(ref string[][] language, string[][] targets)
		{
			language = new string[targets.Length][];
			for (int i = 0; i < targets.Length; i++)
			{
				language[i] = new string[targets[i].Length];
				for (int j = 0; j < language[i].Length; j++)
				{
					language[i][j] = targets[i][j];
				}
			}
		}
	}
}
