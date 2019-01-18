using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic item controller.
/// </summary>
public class ItemController : MonoBehaviour
{
    /// <summary>
    /// The info for this item.
    /// </summary>
    public GenericItem INFO;

    /// <summary>
    /// Despawns the object.
    /// </summary>
    public void DespawnObject()
    {
        Destroy(gameObject);
    }
}
