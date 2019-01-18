using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

// Generic item script, super class for all items in game, regardless if placeable or not.
// Not the same as generic entities, which are meant for things that exist in the world.
[CreateAssetMenu(fileName = "ItemInfo", menuName = "Item/Info", order = 2)]
public class GenericItem : ScriptableObject
{
    /// <summary>
    /// Item name.
    /// </summary>
    public string NAME;

    /// <summary>
    /// The prefab 
    /// </summary>
    public Rigidbody OBJ;

    /// <summary>
    /// The sprite icon to be used in UIs.
    /// </summary>
    public Sprite ICON;

    /// <summary>
    /// Stack size. How many copies of this item can stick in one slot in the inventory screen.
    /// </summary>
    public int STACK_SIZE = 100;

    /// <summary>
    /// Item flags that apply to this item.
    /// </summary>
    [EnumFlag]
    public ITEM_TYPE FLAGS;

    /// <summary>
    /// Spawns the object at a given world position, and returns the entity controller on it.
    /// </summary>
    /// <param name="POS"></param>
    /// <param name="ROT"></param>
    /// <param name="PARENT"></param>
    public ItemController SpawnObject(Vector3 POS, Quaternion ROT, Transform PARENT)
    {
        //Use a hash our ID to then load the path of the item from our dictionary, load it from that path.
        throw new System.NotImplementedException();
    }
}
