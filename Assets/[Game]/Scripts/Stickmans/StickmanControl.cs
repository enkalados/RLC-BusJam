using Base.Global.Enums;
using Base.Pool;
using BusSystem;
using DG.Tweening;
using MeshColorSetter;
using Stickman.Animation;
using UnityEngine;
namespace Stickman
{
	public class StickmanControl : MonoBehaviour
	{
		#region Variables
		[SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
		Colors color;
		int gridX, gridZ;
		bool canClickable;

		const float MOVE_DURATION = .5f;
		const float BUS_WAIT_TIME = .2f;
		Sequence sequence;
		#endregion
		#region Properties 
		MeshColorSet matSet;
		MeshColorSet MaterialSet => (matSet == null) ? matSet = GetComponent<MeshColorSet>() : matSet;
		AnimationControl animationControl;
		AnimationControl AnimationControl => (animationControl == null) ? animationControl = GetComponent<AnimationControl>() : animationControl;
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			AnimationControl.IdleAnimation();
		}
		#endregion
		#region Methods
		internal void SetStickmanColor(Colors color)
		{
			this.color = color;
		}
		internal void SetGridInfo(int gridX, int gridz)
		{
			this.gridX = gridX;
			this.gridZ = gridz;
		}
		internal Colors GetColor()
		{
			return color;
		}
		internal int GetGridX() { return gridX; }
		internal int GetGridZ() { return gridZ; }
		internal bool GetCanClick()
		{
			return canClickable;
		}
		internal void SetCanClickable(bool state)
		{
			canClickable = state;
			if (state)
			{
				MaterialSet.SetClickableMaterial();
			}
			else
			{
				MaterialSet.SetNormalMaterial();
			}
		}
		internal void Clicked()
		{
			SetCanClickable(false);
		}
		internal void MoveToBus(Vector3 seatPosition, BusPassengerControl busPassengerControl, BusControl currentBus)
		{
			AnimationControl.RunAnimation();
			transform.SetParent(currentBus.transform);
			sequence = DOTween.Sequence();
			sequence.Append(transform.DOMove(currentBus.transform.position, MOVE_DURATION));
			sequence.AppendCallback(() => transform.SetPositionAndRotation(seatPosition, Quaternion.Euler(new Vector3(0,-270,0))));
			sequence.AppendInterval(BUS_WAIT_TIME);
			sequence.AppendCallback(() =>
			{
				AnimationControl.SitAnimation();
				currentBus.PassengerArrived();
			});
		}
		internal void MoveToTile(GameObject tle)
		{
			AnimationControl.RunAnimation();
			transform.DOMove(tle.transform.position, MOVE_DURATION).OnComplete(AnimationControl.IdleAnimation);
		}
		internal void DestroyStickman()
		{
			PoolingManager.Instance.DestroyPoolObject(gameObject.GetComponent<PoolObject>());
		}
		internal void ResetStickman(GameObject parent)
		{
			transform.SetParent(parent.transform);
			transform.SetLocalPositionAndRotation(new Vector3(gridX, 0, -gridZ), Quaternion.identity);
			SetCanClickable(false);
		}
		#endregion
	}
}