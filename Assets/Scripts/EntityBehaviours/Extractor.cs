using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

/// <summary>
/// Behaviour that extracts objects from nearby inventories and puts it in our inventory.
/// </summary>
[RequireComponent(typeof(EntityController))]
[RequireComponent(typeof(IInventory))]
public class Extractor : MonoBehaviour
{
    /// <summary>
    /// How fast this extractor is at pulling and placing stuff from inventories.
    /// Speed is all relative to 1 second. 
    /// A speed of 1 means one second.
    /// 0.5 is two seconds.
    /// 2 is half a second.
    /// Higher values are faster.
    /// NOTE: 0.5 of the value is extract, 0.5 is putting into the output.
    /// </summary>
    public float EXTRACT_SPEED = 1;

    /// <summary>
    /// The timer for extracting. When it reaches zero we perform extraction, then reset.
    /// NOTE: Timer may not be used if we have an animation that controls interactions.
    /// </summary>
    private float EXTRACT_TIMER;

    /// <summary>
    /// The directions this machine will look for inputs.
    /// </summary>
    [EnumFlag]
    public INTERACTION_DIR INPUT_DIR;

    /// <summary>
    /// The side it will try to output from.
    /// </summary>
    public InsertSide OUTPUT_SIDE;

    /// <summary>
    /// The distance this extractor can interact with. 
    /// Range is defined by grid coordinates.
    /// </summary>
    public int INPUT_INTERACT_DISTANCE = 1;

    /// <summary>
    /// The direction this machine will look for outputs.
    /// </summary>
    [EnumFlag]
    public INTERACTION_DIR OUTPUT_DIR;

    /// <summary>
    /// The distance this extractor can output to.
    /// Range is defined by grid coordinates.
    /// </summary>
    public int OUTPUT_INTERACT_DISTANCE = 1;

    /// <summary>
    /// The inventory or "hand" this extractor can hold.
    /// Larger sizes means it can hold more at a time.
    /// </summary>
    private IInventory INV;

    /// <summary>
    /// The inventory we are targeting to extract from.
    /// </summary>
    [SerializeField]
    private EntityController IN_TARGET;

    /// <summary>
    /// The inventory we are targeting to extract to.
    /// </summary>
    [SerializeField]
    private EntityController OUT_TARGET;

    /// <summary>
    /// The entity info for this given entity.
    /// </summary>
    private GenericEntity INFO;

    private INTERACTION_DIR_UTILITY ID_UTIL;
    private ENTITY_FLAG_UTILITY ENT_UTIL;

    /// <summary>
    /// States to control everything.
    /// </summary>
    private enum EXTRACTOR_STATE { Neutral, Extracting, Depositing };
    [SerializeField]
    private EXTRACTOR_STATE STATE = EXTRACTOR_STATE.Neutral;

    private void Start()
    {
        // Set INV to our inventory.
        ID_UTIL = GameManager.Instance.ID_UTILITY;
        ENT_UTIL = GameManager.Instance.ENTITY_UTILITY;
        INFO = GetComponent<EntityController>().INFO;
        INV = GetComponent<Inventory>();
    }

    private void Update()
    {
        if(STATE == EXTRACTOR_STATE.Neutral)
        {
            EXTRACT_TIMER = (1f / EXTRACT_SPEED) / 2f;
            if (INV.IsFull())
            {
                STATE = EXTRACTOR_STATE.Depositing;
            }
            else
            {
                STATE = EXTRACTOR_STATE.Extracting;
            }
        }
        else if(STATE == EXTRACTOR_STATE.Extracting)
        {
            //While TARGET_INV is null, search for a new target.
            if (IN_TARGET == null)
            {
                SetInTarget();
            }
            // IF we have a target inventory, and we're not full, extract more items.
            else if (!INV.IsFull())
            {
                ExtractItem();
            }
            else if (INV.IsFull())
            {
                STATE = EXTRACTOR_STATE.Neutral;
            }
        }
        else if(STATE == EXTRACTOR_STATE.Depositing)
        {
            if (OUT_TARGET == null)
            {
                SetOutTarget();
            }
            //If we have a full inventory, attempt to output what we've extracted.
            //This is separate from the above so if the extractor is holding items while we break
            //it's target inventory, it will still complete moving the object to its destination.
            else if (INV.IsFull())
            {
                OutputItems();
            }
            else if (!INV.IsFull())
            {
                STATE = EXTRACTOR_STATE.Neutral;
            }
        }  
    }

