
using System.Collections.Generic;
using UnityEngine;

//Directive
[CreateAssetMenu(fileName = "SO_ItemList", menuName="ScritableObjects/Item/Item List")]

public class SO_ItemList : ScriptableObject
{
  [SerializeField]
  public List<ItemDetails> itemDetails;

}
