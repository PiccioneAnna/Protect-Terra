using Inventory;

public class ItemGoal : Quest.QuestGoal
{
    private Manager inventoryManager;
    public Item item;
    private int count;

    public override string GetDescription()
    {
        return "Obtain" + count + " " + item.name;
    }

    public override void Initialize()
    {
        base.Initialize();
        inventoryManager = GameManager.Instance.inventory;
        EventManager.Instance.AddListener<ItemGameEvent>(OnItem);
    }

    private void OnItem(ItemGameEvent ge)
    {
        foreach (Slot slot in inventoryManager.inventorySlots)
        {
            Item itemInSlot = slot.GetComponentInChildren<Item>();
            if(itemInSlot != null && itemInSlot.item == item.item)
            {
                count = inventoryManager.CheckItemCount(item.item);
            }
        }

        if (count > 0)
        {
            CurrentAmount = count;
            Evaluate();
        }
    }
}
