using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

/// <summary>
/// Behavior that adds an inventory to the given entity.
/// Has helper functions for manipulating the inventory.
/// </summary>
[RequireComponent(typeof(EntityController))]
public class Inventory : MonoBehaviour, IInventory
{
    /// <summary>
    /// Modifiers for how this inventory should be treated.
    /// </summary>
    [EnumFlag]
    public INVENTORY_MODIFIERS INV_FLAGS;

    /// <summary>
    /// How big of an inventory this has in terms of slots.
    /// </summary>
    public int inventory_size = 10;

    /// <summary>
    /// Each index is one of the given slots in the inventory.
    /// Stores what type of item is in each slot.
    /// </summary>
    public GenericItem[] contained_items;

    /// <summary>
    /// How many of an item are in each slot.
    /// </summary>
    public int[] item_counts;

    //Init the inventory
    private void Start()
    {
        InitInventory();
    }

    /// <summary>
    /// Initialize the inventory
    /// Sets contained_items to length inventory_size of nulls.
    /// Sets item_counts to length inventory_size of 0s.
    /// </summary>
    public void InitInventory()
    {
        //Initialize all inventory parts
        contained_items = new GenericItem[inventory_size];
        item_counts = new int[inventory_size];

        //Set all inventory items to empty
        //Set all item count values to 0
        for (int i = 0; i < inventory_size; i++)
        {
            contained_items[i] = null;
            item_counts[i] = 0;
        }
    }

    /// <summary>
    /// Load inventory based on inputted list
    /// </summary>
    /// <param name="ITEMS"></param>
    /// <param name="COUNTS"></param>
    public void LoadInventory(GenericItem[] ITEMS, int[] COUNTS)
    {
        //If the length of both arrays aren't the same, then this doesn't work
        if (ITEMS.Length != COUNTS.Length)
        {
            throw new System.Exception("Invalid inventory loading. Item list length doesn't match the item count length.");
        }

        // Init the inventory before loading in items
        InitInventory();

        // For the items in items and counts, load them into the array.
        for(int index = 0; index < ITEMS.Length; index++)
        {
            contained_items[index] = ITEMS[index];
            item_counts[index] = COUNTS[index];
        }
    }

