using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//Utilities for all scripts
namespace Utilities
{
    /// <summary>
    /// Constant variables that need to be used globally.
    /// </summary>
    public class CONSTANTS
    {
        /// <summary>
        /// Length of each grid length in game.
        /// </summary>
        public static float grid_length = 1f;
    }

    /// <summary>
    ///  Item flags that can be given to any item
    /// </summary>
    [System.Flags]
    public enum ITEM_TYPE
    {
        //The << bit shift operator easily creates powers of two.
        Placeable = 1 << 0,
        Droppable = 1 << 1,
        Solid = 1 << 2,
        Liquid = 1 << 3,
        Gas = 1 << 4,
        ByHand = 1 << 5,
        ByAutomation = 1 << 6,
        ByPumps = 1 << 7
    }

    /// <summary>
    /// Utility class for methods for manipulating tags
    /// </summary>
    public class FLAG_UTILITY
    {
        /// <summary>
        /// Adds new tags to items
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public ITEM_TYPE AddTag(ITEM_TYPE FLAGS, ITEM_TYPE NEW_FLAG)
        {
            return FLAGS |= NEW_FLAG;
        }

        /// <summary>
        /// Removes a given flag for an item
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public ITEM_TYPE RemoveTag(ITEM_TYPE FLAGS, ITEM_TYPE NEW_FLAG)
        {
            return FLAGS &= ~NEW_FLAG;
        }

        /// <summary>
        /// Toggles a given flag within the set of flags.
        /// If it was false, makes it true, if it was true, makes it false.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public ITEM_TYPE ToggleTag(ITEM_TYPE FLAGS, ITEM_TYPE NEW_FLAG)
        {
            return FLAGS ^= NEW_FLAG;
        }

        /// <summary>
        /// Checks if a given set of flags contains a flag
        /// Cannot check for flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public bool HasTag(ITEM_TYPE FLAGS, ITEM_TYPE NEW_FLAG)
        {
            return (FLAGS & NEW_FLAG) != 0;
        }

        /// <summary>
        /// Checks if a given set of flags contains flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <returns></returns>
        //public bool isNone(ITEM_TYPE FLAGS)
        //{
        //    return (FLAGS & ITEM_TYPE.None) == ITEM_TYPE.None;
        //}
    }

    /// <summary>
    ///  Inventory flags that can be given to any inventory
    /// </summary>
    [System.Flags]
    public enum INVENTORY_MODIFIERS
    {
        //The << bit shift operator easily creates powers of two.
        PlayerAccessible = 1 << 0,
        MachineAccessible = 1 << 1,
        EntityAccessible = 1 << 2,
        DropOnDeath = 1 << 3
    }

    /// <summary>
    /// Utility class for methods for manipulating tags
    /// </summary>
    public class INVENTORY_MODIFIER_UTILITY
    {
        /// <summary>
        /// Adds new tags to items
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public INVENTORY_MODIFIERS AddTag(INVENTORY_MODIFIERS FLAGS, INVENTORY_MODIFIERS NEW_FLAG)
        {
            return FLAGS |= NEW_FLAG;
        }

        /// <summary>
        /// Removes a given flag for an item
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public INVENTORY_MODIFIERS RemoveTag(INVENTORY_MODIFIERS FLAGS, INVENTORY_MODIFIERS NEW_FLAG)
        {
            return FLAGS &= ~NEW_FLAG;
        }

        /// <summary>
        /// Toggles a given flag within the set of flags.
        /// If it was false, makes it true, if it was true, makes it false.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public INVENTORY_MODIFIERS ToggleTag(INVENTORY_MODIFIERS FLAGS, INVENTORY_MODIFIERS NEW_FLAG)
        {
            return FLAGS ^= NEW_FLAG;
        }

        /// <summary>
        /// Checks if a given set of flags contains a flag
        /// Cannot check for flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public bool HasTag(INVENTORY_MODIFIERS FLAGS, INVENTORY_MODIFIERS NEW_FLAG)
        {
            return (FLAGS & NEW_FLAG) != 0;
        }

        /// <summary>
        /// Checks if a given set of flags contains flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <returns></returns>
        //public bool isNone(INVENTORY_MODIFIERS FLAGS)
        //{
        //    return (FLAGS & INVENTORY_MODIFIERS.None) == INVENTORY_MODIFIERS.None;
        //}
    }

    /// <summary>
    /// The rotations that exist in this game.
    /// </summary>
    public enum ROTATION_VAL
    {
        Zero,
        Ninety,
        OneEighty,
        TwoSeventy
    }

    /// <summary>
    /// The directions this machine/entity should interact in. Can be more than one.
    /// Can be used for both output or input for a machine or entity.
    /// </summary>
    [System.Flags]
    public enum INTERACTION_DIR
    {
        //The << bit shift operator easily creates powers of two.
        Front = 1 << 0,
        Back = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        Top = 1 << 4,
        Bottom = 1 << 5
    }

    /// <summary>
    /// Utility class for methods for manipulating tags
    /// </summary>
    public class INTERACTION_DIR_UTILITY
    {
        /// <summary>
        /// Adds new tags to items
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public INTERACTION_DIR AddTag(INTERACTION_DIR FLAGS, INTERACTION_DIR NEW_FLAG)
        {
            return FLAGS |= NEW_FLAG;
        }

        /// <summary>
        /// Removes a given flag for an item
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public INTERACTION_DIR RemoveTag(INTERACTION_DIR FLAGS, INTERACTION_DIR NEW_FLAG)
        {
            return FLAGS &= ~NEW_FLAG;
        }

        /// <summary>
        /// Toggles a given flag within the set of flags.
        /// If it was false, makes it true, if it was true, makes it false.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public INTERACTION_DIR ToggleTag(INTERACTION_DIR FLAGS, INTERACTION_DIR NEW_FLAG)
        {
            return FLAGS ^= NEW_FLAG;
        }

