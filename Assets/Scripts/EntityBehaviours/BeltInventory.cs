using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

/// <summary>
/// Inventory that uses the Inventory interface to work with other things.
/// </summary>
[RequireComponent(typeof(EntityController))]
public class BeltInventory : MonoBehaviour, IBelt
{
    /// <summary>
    /// Left and right belt that needs to be filled.
    /// </summary>
    List<ItemController> LEFT_BELT = new List<ItemController>(),
        RIGHT_BELT = new List<ItemController>();

    /// <summary>
    /// List of transforms where we want things to show in.
    /// </summary>
    public List<Transform> ITEM_POSITIONS = new List<Transform>();

    /// <summary>
    /// Grid controller. Used for calculations.
    /// </summary>
    GridController GC;

    void Start()
    {
        GC = GetComponent<GridController>();
    }

    /// <summary>
    /// Returns the GenericEntity for this belt inventory.
    /// </summary>
    /// <returns></returns>
    public GenericEntity GetEntity()
    {
        return GetComponent<EntityController>().INFO;
    }

    /// <summary>
    /// Returns the list that pertains to the inputted side of the belt.
    /// </summary>
    /// <param name="side"></param>
    /// <returns></returns>
    public List<ItemController> GetSideList(BeltSide side)
    {
        switch (side)
        {
            case BeltSide.Left:
                return LEFT_BELT;
            case BeltSide.Right:
                return RIGHT_BELT;
            default:
                throw new System.Exception("Error, invalid side.");
        }
    }

    /// <summary>
    /// Returns the entitycontroller in the given position on the given side.
    /// </summary>
    /// <param name="side"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public ItemController GetItem(BeltSide side, BeltPos pos)
    {
        //Get the right side of the belt.
        List<ItemController> SIDE_LIST = GetSideList(side);

        //Return the entity in a given position from the side list.
        switch (pos)
        {
            case BeltPos.Back:
                return SIDE_LIST[(int)BeltPos.Back];
            case BeltPos.Middle:
                return SIDE_LIST[(int)BeltPos.Middle];
            case BeltPos.Front:
                return SIDE_LIST[(int)BeltPos.Front];
            default:
                throw new System.Exception("Error, invalid belt position.");
        }
    }

    /// <summary>
    /// Returns the side of the belt given a direction and side.
    /// </summary>
    /// <param name="insertDir"></param>
    /// <param name="insertSide"></param>
    /// <returns></returns>
    public BeltSide GetBeltSide(InsertDir insertDir, InsertSide insertSide)
    {
        switch (insertDir)
        {
            case InsertDir.Back:
                if(insertSide == InsertSide.Left)
                {
                    return BeltSide.Left;
                }
                else
                {
                    return BeltSide.Right;
                }
            case InsertDir.Front:
                if(insertSide == InsertSide.Left)
                {
                    return BeltSide.Right;
                }
                else
                {
                    return BeltSide.Left;
                }
            case InsertDir.Left:
                return BeltSide.Left;
            case InsertDir.Right:
                return BeltSide.Right;
            case InsertDir.Top:
                if(insertSide == InsertSide.Left)
                {
                    return BeltSide.Left;
                }
                else
                {
                    return BeltSide.Right;
                }
            case InsertDir.Bottom:
                if(insertSide == InsertSide.Left)
                {
                    return BeltSide.Left;
                }
                else
                {
                    return BeltSide.Right;
                }
            default:
                throw new System.Exception("Error, invalid belt side.");
        }
    }

