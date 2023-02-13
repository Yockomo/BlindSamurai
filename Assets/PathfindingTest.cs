using System;
using Pathfinding;
using UnityEngine;
using Zenject;

public class PathfindingTest : MonoBehaviour
{
    private AIDestinationSetter destinationSetter;
    private Transform target;
    [Inject]
    private void Construct(Transform playerTransform)
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
        target = playerTransform;
    }

    private void Start()
    {
        destinationSetter.target = target;
    }
}