    /// <summary>
    /// Checks to see if the inventory contains a given item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool ContainsItem(GenericItem item)
    {
        for(int i = 0; i < inventory_size; i++)
        {
            if(contained_items[i] == item)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns the first empty index in the inventory.
    /// </summary>
    /// <returns> Index of the first empty index. -1 if there is no empty index.</returns>
    public int GetNextEmptyIndex()
    {
        for(int i = 0; i < inventory_size; i++)
        {
            if(item_counts[i] == 0)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Returns all empty indexes in this inventory.
    /// </summary>
    /// <returns></returns>
    public List<int> GetAllEmptyIndexes()
    {
        List<int> EMPTIES = new List<int>();
        for (int i = 0; i < inventory_size; i++)
        {
            if (item_counts[i] == 0)
            {
                EMPTIES.Add(i);
            }
        }
        return EMPTIES;
    }

    /// <summary>
    /// Helper function that decrements the item at given index with a given count.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void DecrementItem(int index, int count)
    {
        // Decrement by count.
        item_counts[index] -= count;

        // If the item count is 0, set item to null.
        if(item_counts[index] == 0)
        {
            contained_items[index] = null;
        }

        // If the item count is negative throw an error.
        if(item_counts[index] < 0)
        {
            throw new System.Exception("Error, negative count of item.");
        }
    }

    /// <summary>
    /// Attempts to extract the first item we find in the inventory.
    /// Used so that we only extract items if they have somewhere to go.
    /// Doesn't do any decrementing, it's just searching for a candidate to extract.
    /// </summary>
    /// <returns> Returns the item we want to try to extract. </returns>
    public GenericItem AttemptExtractItem()
    {
        // Loop through the inventory and return the first item we find.
        for (int i = 0; i < inventory_size; i++)
        {
            if (item_counts[i] > 0)
            {
                return contained_items[i];
            }
        }
        return null;
    }

    /// <summary>
    /// Attempts to add an item to the inventory.
    /// Doesn't actually decrement, just checks if it is possible.
    /// </summary>
    /// <returns>True if possible, false if not.</returns>
    public bool AttemptAddItem(GenericItem ITEM, int COUNT)
    {
        //Iterate over all contained items
        for (int i = 0; i < contained_items.Length; i++)
        {
            //Check if its the item we want, and it has less than the max stack size.
            //If it is the max stack size we can ignore it and find other items.
            //Whenever we reach this if statement, we will always return and terminate this function call.
            if (contained_items[i] != null && contained_items[i] == ITEM && item_counts[i] < ITEM.STACK_SIZE)
            {
                // Increment the given index i by COUNT.
                // If the COUNT pushes the stack over max stack size, call AddItem again with the difference when we add as much as we can.
                // If adding the full count will not go over stack size, jsut add it.
                if (item_counts[i] + COUNT <= contained_items[i].STACK_SIZE)
                {
                    return true;
                }
                // If it will go over, add as much as we can then call this function again.
                else
                {
                    //Get the difference that is going to be a new stack.
                    int diff = item_counts[i] + COUNT - contained_items[i].STACK_SIZE;

                    //There is some left over, try and add it.
                    return AttemptAddItem(contained_items[i], diff);
                }
            }
        }

        //If we've reached this point, there are no copies of the item that aren't at max stack size.
        //Get all empty indexes.
        List<int> EMPTIES = GetAllEmptyIndexes();

        //Check if the count of the empty indexes list multiplied the stack size of the item is greater or equal to the count.
        if(EMPTIES.Count * ITEM.STACK_SIZE >= COUNT)
        {
            return true;
        }


        //If we reach this point, we can't fit all these items in. Return false.
        return false;
    }

    /// <summary>
    /// Extract a COUNT number of a given ITEM from inventory.
    /// One is by searching for a specific item, used when called by the system and not by player.
    /// In a sense, this one requests a given item and is then given it, instead of being chosen.
    /// </summary>
    /// <param name="ITEM"></param>
    /// <param name="COUNT"></param>
    /// <returns>Returns the item if successful, null if it fails.</returns>
    public GenericItem ExtractItem(GenericItem ITEM, int COUNT)
    {
        //Iterate over all contained items
        for (int i = 0; i < contained_items.Length; i++)
        {
            //Check if its the item we want.
            if (contained_items[i] != null && contained_items[i] == ITEM)
            {
                // If this one stack has enough to fulfill our extract condition, just get it from this stack.
                if(item_counts[i] >= COUNT)
                {
                    // Decrement the item.
                    DecrementItem(i, COUNT);
                    return ITEM;
                }
                else //Else we have to go find more from other stacks
                {
                    //List of indexes we are going to use 
                    List<int> indexes = new List<int>();

                    //Add the first index we found.
                    indexes.Add(i);

                    //Number of items we have found to extract. Will work if this value reaches the count we want.
                    int cur_count = item_counts[i];

                    // Set j to i + 1. No need to check the same slot.
                    int j = i + 1;

                    // Since we already found one instance of the item, we can just keep searching the rest of hte list.
                    while( j < item_counts.Length && cur_count < COUNT)
                    {
                        // If this item is the right one
                        if(contained_items[j] != null && contained_items[j] == ITEM)
                        {
                            //Add this index to our index list.
                            indexes.Add(j);

                            //Increment our current count
                            cur_count += item_counts[j];

                            //If our current count is already greater than count, then just break.
                            if (cur_count >= COUNT)
                            {
                                break;
                            }
                        }
                        // Increment j
                        j++;
                    }

                    //If our current count is less than the requested amount, 
                    // then there is no way we can satisfy this condition.
                    // There is no more need to continue through the loops since we have searched every
                    // slot by this point.
                    if(cur_count < COUNT)
                    {
                        return null;
                    } // If cur_count is equal to or greater than count, then we succeeded. 
                    // Decrement the rights items down, then return true;
                    else
                    {
                        //Loop through all the chosen indexes
                        foreach (int index in indexes) {

                            //If the item count at the given index is less than or equal to count, set it to zero and remove it.
                            if(item_counts[index] < COUNT)
                            {
                                //Decrement count so its updated for the next run.
                                COUNT -= item_counts[index];

                                //Decrement the item to zero.
                                DecrementItem(index, item_counts[index]);                    
                            }
                            //If the item count is greater than COUNT, then only deduct the remaining COUNT.
                            //The last item on the list should always end here.
                            else if(item_counts[index] >= COUNT)
                            {
                                DecrementItem(index, COUNT);
                                return ITEM;
                            }
                            throw new System.Exception("Error, somehow got through all indexes but didn't get enough.");
                        }
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Same as ExtractItem(GenericItem ITEM, int Count) but for a single item. 
    /// Useful for calling as an event function.
    /// Returns true if successful, else false.
    /// </summary>
    /// <param name="ITEM"></param>
    /// <returns></returns>
    public GenericItem ExtractSingleItem(GenericItem ITEM)
    {
        return ExtractItem(ITEM, 1);
    }

    /// <summary>
    /// Decrements count of an item at a given item_index by a given count. 
    /// This second way is called by the player and refers to the actual item_index that they click on
    /// in the UI. We want the player to be able to choose which item to take out, and not just request.
    /// Returns the item if successful, else null.
    /// </summary>
    /// <param name="item_index"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public GenericItem ExtractItem(int item_index, int count)
    {
        //Since this can be called by the player on empty inventory slots, check that there is an item there
        if(contained_items[item_index] != null && item_counts[item_index] >= count)
        {
            // Just decrement the item count. If it reaches zero remove item from the normal list.
            item_counts[item_index] -= count;
            if(item_counts[item_index] <= 0)
            {
                item_counts[item_index] = 0;
                contained_items[item_index] = null;
            }
            return contained_items[item_index];
        }
        return null;
    }

    /// <summary>
    /// Same as ExtractItem(int item_index, int count) but for a single item. 
    /// Useful for calling as an event function.
    /// Returns true if sucessful, else returns false.
    /// </summary>
    /// <param name="item_index"></param>
    /// <returns></returns>
    public GenericItem ExtractSingleItem(int item_index)
    {
        return ExtractItem(item_index, 1);
    }

    /// <summary>
    /// Adds COUNT number of ITEM to inventory.
    /// This first one is automatic additions by the system, not the player. 
    /// This function will recursively call itself if we try to add more than a stack of the given item can be.
    /// It will fill as many slots as necessary.
    /// Returns 0 if it placed all items, or a positive number for remainder items that couldn't be placed.
    /// </summary>
    /// <param name="ITEM"></param>
    /// <param name="COUNT"></param>
    /// <returns></returns>
    public int AddItem(GenericItem ITEM, int COUNT)
    {
        //Iterate over all contained items
        for (int i = 0; i < contained_items.Length; i++)
        {
            //Check if its the item we want, and it has less than the max stack size.
            //If it is the max stack size we can ignore it and find other items.
            //Whenever we reach this if statement, we will always return and terminate this function call.
            if (contained_items[i] != null && contained_items[i]== ITEM && item_counts[i] < ITEM.STACK_SIZE)
            {
                // Increment the given index i by COUNT.
                // If the COUNT pushes the stack over max stack size, call AddItem again with the difference when we add as much as we can.
                // If adding the full count will not go over stack size, jsut add it.
                if (item_counts[i] + COUNT <= contained_items[i].STACK_SIZE)
                {
                    item_counts[i] += COUNT;

                    //No remainder if we reach this.
                    return 0;
                }
                // If it will go over, add as much as we can then call this function again.
                else
                {
                    //Get the difference that is going to be a new stack.
                    int diff = item_counts[i] + COUNT - contained_items[i].STACK_SIZE;

                    //Set this item count to the max size.
                    item_counts[i] = contained_items[i].STACK_SIZE;

                    //There is some left over, try and add it.
                    return AddItem(contained_items[i], diff);
                }
            }
        }

        //If we've reached this point, there are no copies of the item that aren't at max stack size.
        //Thus we should try and find an empty index to add the item.
        int empty_index = GetNextEmptyIndex();

        // If it is -1, there is no empty slot.
        if(empty_index != -1)
        {
            //Set the empty index to this new item.
            contained_items[empty_index] = ITEM;

            //IF count for an item is less than a full stack of the given item, just place them all in this slot.
            if (COUNT <= ITEM.STACK_SIZE)
            {
                item_counts[empty_index] = COUNT;

                //Since we've placed the rest of everything, just return 0.
                return 0;
            }// Since the count is greater than the stack size, set it to the max and call AddItem again on the smaller stack.
            else
            {
                item_counts[empty_index] = ITEM.STACK_SIZE;
                AddItem(ITEM, COUNT - ITEM.STACK_SIZE);
            }
            
        }

        //If we ever reach this point, the inventory has no open stacks of the given item with space for more, or empty slots to make new slots.
        //Return count so we know how many copies couldn't make it in.
        return COUNT;
    }

    /// <summary>
    /// Same as AddItem(GenericItem ITEM, int COUNT) but for single items. 
    /// Useful for calling as an event function since it only takes one argument.
    /// Returns the remainder after attempting to add something, so if it is 1 here we failed, 0 if we added it successfully.
    /// </summary>
    /// <param name="ITEM"></param>
    /// <returns></returns>
    public virtual int AddSingleItem(GenericItem ITEM)
    {
        return AddItem(ITEM, 1);
    }

    /// <summary>
    /// This second AddItem method is for adding an item to a specific slot, 
    /// called by player when clicking on the UI. 
    /// Returns remainder of items that couldn't be placed.
    /// 0 is returned if everything is placed.
    /// </summary>
    /// <param name="item_index"></param>
    /// <param name="count"></param>
    /// <param name="ITEM"></param>
    /// <returns></returns>
    public int AddItem(int item_index, int count, GenericItem ITEM)
    {
        // If the item slot is empty, just set that slot to that item.
        if(contained_items[item_index] == null)
        {
            contained_items[item_index] = ITEM;
            //IF the count of the item is greater than the max stack size, we need to only put enough to go here.
            if(count <= ITEM.STACK_SIZE)
            {
                item_counts[item_index] = count;
                return 0;
            }
            else //Since the count is greater than stack size, just place a full stack and return remainder.
            {
                item_counts[item_index] = ITEM.STACK_SIZE;
                return count - ITEM.STACK_SIZE;
            }
        } //If the item slot is not empty, only do something if it's the same item.
        else if(contained_items[item_index] == ITEM)          
        {
            //If the number we're trying to put in plus the stack we're interacting with is less than or equal to the max stack size, just put it in.
            if(count + item_counts[item_index] <= ITEM.STACK_SIZE)
            {
                item_counts[item_index] += count;
                return 0;
            }
            else //Since the count plus the current stack is greater than max stack size, place as much as we can and return remainder.
            {
                item_counts[item_index] = ITEM.STACK_SIZE;
                return count - ITEM.STACK_SIZE;
            }
        }

        //If we reach this point, we couldn't add any in because we clicked on slot with an item not the same as ours.
        return count;
    }

    /// <summary>
    /// Same as AddItem(int item_index, int count, GenericItem ITEM) but for a single item. 
    /// Not as useful since it still has two arguments unlike the other single calls.
    /// Returns 1 if it fails since this returns remainder.
    /// </summary>
    /// <param name="item_index"></param>
    /// <param name="ITEM"></param>
    /// <returns></returns>
    public int AddSingleItem(int item_index, GenericItem ITEM)
    {
        return AddItem(item_index, 1, ITEM);
    }

    /// <summary>
    /// Checks to see if the inventory is full.
    /// </summary>
    /// <returns>True for a full inventory, false for having some spots.</returns>
    public virtual bool IsFull()
    {
        //Loop through the inventory
        for(int i = 0; i < inventory_size; i++)
        {
            //If there is no item in the slot, return false.
            if(contained_items[i] == null)
            {
                return false;
            }//If there is an item in the slot but its not at maximum stack size, return false.
            else if(item_counts[i] < contained_items[i].STACK_SIZE)
            {
                return false;
            }
        }
        //If we've reached this point, every slot is filled to maximum stack size.
        return true;
    }

    /// <summary>
    /// Counts how many copies of a given item we have in this inventory.
    /// </summary>
    /// <param name=""></param>
    /// <returns></returns>
    public int CountItem(GenericItem ITEM)
    {
        int count = 0;
        for(int i = 0; i < inventory_size; i++)
        {
            if(contained_items[i] == ITEM)
            {
                count += item_counts[i];
            }
        }
        return count;
    }

    /// <summary>
    /// Returns a list which holds every unique item and its totals in our inventory.
    /// </summary>
    /// <returns></returns>
    public List<ItemAmount> GetItemAmountList()
    {
        List<GenericItem> UNIQUE_ITEMS = new List<GenericItem>();
        List<int> ITEM_COUNTS = new List<int>();

        for (int i = 0; i < inventory_size; i++)
        {
            //If our unique items list doesn't contain the item, add it.
            if (!UNIQUE_ITEMS.Contains(contained_items[i]))
            {
                UNIQUE_ITEMS.Add(contained_items[i]);
                ITEM_COUNTS.Add(item_counts[i]);
            }
            else //If our unique items list contains the item update the count
            {
                int index = UNIQUE_ITEMS.IndexOf(contained_items[i]);
                ITEM_COUNTS[index] += item_counts[i];
            }
        }

        List<ItemAmount> return_list = new List<ItemAmount>();
        for(int i = 0; i < UNIQUE_ITEMS.Count; i++)
        {
            return_list.Add(new ItemAmount(UNIQUE_ITEMS[i], ITEM_COUNTS[i]));
        }
        return return_list;
    }

    /// <summary>
    /// Returns a list of unique items.
    /// </summary>
    /// <returns></returns>
    public List<GenericItem> GetUniqueItemsList()
    {
        List<GenericItem> UNIQUE_ITEMS = new List<GenericItem>();

        for (int i = 0; i < inventory_size; i++)
        {
            //If our unique items list doesn't contain the item, add it.
            if (!UNIQUE_ITEMS.Contains(contained_items[i]))
            {
                UNIQUE_ITEMS.Add(contained_items[i]);
            }
        }

        return UNIQUE_ITEMS;
    }

    /// <summary>
    /// Returns the entity we are attached to.
    /// </summary>
    /// <returns></returns>
    public GenericEntity GetEntity()
    {
        return GetComponent<EntityController>().INFO;
    }
}
