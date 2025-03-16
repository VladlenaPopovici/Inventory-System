using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InventoryBag : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public Transform[] itemSlots;
    public Image[] uiSlots;

    private Animator _animator;
    private static readonly int OnHover = Animator.StringToHash("OnHover");

    [SerializeField] private GameObject panelUI;
    
    public UnityEvent<Item> onItemAdded;
    public UnityEvent<Item> onItemRemoved;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        HideContents();
    }

    public void AddItem(Item item)
    {
        items.Add(item);
        item.transform.SetParent(itemSlots[item._id]);
        item.transform.localPosition = Vector3.zero;
        var slotUI = uiSlots[item._id].GetComponent<SlotUI>();
        slotUI.hasItem = true;
        slotUI.item = item;
        item.PickUp();
        
        onItemAdded.Invoke(item);
        
        ServerManager.Instance.SendDataToServer(item._id.ToString(), "item_added");
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        item.RespawnItem();
        
        onItemRemoved.Invoke(item);
        
        ServerManager.Instance.SendDataToServer(item._id.ToString(), "item_removed");
    }

    private void OnMouseOver()
    {
        _animator.SetBool(OnHover, true);
    }
    
    public void ShowContents()
    {
        panelUI.SetActive(true);
        
        foreach (var uiSlot in uiSlots)
        {
            if (uiSlot.GetComponent<SlotUI>().hasItem)
            {
                uiSlot.enabled = true;
            }
            else
            {
                uiSlot.enabled = false;
            }
        }
        
    }
    
    public void HideContents()
    {
        panelUI.SetActive(false);
        foreach (Image slot in uiSlots)
        {
            slot.enabled = false;
        }
    }
    
    private void OnMouseDown()
    {
        ShowContents();
    }

    private void OnMouseUp()
    {
        HideContents();
    }

    private void OnMouseExit()
    {
        _animator.SetBool(OnHover, false);
    }
    
    public bool IsOverBag(Vector3 position)
    {
        Collider collider = GetComponent<Collider>();
        return collider.bounds.Contains(position);
    }
}