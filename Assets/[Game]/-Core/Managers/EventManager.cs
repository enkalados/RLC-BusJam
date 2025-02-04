using Base.Global.Enums;
using UnityEngine.Events;

namespace Base.Utilities.Events
{
	public static class EventManager
	{
		public static IntEvent OnMoneyAdded = new IntEvent();
		public static IntEvent OnMoneyRemoved = new IntEvent();
		public static IntEvent OnMoneyUpdated = new IntEvent();

		public static UnityEvent OnBusFull = new UnityEvent();
		//public static IdleUpgradeEvent OnUpgradedElement = new IdleUpgradeEvent();
	}
	//public class IdleUpgradeEvent : UnityEvent<UpgradeElementID, int> { }
	public class IntEvent : UnityEvent<int> { }
}