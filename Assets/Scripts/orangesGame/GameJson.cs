using LitJson;
using UnityEngine;

namespace orangesGame
{
	public class GameJson : Singleton<GameJson>
	{
		public const string JSON_BARRIERS = "BarrierConfig";

		public const string JSON_LEVEL = "Level";

		public const string JSON_FIRESPEED = "FireSpeed";

		public const string JSON_FIREPOWER = "FirePower";

		public const string JSON_COINLEVEL = "CoinLevel";

		public const string JSON_COIN = "Coin";

		public const string JSON_VENOM = "Venom";

		public const string JSON_WEAPON = "Weapon";

		public const string JSON_WEAPONLEVEL = "WeaponLevel";

		public const string JSOM_COMMON = "Common";

		public const string JSON_BOSS = "BossLevel";

		public const string JSON_TIMEGIFT = "TimeGift";

		public const string JSON_DIAMONDGIFT = "DiamondGift";

		public const string JSON_ITEMGIFT = "ItemGift";

		public const string JSON_MENBERGIFT = "MenberGift";

		public const string JSON_LUCKYDRAW = "LuckyDraw";

		public const string JSON_BATTLE_ITEM = "BattleItem";

		public const string JSON_Language = "Language";

		public const string JSON_WeaponParamRate = "WeaponParamRate";

		public const string JSON_CoinCostRate = "CoinCostRate";

		public const string JSON_BARRIER_COMBINE = "BarrierCombine";

		public const string JSON_TEST = "TestWeapon";

		public const string JSON_NPC = "NpcLevel";

		public BarrierCombineStruct[] BarrierCombineJson;

		public LevelStruct[] LevelJson;

		public FireSpeedStruct[] FireSpeedJson;

		public FirePowerStruct[] FirePowerJson;

		public CoinStruct[] CoinJson;

		public VenomStruct[] VenomJson;

		public WeaponStruct[] WeaponJson;

		public WeaponLevelStruct[] WeaponLevelJson;

		public CommonStruct[] CommonJson;

		public CoinLevelStruct[] CoinLevelJson;

		public TestWeaponStruct[] TestWeaponJson;

		public BossLevelStruct[] BossLevelJson;

		public TimeGiftStruct[] TimeGiftJson;

		public ItemGiftStruct[] ItemGiftJson;

		public DiamondGiftStruct[] DiamondGiftJson;

		public MenberGiftStruct[] MenberGiftJson;

		public LuckyDrawStruct[] LuckyDrawJson;

		public BattleItemStruct[] BattleItemJson;

		public LanguageStruct[] LanguageJson;

		public WeaponParamRateStruct[] WeaponParamRateStruct;

		public CoinCostRateStruct[] CoinCostRateStruct;

		public NpcLevelStruct[] NpcLevelStruct;

		public void Init()
		{
			LevelJson = GetJson<LevelStruct[]>("Level");
			FireSpeedJson = GetJson<FireSpeedStruct[]>("FireSpeed");
			FirePowerJson = GetJson<FirePowerStruct[]>("FirePower");
			CoinJson = GetJson<CoinStruct[]>("Coin");
			VenomJson = GetJson<VenomStruct[]>("Venom");
			WeaponJson = GetJson<WeaponStruct[]>("Weapon");
			WeaponLevelJson = GetJson<WeaponLevelStruct[]>("WeaponLevel");
			CommonJson = GetJson<CommonStruct[]>("Common");
			CoinLevelJson = GetJson<CoinLevelStruct[]>("CoinLevel");
			BossLevelJson = GetJson<BossLevelStruct[]>("BossLevel");
			BarrierCombineJson = GetJson<BarrierCombineStruct[]>("BarrierCombine");
			TimeGiftJson = GetJson<TimeGiftStruct[]>("TimeGift");
			ItemGiftJson = GetJson<ItemGiftStruct[]>("ItemGift");
			DiamondGiftJson = GetJson<DiamondGiftStruct[]>("DiamondGift");
			MenberGiftJson = GetJson<MenberGiftStruct[]>("MenberGift");
			LuckyDrawJson = GetJson<LuckyDrawStruct[]>("LuckyDraw");
			BattleItemJson = GetJson<BattleItemStruct[]>("BattleItem");
			LanguageJson = GetJson<LanguageStruct[]>("Language");
			WeaponParamRateStruct = GetJson<WeaponParamRateStruct[]>("WeaponParamRate");
			CoinCostRateStruct = GetJson<CoinCostRateStruct[]>("CoinCostRate");
			NpcLevelStruct = GetJson<NpcLevelStruct[]>("NpcLevel");
		}

		public void InitTest()
		{
			TestWeaponJson = GetJson<TestWeaponStruct[]>("TestWeapon");
		}

		private T GetJson<T>(string key)
		{
			string text = Resources.Load<TextAsset>($"GameJson/{key}").text;
			return JsonMapper.ToObject<T>(text);
		}
	}
}
