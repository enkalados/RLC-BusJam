using Base.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
namespace Timer
{
	public class TimerControl : MonoBehaviour
	{
		#region Variables
		[SerializeField] TextMeshProUGUI timeText;
		float currentTime=0;
		float lastCountdownCheck = 11f;
		float dangerTimeStart = 10f;
		bool timerStarted=false;

		const float TEXT_SCALE = 2f;
		const float TEXT_SCALE_DURATION = .3f;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			GetTimerData();

			LevelManager.OnLevelFinish.AddListener(GetTimerData);
			LevelManager.OnLevelStart.AddListener(StartTimer);
		}
		private void OnDisable()
		{
			LevelManager.OnLevelFinish.RemoveListener(GetTimerData);
			LevelManager.OnLevelStart.RemoveListener(StartTimer);
		}
		private void Update()
		{
			if (timerStarted)
			{
				currentTime -= Time.deltaTime;
				UpdateText();
				if(currentTime < dangerTimeStart)
				{
					if (Mathf.Floor(lastCountdownCheck) != Mathf.Floor(currentTime))
					{
						lastCountdownCheck = currentTime;
						DangerTimeAnimation();
					}
				}
				if (currentTime <= 0)
				{
					TimerFail();
				}
			}
		}
		#endregion
		#region Methods
		void TimerFail()
		{
			StopTimer();
			LevelManager.Instance.CompleteLevel(false);
		}
		void StartTimer()
		{
			timerStarted = true;
		}
		void StopTimer()
		{
			timerStarted = false;
		}
		void GetTimerData()
		{
			ResetTimer();
			currentTime=LevelManager.Instance.GetCurrentLevelData().Timer;
			UpdateText();
		}
		void ResetTimer()
		{
			StopTimer();
			timeText.color = Color.white;
			currentTime = 0;
			UpdateText();
		}
		void UpdateText()
		{
			timeText.text = (int)currentTime + "";
		}
		void DangerTimeAnimation()
		{
			timeText.color = Color.red;
			timeText.transform.localScale = Vector3.one* TEXT_SCALE;
			timeText.transform.DOScale(Vector3.one, TEXT_SCALE_DURATION);
		}
		#endregion
	}
}