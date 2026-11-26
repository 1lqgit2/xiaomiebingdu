using UnityEngine;
using UnityEngine.UI;

namespace orangesGame
{
	public class DialogueView : UIView
	{
		public Transform tranDialogue;

		public Button btnClick;

		public Text txtContent;

		public Text txtName;

		public Image imgDialogueBg;

		public GameObject objBg;

		public Text continueTxt;

		public CanvasGroup canvasGroup;

		public Transform tranNpc;

		public GameObject objStar;

		public override void OnLoad()
		{
			base.OnLoad();
			Transform layerTran = Singleton<UIViewManager>.Instance.GetLayerTran(100);
			CreateView("GameUI/DialogueView/ui_dialogueview", layerTran);
		}

		public override void OnInit()
		{
			base.OnInit();
			Transform transform = base.Root.transform;
			objBg = transform.Find("img_bg_mark").gameObject;
			btnClick = transform.Find("btn_click").GetComponent<Button>();
			tranDialogue = transform.Find("root_dialogue");
			continueTxt = tranDialogue.Find("img_bg_0/txt_clickcontent").GetComponent<Text>();
			txtContent = tranDialogue.Find("img_bg_0/txt_content").GetComponent<Text>();
			txtName = tranDialogue.Find("img_bg_0/txt_name").GetComponent<Text>();
			imgDialogueBg = tranDialogue.Find("img_bg_0").GetComponent<Image>();
			tranNpc = imgDialogueBg.transform.Find("root_npn");
			canvasGroup = imgDialogueBg.GetComponent<CanvasGroup>();
			objStar = transform.Find("ui_storyview").gameObject;
			ApplicationFacade.Instance.RegisterMediator(new DialogueViewMediator("DIALOGUE", this));
		}

		public override void OnRemove()
		{
			base.OnRemove();
			ApplicationFacade.Instance.RemoveMediator("DIALOGUE");
		}
	}
}
