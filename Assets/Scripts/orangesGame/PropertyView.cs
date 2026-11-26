using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class PropertyView : UIView
	{
		public RectTransform tranProperty;

		public Button[] btnPropertys;

		public Transform[] tranUpGradeTips;

		public Transform[] tranGuideTips;

		public Image[] imgWeapons;

		public RectTransform rectTranFreeTip;

		public Button btnSelcet;

		public GameObject objNewWeaponTip;

		public Vector3 mainPropertyPos;

		public RectTransform tranMain;

		public Button[] btnMainUpGrades;

		public Text[] txtMainUpGradeCosts;

		public Image[] imgMainUpGradeIcons;

		public Text[] txtMainUpGradeTitles;

		public Text[] txtMainLevels;

		public Transform upTranSpeed;

		public Transform upTranFire;

		public RectTransform tranWeapon;

		public RectTransform tranListParent;

		public Button[] btnWeaponUpGrades;

		public Text[] txtWeaponUpGradeCosts;

		public Text[] txtWeaponUpGradeTitles;

		public Text[] txtWeaponLevels;

		public GameObject objMaxWeapon;

		public RectTransform tranSelectWeapon;

		public Text txtContent;

		public Image[] imgWeaponUpGradeIcons;

		public RectTransform tranCoin;

		public Button[] btnCoinUpGrades;

		public Text[] txtCoinUpGradeCosts;

		public Image[] imgCoinUpGradeIcons;

		public Text[] txtCoinUpGradeTitles;

		public Text[] txtCoinLevels;

		public Animator ani;

		private Sequence _sequenceIn;

		private Sequence _sequenceOut;

		private const string ANIM_IN = "anim_propertyview_in";

		private const string ANIM_OUT = "anim_propertyview_out";

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(80);
			CreateView("GameUI/PropertyView/ui_propertyview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			btnSelcet = transform.Find("BtnSelect").GetComponent<Button>();
			tranProperty = (transform.Find("TranProperty") as RectTransform);
			btnPropertys = new Button[3];
			tranUpGradeTips = new Transform[3];
			tranGuideTips = new Transform[3];
			for (int i = 0; i < btnPropertys.Length; i++)
			{
				btnPropertys[i] = tranProperty.Find("BtnProperty" + i).GetComponent<Button>();
				tranUpGradeTips[i] = btnPropertys[i].transform.Find("ImgTip");
				tranGuideTips[i] = btnPropertys[i].transform.Find("TranFinger");
			}
			mainPropertyPos = btnPropertys[0].transform.position;
			imgWeapons = new Image[2];
			for (int j = 0; j < imgWeapons.Length; j++)
			{
				imgWeapons[j] = tranProperty.transform.Find("BtnProperty1/ImgIcon" + j).GetComponent<Image>();
			}
			rectTranFreeTip = (tranProperty.Find("ImgFreeFrame") as RectTransform);
			objNewWeaponTip = tranProperty.Find("ImgUnTip").gameObject;
			tranMain = (transform.Find("TranMain") as RectTransform);
			btnMainUpGrades = new Button[2];
			txtMainUpGradeCosts = new Text[2];
			txtMainUpGradeTitles = new Text[2];
			txtMainLevels = new Text[2];
			imgMainUpGradeIcons = new Image[2];
			for (int k = 0; k < btnMainUpGrades.Length; k++)
			{
				btnMainUpGrades[k] = tranMain.Find("BtnProperty" + k).GetComponent<Button>();
				imgMainUpGradeIcons[k] = btnMainUpGrades[k].transform.Find("ImgIcon").GetComponent<Image>();
				txtMainUpGradeCosts[k] = imgMainUpGradeIcons[k].transform.Find("TxtCost").GetComponent<Text>();
				txtMainUpGradeTitles[k] = tranMain.Find("TxtProperty" + k).GetComponent<Text>();
				txtMainLevels[k] = tranMain.Find("TxtPropertyLevel" + k).GetComponent<Text>();
			}
			upTranSpeed = tranMain.Find("TranFingerSpeed");
			upTranFire = tranMain.Find("TranFingerFire");
			tranCoin = (transform.Find("TranCoin") as RectTransform);
			btnCoinUpGrades = new Button[2];
			txtCoinUpGradeCosts = new Text[2];
			txtCoinUpGradeTitles = new Text[2];
			txtCoinLevels = new Text[2];
			imgCoinUpGradeIcons = new Image[2];
			for (int l = 0; l < btnCoinUpGrades.Length; l++)
			{
				btnCoinUpGrades[l] = tranCoin.Find("BtnProperty" + l).GetComponent<Button>();
				imgCoinUpGradeIcons[l] = btnCoinUpGrades[l].transform.Find("ImgIcon").GetComponent<Image>();
				txtCoinUpGradeCosts[l] = imgCoinUpGradeIcons[l].transform.Find("TxtCost").GetComponent<Text>();
				txtCoinUpGradeTitles[l] = tranCoin.Find("TxtProperty" + l).GetComponent<Text>();
				txtCoinLevels[l] = tranCoin.Find("TxtPropertyLevel" + l).GetComponent<Text>();
			}
			tranWeapon = (transform.Find("TranWeapon") as RectTransform);
			tranListParent = (tranWeapon.Find("SlvWeapon/Viewport/Content") as RectTransform);
			btnWeaponUpGrades = new Button[2];
			txtWeaponUpGradeCosts = new Text[2];
			txtWeaponUpGradeTitles = new Text[2];
			txtWeaponLevels = new Text[2];
			imgWeaponUpGradeIcons = new Image[2];
			for (int m = 0; m < btnWeaponUpGrades.Length; m++)
			{
				btnWeaponUpGrades[m] = tranWeapon.Find("BtnProperty" + m).GetComponent<Button>();
				imgWeaponUpGradeIcons[m] = btnWeaponUpGrades[m].transform.Find("ImgIcon").GetComponent<Image>();
				txtWeaponUpGradeCosts[m] = imgWeaponUpGradeIcons[m].transform.Find("TxtCost").GetComponent<Text>();
				txtWeaponUpGradeTitles[m] = tranWeapon.Find("TxtProperty" + m).GetComponent<Text>();
				txtWeaponLevels[m] = tranWeapon.Find("TxtPropertyLevel" + m).GetComponent<Text>();
			}
			objMaxWeapon = tranWeapon.Find("ImgMaxWeaponTip").gameObject;
			tranSelectWeapon = (tranWeapon.Find("ImgSelectWeapon") as RectTransform);
			txtContent = objMaxWeapon.transform.Find("ImgMaxWeaponTip/TxtContent").GetComponent<Text>();
			ApplicationFacade.Instance.RegisterMediator(new PropertyViewMediator("PROPERTY", this));
			ani = transform.GetComponent<Animator>();
		}

		public override void OnRegister()
		{
			base.OnRegister();
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("PROPERTY");
		}

		public override void OnShow(Action callback = null)
		{
			base.OnShow();
			if (_sequenceIn == null)
			{
				_sequenceIn = DOTween.Sequence();
				_sequenceIn.onComplete = delegate
				{
					if (callback != null)
					{
						callback();
					}
				};
				_sequenceIn.SetAutoKill(autoKillOnCompletion: false);
				_sequenceIn.Play();
			}
			else
			{
				_sequenceIn.Restart();
			}
			ani.Play("anim_propertyview_in");
		}

		public override void OnHide(Action callback = null)
		{
			tranMain.gameObject.SetActive(value: false);
			tranWeapon.gameObject.SetActive(value: false);
			tranCoin.gameObject.SetActive(value: false);
			if (_sequenceOut == null)
			{
				_sequenceOut = DOTween.Sequence();
				_sequenceOut.onComplete = delegate
				{
					if (callback != null)
					{
						callback();
					}
				};
				_sequenceOut.SetAutoKill(autoKillOnCompletion: false);
				_sequenceOut.Play();
			}
			else
			{
				_sequenceOut.Restart();
			}
			ani.Play("anim_propertyview_out");
			Timer.Register(0.3f, MoveOutCallback);
		}

		public void MoveOutCallback()
		{
			base.OnHide();
		}
	}
}
