using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemConfiguration _itemConfiguration;
    
    public uint _id;
    private string _type;
    private string _itemName;
    private float _weight;
    private Sprite _icon;

    private Rigidbody _itemRigidbody;

    [SerializeField] private Vector3 _spawnPoint;

    public Image icon;

    private InventoryBag _inventoryBag;
    
    void Start()
    {
        _itemRigidbody = GetComponent<Rigidbody>();
        _spawnPoint = transform.position;
        _inventoryBag = FindObjectOfType<InventoryBag>();

        if (_itemConfiguration != null)
        {
            _id = _itemConfiguration.id;
            _type = _itemConfiguration.type;
            _itemName = _itemConfiguration.itemName;
            _weight = _itemConfiguration.weight;
            _icon = _itemConfiguration.icon;
        }

    }
    
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPosition();
        PickUp();
    }

    private void OnMouseDrag()
    {
        Vector3 mousePosition = GetMouseWorldPosition() + offset;
        transform.position = mousePosition;
    }

    private void OnMouseUp()
    {
        Vector3 mousePosition = GetMouseWorldPosition() + offset;

        RaycastHit hit;
        if (Physics.Raycast(mousePosition + Vector3.up * 10, Vector3.down, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Ground"))
            {
                mousePosition.y = hit.point.y + 0.1f;
            }
        
        }
        
        if (_inventoryBag.IsOverBag(transform.position))
        {
            _inventoryBag.AddItem(this);
        }
        else
        {
            transform.position = mousePosition;

            Drop();
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void PickUp()
    {
        _itemRigidbody.isKinematic = true;
    }

    public void RespawnItem()
    {
        _spawnPoint.y = 1f;
        // transform.position = _spawnPoint;
        Drop();
        _itemRigidbody.MovePosition(_spawnPoint);
    }

    public void Drop()
    {
        _itemRigidbody.isKinematic = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        //TODO think more about physics
        if (other.collider.CompareTag("Wall"))
        {
            _spawnPoint.y = 2f;
            // transform.position = _spawnPoint;
            Drop();
            _itemRigidbody.AddForce(_spawnPoint);
            
            var vector3 = transform.position;
            vector3.z = _spawnPoint.z;
            transform.position = vector3;
        }
    }
}
