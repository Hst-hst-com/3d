using UnityEngine;
using UnityEngine.Events;

public class Checkpoint : MonoBehaviour
{
    public ItemData itemData;
    public UnityEvent onCollected;
    public string collectMessage = "已打卡：{0}";
    public AudioClip collectSound;

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected) return;
        if (!other.CompareTag("Player")) return;
        Collect();
    }

    private void Collect()
    {
        isCollected = true;

        onCollected?.Invoke();

        if (TryGetComponent<Collider>(out var col))
            col.enabled = false;

        InventoryManager.Instance.AddItem(itemData);

        // ⭐ 改成 CollectPromptManager
        if (CollectPromptManager.Instance != null)
        {
            CollectPromptManager.Instance.ShowCheckpointCollected(itemData);
        }
        else
        {
            Debug.Log($"✅ 收集到：{itemData.itemName}");
        }

        PlayCollectSound();
    }

    private void PlayCollectSound()
    {
        if (collectSound != null && AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(collectSound);
        }
    }
}