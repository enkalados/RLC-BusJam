using UnityEngine;
using Lean.Touch;
using CW.Common;

public class LeanDragTranslateWithClamp : LeanDragTranslate
{
	[SerializeField] private ClampData _clampData;
	public ClampData ClampData => _clampData;

	[SerializeField] private float moveSpeed;
	public float MoveSpeed => moveSpeed;

	protected override void Translate(Vector2 screenDelta)
	{
		// Make sure the camera exists
		var camera = CwHelper.GetCamera(this._camera, gameObject);

		if (camera != null)
		{
			// Screen position of the transform
			var screenPoint = camera.WorldToScreenPoint(transform.position);

			// Add the deltaPosition
			screenPoint += (Vector3)screenDelta * Sensitivity;

			// Convert back to world space
			Vector3 worldPosition = camera.ScreenToWorldPoint(screenPoint);

			transform.localPosition = Clamp(worldPosition);
		}
		else
		{
			Debug.LogError("Failed to find camera. Either tag your camera as MainCamera, or set one in this component.", this);
		}
	}

	private Vector3 Clamp(Vector3 worldPosition)
	{
		Vector3 targetLocalPosition = ConvertLocalPosition(worldPosition);
		Vector3 clampedPosition = transform.localPosition;

		//We want to get only x position
		clampedPosition.x = targetLocalPosition.x;

		float border = ClampData.MovementWidth / 2f;
		clampedPosition.x = Mathf.Clamp(clampedPosition.x, -border, border);

		return clampedPosition;
	}

	private Vector3 ConvertLocalPosition(Vector3 worldPosition)
	{
		Vector3 localPosition = worldPosition;
		Transform parent = transform.parent;

		if (parent != null)
			localPosition = parent.InverseTransformPoint(worldPosition);

		return localPosition;
	}
}

#if UNITY_EDITOR
namespace Lean.Touch.Editor
{
	using UnityEditor;
	using TARGET = LeanDragTranslateWithClamp;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET), true)]
	public class LeanDragTranslate_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);
			Draw("moveSpeed");
			Draw("_clampData", "The clamp data the clamp will be calculated using this.");
			Draw("Use");

			Draw("_camera", "The camera the translation will be calculated using.\n\nNone/null = MainCamera.");
			Draw("sensitivity", "The movement speed will be multiplied by this.\n\n-1 = Inverted Controls.");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
			Draw("inertia", "This allows you to control how much momentum is retained when the dragging fingers are all released.\n\nNOTE: This requires <b>Damping</b> to be above 0.");
		}
	}
}
#endif
