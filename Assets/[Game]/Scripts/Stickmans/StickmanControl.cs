using Base.Global.Enums;
using Base.Pool;
using BusSystem;
using DG.Tweening;
using MeshColorSetter;
using UnityEngine;
namespace Stickman
{
	public class StickmanControl : MonoBehaviour
	{
		#region Variables
		[SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
		Colors color;
		int gridX, gridZ;
		[SerializeField] bool canClickable;

		const float MOVE_DURATION = .5f;
		const float BUS_WAIT_TIME = .5f;
		Sequence sequence;
		#endregion
		#region Properties 
		MeshColorSet matSet;
		MeshColorSet MaterialSet => (matSet == null) ? matSet = GetComponent<MeshColorSet>() : matSet;
		#endregion
		#region MonoBehaviour Methods
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
		internal void MoveToBus(BusControl bus, BusPassengerControl busPassengerControl)
		{
			transform.SetParent(bus.transform);
			if (sequence == null)
			{
				sequence = DOTween.Sequence();
			}
			sequence.Append(transform.DOMove(bus.transform.position, MOVE_DURATION));
			sequence.AppendCallback(() => transform.position = bus.GetEmptySeat());
			sequence.AppendInterval(BUS_WAIT_TIME);
			sequence.AppendCallback(() => busPassengerControl.CheckNewBus());
		}
		internal void MoveToTile(GameObject tle)
		{
			transform.DOMove(tle.transform.position, MOVE_DURATION);
		}
		internal void DestroyStickman()
		{
			PoolingManager.Instance.DestroyPoolObject(gameObject.GetComponent<PoolObject>());
		}
		internal void ResetStickman(GameObject parent)
		{
			transform.SetParent(parent.transform);
			transform.SetLocalPositionAndRotation(new Vector3(gridX, 0, -gridZ), Quaternion.identity);
		}
		#endregion
	}
}