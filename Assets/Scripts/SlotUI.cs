using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   public bool hasItem;

   private InventoryBag _inventoryBag;

   public Item item;

   private bool _isHighlighted;

   private void Awake()
   {
      _inventoryBag = FindObjectOfType<InventoryBag>();
      _isHighlighted = false;
   }

   private void OnDisable()
   {
      if (_isHighlighted)
      {
         _inventoryBag.RemoveItem(item);
         hasItem = false;
         _isHighlighted = false;
      }
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      _isHighlighted = true;
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      _isHighlighted = false;
   }
}
