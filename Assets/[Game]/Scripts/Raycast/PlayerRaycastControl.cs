using GridSystem;
using Stickman;
using UnityEngine;
namespace PlayerRay
{
	public class PlayerRaycastControl : MonoBehaviour
	{
		#region Variables
		Vector3 mousePos;
		Ray ray;
		RaycastHit hit;
		#endregion
		#region Properties 
		GridStickmanControl gridControl;
		GridStickmanControl GridStickmanControl => (gridControl == null) ? gridControl = GetComponent<GridStickmanControl>() : gridControl;
		#endregion
		#region MonoBehaviour Methods
		private void Update()
		{
			mousePos = Input.mousePosition;
			ray = Camera.main.ScreenPointToRay(mousePos);

			if (Physics.Raycast(ray, out hit, 50))
			{
				Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
				if (hit.collider.gameObject.TryGetComponent(out StickmanControl pointer))
				{
					Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.green);
					if (Input.GetMouseButtonDown(0))
					{
						if (pointer.GetCanClick())
						{
							GridStickmanControl.CheckClickedStickman(pointer.GetGridX(), pointer.GetGridZ());
						}
					}

				}
			}
		}
		#endregion
		#region Methods
		#endregion
	}
}