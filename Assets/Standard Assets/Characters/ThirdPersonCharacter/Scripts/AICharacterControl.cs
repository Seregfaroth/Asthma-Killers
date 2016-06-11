using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof (NavMeshAgent))]
    [RequireComponent(typeof (ThirdPersonCharacter))]
    public class AICharacterControl : MonoBehaviour
    {
        public float patrolSpeed = 2f;
        private float patrolTimer;                  // A timer for the patrolWaitTime.
        public NavMeshAgent agent;                  // the navmesh agent required for the path finding
        public ThirdPersonCharacter character;      // the character we are controlling
        public Transform target;                    // target to aim for
        public float patrolWaitTime = 1f;           // The amount of time to wait when the patrol way point is reached.
        public Transform[] patrolWayPoints;         // An array of transforms for the patrol route.
        private int wayPointIndex;                  // A counter for the way point array.
        public GUIText distanceText;
        private GameObject player;                  // Reference to the player.
        private float distance;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<NavMeshAgent>();
            character = GetComponent<ThirdPersonCharacter>();

	        agent.updateRotation = false;
	        agent.updatePosition = true;
            distance = 1.0f;
            agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindGameObjectWithTag("Player");

        }


        private void Update()
        {
            distance = CalculatePathLength(player.transform.position);
            distanceText.text = "Distance: " + distance;
            if (distance < 20)
            { 
                target = player.transform;
                Chasing();
            }
            else
            {
                target = null;
                Patrolling();
            }

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }


        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        private void Chasing()
        {
            agent.SetDestination(target.position);

            if (agent.remainingDistance > agent.stoppingDistance)
                character.Move(agent.desiredVelocity, false, false);
            else
                character.Move(Vector3.zero, false, false);
        }

        private void Patrolling()
        {
            // Set an appropriate speed for the NavMeshAgent.
            agent.speed = patrolSpeed;

            // If near the next waypoint
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                // ... increment the timer.
                patrolTimer += Time.deltaTime;

                // If the timer exceeds the wait time...
                if (patrolTimer >= patrolWaitTime)
                {
                    // ... increment the wayPointIndex.
                    if (wayPointIndex == patrolWayPoints.Length - 1)
                        wayPointIndex = 0;
                    else
                        wayPointIndex++;

                    // Reset the timer.
                    patrolTimer = 0;
                }
            }
            else
                // If not near a destination, reset the timer.
                patrolTimer = 0;

            // Set the destination to the patrolWayPoint.
            agent.destination = patrolWayPoints[wayPointIndex].position;
        }

        float CalculatePathLength(Vector3 targetPosition)
        {
            // Create a path and set it based on a target position.
            NavMeshPath path = new NavMeshPath();
            if (agent.enabled)
                agent.CalculatePath(targetPosition, path);

            // Create an array of points which is the length of the number of corners in the path + 2.
            Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

            // The first point is the enemy's position.
            allWayPoints[0] = transform.position;

            // The last point is the target position.
            allWayPoints[allWayPoints.Length - 1] = targetPosition;

            // The points inbetween are the corners of the path.
            for (int i = 0; i < path.corners.Length; i++)
            {
                allWayPoints[i + 1] = path.corners[i];
            }

            // Create a float to store the path length that is by default 0.
            float pathLength = 0;

            // Increment the path length by an amount equal to the distance between each waypoint and the next.
            for (int i = 0; i < allWayPoints.Length - 1; i++)
            {
                pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
            }

            return pathLength;
        }
    }
}
