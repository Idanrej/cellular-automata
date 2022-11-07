
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour
{
    public enum State
    {
        Land,
        Sea,
        Iceberg,
        Forest,
        City

    }
    public enum WindDirection
    {
        North,
        South,
        East,
        West,
        None
    }
    private State GetState()
    {
        int rnd = Random.Range(0, 5);
        switch (rnd)
        {
            case 0:
                return State.Land;
            case 1:
                return State.Sea;
            case 2:
                return State.Iceberg;
            case 3: 
                return State.Forest;
            case 4:
                return State.City;
            default:
                return State.City;
        }
    }
    public WindDirection GetWindState()
    {
        int rnd = Random.Range(0, 5);
        switch (rnd)
        {
            case 0:
                return WindDirection.North;
            case 1:
                return WindDirection.South;
            case 2:
                return WindDirection.East;
            case 3:
                return WindDirection.West;
            case 4:
                return WindDirection.None;
            default:
                return WindDirection.West;
        }
    }
    public bool isAlive = false;
    public int numNeighbors = 0;
    public State state;
    public int temperature;
    public int airPolotion;
    public bool cloud;
    public WindDirection wind;


    private void Start()
    {
        state = GetState();
        wind = GetWindState();
        temperature = 25;
        airPolotion = 0;
        cloud = false;
        
    }

    public void SetAlive(bool alive)
    {
        isAlive = alive;
        if (alive)
        {
            //GetComponent<SpriteRenderer>().enabled = true;
            if(this.state == State.Forest)
                GetComponent<SpriteRenderer>().color = Color.green;
            if (this.state == State.Iceberg)
                GetComponent<SpriteRenderer>().color = Color.cyan;
            if (this.state == State.City)
                GetComponent<SpriteRenderer>().color = Color.gray;
            if (this.state == State.Land)
                GetComponent<SpriteRenderer>().color = Color.yellow;
            if (this.state == State.Sea)
                GetComponent<SpriteRenderer>().color = Color.blue;

        }
        else
        {
            //GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<SpriteRenderer>().color = Color.white;
        }

    }
}
