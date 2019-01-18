using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    /// <summary>
    /// The scriptable object that contains the info for this entity.
    /// </summary>
    public GenericEntity INFO;

    /// <summary>
    /// The current health of the given entity.
    /// </summary>
    private float HEALTH;

    private void Start()
    {
        InitEntity();
    }

    private void Update()
    {
        CheckIfDead();
    }

    /// <summary>
    /// Checks if the entity should be dead.
    /// </summary>
    public void CheckIfDead()
    {
        if (HEALTH <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Initializes entity.
    /// </summary>
    public void InitEntity()
    {
        HEALTH = INFO.MAX_HEALTH;
    }
}
