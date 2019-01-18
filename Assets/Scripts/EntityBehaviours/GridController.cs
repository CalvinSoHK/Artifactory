using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

/// <summary>
/// Controls position of object so it is always in a grid position.
/// This should not be attached to anything that needs physics attached.
/// </summary>
[RequireComponent(typeof(EntityController))]
public class GridController : MonoBehaviour
{
    /// <summary>
    /// The anchor specfies where the object should be lining up to the grid.
    /// In general, should be placed at the lowest x and z corner of the object.
    /// </summary>
    public Transform anchor;

    /// <summary>
    /// The in-game grid position separate from the Unity grid.
    /// </summary>
    public int[] GRID_POS = new int[3];

    /// <summary>
    /// Rotation from our set values.
    /// </summary>
    public ROTATION_VAL[] GRID_ROT = new ROTATION_VAL[3];

    /// <summary>
    /// The target position we should be at calculated by multiplying the GRID_POS by grid_length
    /// </summary>
    public Vector3 TARGET_POS;

    /// <summary>
    /// The target rotation we should be at.
    /// </summary>
    public Vector3 TARGET_ROT;

    /// <summary>
    /// Whether or not the GameManager has this grid controller accounted for.
    /// When false, we need to keep trying to reach the GameManager to update it.
    /// </summary>
    private bool ACCOUNTED = false;

    private void Awake()
    {
        InformGameManager();
    }

    private void Start()
    {
        //Can't use update, since rotations are likely to seem correct sometimes.
        SetPosition(GRID_POS);
        SetRotation(GRID_ROT);
    }

    private void Update()
    {
        //If it hasn't been updated to the gridcontroller yet, do it.
        InformGameManager();

        //Update position and rot.
        UpdatePosAndRot();
    }

    /// <summary>
    /// Tries to inform the game manager.
    /// </summary>
    public void InformGameManager()
    {
        if (!ACCOUNTED && GameManager.Instance != null)
        {
            GameManager.Instance.AddGridController(this);
            ACCOUNTED = true;
        }
    }

    /// <summary>
    /// Function that updates position and rotation.
    /// </summary>
    public void UpdatePosAndRot()
    {
        //If our target position is not correct, update it.
        //Room for optimization since we are using calculate twice here.
        //It is being called again in set.
        if (TARGET_POS != CalculateTargetPos(GRID_POS))
        {
            SetPosition(GRID_POS);
        }

        //If our target rotation is not correct, update it.
        //Room for optimization since we are using calculate twice here.
        //It is being called again in set.
        if (TARGET_ROT != CalculateRotVector(GRID_ROT))
        {
            SetRotation(GRID_ROT);
        }
    }

    /// <summary>
    /// Calculates the target position by multiplying the grid position by our grid_length.
    /// </summary>
    /// <param name="GRID_POS"></param>
    /// <returns></returns>
    public Vector3 CalculateTargetPos(int[] GRID_POS_IN)
    {
        Vector3 TEMP = new Vector3(GRID_POS_IN[0], GRID_POS_IN[1], GRID_POS_IN[2]);
        return TEMP * CONSTANTS.grid_length - anchor.localPosition;
    }

    /// <summary>
    /// Calculates grid position based on our in engine position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns> Returns int array of length 3 of grid position. </returns>
    public int[] CalculateGridPos(Vector3 position)
    {
        //Debug.Log(position);
        Vector3 adjusted_pos = position + anchor.localPosition;
        //Debug.Log(adjusted_pos);
        int[] TEMP = new int[3];
        TEMP[0] = (int)(adjusted_pos.x / CONSTANTS.grid_length);
        TEMP[1] = (int)(adjusted_pos.y / CONSTANTS.grid_length);
        TEMP[2] = (int)(adjusted_pos.z / CONSTANTS.grid_length);
        return TEMP;
    }

    /// <summary>
    /// Places this object at the given GRID_POS.
    /// </summary>
    /// <param name="GRID_POS"></param>
    public void SetPosition(int[] GRID_POS_IN)
    {
        //Calculate target position and set it there.
        TARGET_POS = CalculateTargetPos(GRID_POS_IN);
        transform.position = TARGET_POS;
    }

    /// <summary>
    /// Rotates the object based on ROT_IN
    /// </summary>
    /// <param name="ROT_IN"></param>
    public void SetRotation(ROTATION_VAL[] ROT_IN)
    {
        TARGET_ROT = CalculateRotVector(ROT_IN);
        transform.rotation = Quaternion.Euler(TARGET_ROT);
    }

    /// <summary>
    /// Convert rotation enum to the actual value.
    /// </summary>
    /// <returns></returns>
    public float RotEnumToVal(ROTATION_VAL value)
    {
        switch (value)
        {
            case ROTATION_VAL.Zero:
                return 0;
            case ROTATION_VAL.Ninety:
                return 90;
            case ROTATION_VAL.OneEighty:
                return 180;
            case ROTATION_VAL.TwoSeventy:
                return 270;
            default:
                throw new System.Exception("Error, no rotational value input. Defaulting to 0.");
        }
    }

    /// <summary>
    /// Does the above conversion for a full vector.
    /// Returns an actual vector to be used by the transform.
    /// </summary>
    /// <param name="ROT_ENUM_VECTOR"></param>
    /// <returns></returns>
    public Vector3 CalculateRotVector(ROTATION_VAL[] ROT_ENUM_VECTOR)
    {
        return new Vector3(RotEnumToVal(ROT_ENUM_VECTOR[0]), RotEnumToVal(ROT_ENUM_VECTOR[1]), RotEnumToVal(ROT_ENUM_VECTOR[2]));
    }
}
