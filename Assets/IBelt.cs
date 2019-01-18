using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

/// <summary>
/// Interface for belts and their inventories.
/// </summary>
public interface IBelt
{
    /// <summary>
    /// Returns entity that this script is attached to.
    /// </summary>
    /// <returns></returns>
    GenericEntity GetEntity();

    /// <summary>
    /// Returns true and false, tells us if an object is on the belt.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool ContainsItem(GenericItem item);

    /// <summary>
    /// Returns insert direction based on this belt's transform and inserter's transform.
    /// </summary>
    /// <param name="inserter"></param>
    /// <returns></returns>
    InsertDir GetInsertDir(GridController inserter);

    /// <summary>
    /// Attempts to add an item to the belt. 
    /// Returns true if possible, else null.
    /// Since this isn't really an inventory that holds stacks, we don't need to know what item is being added.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool AttemptAddItem(Extractor inserter);

    /// <summary>
    /// Adds an item to the belt.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    GenericItem AddItem(GenericItem item, Extractor inserter);

    /// <summary>
    /// Attempts to extract item from the belt.
    /// Returns item if possible, else null.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    bool AttemptExtractItem(GenericItem item, Extractor inserter);

    /// <summary>
    /// Extract an item from the belt.
    /// Returns item if it works, else null.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    GenericItem ExtractItem(GenericItem item, Extractor inserter);
}
