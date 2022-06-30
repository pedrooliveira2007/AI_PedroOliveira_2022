using UnityEngine;

public class Seat_Manager : MonoBehaviour
{
    /// <summary>
    /// Holds its table.
    /// </summary>
    [SerializeField]
    private Table_Manager mytable;
    /// <summary>
    /// Holds its availability to be selected.
    /// </summary>
    [SerializeField]
    private bool available = true;

    /// <summary>
    /// Returns availability.
    /// </summary>
    /// <returns></returns>
    internal bool Available()
    {
        return available;
    }

    /// <summary>
    /// Removes it from being able to be selected.
    /// And returns the vector3 position of it.
    /// </summary>
    /// <returns></returns>
    internal Vector3 Position()
    {
        available = false;
        return transform.position;
    }

    /// <summary>
    /// Enables it to be selected.
    /// And informs the table that it has been free'd up in case of a subscribe.
    /// </summary>
    internal void SeatFree()
    {
        available = true;
        mytable.SeatFree(this);
    }

}
