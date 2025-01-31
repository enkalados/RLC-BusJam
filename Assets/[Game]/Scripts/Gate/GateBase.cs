using Base.Collectable;
using DG.Tweening;
using Shootable;
using TMPro;
using UnityEngine;

namespace Gate
{
	public abstract class GateBase : MonoBehaviour, ICollectable, IShootable
	{
		#region Variables
		[SerializeField] internal TextMeshPro valueText;
		[SerializeField] internal float currentValue;
		[SerializeField] MeshRenderer gateMesh;
		[SerializeField] Material possitiveMaterial;
		[SerializeField] Material negativeMaterial;
		#region DoTween Parameters
		const float hitCompletedTweenDuration = .3f;
		#endregion
		#endregion
		#region Properties 

		#endregion
		#region MonoBehaviour Methods
		private void Start()
		{
			CheckValueForMaterial();
			UpdateValueText();
		}
		#endregion
		#region My Methods
		public void Collect(Collector collector)
		{
			OnCollected(collector);
		}
		protected virtual void OnCollected(Collector collector)
		{
			CloseAnimation();
		}
		public void Shoot(int value)
		{
			OnShooted(value);
		}
		protected virtual void OnShooted(int value)
		{
			UpdateValueText();
			CheckValueForMaterial();
			HitAnimation();
		}
		protected virtual void UpdateValueText()
		{
			if (valueText == null) { return; }
			valueText.text = currentValue.ToString("F1");
		}
		void CloseAnimation()
		{
			GetComponent<Collider>().enabled = false;
			transform.DOMoveY(-10, .3f);
		}
		void HitAnimation()
		{
			transform.DOComplete();
			transform.localScale = Vector3.one + Vector3.one * 0.1f;
			transform.DOScale(1, hitCompletedTweenDuration).SetEase(Ease.OutBack);
		}
		void CheckValueForMaterial()
		{
			if (currentValue >= 0)
			{
				gateMesh.material = possitiveMaterial;
			}
			else
			{
				gateMesh.material = negativeMaterial;
			}
		}
		#endregion
	}
}