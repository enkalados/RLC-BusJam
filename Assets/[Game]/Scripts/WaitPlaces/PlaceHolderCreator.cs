using Base.Global.Enums;
using Base.Managers;
using Base.Pool;
using BusSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace GridSystem.WaitPlace
{
	public class PlaceHolderCreator : MonoBehaviour
	{
		#region Variables
		GameObject placesParent;
		string parentName = "WaitTilesParent";
		List<WaitingTile> places = new List<WaitingTile>();
		int placeCount;
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
			if (placeCount != LevelManager.Instance.GetCurrentLevelData().PlaceHoldersCount)
			{
				placeCount = LevelManager.Instance.GetCurrentLevelData().PlaceHoldersCount;
				CreateWaitPlaces();
				BusPassengerControl.SetWaitPlacesList(places);
			}
		}
		void CreateWaitPlaces()
		{
			placesParent = GameObject.Find(parentName);
			for (int i = 0; i < placeCount; i++)
			{
				places.Add(PoolingManager.Instance.Instantiate(PoolID.PlaceHolderTile, placesParent.transform, placesParent.transform.position, Quaternion.identity).GetComponent<WaitingTile>());
				placesParent.GetComponent<OrderObjectsInEditor>().OrderObjects();
			}
		}
		#endregion
#if UNITY_EDITOR
		public void SetPlacesDataEditor(int count, PoolObject placeTile)
		{
			CreatePlacesEditor(count, placeTile);
		}
		void CreatePlacesEditor(int count, PoolObject placeTile)
		{
			placesParent = GameObject.Find(parentName);
			for (int i = 0; i < count; i++)
			{
				PoolObject item = (PoolObject)PrefabUtility.InstantiatePrefab(placeTile, placesParent.transform);
				item.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
				placesParent.GetComponent<OrderObjectsInEditor>().OrderObjects();
			}
		}
#endif
	}
}