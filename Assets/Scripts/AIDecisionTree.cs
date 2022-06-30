using UnityEngine;
using LibGameAI.DecisionTrees;

/// <summary>
/// Class responsible for the agent's decision tree
/// </summary>
public class AIDecisionTree : MonoBehaviour
{
    [SerializeField]
    private int randomDecisionDurationInFrames = 240;

    // References the player
    private Pawn_Manager pawn;


    // The root of the decision tree
    private IDecisionTreeNode root;

    // Create the decision tree
    protected void Start()
    {
        pawn = GetComponent<Pawn_Manager>();

        // Generates the leaf actions
        IDecisionTreeNode fleeToExit = new ActionNode(pawn.FleeToExitAction);
        IDecisionTreeNode goEat = new ActionNode(pawn.GoEatAction);
        IDecisionTreeNode goToConcert = new ActionNode(pawn.GoConcertAction);
        IDecisionTreeNode goRest = new ActionNode(pawn.RestAction);
        IDecisionTreeNode nothing = new ActionNode(pawn.Nothing);


        // Create the random decision behaviour node
        RandomDecisionBehaviour rdb = new RandomDecisionBehaviour(
            () => Random.value,
            () => Time.frameCount,
            randomDecisionDurationInFrames,
            0.55f);

        //Create nodes

        IDecisionTreeNode IsTired = new DecisionNode(pawn.IsTired, goRest, goToConcert);
        IDecisionTreeNode IsHungry = new DecisionNode(pawn.IsHungry, goEat, IsTired);
        IDecisionTreeNode IsSubscribed = new DecisionNode(pawn.IsSubscribed, nothing, IsHungry);
        root = new DecisionNode(pawn.IsInPanic, fleeToExit, IsHungry);


    }

    // Run the decision tree and execute the returned action
    private void FixedUpdate()
    {
        if(!pawn.IsEating() && !pawn.IsResting() && pawn.GetGoal() != 3 && pawn.GetGoal() != 4)
        {
            ActionNode actionNode = root.MakeDecision() as ActionNode;
            actionNode.Execute();
        }
    }
}