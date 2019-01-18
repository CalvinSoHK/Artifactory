using System.Collections.Generic;
/// <summary>
/// Interface to interact with inventories.
/// </summary>
public interface IInventory
{
    /// <summary>
    /// Returns the entity this inventory is attached to.
    /// </summary>
    /// <returns></returns>
    GenericEntity GetEntity();

    /// <summary>
    /// Whether or not the inventory contains a given item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool ContainsItem(GenericItem item);

    /// <summary>
    /// Attempts to find a single item to extract, returns an item if possible, else null.
    /// </summary>
    /// <returns></returns>
    GenericItem AttemptExtractItem();

    /// <summary>
    /// Attempts to add an item to the inventory with a given count.
    /// Returns true if it succeeds, false otherwise.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    bool AttemptAddItem(GenericItem item, int count);

    /// <summary>
    /// Attempts to extract a given COUNT of items type ITEM.
    /// Returns the item itself it succeeds, else null.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    GenericItem ExtractItem(GenericItem item, int count);

    /// <summary>
    /// Attempts to extract one copy of given item from inventory.
    /// Returns the item itself if it succeeds, else null.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    GenericItem ExtractSingleItem(GenericItem item);

    /// <summary>
    /// Used by the UI when the player manually clicks on a given item slot.
    /// Extracts items from the given slot index with a given count.
    /// Returns the item if successful, null otherwise.
    /// </summary>
    /// <param name="item_index"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    GenericItem ExtractItem(int item_index, int count);

    /// <summary>
    /// Used by the UI when the player manually clicks on a given item slot.
    /// Extracts a single copy of the item from the given slot index.
    /// Returns the item if successful, null otherwise.
    /// </summary>
    /// <param name="item_index"></param>
    /// <returns></returns>
    GenericItem ExtractSingleItem(int item_index);

    /// <summary>
    /// Adds the given ITEM with a given COUNT to the inventory.
    /// Returns an int that is the remainder of items that couldn't be placed in.
    /// 0, means a perfect fit.
    /// Any positive number means we couldn't place everything.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    int AddItem(GenericItem item, int count);

    /// <summary>
    /// Adds one copy of the given ITEM to the inventory.
    /// Returns an int that is the remainder of items that couldn't be placed in.
    /// 0, means a perfect fit.
    /// Any positive number means we couldn't place everything.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    int AddSingleItem(GenericItem item);

    /// <summary>
    /// Adds an ITEM to a given slot INDEX with a given COUNT to the inventory.
    /// Returns an int that is the remainder of items that couldn't be placed in.
    /// 0, means a perfect fit.
    /// Any positive number means we couldn't place everything.
    /// </summary>
    /// <param name="item_index"></param>
    /// <param name="count"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    int AddItem(int item_index, int count, GenericItem item);

    /// <summary>
    /// Adds a single ITEM at the given slot INDEX.
    /// Returns an int that is the remainder of items that couldn't be placed in.
    /// 0, means a perfect fit.
    /// Any positive number means we couldn't place everything.
    /// </summary>
    /// <param name="item_index"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    int AddSingleItem(int item_index, GenericItem item);

    /// <summary>
    /// Checks to see if the inventory is full.
    /// </summary>
    /// <returns></returns>
    bool IsFull();

    /// <summary>
    /// Returns a count of how many of the given item are in this inventory.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    int CountItem(GenericItem item);

    /// <summary>
    /// Returns a list of Item Amounts, which have both Item and Count.
    /// </summary>
    /// <returns></returns>
    List<ItemAmount> GetItemAmountList();

    /// <summary>
    /// Returns a list of unique items.
    /// </summary>
    /// <returns></returns>
    List<GenericItem> GetUniqueItemsList();

}
