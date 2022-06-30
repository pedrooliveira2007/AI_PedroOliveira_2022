using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Pawn_Manager : MonoBehaviour
{
    /// <summary>
    /// Holds the reference to the UI Counter.
    /// </summary>
    private UI_Counter ui;

    /// <summary>
    /// 0 = Moving 1 = AtZone1 2 = AtZone2 3 = Eating 4 = Resting 5 = Waiting
    /// </summary>
    [SerializeField]
    private int currentState = 5;

    /// <summary>
    /// 0 = Waiting/Roaming , 1 = GoStage , 3 = GoEating , 4 = GoResting , 5 = Exit
    /// </summary>
    public int goal = 0;

    /// <summary>
    /// Variable that defines the ammount of time the slow down will last for.
    /// </summary>
    private float timer = 1;
    /// <summary>
    /// Variable that holds current timer for the ammount of time the slow down will last for.
    /// </summary>
    private float currentTimer = 0;

    /// <summary>
    /// Agent's move speed.
    /// </summary>
    private float moveSpeed = 3.5f;

    /// <summary>
    /// Agent's max hunger.
    /// </summary>
    private float maxHunger = 100;
    /// <summary>
    /// Agent's current hunger.
    /// </summary>
    [SerializeField]
    public float currentHunger = 90;
    /// <summary>
    /// Agent's eating speed.
    /// </summary>
  
    private float eatRate = 1;
    /// <summary>
    /// Agent's hunger rate multiplyer.
    /// </summary>
    private float hungerRate = 1;
    /// <summary>
    /// Agent's max fatigue.
    /// </summary>
    private float maxFatigue = 100;
    /// <summary>
    /// Agent's current fatigue.
    /// </summary>
    [SerializeField]
    private float currentFatigue = 90;
    /// <summary>
    /// Defines if the player is slowing down or not.
    /// </summary>
    private bool slowing = false;
    /// <summary>
    /// Defines if the player is panicking or not.
    /// </summary>
    private bool panicking = false;
    /// <summary>
    /// Defines if the player is subscribed or not.
    /// </summary>
    private bool subscribed = false;
    /// <summary>
    /// Defines if the player has received his subscription or not.
    /// </summary>
    private bool returnedSub = false;
    /// <summary>
    /// Defines if the player is currently eating or not.
    /// </summary>
    private bool eating = false;
    /// <summary>
    /// Defines if the player is currently resting or not.
    /// </summary>
    private bool resting = false;

    /// <summary>
    /// Holds the vector3 that the user has subscribed for.
    /// </summary>
    private Vector3 subscribeVector;

    /// <summary>
    /// Holds a reference to his NavMeshAgent.
    /// </summary>
    private NavMeshAgent myAgent;

    /// <summary>
    /// Holds a reference to the location_manager that will be used to
    /// know what location is whatever the agent wants to go to.
    /// </summary>
    private Location_Manager gps;

    /// <summary>
    /// Holds a reference of the Seat that is in use by the player.
    /// </summary>
    private Seat_Manager currentSeat;

    [SerializeField]
    private MeshRenderer mesh;

    /// <summary>
    /// Returns nothing and does nothing.
    /// </summary>
    internal void Nothing()
    {

    }

    /// <summary>
    /// Returns the bool of eating.
    /// </summary>
    /// <returns></returns>
    internal bool IsEating()
    {
        return eating;
    }

    /// <summary>
    /// Returns the int of goal.
    /// </summary>
    /// <returns></returns>
    internal int GetGoal()
    {
        return goal;
    }

    /// <summary>
    /// Return if the player is subscribed to seat list or not.
    /// </summary>
    /// <returns></returns>
    internal bool IsSubscribed()
    {
        return subscribed;
    }

    /// <summary>
    /// Return if nps is resting or not
    /// </summary>
    /// <returns></returns>
    internal bool IsResting()
    {
        return resting;
    }


    /// <summary>
    /// Holds a reference of the collider to trigger panic in nearby pawns. 
    /// </summary>
    [SerializeField]
    private SphereCollider panicBox;

    internal bool Wait()
    {
        return true;
    }

    /// <summary>
    /// Checks if the user is tired. In other words, if it lower than maxFatigue/4.
    /// </summary>
    /// <returns></returns>
    internal bool IsTired()
    {
        if (currentFatigue <= maxFatigue/4)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Checks if the user is hungry. In other words, if it lower than maxHunger/4.
    /// </summary>
    /// <returns></returns>
    internal bool IsHungry()
    {
        if (currentHunger <= maxHunger/4)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Moves the agent to the closest exit given via the GPS.
    /// </summary>
    internal void FleeToExitAction()
    {
        myAgent.isStopped = false;
        myAgent.speed = moveSpeed;
        myAgent.destination = gps.GetExit(transform.position);
        currentState = 0;
        goal = 5;
    }

    /// <summary>
    /// Moves the agent to the closest Available Seat_Manager, given via the GPS.
    /// If none are Available subscribes to the FoodZone_Manager, waiting until
    /// a seat has been opened and then going to it.
    /// </summary>
    internal void GoEatAction()
    {
        myAgent.isStopped = false;
        if (gps.IsFoodOpen())
        {
            subscribeVector = gps.GetFoodSpot();
            myAgent.SetDestination(subscribeVector);
            currentState = 0; //walk to the available seat
            goal = 3;
        }
        else
        {
            gps.SubscribeToMeal(this); //subscribed to seat wait list
            currentState = 5; //wait till the seat is available
            goal = 3;
        }

    }

    /// <summary>
    /// Moves the agent to the first concert vector3 given via the GPS.
    /// </summary>
    internal void GoConcertAction()
    {
        if(goal !=1)
        {
            myAgent.isStopped = false;
            myAgent.SetDestination(gps.GiveConcertZone());
            currentState = 0;
            goal = 1;
        }

    }

    /// <summary>
    /// Moves the agent to the resting area given via the GPS.
    /// </summary>
    internal void RestAction()
    {
        myAgent.isStopped = false;
        myAgent.SetDestination(gps.GetGreenArea());
        currentState = 0;
        goal = 4;
    }

    /// <summary>
    /// Checks if the user is in panic.
    /// </summary>
    /// <returns></returns>
    internal bool IsInPanic()
    {
        return panicking;
    }


    /// <summary>
    /// Initializer of the personality of the Pawn.
    /// </summary>
    /// <param name="personality"></param>
    /// <param name="gps"></param>
    internal void SetPersonality(Agent_Personality personality, Location_Manager gps)
    {
        ui = GameObject.FindGameObjectWithTag("UI").GetComponent<UI_Counter>();
        ui.AliveCount(1);
        this.gps = gps;
        moveSpeed = personality.moveSpeed;
        maxHunger = personality.maxHunger;
        currentHunger = Random.Range(personality.maxHunger, personality.minHunger);
        eatRate = personality.eatRate;
        hungerRate = personality.hungerRate;
        maxFatigue = personality.maxFatigue;
        currentFatigue = Random.Range(personality.maxFatigue, personality.minFatigue);
        mesh.material = personality.myMaterial;
        myAgent = GetComponent<NavMeshAgent>();
        myAgent.speed = moveSpeed;
        goal = 0;
    }

    /// <summary>
    /// Used to translate the current State to a string for setting up tags.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    private string GetState(int state)
    {
        switch (state)
        {
            case 0:
                return "Waiting/Moving";
            case 1:
                return "Stage";
            case 3:
                return "Eating";
            case 4:
                return "Resting";
            case 5:
                return "GoSeat";
            default:
                return "";

        }
    }

    private void FixedUpdate()
    {
        // Used to update the fatigue.
        if (currentState == 4 && resting)
        {
            if (currentFatigue >= maxFatigue)
            {
                currentFatigue = maxFatigue;
                resting = false;
                currentState = 0;
                goal = 0;
            }
            else
            {
                currentFatigue += Time.fixedDeltaTime * 4;
            }
        }
        else
        {
            currentFatigue -= Time.fixedDeltaTime / 2;
        }

        // Used to update the hunger.
        if (currentState == 3 && eating)
        {
            if (currentHunger >= maxHunger)
            {
                currentHunger = maxHunger;
                eating = false;
                currentSeat.SeatFree();
                currentState = 0;
                goal = 0;
               
            }
            else
            {
                currentHunger += Time.fixedDeltaTime * 2 * eatRate;
                // They are sitting while eating, it should reduce the fatigue by a slower ammount.
                currentFatigue += Time.fixedDeltaTime / 5;
            }
        }
        else
        {
            currentHunger -= Time.fixedDeltaTime / 2;
        }
        // Used to slow down the agent to the destination when going to the concerts. 
        if ( !slowing && goal == 1 && currentState == 0)
        {
            if (Vector3.Distance(myAgent.destination, transform.position) < 9)
            {
                currentTimer = 0;
                StartCoroutine(GradualSlow());
                slowing = true;
            }
        }
    }


    /// <summary>
    /// Used to enable the slow down of the agent when they are in a radius of their target.
    /// </summary>
    /// <returns></returns>
    IEnumerator GradualSlow()
    {
        currentTimer += Time.fixedDeltaTime;
        float ratio = currentTimer / timer;
        ratio = moveSpeed * ratio;
        myAgent.speed = moveSpeed - ratio;
        yield return new WaitForFixedUpdate();
        if (currentTimer < timer)
        {
            StartCoroutine(GradualSlow());
        }
        else
        {
            myAgent.isStopped = true;
            currentState = goal;
            gameObject.tag = GetState(goal);
            slowing = false;
            myAgent.speed = moveSpeed;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Rest" && goal == 4)
        {
            resting = true;
            currentState = 4;
        }

        if (other.gameObject.tag == "Exit" && goal == 5)
        {
            ui.EscapedCount(1);
            Destroy(gameObject);
        }

        // If the collider is the seat.
        if (other.gameObject.tag == "Available" && goal == 3)
        {
            Debug.Log("Im here");
                if (other.gameObject.transform.position == subscribeVector)
            {
                currentState = 3;
                currentSeat = other.gameObject.GetComponent<Seat_Manager>();
                eating = true;
            }

        }

        if(other.gameObject.tag == "Explosion")
        {
            ui.DeadCount(1);
            Destroy(gameObject);
        }
        if(other.gameObject.tag == "Criple")
        {
            panicking = true;
            moveSpeed /= 2;
        }
        if (other.gameObject.tag == "Panic")
        {
            panicBox.gameObject.SetActive(true);
            panicking = true;
        }
    }

    /// <summary>
    /// Receives the location they subscribed for, then moves to the target
    /// continuing the rest of the AI function.
    /// </summary>
    /// <param name="pos"></param>
    internal void ReceiveSub(Vector3 pos)
    {

        returnedSub = true;
        subscribeVector = pos;
        myAgent.SetDestination(subscribeVector);
    }

}
