using Base.Global.Enums;
using System.Drawing;
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
		int matIndex;
		Colors color;
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal void SetColor(Colors color, int matIndex=0)
		{
			this.matIndex = matIndex;
			this.color = color;

			try
			{
				if (meshRenderer2 != null)
				{
					m_materials = meshRenderer2.sharedMaterials;
					m_materials[matIndex] = materials.First(mat => mat.Color == color).Material;
					meshRenderer2.sharedMaterials = m_materials;
				}
				else if (meshRenderer != null)
				{
					m_materials = meshRenderer.sharedMaterials;
					m_materials[matIndex] = materials.First(mat => mat.Color == color).Material;
					meshRenderer.sharedMaterials = m_materials;
				}
			}
			catch (System.Exception)
			{
				Debug.LogWarning("Check Stickman Color Data");
				throw;
			}


		}
		internal void SetNormalMaterial()
		{
			if (meshRenderer2 != null)
			{
				m_materials = meshRenderer2.sharedMaterials;
				m_materials[matIndex] = materials.First(mat => mat.Color == color).Material;
				meshRenderer2.sharedMaterials = m_materials;
			}
			else if (meshRenderer != null)
			{
				m_materials = meshRenderer.sharedMaterials;
				m_materials[matIndex] = materials.First(mat => mat.Color == color).Material;
				meshRenderer.sharedMaterials = m_materials;
			}
		}
		internal void SetClickableMaterial()
		{
			if (meshRenderer2 != null)
			{
				m_materials = meshRenderer2.sharedMaterials;
				m_materials[matIndex] = materials.First(mat => mat.Color == color).ClickableMaterial;
				meshRenderer2.sharedMaterials = m_materials;
			}
			else if (meshRenderer != null)
			{
				m_materials = meshRenderer.sharedMaterials;
				m_materials[matIndex] = materials.First(mat => mat.Color == color).ClickableMaterial;
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
		public Material ClickableMaterial;
	}
}