using UnityEngine;
using UnityEngine.UI;

namespace UnityTimer.Examples
{
	public class TestTimerBehaviour : MonoBehaviour
	{
		[Header("Controls")]
		public InputField DurationField;

		public Button StartTimerButton;

		public Button CancelTimerButton;

		public Button PauseTimerButton;

		public Button ResumeTimerButton;

		public Toggle IsLoopedToggle;

		public Toggle UseGameTimeToggle;

		public Slider TimescaleSlider;

		public Text NeedsRestartText;

		[Header("Stats Texts")]
		public Text TimeElapsedText;

		public Text TimeRemainingText;

		public Text PercentageCompletedText;

		public Text PercentageRemainingText;

		public Text NumberOfLoopsText;

		public Text IsCancelledText;

		public Text IsCompletedText;

		public Text IsPausedText;

		public Text IsDoneText;

		public Text UpdateText;

		private int _numLoops;

		private Timer _testTimer;

		private void Awake()
		{
			ResetState();
		}

		private void ResetState()
		{
			_numLoops = 0;
			CancelTestTimer();
		}

		public void StartTestTimer()
		{
			ResetState();
			_testTimer = Timer.Register(GetDurationValue(), delegate
			{
				_numLoops++;
			}, delegate(float secondsElapsed)
			{
				UpdateText.text = $"Timer ran update callback: {secondsElapsed:F2} seconds";
			}, IsLoopedToggle.isOn, !UseGameTimeToggle.isOn);
			CancelTimerButton.interactable = true;
		}

		public void CancelTestTimer()
		{
			Timer.Cancel(_testTimer);
			CancelTimerButton.interactable = false;
			NeedsRestartText.gameObject.SetActive(value: false);
		}

		public void PauseTestTimer()
		{
			Timer.Pause(_testTimer);
		}

		public void ResumeTestTimer()
		{
			Timer.Resume(_testTimer);
		}

		private void Update()
		{
			if (_testTimer != null)
			{
				Time.timeScale = TimescaleSlider.value;
				_testTimer.isLooped = IsLoopedToggle.isOn;
				TimeElapsedText.text = $"Time elapsed: {_testTimer.GetTimeElapsed():F2} seconds";
				TimeRemainingText.text = $"Time remaining: {_testTimer.GetTimeRemaining():F2} seconds";
				PercentageCompletedText.text = $"Percentage completed: {_testTimer.GetRatioComplete() * 100f:F4}%";
				PercentageRemainingText.text = $"Percentage remaining: {_testTimer.GetRatioRemaining() * 100f:F4}%";
				NumberOfLoopsText.text = $"# Loops: {_numLoops}";
				IsCancelledText.text = $"Is Cancelled: {_testTimer.isCancelled}";
				IsCompletedText.text = $"Is Completed: {_testTimer.isCompleted}";
				IsPausedText.text = $"Is Paused: {_testTimer.isPaused}";
				IsDoneText.text = $"Is Done: {_testTimer.isDone}";
				PauseTimerButton.interactable = !_testTimer.isPaused;
				ResumeTimerButton.interactable = _testTimer.isPaused;
				NeedsRestartText.gameObject.SetActive(ShouldShowRestartText());
			}
		}

		private bool ShouldShowRestartText()
		{
			return !_testTimer.isDone && ((UseGameTimeToggle.isOn && _testTimer.usesRealTime) || (!UseGameTimeToggle.isOn && !_testTimer.usesRealTime) || Mathf.Abs(GetDurationValue() - _testTimer.duration) >= Mathf.Epsilon);
		}

		private float GetDurationValue()
		{
			float result;
			return (!float.TryParse(DurationField.text, out result)) ? 0f : result;
		}
	}
}
