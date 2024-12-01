using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents a slot in the inventory UI.
/// </summary>
public class InventorySlot_UI : MonoBehaviour
{
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private InventorySlot assignedInventorySlot;

    private Button button;

    public InventorySlot AssignedInventorySlot => assignedInventorySlot;
    public InventoryDisplay ParentDisplay { get; private set; }

    /// <summary>
    /// Initializes the inventory slot UI.
    /// </summary>
    private void Awake()
    {
        ClearSlot();
        
        itemSprite.preserveAspect = true;

        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUISlotClick);

        ParentDisplay = transform.parent.GetComponent<InventoryDisplay>();
    }

    /// <summary>
    /// Initializes the inventory slot UI.
    /// </summary>
    /// <param name="slot"></param>
    public void Init(InventorySlot slot)
    {
        assignedInventorySlot = slot;
        UpdateUISlot(slot);
    }

    /// <summary>
    /// Updates the UI slot with the given slot data.
    /// </summary>
    /// <param name="slot"></param>
    public void UpdateUISlot(InventorySlot slot)
    {

        if(slot.ItemData != null)
        {
            itemSprite.sprite = slot.ItemData.Icon;
            itemSprite.color = Color.white;

            if (slot.StackSize > 1) itemCount.text = slot.StackSize.ToString();
            else itemCount.text = "";
        }
        else
        {
            itemSprite.color = Color.clear;
            itemSprite.sprite = null;
            itemCount.text = "";
        }
    }

    /// <summary>
    ///Updates the UI slot.
    /// </summary>
    public void UpdateUISlot()
    {
        if (assignedInventorySlot != null) UpdateUISlot(assignedInventorySlot);
    }

    /// <summary>
    /// Clears the slot.
    /// </summary>
    public void ClearSlot()
    {
        assignedInventorySlot?.ClearSlot();
        itemSprite.sprite = null;
        itemSprite.color = Color.clear;
        itemCount.text = "";
    }
    
    /// <summary>
    ///Called when the UI slot is clicked.
    /// </summary>

    public void OnUISlotClick()
    {
        ParentDisplay?.SlotClicked(this);
    }
}