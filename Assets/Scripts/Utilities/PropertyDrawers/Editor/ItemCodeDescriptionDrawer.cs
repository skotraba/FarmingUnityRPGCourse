﻿
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ItemCodeDescription))]
public class NewBehaviourScript : PropertyDrawer
{
  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {
      //Change return height to be double
    return EditorGUI.GetPropertyHeight(property) * 2;
  }

  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    EditorGUI.BeginProperty(position, label, property);

    if(property.propertyType == SerializedPropertyType.Integer)
    {
        EditorGUI.BeginChangeCheck(); //Start of check for changed values

        //Draw item code
        var newValue = EditorGUI.IntField(new Rect(position.x, position.y, position.width, position.height/2), label, property.intValue);

        //Draw item description
        EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Description", GetItemDescription(property.intValue));

        //if item code value has changed, then set value to new value
        if (EditorGUI.EndChangeCheck())
        {
            property.intValue = newValue;
        }
    }

    EditorGUI.EndProperty();
  }


  private string GetItemDescription(int itemCode)
  {
    SO_ItemList so_ItemList;
    so_ItemList = AssetDatabase.LoadAssetAtPath("Assets/ScritableObjects/Item/SO_ItemList.asset", typeof(SO_ItemList)) as SO_ItemList;

    List<ItemDetails> itemDetailsList = so_ItemList.itemDetails;

    ItemDetails itemDetail = itemDetailsList.Find(x => x.itemCode == itemCode);

    if(itemDetail != null)
    {
        return itemDetail.itemLongDescription;
    }
    else
    {
        return "";
    }

   }
}
