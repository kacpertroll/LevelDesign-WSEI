using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Prosty system ekwipunku: gracz mo¿e trzymaæ jeden przedmiot naraz.
/// Podnieœ przedmiot wywo³uj¹c PickUp(ItemData) — np. przez Unity Event
/// na obiekcie kluczyka w scenie.
/// </summary>
public class InventoryController : MonoBehaviour
{
    [Header("Aktualny przedmiot")]
    [SerializeField] private ItemData currentItem;

    [Header("Events")]
    public UnityEvent<ItemData> OnItemPickedUp;
    public UnityEvent OnItemUsed;

    // -------------------------------------------------------

    /// <summary>
    /// Sprawdza czy gracz posiada dany przedmiot.
    /// </summary>
    public bool HasItem(ItemData item)
    {
        return currentItem != null && currentItem == item;
    }

    /// <summary>
    /// Gracz podnosi przedmiot. Podepnij do Unity Event na pickupie w scenie.
    /// </summary>
    public void PickUp(ItemData item)
    {
        currentItem = item;
        Debug.Log($"[Inventory] Podniesiono: {item.itemName}");
        OnItemPickedUp?.Invoke(item);
    }

    /// <summary>
    /// Usuwa przedmiot z ekwipunku (np. po u¿yciu klucza).
    /// </summary>
    public void UseCurrentItem()
    {
        if (currentItem == null) return;

        Debug.Log($"[Inventory] U¿yto: {currentItem.itemName}");
        currentItem = null;
        OnItemUsed?.Invoke();
    }

    /// <summary>
    /// Getter — przydatny do warunków w innych skryptach.
    /// </summary>
    public ItemData GetCurrentItem() => currentItem;
}