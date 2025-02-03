using Base.Global.Enums;
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
		internal int GetGridX() { return gridX; }
		internal int GetGridZ() { return gridZ; }
		internal bool GetCanClick()
		{
			return canClickable;
		}
		internal void SetCanClickable(bool state)
		{
			print(gridX+" - "+gridZ+" - "+state);
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
			gameObject.transform.localScale = Vector3.zero;
		}
		#endregion
	}
}