        /// <summary>
        /// Checks if a given set of flags contains a flag
        /// Cannot check for flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public bool HasTag(INTERACTION_DIR FLAGS, INTERACTION_DIR NEW_FLAG)
        {
            return (FLAGS & NEW_FLAG) != 0;
        }

        /// <summary>
        /// Checks if a given set of flags contains flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <returns></returns>
        //public bool isNone(INTERACTION_DIR FLAGS)
        //{
        //    return (FLAGS & INTERACTION_DIR.None) == INTERACTION_DIR.None;
        //}
    }

    /// <summary>
    /// The type of entity this is. Can hold multiple flags.
    /// Being certain entity types gives certain permissions.
    /// Example: You must be a machine to access inventories only accessible by machines.
    /// Example: You must be DropsAsItem and Breakable to be broken and turned into a item to be picked up.
    /// Note: If Breakable but not DropsAsItem, the entity is permanently destroyed on breaking. 
    /// Can be used to create Trees, which drop their inventory of wood when broken, but don't drop themselves as items.
    /// </summary>
    [System.Flags]
    public enum ENTITY_TYPE
    {
        //The << bit shift operator easily creates powers of two.
        Player = 1 << 0,
        Machine = 1 << 1,
        Storage = 1 << 2,
        Breakable = 1 << 3,
        DropsAsItem = 1 << 4,
        Repairable = 1 << 5,
        Smelter = 1 << 6,
        AutoCrafter = 1 << 7,
        TransportEntity = 1 << 8,
        InventoryEntity = 1 << 9
    }

    /// <summary>
    /// Utility class for methods for manipulating tags
    /// </summary>
    public class ENTITY_FLAG_UTILITY
    {
        /// <summary>
        /// Adds new tags to items
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public ENTITY_TYPE AddTag(ENTITY_TYPE FLAGS, ENTITY_TYPE NEW_FLAG)
        {
            return FLAGS |= NEW_FLAG;
        }

        /// <summary>
        /// Removes a given flag for an item
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public ENTITY_TYPE RemoveTag(ENTITY_TYPE FLAGS, ENTITY_TYPE NEW_FLAG)
        {
            return FLAGS &= ~NEW_FLAG;
        }

        /// <summary>
        /// Toggles a given flag within the set of flags.
        /// If it was false, makes it true, if it was true, makes it false.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public ENTITY_TYPE ToggleTag(ENTITY_TYPE FLAGS, ENTITY_TYPE NEW_FLAG)
        {
            return FLAGS ^= NEW_FLAG;
        }

        /// <summary>
        /// Checks if a given set of flags contains a flag
        /// Cannot check for flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public bool HasTag(ENTITY_TYPE FLAGS, ENTITY_TYPE NEW_FLAG)
        {
            return (FLAGS & NEW_FLAG) != 0;
        }

        /// <summary>
        /// Checks if a given set of flags contains flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <returns></returns>
        //public bool isNone(ENTITY_TYPE FLAGS)
        //{
        //    return (FLAGS & ENTITY_TYPE.None) == ENTITY_TYPE.None;
        //}
    }

    /// <summary>
    /// Flags that determine how the product is crafted in a recipe.
    /// Example: Smelting means all smelting machines can produce this.
    /// </summary>
    public enum CRAFT_TYPE
    {
        HandCraftable = 1 << 0,
        MachineCraftable = 1 << 1,
        Smelting = 1 << 2
    }

    /// <summary>
    /// Utility class for methods for manipulating tags
    /// </summary>
    public class CRAFT_FLAG_UTILITY
    {
        /// <summary>
        /// Adds new tags to items
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public CRAFT_TYPE AddTag(CRAFT_TYPE FLAGS, CRAFT_TYPE NEW_FLAG)
        {
            return FLAGS |= NEW_FLAG;
        }

        /// <summary>
        /// Removes a given flag for an item
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public CRAFT_TYPE RemoveTag(CRAFT_TYPE FLAGS, CRAFT_TYPE NEW_FLAG)
        {
            return FLAGS &= ~NEW_FLAG;
        }

        /// <summary>
        /// Toggles a given flag within the set of flags.
        /// If it was false, makes it true, if it was true, makes it false.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public CRAFT_TYPE ToggleTag(CRAFT_TYPE FLAGS, CRAFT_TYPE NEW_FLAG)
        {
            return FLAGS ^= NEW_FLAG;
        }

        /// <summary>
        /// Checks if a given set of flags contains a flag
        /// Cannot check for flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <param name="NEW_FLAG"></param>
        /// <returns></returns>
        public bool HasTag(CRAFT_TYPE FLAGS, CRAFT_TYPE NEW_FLAG)
        {
            return (FLAGS & NEW_FLAG) != 0;
        }

        /// <summary>
        /// Checks if a given set of flags contains flag none.
        /// </summary>
        /// <param name="FLAGS"></param>
        /// <returns></returns>
        //public bool isNone(CRAFT_TYPE FLAGS)
        //{
        //    return (FLAGS & CRAFT_TYPE.None) == CRAFT_TYPE.None;
        //}
    }

    /// <summary>
    /// Insert direction, used to determine where objects should go when inputted onto belts.
    /// </summary>
    public enum InsertDir { Front, Back, Left, Right, Top, Bottom};

    /// <summary>
    /// Insert side. Inputs either on the left or right.
    /// </summary>
    public enum InsertSide { Left, Right };

    /// <summary>
    /// Belt side enums.
    /// </summary>
    public enum BeltSide { Left, Right };

    /// <summary>
    /// Belt positions on a side.
    /// </summary>
    public enum BeltPos { Back, Middle, Front };
}
