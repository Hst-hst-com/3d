using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<ItemData> collectedItems = new List<ItemData>();

    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(ItemData item)
    {
        if (item == null) return;

        if (collectedItems.Exists(i => i.id == item.id))
        {
            Debug.Log($"已拥有 {item.itemName}，不重复添加");
            return;
        }

        collectedItems.Add(item);
        
        // ⭐ 触发事件，通知所有订阅者（PlayerStats 会收到并更新得分）
        OnInventoryChanged?.Invoke();
        
        Debug.Log($"✅ 收集到：{item.itemName}，当前总数：{collectedItems.Count}");
    }

    public void RemoveItem(ItemData item)
    {
        if (collectedItems.Contains(item))
        {
            collectedItems.Remove(item);
            OnInventoryChanged?.Invoke();
        }
    }

    public List<ItemData> GetAllItems() => collectedItems;
    public int GetItemCount() => collectedItems.Count;
    public void ClearInventory()
    {
        collectedItems.Clear();
        OnInventoryChanged?.Invoke();
    }
    public bool HasItem(int itemId) => collectedItems.Exists(i => i.id == itemId);
}