    /// <summary>
    /// Returns the position of the item given a direction and side.
    /// </summary>
    /// <param name="insertDir"></param>
    /// <param name="insertSide"></param>
    /// <returns></returns>
    public BeltPos GetBeltPos(InsertDir insertDir, InsertSide insertSide)
    {
        switch (insertDir)
        {
            case InsertDir.Back:
                return BeltPos.Back;
            case InsertDir.Front:
                return BeltPos.Front;
            case InsertDir.Top:
                return BeltPos.Middle;
            case InsertDir.Bottom:
                return BeltPos.Middle;
            case InsertDir.Left:
                if(insertSide == InsertSide.Left)
                {
                    return BeltPos.Front;
                }
                else
                {
                    return BeltPos.Back;
                }
            case InsertDir.Right:
                if(insertSide == InsertSide.Left)
                {
                    return BeltPos.Back;
                }
                else
                {
                    return BeltPos.Front;
                }
            default:
                throw new System.Exception("Error, invalid belt position.");
        }
    }

    /// <summary>
    /// Returns the index in the item positions list for the given slot.
    /// </summary>
    /// <param name="side"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public int GetIndex(BeltSide side, BeltPos pos)
    {
        if (side == BeltSide.Left)
        {
            return (int)pos;
        }
        else
        {
            return (int)pos + 3;
        }
    }

    /// <summary>
    /// Returns true or false depending on if the slot that is meant to be used on insert based on direction is empty.
    /// Inserters output either to their left or right half of the square. As such:
    /// Left insert direction inserters outputting to the left output to the front of the belt.
    /// Left insert direction inserters outputting to the right output to the back of the belt.
    /// </summary>
    /// <param name="insertDir"></param>
    /// <returns></returns>
    public bool isValidInsert(InsertDir insertDir, InsertSide insertSide)
    {
        BeltSide side = GetBeltSide(insertDir, insertSide);
        BeltPos pos = GetBeltPos(insertDir, insertSide);
        ItemController EC = GetItem(side, pos);
        if(EC == null)
        {
            return true;
        }
        else
        {
            return false;
        }       
    }

