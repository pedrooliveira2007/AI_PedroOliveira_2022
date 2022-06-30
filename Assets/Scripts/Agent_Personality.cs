using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Personality", menuName = "ScriptableObjects/Personality", order = 1)]
public class Agent_Personality : ScriptableObject
{
    public float moveSpeed = 100;
    public float maxHunger = 100;
    public float minHunger = 90;
    public float eatRate = 1;
    public float hungerRate = 1;
    public float maxFatigue = 100;
    public float minFatigue = 90;
    public Material myMaterial;
}
