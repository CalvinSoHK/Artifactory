using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[System.Serializable]
public struct ItemAmount
{
    public GenericItem Item;
    [Range(1, 999)]
    public int Amount;

    /// <summary>
    /// Constructor for item amount.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public ItemAmount(GenericItem item, int count)
    {
        this.Item = item;
        this.Amount = count;
    }
}

/// <summary>
/// Scriptable objects don't need to be attached to a gameobject, ideal for being easily editted data packets.
/// </summary>
[CreateAssetMenu(fileName = "Recipe", menuName = "Item/Recipe", order = 2)]
public class GenericRecipe : ScriptableObject
{
    /// <summary>
    /// The product created from this recipe.
    /// </summary>
    public GenericItem PRODUCT;

    /// <summary>
    /// The crafting flags that control how and where this recipe can be used.
    /// </summary>
    [EnumFlag]
    public CRAFT_TYPE FLAGS;

    /// <summary>
    /// The ingredients needed for this recipe.
    /// </summary>
    public List<ItemAmount> INGREDIENT_LIST;

    /// <summary>
    /// The base time it takes to craft outside of crafting speed.
    /// </summary>
    public float CRAFT_TIME;

    /// <summary>
    /// Tells us if the given inventory can craft this recipe.
    /// </summary>
    /// <param name="inventory"></param>
    /// <returns></returns>
    public bool CanCraft(IInventory inventory)
    {
        foreach(ItemAmount ITEM_AMOUNT in INGREDIENT_LIST)
        {
            //If the count of the required ingredient is less than the amount
            if(inventory.CountItem(ITEM_AMOUNT.Item) < ITEM_AMOUNT.Amount)
            {
                return false;
            }
        }
        //If we exit for the for loop every item has a correct number of copies.
        return true;
    }

    /// <summary>
    /// Craft this recipe from the given inventory.
    /// </summary>
    /// <param name="inventory"></param>
    public void Craft(IInventory inventory)
    {
        foreach (ItemAmount ITEM_AMOUNT in INGREDIENT_LIST)
        {
            inventory.ExtractItem(ITEM_AMOUNT.Item, ITEM_AMOUNT.Amount);
        }
    }

    /// <summary>
    /// Tells us if the recipe requires a given item to be crafted.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool RequireItem(GenericItem item)
    {
        foreach(ItemAmount ITEM_AMOUNT in INGREDIENT_LIST)
        {
            if(ITEM_AMOUNT.Item == item)
            {
                return true;
            }
        }
        return false;
    }
}
