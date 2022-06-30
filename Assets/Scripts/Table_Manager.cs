using UnityEngine;

public class Table_Manager : MonoBehaviour
{
    [SerializeField]
    private FoodZone_Manager food;
    [SerializeField]
    private Seat_Manager[] seats;



    internal bool Available()
    {
        bool open = false;
        for (int i = 0; i < seats.Length; i++)
        {
            if (seats[i].Available())
            {
                open = true;
                i = 1000;
            }
        }
        return open;
    }

    /// <summary>
    /// Finds how many seats are Available.
    /// </summary>
    /// <returns></returns>
    internal int GetOpenSeats()
    {
        int open = 0;
        for (int i = 0; i < seats.Length; i++)
        {
            if (seats[i].Available())
            {
                open++;
            }
        }
        return open;
    }

    /// <summary>
    /// Returns the first seat it finds.
    /// </summary>
    /// <returns></returns>
    internal Vector3 SeatPosition()
    {
        int open = 10000;
        for (int i = 0; i < seats.Length; i++)
        {
            if (seats[i].Available())
            {
                open = i;
                i = 1000;
            }
        }
        return seats[open].Position();
    }

    /// <summary>
    /// Informs the FoodZone_Manager in case of a Subscribe..
    /// </summary>
    /// <param name="pos"></param>
    internal void SeatFree(Seat_Manager pos)
    {
        food.SeatFree(pos);
    }
}
