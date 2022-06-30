using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI calss responsible for update in the UI the number of agents in the simulation aswel as the speed.
/// </summary>
public class UI_Counter : MonoBehaviour
{
    [SerializeField]
    private Text alive;
    [SerializeField]
    private Text dead;
    [SerializeField]
    private Text currentSpeed;
    [SerializeField]
    private Text escaped;

    private int aliveCount = 0;
    private int escapedCount = 0;
    private int deadCount = 0;
    private float speedMult = 1f;

    private void Start()
    {
        currentSpeed.text = speedMult.ToString() + " x speed";
        dead.text = deadCount.ToString() + " Dead";
        alive.text = aliveCount.ToString() + " Alive";
        escaped.text = escapedCount.ToString() + " Escaped";
    }

    //updates how much alive entities are in the simulation
    internal void AliveCount(int value)
    {
        aliveCount += value;
        alive.text = aliveCount.ToString() + " Alive";
    }

    /// <summary>
    /// updates how many entities died so far
    /// </summary>
    /// <param name="value">dead entities</param>
    internal void DeadCount(int value)
    {
        deadCount += value;
        aliveCount -= value;
        dead.text = deadCount.ToString() + " Dead";
        alive.text = aliveCount.ToString() + " Alive";
    }

    /// <summary>
    /// updates how many entities escaped so far
    /// </summary>
    /// <param name="value">escaped entities</param>
    internal void EscapedCount(int value)
    {
        escapedCount += value;
        escaped.text = escapedCount.ToString() + " Escaped";
    }

    /// <summary>
    /// updates the actual speed value
    /// </summary>
    /// <param name="value"> actual time speed</param>
    internal void SpeedChange(float value)
    {
        speedMult += value;
        currentSpeed.text = speedMult.ToString() + " X";
    }
}
