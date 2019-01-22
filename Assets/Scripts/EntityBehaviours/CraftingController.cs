using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Crafting controller for entities that need to craft automatically.
/// </summary>
public class CraftingController : MonoBehaviour
{
    /// <summary>
    /// Recipe that we are crafting. When null, we have no recipe.
    /// </summary>
    public GenericRecipe RECIPE;

    /// <summary>
    /// Given the items in our input inventory, find a possible recipe.
    /// </summary>
    /// <returns></returns>
    public GenericRecipe FindPossibleRecipe()
    {
        return RECIPE;
    }
}
