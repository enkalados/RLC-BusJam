using UnityEngine;
using DG.Tweening;
using System;
using Base.Managers;
using Base.Global.Enums;

namespace Base.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public class EasyPanel : MonoBehaviour, IPanel
    {
        #region Variables
        [SerializeField] private PanelAnimationTypes panelAnimationTypes;

		const float PANEL_FADE_DURATION = .25f;
		const float PANEL_SCALE_DURATION = .25f;
        #endregion
        #region Properties 
        [field: SerializeField] public PanelID PanelID { get; private set; }

        private CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup { get { return canvasGroup == null ? canvasGroup = GetComponent<CanvasGroup>() : canvasGroup; } }
        #endregion
        #region MonoBehaviour Methods
        protected virtual void OnEnable()
        {
            UIManager.Instance.AddPanel(this);
        }
        protected virtual void OnDisable()
        {
            UIManager.Instance?.RemovePanel(this);
        }
		#endregion
		#region My Methods
		//[Button]
		public virtual void ShowPanel()
		{
			CanvasGroup.alpha = 1;
			CanvasGroup.blocksRaycasts = true;
			CanvasGroup.interactable = true;
		}

		//[Button]
		public virtual void ShowPanelAnimated()
		{
			switch (panelAnimationTypes)
			{
				case PanelAnimationTypes.Fade:
					FadePanel(1, PANEL_FADE_DURATION, ShowPanel);
					break;
				case PanelAnimationTypes.Scale:
					ScalePanel(true, PANEL_SCALE_DURATION);
					break;
			}
		}

		//[Button]
		public virtual void HidePanel()
		{
			CanvasGroup.alpha = 0;
			CanvasGroup.blocksRaycasts = false;
			CanvasGroup.interactable = false;
		}

		//[Button]
		public virtual void HidePanelAnimated()
		{
			switch (panelAnimationTypes)
			{
				case PanelAnimationTypes.Fade:
					FadePanel(0, PANEL_FADE_DURATION, HidePanel);
					break;
				case PanelAnimationTypes.Scale:
					ScalePanel(false, PANEL_SCALE_DURATION);
					break;
			}
		}

		private void FadePanel(float value, float duration, Action onComplete = null)
		{
			CanvasGroup.DOFade(value, duration).OnComplete(() => onComplete());
		}

		private void ScalePanel(bool isShow, float duration)
		{
			transform.DOComplete();
			if (isShow)
			{
				transform.localScale = Vector3.zero;

				transform.DOScale(Vector3.one, duration).OnStart(ShowPanel);
			}
			else
			{
				transform.DOScale(Vector3.zero, duration).OnComplete(() => { HidePanel(); transform.localScale = Vector3.one; });
			}
		}
		#endregion
	}
}