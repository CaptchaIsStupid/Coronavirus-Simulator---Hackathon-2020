using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class move : MonoBehaviour
{
    public float distanceToHouse;

    public bool goingHome = false;

    public NavMeshAgent agent;
    public World world;
    public int slotNum,decider;
    public GameObject destination;

    // Update is called once per frame
    public void Start()
    {
        agent.radius = 0.15f;

        world = GameObject.Find("World").GetComponent<World>();
    }

    public void Avoidance()
    {
        agent.radius = ((world.population[slotNum].coop*3.5f) / 100f) + 0.15f;
    }

    public void GoToPark()
    {
        agent.enabled = true;

        decider = Random.Range(0, 2);

        if (decider == 0)
        {
            destination = GameObject.Find("Park Center");
        }
        else
        {
            if (decider == 1)
            {
                destination = GameObject.Find("Park Left Corner");
            }
        }

        agent.destination = destination.transform.position;

        StartCoroutine(waitThenGoHome());
    }

    public void GoToStore()
    {
        agent.enabled = true;

        destination = GameObject.Find("Grocery Store");

        agent.destination = destination.transform.position;

        StartCoroutine(waitThenGoHome());
    }

    IEnumerator waitThenGoHome()
    {
        yield return new WaitForSeconds(10);

        goingHome = true;

        if (world.population[slotNum].wentShopping)
        {
            for(int i = 0; i < world.neighborhood[world.population[slotNum].houseSlot].family.Length; i++)
            {
                world.neighborhood[world.population[slotNum].houseSlot].family[i].hunger = 0;   //makes people not hungry after shopping
            }
        }

        if (world.population[slotNum].wentOutside)
        {
            world.population[slotNum].boredom = 5;
        }

        destination = GameObject.Find("House" + (world.population[slotNum].houseSlot + 1));

        agent.destination = destination.transform.position;
    }

    void checkDistance()
    {
        distanceToHouse = Vector3.Distance(world.objectPopulation[slotNum].transform.position, destination.transform.position);
    }

    public void Update()
    {
        if (goingHome)
        {
            checkDistance();
        }

        float distanceThreshold = 0.9f;

        if (distanceToHouse < distanceThreshold && goingHome)
        {
            goingHome = false;
            world.population[slotNum].wentOutside = false;
            world.population[slotNum].wentShopping = false;

            world.objectPopulation[slotNum].transform.SetPositionAndRotation(world.spawnLocations[world.population[slotNum].houseSlot].transform.position,
                world.spawnLocations[world.population[slotNum].houseSlot].transform.rotation);
        }

        Avoidance();

        if(world.population[slotNum].wentOutside || world.population[slotNum].wentShopping)
        {
            agent.enabled = true;
        }
        else
        {
            agent.enabled = false;
        }
    }
}
