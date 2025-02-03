using Base.Global.Enums;
using Base.Managers;
using Base.Pool;
using MeshColorSetter;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace BusSystem.Creator
{
	public class BusCreator : MonoBehaviour
	{
		#region Variables
		[SerializeField] GameObject busParent;
		List<GameObject> busObjectcs = new List<GameObject>();
		List<Colors> busList = new List<Colors>();
		const float DIST_BETWEEEN_BUS = -7;
		const int MAT_COLOR_INDEX = 1;

		#endregion
		#region Properties 
		BusPassengerControl busPassengerControl;
		BusPassengerControl BusPassengerControl => (busPassengerControl == null) ? busPassengerControl = GetComponent<BusPassengerControl>() : busPassengerControl;
		#endregion
		#region MonoBehaviour Methods
		private void OnEnable()
		{
			LevelManager.OnLevelStart.AddListener(GetData);
		}
		private void OnDisable()
		{
			LevelManager.OnLevelStart.RemoveListener(GetData);
		}
		#endregion
		#region Methods
		void GetData()
		{
			SetBusData(LevelManager.Instance.GetCurrentLevelData().BusColorList.ToList());
			BusPassengerControl.SetBusControlsPlacesList(busObjectcs);
		}
		void SetBusData(List<Colors> busList)
		{
			this.busList = busList;
			CreateBus();
		}
		void CreateBus()
		{
            for (int i = 0; i < busList.Count; i++)
            {
				PoolObject createdBus = PoolingManager.Instance.Instantiate(PoolID.Bus1, busParent.transform);
				createdBus.transform.SetLocalPositionAndRotation(new Vector3(0,0, DIST_BETWEEEN_BUS * i), Quaternion.identity);
				createdBus.GetComponent<MeshColorSet>().SetColor(busList[i], MAT_COLOR_INDEX);
				createdBus.GetComponent<BusControl>().SetBusColor(busList[i]);
				createdBus.GetComponent<BusControl>().SetDistanceValue(DIST_BETWEEEN_BUS);
				busObjectcs.Add(createdBus.gameObject);
			}
        }
		#endregion
#if UNITY_EDITOR
		public void SetBusDataEditor(List<Colors> busList, PoolObject bus)
		{
			CreateBusEditor(busList, bus);
		}
		void CreateBusEditor(List<Colors> busList, PoolObject bus)
		{
			for (int i = 0; i < busList.Count; i++)
			{
				PoolObject item = (PoolObject)PrefabUtility.InstantiatePrefab(bus,busParent.transform);
				item.transform.SetLocalPositionAndRotation(new Vector3(0, 0, DIST_BETWEEEN_BUS * i), Quaternion.identity);
				item.GetComponent<MeshColorSet>().SetColor(busList[i], MAT_COLOR_INDEX);
				item.GetComponent<BusControl>().SetBusColor(busList[i]);
			}
		}
#endif
	}
}