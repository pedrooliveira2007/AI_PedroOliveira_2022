using System.Collections;
using UnityEngine;
/// <summary>
/// class responsible to generate the agents
/// </summary>
public class PawnGenerator : MonoBehaviour
{
    /// <summary>
    /// Array that holds all the possible personalities.
    /// </summary>
    [SerializeField]
    private Agent_Personality[] personalityArray;

    /// <summary>
    /// Holds the number of agents to spawn.
    /// </summary>
    [SerializeField]
    private int numAgents;

    /// <summary>
    /// Number of agents spawned.
    /// </summary>
    private int spawnedAgents;

    /// <summary>
    /// Holds the reference for Location_Manager, acts for our guide to know
    /// what is where.
    /// </summary>
    [SerializeField]
    private Location_Manager gps;

    /// <summary>
    /// Locations of entity spawns.
    /// </summary>
    [SerializeField]
    private Transform[] spawnLocation;

    /// <summary>
    /// Prefab for the pawn, to spawn in the scenery.
    /// </summary>
    [SerializeField]
    private GameObject pawnPrefab;


    private void Start()
    {
        gps = GetComponent<Location_Manager>();
    }

    /// <summary>
    /// Spawns all the agents.
    /// Function to call on a click of a button on the UI.
    /// </summary>
    public void StartSpawning()
    {
        if (spawnedAgents < numAgents)
            StartCoroutine(SpawnAgents());
    }

    /// <summary>
    /// Function to spawn the number of pawns specified.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnAgents()
    {
        spawnedAgents++;

        int value = Mathf.FloorToInt(Random.Range(0, spawnLocation.Length ));
        Instantiate(pawnPrefab, spawnLocation[value].position, spawnLocation[value].rotation).GetComponent<Pawn_Manager>().
           SetPersonality(personalityArray[Mathf.FloorToInt(Random.Range(0, personalityArray.Length - 1))], gps);

        yield return new WaitForSeconds(0.1f);
        // It is recursive as it is more optimized than making a FOR, as it will
        // make a new object everytime we yield return.
        if (spawnedAgents < numAgents)
        {
            StartCoroutine(SpawnAgents());
        }
    }
    /// <summary>
    /// Modifies the scale of speed of the game, used for UI.
    /// </summary>
    /// <param name="value"></param>
    public void SetTimeScale(float value)
    {
        Time.timeScale += value;
    }

}
