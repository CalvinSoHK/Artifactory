using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script is meant to manage an object that just spawns a given resource over and over again.
// Specifically meant for debugging.
public class ResourceGenerator : MonoBehaviour
{
    /// <summary>
    /// Whether or not to treat this object as an inventory with items in it
    /// </summary>
    public bool inventory = false;
    private Inventory inv;

    /// <summary>
    /// How long before it spawns again
    /// </summary>
    public float interval = 1;

    /// <summary>
    /// Internal timer for spawning things
    /// </summary>
    private float timer = 0;

    /// <summary>
    /// The object we want to be spawning
    /// </summary>
    public GenericItem spawn_item;

    private void Start()
    {
        //Only add an inventory script if we didn't already add one.
        if (inventory && GetComponent<Inventory>() == null)
        {
            gameObject.AddComponent<Inventory>();
        }
        inv = GetComponent<Inventory>();
    }


    // Update is called once per frame
    void Update()
    {
        if( timer < interval)
        {
            timer += Time.deltaTime;
        }
        else
        {
            //If this isn't an inventory style debugger, spawn the actual object at the given location.
            if (!inventory)
            {
                //Spawn item at our given position
                spawn_item.SpawnObject(transform.position, transform.rotation, null);
            }
            else //Since it is an inventory style debugger, add a copy of it to our inventory
            {
                inv.AddSingleItem(spawn_item);
            }
            timer = 0;
        }
    }
}
