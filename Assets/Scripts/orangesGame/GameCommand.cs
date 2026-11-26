using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace orangesGame
{
	public class GameCommand : SimpleCommand, ICommand
	{
		public override void Execute(INotification notification)
		{
			string name = notification.Name;
			if (name == null)
			{
				return;
			}
			if (!(name == "GAME_MAIN"))
			{
				if (!(name == "GAME_START"))
				{
					if (!(name == "GAME_PASS"))
					{
						if (!(name == "GAME_FAIL"))
						{
							if (!(name == "GAME_CONTINUE"))
							{
								if (name == "GAME_RESULT")
								{
									GameAnalysisData.StartGameTime = Time.unscaledTime;
									GameAnalysisData.AddTotalGameTime("GAME_RESULT");
									GameGlobalData.ReduceADSTime(GameAnalysisData.GetTotalGameTime());
									SetUmLevelFial(retry: false);
									GameGlobalData.GameState = GameState.Result;
									GameGlobalData.IsDie = false;
									Singleton<BallManger>.Instance.Stop();
									GameGlobalData.SetSlotProgress(GameGlobalData.SlotProgress + 1);
									SendNotification("BATTLE_END_GAME");
									Singleton<PlayerManager>.Instance.WingmanOut();
								}
							}
							else
							{
								// 复活统一恢复时间缩放：看广告复活时 timeScale 被置 0，不恢复会让战斗停在原地
								Time.timeScale = 1f;
								SetUmContinue((int)notification.Body);
								GameGlobalData.IsDie = false;
								GameGlobalData.SetCurHeartCount(1);
								Singleton<PlayerManager>.Instance.ContinueLeader();
								Singleton<BallManger>.Instance.Continue();
								Singleton<PlayerManager>.Instance.WingmanPlay();
								GameAnalysisData.StartGameTime = Time.unscaledTime;
								GameAnalysisData.UseRetry = true;
								GameGlobalData.ReviveCount++;
							}
						}
						else
						{
							GameAnalysisData.AddTotalGameTime("GAME_FAIL");
							Time.timeScale = 1f;
							GameGlobalData.IsGameAuto = false;
							SetAdFailLevel();
							Singleton<BulletManager>.Instance.Clean();
							Singleton<BallManger>.Instance.Pause();
							Singleton<PlayerManager>.Instance.PauseLeader();
							Singleton<PlayerManager>.Instance.LeaderHurt();
							Singleton<PlayerManager>.Instance.WingmanPause();
							Singleton<PlayerManager>.Instance.LeaderDropItems(CoinManager.Type.FastFireRate);
							Singleton<PlayerManager>.Instance.LeaderDropItems(CoinManager.Type.WeakenVenom);
							Singleton<PlayerManager>.Instance.LeaderDropItems(CoinManager.Type.EnhanceBulletDamage);
							Singleton<WeaponMaxManager>.Instance.ShowMaxWeapon(isSuc: false);
						}
					}
					else
					{
						GameAnalysisData.AddTotalGameTime("GAME_PASS");
						GameGlobalData.ReduceADSTime(GameAnalysisData.GetTotalGameTime());
						SetUmLevelSuccess();
						Time.timeScale = 1f;
						GameGlobalData.IsGameAuto = false;
						Singleton<PlayerManager>.Instance.PauseLeader();
						Singleton<BulletManager>.Instance.Clean();
						GameGlobalData.GameState = GameState.Result;
						GameGlobalData.AddCurLevel(1);
						GameGlobalData.SetSlotProgress(GameGlobalData.SlotProgress + 1);
						Singleton<GameItemManager>.Instance.CleanAllItems();
						Timer.Register(1.5f, delegate
						{
							SendNotification("BATTLE_PASS_GAME");
						}, null, false, true);
						Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.PassLevel_5, 1);
						Singleton<WeaponMaxManager>.Instance.ShowMaxWeapon(isSuc: true);
					}
				}
				else
				{
					SendNotification("BATTLE_WALL_ACTIVE");
					GameGlobalData.ItemsInCurrentLevel.Clear();
					GameGlobalData.CoinInCurrentLevel.Clear();
					GameGlobalData.GetDropItemsInGame = string.Empty;
					GameAnalysisData.StartGameTime = Time.unscaledTime;
					GameAnalysisData.TotalGameTime = 0f;
					GameAnalysisData.UseRetry = false;
					if (!GameGlobalData.startGame)
					{
						GameGlobalData.startGame = true;
					}
					if (GameGlobalData.IsFirstStart)
					{
						GameGlobalData.IsFirstStart = false;
					}
					string type = "type@战斗";
					bool isFirst = false;
					if (GameGlobalData.IsTutorial())
					{
						type = "type@新手战斗";
						isFirst = true;
					}
					SetUmStart(type, isFirst);
					GameGlobalData.ReviveCount = 0;
					GameGlobalData.GameState = GameState.Gaming;
					if (GameGlobalData.TempWeaponID >= 0 && GameGlobalData.TempWeaponID == GameGlobalData.WeaponID && GameGlobalData.MaxWeaponState == 1)
					{
						GameGlobalData.SetMaxWeaponState(2);
					}
					GameProxy gameProxy = base.Facade.RetrieveProxy("GAMING") as GameProxy;
					gameProxy.SetLevelData();
					Singleton<PlayerManager>.Instance.LeaderRest();
					Singleton<PlayerManager>.Instance.SetWeaponsShooting(isShoot: true);
					Singleton<UIViewManager>.Instance.HideView("MAIN_VIEW", "MAIN_HIDE");
					Singleton<UIViewManager>.Instance.HideView("PROPERTY_VIEW");
					Singleton<PlayerManager>.Instance.LeaderScale();
					GameGlobalData.SetTutorial();
					GameGlobalData.DropItemsInLevel = 0;
					if (GameGlobalData.ActualLevel == 1 && GameGlobalData.IsTutorialNew())
					{
						SendNotification("TUTORIAL_SET_STEP");
					}
					Action body = gameProxy.Start;
					SendNotification("DIALOGUE_CHECK_START", body);
				}
				return;
			}
			GameState gameState = GameGlobalData.GameState;
			GameGlobalData.GameState = GameState.Main;
			Singleton<TimeRefreshTask>.Instance.DoRefresh();
			Singleton<DiamondManager>.Instance.Clean();
			if (gameState == GameState.Result)
			{
				GameGlobalData.CurGetCoin = 0f;
				GameGlobalData.ShowValue = 0;
				GameGlobalData.NeedHitCount = 0;
				GameGlobalData.HitCount = 0;
				GameGlobalData.HitCountHp = 0f;
				if (GameGlobalData.FreeUpGradeCount != 0)
				{
					GameGlobalData.FreeUpGradeCount--;
				}
				GameGlobalData.IsDie = false;
				GameGlobalData.IsHurt = false;
				GameGlobalData.SetCurHeartCount(1);
				Singleton<GameItemManager>.Instance.CleanAllItems();
				Singleton<PlayerManager>.Instance.Reset(isTouch: true);
				Singleton<BallManger>.Instance.Clean();
				SendNotification("BG_CHANGE");
				Singleton<TutorialManager>.Instance.InitStart();
				if (GameGlobalData.TutorialState == 1)
				{
					ApplicationFacade.Instance.SendNotification("TUTORIAL_SET_STEP");
				}
				Singleton<UIViewManager>.Instance.SetLayerPos(1, Vector3.zero);
				if (Application.platform != RuntimePlatform.IPhonePlayer && GameGlobalData.IsEvaluate == 0 && GameGlobalData.ActualLevel % 10 == 1 && GameGlobalData.CurLevel >= 5 && GameGlobalData.ActualLevelReject != GameGlobalData.ActualLevel)
				{
					Singleton<UIViewManager>.Instance.ShowView("GRADE_VIEW");
				}
				ShowInnerAds();
				Singleton<AdjustConfig>.Instance.FinishLvTrack();
				GameAnalysisData.UseItem();
				Singleton<BattleItemManager>.Instance.CleanUseItem();
				Singleton<ReloadDataManager>.Instance.SetData();
			}
			else
			{
				string text = (!GameGlobalData.IsTutorial()) ? "type@登录首页" : "type@新手登录首页";
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("GameBaseLoadSuc", new string[1]
				{
					text
				});
				int num = (int)(GameGlobalData.GetTimestamp() - GameGlobalData.curTime);
				string text2 = (!GameGlobalData.IsTutorial()) ? "type@加载" : "type@新手第一次加载";
				Singleton<AnalyzeSdk>.Instance.OnEventValue("GameBaseLoadTime", new string[1]
				{
					text2
				}, num);
				GameAnalysisData.SendLoginSuc(GameGlobalData.PlayerType, num);
				if (GameGlobalData.CurHeart == 0)
				{
					GameGlobalData.SetCurHeartCount(1);
				}
				Singleton<PlayerManager>.Instance.LeaderSpawn();
				Singleton<UIViewManager>.Instance.ShowView("BATTLE_VIEW");
				Singleton<LuckyDrawManager>.Instance.InitList();
				if (Singleton<DialogueManager>.Instance.CanShowFirstNpc())
				{
					Singleton<UIViewManager>.Instance.ShowView("STORY_VIEW");
					SendNotification("STORY_VIEW_SHOW");
				}
			}
			Singleton<UIViewManager>.Instance.ShowView("MAIN_VIEW", "MAIN_INIT");
			if (Application.platform != RuntimePlatform.IPhonePlayer && GameGlobalData.ActualLevel > 30 && !Singleton<VipManager>.Instance.IsShowVipView && !Singleton<VipManager>.Instance.VipStatus)
			{
				Singleton<UIViewManager>.Instance.ShowView("VIP_VIEW", "VIP_INIT");
				Singleton<VipManager>.Instance.IsShowVipView = true;
			}
			GameGlobalData.ShopData.CheckGiftLevel();
			SendNotification("MAIN_TIME_LIMIT_TIPS", true);
			SendNotification("PORPERTY_SHOW");
			SendNotification("BATTLE_INIT");
			Singleton<DailyTaskHandle>.Instance.AddProgress(DailyTaskType.DailyLogin, 1);
			SendNotification("MAIN_TASK_TIP");
			GameGlobalData.InitADSTimestampNew();
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				Singleton<UIViewManager>.Instance.ShowView("LEVEL_VIEW");
				SendNotification("LEVEL_VIEW_SHOW");
			}
		}

		private void DelayOpenResult()
		{
			Timer.Register(1f, delegate
			{
				Singleton<UIViewManager>.Instance.ShowView("RESULT_VIEW");
				if (GameGlobalData.ReviveCount >= GameGlobalData.ReviveCountMax)
				{
					if (Application.platform == RuntimePlatform.IPhonePlayer)
					{
						SendNotification("DIE_INIT");
					}
					else
					{
						SendNotification("CONTINUE_INIT_NOAD");
					}
				}
				else
				{
					bool flag = Singleton<AdManager>.Instance.IsLoad("0a2cad70-9e1c-450f-bcdc-f3798b324b95", "DelayOpenResult");
					bool flag2 = Singleton<DiamondManager>.Instance.IsEnough(Singleton<DiamondManager>.Instance.CalCostDiamondRetry());
					if (flag)
					{
						SendNotification((!flag2) ? "CONTINUE_INIT" : "DIAMOND_CONTINUE_INIT");
					}
					else
					{
						Singleton<AnalyzeSdk>.Instance.OnEventByArgs("GetADSFailed", new string[1]
						{
							"type@1"
						});
						SendNotification((!flag2) ? "DIE_INIT" : "CONTINUE_INIT_NOAD");
					}
				}
			}, null, false, true);
		}

		private void SetAdFailLevel()
		{
			bool flag = false;
			if (!flag)
			{
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("GetADSFailed", new string[1]
				{
					"type@2"
				});
			}
			bool flag2 = false;
			if (!flag || !flag2)
			{
				DelayOpenResult();
			}
		}

		private void ShowInnerAds()
		{
			bool flag = Singleton<AdManager>.Instance.IsLoad("35eeefa8-c011-4112-bc9b-4a04fe3c413c", "ShowInnerAds");
			if (!flag)
			{
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("GetADSFailed", new string[1]
				{
					"type@2"
				});
			}
			bool flag2 = GameGlobalData.CanShowADSScreenNew();
			if (flag && flag2)
			{
				Singleton<AdManager>.Instance.ShowInterstitial("35eeefa8-c011-4112-bc9b-4a04fe3c413c", "failed", AdShowInterstitialCallback);
			}
		}

		private void AdShowInterstitialCallback(bool isReward)
		{
			if (isReward)
			{
				GameGlobalData.SetRemainADSTime();
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("ADSVideoScreen", new string[1]
				{
					"type@关卡结算后"
				});
				GameAnalysisData.ShowInterstitial("ShowInnerAds");
			}
		}

		private void SetUmContinue(int type)
		{
			List<string> list = UmConfig.SetCommonParam();
			float curHitCountPro = GameGlobalData.GetCurHitCountPro();
			list.Add($"progress@{curHitCountPro}");
			list.Add($"useItem@{type}");
			Singleton<AnalyzeSdk>.Instance.OnEventByArgs("BattleRetry", list.ToArray());
			GameAnalysisData.SendRetry(curHitCountPro);
		}

		private void SetUmStart(string type, bool isFirst)
		{
			int actualLevel = GameGlobalData.ActualLevel;
			Singleton<AnalyzeSdk>.Instance.StartLevel(actualLevel.ToString());
			List<string> list = UmConfig.SetCommonParam();
			list.Add(type);
			Singleton<AnalyzeSdk>.Instance.OnEventByArgs("StartBattle", list.ToArray());
			if (isFirst)
			{
				List<string> list2 = new List<string>();
				list2.Add(type);
				Singleton<AnalyzeSdk>.Instance.OnEventByArgs("GameBaseBattle", list2.ToArray());
			}
			GameAnalysisData.SendStartBattle(GameGlobalData.PlayerType, GameGlobalData.isCanUpadteMain, GameGlobalData.isCanUpadteCoin, GameGlobalData.isCanUpadteWeapon);
		}

		private void SetUmLevelSuccess()
		{
			Singleton<AnalyzeSdk>.Instance.FinishLevel(GameGlobalData.ActualLevel.ToString());
			List<string> list = new List<string>();
			list.Add($"failedtimes@{GameGlobalData.FailCount}");
			list.Add($"lvId@{GameGlobalData.CurLevel}");
			Singleton<AnalyzeSdk>.Instance.OnEventByArgs("BattleSuc", list.ToArray());
			GameGlobalData.SetFailCount(0);
			int weaponID = GameGlobalData.WeaponID;
			int num = 0;
			int num2 = 0;
			if (weaponID >= 0)
			{
				num = GameGlobalData.GetWeaponLevel(weaponID * 2, 0);
				num2 = GameGlobalData.GetWeaponLevel(weaponID * 2, 1);
			}
			long num3 = GameAnalysisData.GetTotalGameTime();
			GameAnalysisData.SendEndBattle(1, 1f);
			GameGlobalData.MoveDistance = 0f;
		}

		private void SetUmLevelFial(bool retry)
		{
			if (!retry)
			{
				Singleton<AnalyzeSdk>.Instance.FailLevel(GameGlobalData.ActualLevel.ToString());
				GameGlobalData.AddFailCount(1);
			}
			List<string> list = new List<string>();
			float curHitCountPro = GameGlobalData.GetCurHitCountPro();
			list.Add($"progress@{curHitCountPro}");
			list.Add($"lvId@{GameGlobalData.CurLevel}");
			Singleton<AnalyzeSdk>.Instance.OnEventByArgs("BattleFailed", list.ToArray());
			int weaponID = GameGlobalData.WeaponID;
			int num = 0;
			int num2 = 0;
			if (weaponID >= 0)
			{
				num = GameGlobalData.GetWeaponLevel(weaponID * 2, 0);
				num2 = GameGlobalData.GetWeaponLevel(weaponID * 2, 1);
			}
			long num3 = GameAnalysisData.GetTotalGameTime();
			GameAnalysisData.SendEndBattle(0, curHitCountPro);
			GameGlobalData.MoveDistance = 0f;
		}
	}
}
