using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(fileName = "EntityInfo", menuName = "Entity/Info", order = 1)]
public class GenericEntity : ScriptableObject
{
    /// <summary>
    /// Entity name. Such as Furnace.
    /// </summary>
    public string NAME;

    /// <summary>
    /// Flags for how this entity should be treated.
    /// </summary>
    [EnumFlag]
    public ENTITY_TYPE FLAGS;

    /// <summary>
    /// Prefab for this entity.
    /// </summary>
    public Rigidbody OBJ;

    /// <summary>
    /// Maximum health this can have.
    /// </summary>
    public int MAX_HEALTH;

    /// <summary>
    /// How much this entity resists being mined.
    /// Different from health because health going to 0 results in permanent destruction.
    /// This value is meant to be a timer for how long it takes to break the item.
    /// High values means it is broken slower. Lower values means it is mined faster.
    /// Should set machines to be higher values if theyre not meant to be moved around that much.
    /// Example: 1 means it breaks in 1 second with a mining strength of 1
    /// Example: 0.5 means it breaks in 0.5 seconds with a mining strength of 1
    /// Example: 2 means it breaks in 2 seconds with a mining strength
    /// </summary>
    public float BREAK_RESIST;
}