    /// <summary>
    /// Checks if the given item is in the given side and position.
    /// </summary>
    /// <param name="side"></param>
    /// <param name="pos"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool isItemAccessible(BeltSide side, BeltPos pos, GenericItem item)
    {
        ItemController SLOT_ITEM = GetItem(side, pos);
        if(SLOT_ITEM == null)
        {
            return false;
        }
        else if(SLOT_ITEM.INFO == item)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Returns the inserter direction to be used to calculate where the inserter will place objects.
    /// </summary>
    /// <param name="inserter"></param>
    /// <returns></returns>
    public InsertDir GetInsertDir(GridController inserter)
    {
        //Calculate the direction the inserter is from this object.
        //This vector is the normalized vector (direction) towards this belt from the inserter's position.
        //NOTE: This uses grid position because some transform center points won't be perfectly in the center,
        // and may result in incorrect directions.
        int x = inserter.GRID_POS[0] - GC.GRID_POS[0];
        int y = inserter.GRID_POS[1] - GC.GRID_POS[1];
        int z = inserter.GRID_POS[2] - GC.GRID_POS[2];
        Vector3 dirFrom = new Vector3(x, y, z).normalized;

        //Get the difference between their transform forwards.
        //Vector3 diff = (transform.forward - inserter.transform.forward).normalized;

        //Get's the angle between our forward and the inserter's forward, using our up vector for reference.
        //NOTE: SignedAngle returns between -180 to 180.
        float angle = Vector3.SignedAngle(transform.forward, inserter.transform.forward, transform.up);

        //The dirFrom is the direction FROM inserter TO belt.
        //As such, if the direction is equivalent to UP, then we are inserting from below.
        if (dirFrom == Vector3.up)
        {
            return InsertDir.Bottom;
        }
        else if (dirFrom == Vector3.down)
        {
            return InsertDir.Top;
        }
        //If the angle diff between the two transform forwards is zero, they are facing the same direction.
        else if (angle == 0)
        {
            return InsertDir.Back;
        }//If the angle difference between two transform forwards is 180 or -180, they are facing opposite directions.
        else if (angle == 180 || angle == -180)
        {
            return InsertDir.Front;
        }//If the angle difference is positive 90, then we are being inserted from our right.
        else if (angle == 90)
        {
            return InsertDir.Right;
        }//If the angle difference is negative 90, then we are being inserted from our left.
        else if(angle == -90)
        {
            return InsertDir.Left;
        }
        else
        {
            throw new System.Exception("Error, somehow didn't get a valid insert direction.");
        }
    }

    /// <summary>
    /// Returns if the item is contained within this belt.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool ContainsItem(GenericItem item)
    {
        foreach (ItemController EC in LEFT_BELT)
        {
            if (EC.INFO == item)
            {
                return true;
            }
        }

        foreach(ItemController EC in RIGHT_BELT)
        {
            if(EC.INFO == item)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns true if we acn add an item from this inserter.
    /// </summary>
    /// <param name="inserter"></param>
    /// <returns></returns>
    public bool AttemptAddItem(Extractor inserter)
    {
        //Side the inserter is using
        InsertSide insert_side = inserter.OUTPUT_SIDE;
        InsertDir insert_dir = GetInsertDir(inserter.GetComponent<GridController>());
        return isValidInsert(insert_dir, insert_side);
    }

    /// <summary>
    /// Adds an item given an extractor/inserter. Calculates and places in the right slot.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="inserter"></param>
    /// <returns></returns>
    public GenericItem AddItem(GenericItem item, Extractor inserter)
    {
        //Side the inserter is using
        InsertSide insert_side = inserter.OUTPUT_SIDE;
        InsertDir insert_dir = GetInsertDir(inserter.GetComponent<GridController>());
        BeltSide side = GetBeltSide(insert_dir, insert_side);
        BeltPos pos = GetBeltPos(insert_dir, insert_side);

        //If it isn't valid, return immediately.
        if(!isValidInsert(insert_dir, insert_side))
        {
            return null;
        }
        else
        {
            //Get the relevant list.
            List<ItemController> LIST = GetSideList(side);

            //Get the relevant index.
            int index = GetIndex(side, pos);

            //Update the right index.
            LIST[(int)pos] = item.SpawnObject(ITEM_POSITIONS[index].position, ITEM_POSITIONS[index].rotation, ITEM_POSITIONS[index]);
            return LIST[(int)pos].INFO;
        }     
    }
  
    /// <summary>
    /// Attempts to insert an item given an extractor/inserter.
    /// Returns the item if successful, else null.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="inserter"></param>
    /// <returns></returns>
    public bool AttemptExtractItem(GenericItem item, Extractor inserter)
    {
        //Side the inserter is using
        InsertSide insert_side = inserter.OUTPUT_SIDE;
        InsertDir insert_dir = GetInsertDir(inserter.GetComponent<GridController>());
        BeltSide side = GetBeltSide(insert_dir, insert_side);
        BeltPos pos = GetBeltPos(insert_dir, insert_side);

        return isItemAccessible(side, pos, item);
    }

    /// <summary>
    /// Extracts item with given inserter.
    /// Only decrements this inventory, does not add to the inserter's inventory.
    /// Returns the item if successful, else null;
    /// </summary>
    /// <param name="item"></param>
    /// <param name="inserter"></param>
    /// <returns></returns>
    public GenericItem ExtractItem(GenericItem item, Extractor inserter)
    {
        //Side the inserter is using
        InsertSide insert_side = inserter.OUTPUT_SIDE;
        InsertDir insert_dir = GetInsertDir(inserter.GetComponent<GridController>());
        BeltSide side = GetBeltSide(insert_dir, insert_side);
        BeltPos pos = GetBeltPos(insert_dir, insert_side);

        //Get the relevant list.
        List<ItemController> LIST = GetSideList(side);

        //Get the relevant index.
        int index = GetIndex(side, pos);

        //Save the item to be returned
        GenericItem item_ret = LIST[(int)pos].INFO;

        //Update side list.
        LIST[(int)pos].DespawnObject();
        LIST[(int)pos] = null;
        return item_ret;
    }
}
