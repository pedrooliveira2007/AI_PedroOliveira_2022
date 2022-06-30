using UnityEngine;

public class Location_Manager : MonoBehaviour
{
    /// <summary>
    /// Reference to talk to the Foodzone_Manager and enable the Location_Manager
    /// to setup every information in regards to the tables and seats.
    /// </summary>
    [SerializeField]
    private FoodZone_Manager food;

    /// <summary>
    /// Reference to the array of Transforms that hold the locations of the various
    /// positions the 1st concert is seperated.
    /// </summary>
    [SerializeField]
    private Transform[] concertZone;

    /// <summary>
    /// Counter for the current transform the script is at on the 1st Concert Zone.
    /// </summary>
    private int currentConcertZone = 0;

    /// <summary>
    /// Reference to the array of BoxColliders that hold the collisions of the Resting zone
    /// (Green Area)
    /// </summary>
    [SerializeField]
    private BoxCollider[] greenZone;

    /// <summary>
    /// Reference to the array of Transforms that hold the locations of the various
    /// exits that exist.
    /// </summary>
    [SerializeField]
    private Transform[] exitPoints;

    /// <summary>
    /// Counter for what was the last Green zone an pawn was placed at.
    /// </summary>
    private int currentGreenZone = 0;


    /// <summary>
    /// Returns a vector3 of one of the Concert locations.
    /// </summary>
    /// <returns></returns>
    internal Vector3 GiveConcertZone()
    {
        if (currentConcertZone >= concertZone.Length)
        {
            currentConcertZone = 0;
        }
        currentConcertZone++;
        return concertZone[currentConcertZone - 1].position;
    }

    /// <summary>
    /// Returns a boolean depending on if there is a seat Available or not.
    /// </summary>
    /// <returns></returns>
    internal bool IsFoodOpen()
    {
        return food.isSeatFree();
    }

    /// <summary>
    /// Returns a Vector3 with the location of the seat that is attributed to
    /// the function caller.
    /// </summary>
    /// <returns></returns>
    internal Vector3 GetFoodSpot()
    {
        return food.GetLeastOcupiedTable();
    }

    /// <summary>
    /// Subscribes the given pawn to the list of pawns waiting for a seat to eat.
    /// </summary>
    /// <param name="pawn"></param>
    internal void SubscribeToMeal(Pawn_Manager pawn)
    {
        food.Subscribe(pawn);
    }

    /// <summary>
    /// Returns a Vector3 with a random location situated on the resting area.
    /// </summary>
    /// <returns></returns>
    internal Vector3 GetGreenArea()
    {
        if (currentGreenZone >= greenZone.Length)
        {
            currentGreenZone = 0;
        }
        currentGreenZone++;
        return RandomPointInBounds(greenZone[currentGreenZone - 1].bounds);
    }

    /// <summary>
    /// Returns a Vector3 of the closest exit in comparison with the pawn given.
    /// </summary>
    /// <param name="pawn"></param>
    /// <returns></returns>
    internal Vector3 GetExit(Vector3 pawn)
    {
        float shortestDistance = 100000;
        int index = 100;
        for (int i = 0; i < exitPoints.Length; i++)
        {
            float temp = Vector3.Distance(pawn, exitPoints[i].position);
            if(temp < shortestDistance)
            {
                shortestDistance = temp;
                index = i;
            }
        }

        if(index != 100 && shortestDistance != 100000)
        {
            return exitPoints[index].position;
        }
        return new Vector3();
    }

    /// <summary>
    /// Returns a random point in the bounds of the collision box.
    /// Source: https://forum.unity.com/threads/pick-random-point-inside-box-collider.541585/
    /// </summary>
    /// <param name="bounds"></param>
    /// <returns></returns>
    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }


}