    /// <summary>
    /// Outputs items to our target output inventory.
    /// Method is only called when inventory is full.
    /// </summary>
    public void OutputItems()
    {
        // Always decrement timer if we have a target.
        EXTRACT_TIMER -= Time.deltaTime;

        if(EXTRACT_TIMER <= 0)
        { 
            if(ENT_UTIL.HasTag(OUT_TARGET.INFO.FLAGS, ENTITY_TYPE.InventoryEntity))
            {
                OutputToInventory(OUT_TARGET.GetComponent<IInventory>());
            }
            else if(ENT_UTIL.HasTag(OUT_TARGET.INFO.FLAGS, ENTITY_TYPE.TransportEntity))
            {
                OutputToBelt(OUT_TARGET.GetComponent<IBelt>());
            }
        }
    }

    /// <summary>
    /// Handles outputting to things with the inventory interface.
    /// </summary>
    /// <param name="OUT_INV"></param>
    public void OutputToInventory(IInventory OUT_INV)
    {
        //If the inventory is completely full, then just return.
        if (OUT_INV.IsFull())
        {
            return;
        }
        else
        {
            //Get the item list with all its counts.
            List<ItemAmount> ITEM_LIST = INV.GetItemAmountList();

            //For each unique item, attempt to add its count.
            foreach (ItemAmount ITEM_INFO in ITEM_LIST)
            {
                //If attempting to add the items would be successful, 
                if (OUT_INV.AttemptAddItem(ITEM_INFO.Item, ITEM_INFO.Amount))
                {
                    GenericEntity OUT_INFO = OUT_INV.GetEntity();
                    List<GenericItem> UNIQUE_ITEMS = INV.GetUniqueItemsList();
                    if (UNIQUE_ITEMS.Contains(ITEM_INFO.Item))
                    {
                        UNIQUE_ITEMS.Add(ITEM_INFO.Item);
                    }

                    //Check if it is valid for us to place these items into the given entity.
                    if (GameManager.Instance.isItemPlacementValid(UNIQUE_ITEMS, OUT_INV.GetEntity().FLAGS))
                    {
                        OUT_INV.AddItem(ITEM_INFO.Item, ITEM_INFO.Amount);
                        INV.ExtractItem(ITEM_INFO.Item, ITEM_INFO.Amount);
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handles outputting to things with the belt interface.
    /// </summary>
    /// <param name="OUT_BELT"></param>
    public void OutputToBelt(IBelt OUT_BELT)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Extracts items from the target inventory and puts it in our inventory.
    /// </summary>
    public void ExtractItem()
    {
        // Always decrement timer if we have a target.
        EXTRACT_TIMER -= Time.deltaTime;

        // We want it to perform the action the frame of it reaching zero not the frame after.
        // If we do it as else it'll be the frame after.
        if (EXTRACT_TIMER <= 0)
        {
            if (ENT_UTIL.HasTag(IN_TARGET.INFO.FLAGS, ENTITY_TYPE.InventoryEntity))
            {
                InputFromInventory(IN_TARGET.GetComponent<IInventory>());
                
            }
            else if (ENT_UTIL.HasTag(IN_TARGET.INFO.FLAGS, ENTITY_TYPE.TransportEntity))
            {
                InputFromBelt(IN_TARGET.GetComponent<IBelt>());
                //Debug.Log("Extracting from a belt.");
            }
        }
    }

    /// <summary>
    /// Handles getting input from inventory.
    /// </summary>
    /// <param name="IN_INV"></param>
    public void InputFromInventory(IInventory IN_INV)
    {
        //Find the item we would extract if it is possible.
        GenericItem EXTRACT_CANDIDATE = IN_INV.AttemptExtractItem();
        Debug.Log(EXTRACT_CANDIDATE);

        //If we have a candidate, try to add it.
        if (EXTRACT_CANDIDATE != null)
        {
            //Attempt to add item.
            int remainder = INV.AddSingleItem(EXTRACT_CANDIDATE);
            
            //If remainder is 0, it succeeded. Actually decrement target inventory now.
            if (remainder == 0)
            {
                IN_INV.ExtractSingleItem(EXTRACT_CANDIDATE);
            }
        }
        EXTRACT_TIMER = (1 / EXTRACT_SPEED) / 2;
    }

    /// <summary>
    /// Handles getting input from belt...?
    /// </summary>
    /// <param name="IN_BELT"></param>
    public void InputFromBelt(IBelt IN_BELT)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// Sets target input inventory and sets timer when we find one.
    /// </summary>
    public void SetInTarget()
    {
        // Scan for a target.
        IN_TARGET = ScanForTarget(INPUT_DIR, INPUT_INTERACT_DISTANCE);

        // If we got a viable target, set the timer.
        if (IN_TARGET != null)
        {
            EXTRACT_TIMER = (1 / EXTRACT_SPEED) / 2;
        }
    }

    /// <summary>
    /// Sets target output inventory.
    /// </summary>
    public void SetOutTarget()
    {
        // Scan for a target.
        OUT_TARGET = ScanForTarget(OUTPUT_DIR, OUTPUT_INTERACT_DISTANCE);

        // If we got a viable target, set the timer.
        if (OUT_TARGET != null)
        {
            EXTRACT_TIMER = (1 / EXTRACT_SPEED) / 2;
        }
    }

    /// <summary>
    /// Scans for an inventory to extract from.
    /// </summary>
    /// <returns></returns>
    public EntityController ScanForTarget(INTERACTION_DIR DIRECTIONS, int DISTANCE)
    {
        //First get all the search coordinates we need.
        List<int[]> SEARCH_COORDS = CalculateAllDifferences(DIRECTIONS, DISTANCE);

        //List for all possible entities
        List<GameObject> POSSIBLE_TARGETS = new List<GameObject>();

        //Search through all the search coordinates.
        foreach(int[] coord in SEARCH_COORDS)
        {
            //The target object at the given difference.
            GameObject TARGET = GameManager.Instance.FindEntityWithDifference(GetComponent<GridController>(), coord);

            //If we received a target object, and it has an inventory component
            if (TARGET != null)
            {
                //If the entity has the tag to treat it as an inventory...
                if(ENT_UTIL.HasTag(TARGET.GetComponent<EntityController>().INFO.FLAGS, ENTITY_TYPE.InventoryEntity))
                {
                    //If we pass the inventory permission check
                    if (GameManager.Instance.CheckInventoryPermissions(GetComponent<EntityController>().INFO.FLAGS, TARGET.GetComponent<Inventory>().INV_FLAGS))
                    {
                        POSSIBLE_TARGETS.Add(TARGET);
                    }
                }//If the entity has the tag to treat it as a belt...
                else if(ENT_UTIL.HasTag(TARGET.GetComponent<EntityController>().INFO.FLAGS, ENTITY_TYPE.TransportEntity))
                {
                    //All belts are accessible so add as possible targets.
                    POSSIBLE_TARGETS.Add(TARGET);
                }               
            }
        }

        //Return the first target that has inventory component.
        //This means that it will return the direction that is higher in priority, 
        //as in the ones with lower bit values.
        //Order is: Front -> Back -> Left -> Right -> Top -> Bottom.
        foreach(GameObject OBJ in POSSIBLE_TARGETS)
        {
            return OBJ.GetComponent<EntityController>();
        }
        
        //If we find nothing, return null.
        return null;
    }

    /// <summary>
    /// Calculates all the differences in comparison to our grid position that we need to search for targets.
    /// </summary>
    /// <returns></returns>
    private List<int[]> CalculateAllDifferences(INTERACTION_DIR DIRECTIONS, int DISTANCE)
    {
        // The return list of grid position differences to search for.
        List<int[]> differences = new List<int[]>();

        // The rotation to adjust for.
        ROTATION_VAL[] ROT_ADJUST_ENUM = GetComponent<GridController>().GRID_ROT;

        // The rotation values to use when adjusting
        Vector3 ROT_ADJUST = GetComponent<GridController>().CalculateRotVector(ROT_ADJUST_ENUM);

        // List of directions to use when calculating.
        List<Vector3> directions = new List<Vector3>();

        // Add all the directional vectors from our transform depending on our flags.
        if (ID_UTIL.HasTag(DIRECTIONS, INTERACTION_DIR.Front))
        {
            directions.Add(transform.forward);
        }

        if (ID_UTIL.HasTag(DIRECTIONS, INTERACTION_DIR.Back))
        {
            directions.Add(-transform.forward);
        }

        if (ID_UTIL.HasTag(DIRECTIONS, INTERACTION_DIR.Left))
        {
            directions.Add(-transform.right);
        }

        if (ID_UTIL.HasTag(DIRECTIONS, INTERACTION_DIR.Right))
        {
            directions.Add(transform.right);
        }

        if(ID_UTIL.HasTag(DIRECTIONS, INTERACTION_DIR.Top))
        {
            directions.Add(transform.up);
        }

        if(ID_UTIL.HasTag(DIRECTIONS, INTERACTION_DIR.Bottom))
        {
            directions.Add(-transform.up);
        }

        // Calculate the difference that we want to search for each direction we need to look at.
        foreach(Vector3 DIR in directions)
        {
            // The difference we are going to add.
            int[] difference = GetComponent<GridController>().CalculateGridPos(
                GetComponent<GridController>().TARGET_POS + DIR * CONSTANTS.grid_length * DISTANCE);

            // Add the calculate difference.
            differences.Add(difference);
        }
            
        // Return all calculated differences. 
        // IF there are none this will be count 0.
        return differences;
    }
}
