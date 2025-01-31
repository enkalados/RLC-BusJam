namespace Gate
{
    public class FireSpeedGate : GateBase
    {
		#region Variables

		#endregion
		#region Properties 

		#endregion
		#region MonoBehaviour Methods

		#endregion
		#region My Methods
		protected override void OnShooted(int value)
		{
			currentValue += value;
			base.OnShooted(value);
		}
		#endregion
	}
}