using UnityEngine;
using UnityEditor;

public class OrderObjectsInEditor : MonoBehaviour
{
    #region Parameters
    public GameObject needToOrderObjectsParent;
    public bool OrderX;
    //public bool OrderY;
    //public bool OrderZ;

    public float XDistance;
    //public float YDistance;
    //public float ZDistance;
    #endregion
    #region My Methods
    public void OrderObjects()
    {
        int a = needToOrderObjectsParent.transform.childCount % 2;
        if (a == 0) //çift ise
        {
            for (int i = 0; i < needToOrderObjectsParent.transform.childCount; i++)
            {
                needToOrderObjectsParent.transform.GetChild(i).gameObject.transform.localPosition =
                    new Vector3(-(XDistance / 2 + (((needToOrderObjectsParent.transform.childCount / 2) - 1) * XDistance)) + i * XDistance, needToOrderObjectsParent.transform.GetChild(i).gameObject.transform.localPosition.y, needToOrderObjectsParent.transform.GetChild(i).gameObject.transform.localPosition.z);
            }
        }
        else //tek ise
        {
            for (int i = 0; i < needToOrderObjectsParent.transform.childCount; i++)
            {
                needToOrderObjectsParent.transform.GetChild(i).gameObject.transform.localPosition =
                    new Vector3(-(Mathf.FloorToInt(needToOrderObjectsParent.transform.childCount / 2) * XDistance) + i * XDistance, needToOrderObjectsParent.transform.GetChild(i).gameObject.transform.localPosition.y, needToOrderObjectsParent.transform.GetChild(i).gameObject.transform.localPosition.z);
            }
        }
    }
    #endregion
}

[CustomEditor(typeof(OrderObjectsInEditor))]
public class ObjectsOrderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        OrderObjectsInEditor myObject = (OrderObjectsInEditor)target;
        myObject.needToOrderObjectsParent = (GameObject)EditorGUILayout.ObjectField(myObject.needToOrderObjectsParent, typeof(GameObject), true);

        GUILayout.Label("Select Axes To Sort", EditorStyles.boldLabel);
        myObject.OrderX = GUILayout.Toggle(myObject.OrderX, "Order X");
        if (myObject.OrderX)
        {
            myObject.XDistance = EditorGUILayout.FloatField("X Distance:", myObject.XDistance);
        }
        //myObject.OrderY = GUILayout.Toggle(myObject.OrderY, "Order Y");
        //if (myObject.OrderY)
        //{
        //    myObject.YDistance = EditorGUILayout.FloatField("Y Distance:", myObject.YDistance);
        //}
        //myObject.OrderZ = GUILayout.Toggle(myObject.OrderZ, "Order Z");
        //if (myObject.OrderZ)
        //{
        //    myObject.ZDistance = EditorGUILayout.FloatField("Z Distance:", myObject.ZDistance);
        //}
        if (GUILayout.Button("Order Objects"))
        {
            myObject.OrderObjects();
        }
    }
}
