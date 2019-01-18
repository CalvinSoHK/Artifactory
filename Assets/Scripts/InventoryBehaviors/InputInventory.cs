using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A special version of inventory that is just for input. 
/// When processing recipes or crafting it only looks at input inventory.
/// </summary>
public class InputInventory : Inventory
{
    /// <summary>
    /// When true, doesn't use items natural stack size, instead this machine has it's own maximum input size.
    /// </summary>
    public bool STACK_OVERRIDE = true;

    /// <summary>
    /// Used when STACK_OVERRIDE is true.
    /// Overrides how large of a stack each slot is for this inventory regardless for what item it is.
    /// </summary>
    public int STACK_SIZE = 1;

    /// <summary>
    /// Alternate version just for input inventory. 
    /// Uses stack override instead.
    /// </summary>
    /// <param name="ITEM"></param>
    /// <param name="COUNT"></param>
    /// <returns></returns>
    public int AddItem_StackOverride(GenericItem ITEM, int COUNT)
    {
        //Iterate over all contained items
        for (int i = 0; i < contained_items.Length; i++)
        {
            //Check if its the item we want, and it has less than the max stack size.
            //If it is the max stack size we can ignore it and find other items.
            //Whenever we reach this if statement, we will always return and terminate this function call.
            if (contained_items[i] != null && contained_items[i] == ITEM && item_counts[i] < STACK_SIZE)
            {
                // Increment the given index i by COUNT.
                // If the COUNT pushes the stack over max stack size, call AddItem again with the difference when we add as much as we can.
                // If adding the full count will not go over stack size, jsut add it.
                if (item_counts[i] + COUNT <= STACK_SIZE)
                {
                    item_counts[i] += COUNT;

                    //No remainder if we reach this.
                    return 0;
                }
                // If it will go over, add as much as we can then call this function again.
                else
                {
                    //Get the difference that is going to be a new stack.
                    int diff = item_counts[i] + COUNT - STACK_SIZE;

                    //Set this item count to the max size.
                    item_counts[i] = STACK_SIZE;

                    //There is some left over, try and add it.
                    return AddItem(contained_items[i], diff);
                }
            }
        }

        //If we've reached this point, there are no copies of the item that aren't at max stack size.
        //Thus we should try and find an empty index to add the item.
        int empty_index = GetNextEmptyIndex();

        // If it is -1, there is no empty slot.
        if (empty_index != -1)
        {
            //Set the empty index to this new item.
            contained_items[empty_index] = ITEM;

            //IF count for an item is less than a full stack of the given item, just place them all in this slot.
            if (COUNT <= STACK_SIZE)
            {
                item_counts[empty_index] = COUNT;

                //Since we've placed the rest of everything, just return 0.
                return 0;
            }// Since the count is greater than the stack size, set it to the max and call AddItem again on the smaller stack.
            else
            {
                item_counts[empty_index] = STACK_SIZE;
                AddItem(ITEM, COUNT - STACK_SIZE);
            }

        }

        //If we ever reach this point, the inventory has no open stacks of the given item with space for more, or empty slots to make new slots.
        //Return count so we know how many copies couldn't make it in.
        return COUNT;
    }

    /// <summary>
    /// Override the AddSingleItem method so it uses the stack override if viable.
    /// </summary>
    /// <param name="ITEM"></param>
    /// <returns></returns>
    public override int AddSingleItem(GenericItem ITEM)
    {
        if (STACK_OVERRIDE)
        {
            return AddItem_StackOverride(ITEM, 1);
        }
        else
        {
            return base.AddSingleItem(ITEM);
        }     
    }

    /// <summary>
    /// Override isFull so it uses stack override sizes instead of item stack sizes.
    /// </summary>
    /// <returns></returns>
    public override bool IsFull()
    {
        //If stack override is enabled
        if (STACK_OVERRIDE)
        {
            //Loop through the inventory
            for (int i = 0; i < inventory_size; i++)
            {
                //If there is no item in the slot, return false.
                if (contained_items[i] == null)
                {
                    return false;
                }//If there is an item in the slot but its not at maximum stack size, return false.
                else if (item_counts[i] < STACK_SIZE)
                {
                    return false;
                }
            }
            //If we've reached this point, every slot is filled to maximum stack size.
            return true;
        }
        else //If it isn't, just use the base version.
        {
            return base.IsFull();
        }
      
    }
}
