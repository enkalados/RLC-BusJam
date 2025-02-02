using Base.Global.Enums;
using System.Linq;
using UnityEngine;
namespace MeshColorSetter
{
	public class MeshColorSet : MonoBehaviour
	{
		#region Variables
		[SerializeField] MeshRenderer meshRenderer;
		[SerializeField] SkinnedMeshRenderer meshRenderer2;
		[SerializeField] ColorMaterial[] materials;
		Material[] m_materials;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal void SetColor(Colors color, int matIndex=0)
		{
			if (meshRenderer2 != null)
			{
				m_materials = meshRenderer2.sharedMaterials;
				m_materials[matIndex] = materials.First(mat => mat.Color == color).Material;
				meshRenderer2.sharedMaterials = m_materials;
			}
			else if(meshRenderer != null)
			{
				m_materials = meshRenderer.sharedMaterials;
				m_materials[matIndex] = materials.First(mat => mat.Color == color).Material;
				meshRenderer.sharedMaterials = m_materials;
			}

		}
		#endregion
	}
	[System.Serializable]
	public class ColorMaterial
	{
		public Colors Color;
		public Material Material;
	}
}