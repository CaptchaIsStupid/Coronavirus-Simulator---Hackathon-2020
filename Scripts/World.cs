using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class World : MonoBehaviour
{
    public int daycount;

    public WindowGraph windowgraph;

    public int currentPopulationLoaded;

    public int nameNumber;

    int numOfChildren;
    public Material[] materials;
    public Renderer rend;
    public Human human;
    public GameObject person;
    public Human[] population;
    public House housePrefab;
    public move[] moves;
    public World prefab;
    public int total,slotNum;
    public int starter;
    public int housePosition;
    public SpawnHumans spawner;
    public GameObject[] objectPopulation;
    public House[] neighborhood;
    public House[] houseScripts;
    public int houseCounter;    //for counting how many human scripts have been placed in houses
    public GameObject[] spawnLocations;
    public DataStorage data;
    public int decider,p,numInfected,crazy;

    public void Day()
    {
        Vector2 newGraphPoint = new Vector2(daycount * 50,numInfected);

        windowgraph.createCircle(newGraphPoint);

        daycount++;

        changeMaterial();

        for (int i = 0; i < population.Length; i++)
        {
            population[i].mostHungry = 0;
            population[i].GoOutside(this);

            if(!population[i].wentOutside && !population[i].wentShopping)
            {
                population[i].boredom += 10;
            }

            if (neighborhood[population[i].houseSlot].someoneShopping)
            {
                for(int c = 0; c < neighborhood[population[i].houseSlot].family.Length; c++)
                {
                    neighborhood[population[i].houseSlot].family[c].hunger = 0;
                    neighborhood[population[i].houseSlot].someoneShopping = false;
                }
            }

            if(population[i].boredom >= 50 && population[i].coop > 4)
            {
                population[i].coop -= 4;
            }
            else if(population[i].boredom <= 20)
            {
                population[i].coop += 9;
            }

            if (population[i].personalityType.Equals("Extroverted"))
            {
                population[i].boredom += 5;
            }

            if(population[i].boredom >= 10)
            {
                population[i].boredom -= population[i].discipline;
            }

            population[i].hunger += 30;

            if (population[i].infected)
            {
                population[i].gestationProgress++;
            }
        }
    }
    
    private void Start()
    {
        daycount = 0;
        housePosition = 0;
        prefab.slotNum = 0;
        starter = 0;
        slotNum = 0;
        population = new Human[data.population];    //for storing scripts
        moves = new move[data.population];
        houseScripts = new House[neighborhood.Length];
        numInfected = data.infected;
        objectPopulation = new GameObject[data.population]; //for storing gameobjects
        p = 0;
        crazy = 0;
        houseCounter = 0;

        for(int i = 0; i < neighborhood.Length; i++)
        {
            neighborhood[i].slotNum = i;
            neighborhood[i].family = new Human[5];
            neighborhood[i].someoneShopping = false;

            for (int c = 0; c < neighborhood[i].family.Length; c++)
            {
                if (p >= population.Length)
                {
                    break;
                }

                if (houseCounter >= population.Length)
                {
                    break;
                }

                objectPopulation[p] = Instantiate(person);  //adds a gameobject clone to the array
                moves[p] = objectPopulation[p].GetComponent<move>();
                moves[p].slotNum = 1;
                objectPopulation[p].name = "Human" + ' ' + nameNumber;
                nameNumber++;
                population[p] = new Human();    //adds a script to the array
                population[p].houseSlot = i;
                population[p].Slot = p;
                population[p].Move = moves[p];
                neighborhood[i].family[housePosition] = population[p];
                p++;
                slotNum++;
                prefab.slotNum++;

                if (housePosition < 4)
                {
                    housePosition++;
                    houseCounter++;
                }
                else
                {
                    housePosition = 0;
                    houseCounter++;
                }
            }
        }

        spawner.Spawn();

        for (int f = 0; f < numInfected; f++)
        {
            Debug.Log("" + numInfected);
            crazy = Random.Range(0, 5); //picks a random person in the house to infect
            neighborhood[f].family[crazy].infected = true;
        }

        Day();
    }

    void changeMaterial()
    {
        

        for (int i = 0; i < objectPopulation.Length; i++)
        {
            numOfChildren = objectPopulation[i].transform.childCount;
            if (population[i].gestationProgress >= population[i].gestationPeriod)
            {
                for(int c = 0; c < numOfChildren; c++)
                {
                    GameObject child = objectPopulation[i].transform.GetChild(c).gameObject;
                    rend = child.GetComponent<Renderer>();
                    rend.sharedMaterial = materials[2];
                }
            }
            
            else if (population[i].gestationProgress < population[i].gestationPeriod && population[i].infected)
            {
                for (int c = 0; c < numOfChildren; c++)
                {
                    GameObject child = objectPopulation[i].transform.GetChild(c).gameObject;
                    rend = child.GetComponent<Renderer>();
                    rend.sharedMaterial = materials[1];
                }
            }

            else if (population[i].infected == false)
            {
                for (int c = 0; c < numOfChildren; c++)
                {
                    GameObject child = objectPopulation[i].transform.GetChild(c).gameObject;
                    rend = child.GetComponent<Renderer>();
                    rend.sharedMaterial = materials[0];
                }
            }
        }
     }
    

    // Start is called before the first frame update
    void Update()
    {
        
    }
}
