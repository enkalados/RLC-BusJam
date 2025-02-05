using Base.Global.Animation;
using UnityEngine;
namespace Stickman.Animation
{
	public class AnimationControl : MonoBehaviour
	{
		#region Variables
		[SerializeField] Animator animator; 
		#endregion
		#region Properties 
		#endregion
		#region MonoBehaviour Methods
		#endregion
		#region Methods
		internal void RunAnimation()
		{
			animator.SetBool(AnimationKeys.RunAnimationKey,true);
			animator.SetBool(AnimationKeys.SitAnimationKey,false);
		}
		internal void SitAnimation()
		{
			animator.SetBool(AnimationKeys.SitAnimationKey,true);
			animator.SetBool(AnimationKeys.RunAnimationKey,false);
		}
		internal void IdleAnimation()
		{
			animator.SetBool(AnimationKeys.RunAnimationKey,false);
			animator.SetBool(AnimationKeys.SitAnimationKey,false);
		}
		#endregion
	}
}