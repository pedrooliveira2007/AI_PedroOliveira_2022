using System.Collections.Generic;
using UnityEngine;

public class FoodZone_Manager : MonoBehaviour
{
    /// <summary>
    /// List of subscribers.
    /// </summary>
    List<Pawn_Manager> subscribers;

    /// <summary>
    /// Array that holds the ammount of tables.
    /// </summary>
    [SerializeField]
    Table_Manager[] tables;

    /// <summary>
    /// Last check it the buffer was full.
    /// </summary>
    bool fullBuffer = false;
    /// <summary>
    /// If it is full.
    /// </summary>
    bool full = false;
    /// <summary>
    /// Newest free'd spot.
    /// </summary>
    Seat_Manager newestSpot;


    private void Start()
    {
        subscribers = new List<Pawn_Manager>();
    }

    /// <summary>
    /// Gives the the vector3 of a seat of the least ocupied table.
    /// </summary>
    /// <returns> returns the last ocupied table's position</returns>
    internal Vector3 GetLeastOcupiedTable()
    {
        if(fullBuffer && !full)
        {
            full = true;
            fullBuffer = false;
            return newestSpot.Position();
        }
        int leastOcupied = 0;
        int smallestSeats = 0;
        for(int i = 0; i < tables.Length; i++)
        {
            if (tables[i].Available())
            {
                int current = tables[i].GetOpenSeats();
                if (current > leastOcupied)
                {
                    leastOcupied = current;
                    smallestSeats = i;
                }
            }
            else
            {
                Debug.Log("NO available tables");
            }
        }

        if(leastOcupied == 0 && smallestSeats == 0)
        {
           
            return new Vector3();
        }
        else
        {
            return tables[smallestSeats].SeatPosition();
        }

    }

    /// <summary>
    /// Checks if there is an Available seat or not.
    /// </summary>
    /// <returns>returns true if there's a available seat or false if all seats are still ocupied</returns>
    internal bool isSeatFree()
    {
        for (int i = 0; i < tables.Length; i++)
        {
            if (tables[i].Available())
            {
                return true;
            }

        }
        full = true;
        return false;
    }


    /// <summary>
    /// Used to inform the script if there is a new Available seat.
    /// </summary>
    /// <param name="pos">reference for the Seat_Manager</param>
    internal void SeatFree(Seat_Manager pos)
    {
        if (full)
        {
            if (!CheckSubs(pos))
            {
                newestSpot = pos;
                fullBuffer = true;
                full = false;
            }
        }
        else
        {
            full = false;
            fullBuffer = false;
        }
    }

    /// <summary>
    /// Checks if there is any subscribers. And then pops the oldest subscriber
    /// removing him from the list.
    /// </summary>
    /// <param name="pos">reference for the Seat_Manager</param>
    /// <returns></returns>
    private bool CheckSubs(Seat_Manager pos)
    {
        if(subscribers.Count > 0)
        {
            float distance1 = 1000000;
            Pawn_Manager closest = null;
            foreach(Pawn_Manager pawn in subscribers)
            {
                float tempdistance = Vector3.Distance(pawn.transform.position, pos.Position());
                if(tempdistance < distance1)
                {
                    distance1 = tempdistance;
                    closest = pawn;
                }
            }
            closest.ReceiveSub(pos.Position());
            subscribers.Remove(closest);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Subscribes the agent to the list of the class.
    /// </summary>
    /// <param name="pawn"></param>
    internal void Subscribe(Pawn_Manager pawn)
    {
        subscribers.Add(pawn);
    }


}
