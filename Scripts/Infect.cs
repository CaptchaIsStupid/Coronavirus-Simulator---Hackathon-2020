using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infect : MonoBehaviour
{
    public World world;
    public move me;
    public move guy;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Person") && world.population[me.slotNum].numInfected < world.population[me.slotNum].maxInfect)
        {
            Debug.Log("infected");
            guy = other.GetComponent<move>();
            world.population[guy.slotNum].infected = true;
            world.population[guy.slotNum].numInfected++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        world = GameObject.Find("World").GetComponent<World>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
