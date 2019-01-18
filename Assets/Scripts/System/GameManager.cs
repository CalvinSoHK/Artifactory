using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

/// <summary>
/// Game manager. Contains data about the whole world.
/// </summary>
public class GameManager : MonoBehaviour {

    /// <summary>
    /// Singleton code below.
    /// </summary>
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    /// <summary>
    /// List of all entities on the grid that may need to interact with each other.
    /// </summary>
    public List<GridController> ENTITIES;

    /// <summary>
    /// List of all recipes in the game in it's entirety.
    /// </summary>
    public List<GenericRecipe> RECIPE_LIST;

    /// <summary>
    /// Inventory utility for doing flag checks
    /// </summary>
    public INVENTORY_MODIFIER_UTILITY INV_UTILITY = new INVENTORY_MODIFIER_UTILITY();

    /// <summary>
    /// Interaction direction flag utility.
    /// </summary>
    public INTERACTION_DIR_UTILITY ID_UTILITY = new INTERACTION_DIR_UTILITY();

    /// <summary>
    /// Entity utility for doing flag checks.
    /// </summary>
    public ENTITY_FLAG_UTILITY ENTITY_UTILITY = new ENTITY_FLAG_UTILITY();

    /// <summary>
    /// Craft flag utility.
    /// </summary>
    public CRAFT_FLAG_UTILITY CRAFT_UTILITY = new CRAFT_FLAG_UTILITY();

    /// <summary>
    /// OnAwake, load all recipes.
    /// </summary>
    void Start()
    {
        LoadAllRecipes();
    }

    /// <summary>
    /// Load all recipes.
    /// </summary>
    public void LoadAllRecipes()
    {
        GenericRecipe[] array = Resources.LoadAll<GenericRecipe>("Data/Recipe");
        foreach(GenericRecipe recipe in array)
        {
            RECIPE_LIST.Add(recipe);
        }
    }

    /// <summary>
    /// Finds another entity on the grid with a given difference.
    /// Used to find obejcts nearby that need to be interacted with.
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="difference"></param>
    /// <returns>Gameobject if any object matches, else null. </returns>
    public GameObject FindEntityWithDifference(GridController entity, int[] difference)
    {
        //Search list for an entity with a given distance.
        foreach(GridController obj in ENTITIES)
        {
            //First check if the grid position is correct.
            if(obj.GRID_POS[0] == difference[0] && obj.GRID_POS[1] == difference[1] && obj.GRID_POS[2] == difference[2])
            {
                return obj.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Checks an inventories permissions against an entities' flags to see if it has access to the inventory.
    /// </summary>
    /// <param name="entity_flags"></param>
    /// <param name="inventory_flags"></param>
    /// <returns></returns>
    public bool CheckInventoryPermissions(ENTITY_TYPE entity_flags, INVENTORY_MODIFIERS inventory_flags)
    {
        //Check if the inventory is machine accessible and if the entity is a machine.
        if(INV_UTILITY.HasTag(inventory_flags, INVENTORY_MODIFIERS.MachineAccessible) && 
            ENTITY_UTILITY.HasTag(entity_flags, ENTITY_TYPE.Machine))
        {
            return true;
        }// Check if the inventory is player accessible and if the entity is a player.
        else if(INV_UTILITY.HasTag(inventory_flags, INVENTORY_MODIFIERS.PlayerAccessible) &&
            ENTITY_UTILITY.HasTag(entity_flags, ENTITY_TYPE.Player))
        {
            return true;
        }
        //If it reaches this point then we don't have the right permissions.
        return false;
    }

    /// <summary>
    /// Adds a given gridcontroller to the list if it isn't already in it.
    /// </summary>
    /// <param name="GC"></param>
    /// <returns> True if it successfully adds it, else returns false.</returns>
    public bool AddGridController(GridController GC)
    {
        if (!ENTITIES.Contains(GC))
        {
            ENTITIES.Add(GC);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Compares an entites crafting flags and compares it with the craft flags on a recipe
    /// to see if the entity is capable of crafting that recipe.
    /// </summary>
    /// <param name="entity_flags"></param>
    /// <param name="craft_flags"></param>
    /// <returns></returns>
    public bool isCompatibleCraftingType(ENTITY_TYPE entity_flags, CRAFT_TYPE craft_flags)
    {

        if(ENTITY_UTILITY.HasTag(entity_flags, ENTITY_TYPE.Player)  && 
            CRAFT_UTILITY.HasTag(craft_flags, CRAFT_TYPE.HandCraftable))
        {
            return true;
        }
        else if(ENTITY_UTILITY.HasTag(entity_flags, ENTITY_TYPE.AutoCrafter) &&
            CRAFT_UTILITY.HasTag(craft_flags, CRAFT_TYPE.MachineCraftable))
        {
            return true;
        }
        else if(ENTITY_UTILITY.HasTag(entity_flags, ENTITY_TYPE.Smelter) &&
            CRAFT_UTILITY.HasTag(craft_flags, CRAFT_TYPE.Smelting))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the given items can be placed in a given entity based on its flags.
    /// If the entity is storage, it can take any items so can just return true immediately.
    /// Else, we have to check if the possible recipe is valid for the given machine.
    /// </summary>
    /// <returns></returns>
    public bool isItemPlacementValid(List<GenericItem> item_list, ENTITY_TYPE entity_flags)
    {
        //Return true immediately if it is just storage.
        if(ENTITY_UTILITY.HasTag(entity_flags, ENTITY_TYPE.Storage))
        {
            return true;
        }

        //Get all the valid recipes since this entity should be able to craft and isn't just storage.
        List<GenericRecipe> VALID_RECIPES = GetValidRecipes(item_list, entity_flags);

        //If the count is greater than 0, return true;
        if (VALID_RECIPES.Count > 0)
        {
            return true;
        }
        else { return false; }
    }

    /// <summary>
    /// Given a list of items, returns all the valid recipes regardless of item count.
    /// The input list does not care for how much of each item there is.
    /// This is for helping with deciding if an item should be moved by automation.
    /// Also checks crafting types the entity is capable of doing.
    /// </summary>
    /// <param name="item_list"></param>
    /// <returns></returns>
    public List<GenericRecipe> GetValidRecipes(List<GenericItem> item_list, ENTITY_TYPE entity_info)
    {
        List<GenericRecipe> VALID_RECIPES = new List<GenericRecipe>();

        //Copy the recipe list.
        foreach (GenericRecipe RECIPE in RECIPE_LIST)
        {
            VALID_RECIPES.Add(RECIPE);
        }

        //For all valid recipes.
        for (int i = 0; i < VALID_RECIPES.Count; i++)
        {
            //For every item in our item list.
            foreach (GenericItem item in item_list)
            {
                //Debug.Log("Index: " + i + " Length: " + VALID_RECIPES.Count);
                //If the recipe doesn't require a given item, or the recipe type is incompatable with entity capabilities
                if (!VALID_RECIPES[i].RequireItem(item) || !isCompatibleCraftingType(entity_info, VALID_RECIPES[i].FLAGS))
                {
                    //Remove the recipe from the list.
                    VALID_RECIPES.Remove(VALID_RECIPES[i]);

                    //Update the index so it isn't broken.
                    if(i != VALID_RECIPES.Count)
                    {
                        i--;
                    }
                    else if(i == VALID_RECIPES.Count)
                    {
                        break;
                    }
                }
            }
        }
        return VALID_RECIPES;
    }
}
