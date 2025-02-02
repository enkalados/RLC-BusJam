using Base.Global.Enums;
using System.Linq;
using UnityEngine;
namespace Stickman.Creator
{
	public class StickmanSet : MonoBehaviour
	{
		#region Variables
		[SerializeField] MeshRenderer meshRenderer;
		[SerializeField] StickmanColorMat[] materials;
		Material[] m_materials;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal void SetColor(Colors color)
		{
			m_materials = meshRenderer.sharedMaterials;
			m_materials[0] = materials.First(mat => mat.Color == color).Material;
			meshRenderer.sharedMaterials = m_materials;
		}
		#endregion
	}
	[System.Serializable]
	public class StickmanColorMat
	{
		public Colors Color;
		public Material Material;
	}
}