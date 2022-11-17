
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cell : MonoBehaviour
{
    //Enum for all posible states
    public enum State
    {
        Land,
        Sea,
        Iceberg,
        Forest,
        City

    }
    //Enum for all wind direction options.
    public enum WindDirection
    {
        North,
        South,
        East,
        West,
        None
    }
    // Choose a random state foe the cell.
    private State GetState()
    {
        int rnd = Random.Range(0, 6);
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
    //Choose random direction for the cell wind.
    public WindDirection GetWindState()
    {
        int rnd = Random.Range(0, 10);
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
                return WindDirection.None;
        }
    }
    
    public float windEffect = 0;
    public int rainDays;
    public State state;
    public float temperature;
    public float airPolotion;
    public bool cloud;
    public WindDirection wind;
    private bool init = true;
    float forestBaseTemp = 15;
    float seaBaseTemp = 22;
    float cityBaseTemp = 20;
    float cityBasePolotion = 20;
    float icebergBaseTemp = -20;

    float landBaseTemp = 15;

    //Init the cell.
    private void Awake()
    {
        state = GetState();
        wind = GetWindState();
        temperature = 25;
        airPolotion = 0;
        cloud = false;
        
    }

    //Random rain 
    public bool GetRain()
    {
        int rnd = Random.Range(0, 10);
        if (rnd > 7)
        {
            return true;
        }
        return false;
    }

    // Random cloud
    public void GetCloud()
    {
        int rnd = Random.Range(0, 10);
        if(rnd > 7)
        {
            this.cloud = true;
        }
        else
        {
            this.cloud = false;
        }
    }
    // Set the cell alive - give it its color and initial temp, polotion.
    // if the state has changed change color.
    public void SetAlive()
    {

        this.GetCloud();
        if (this.state == State.Forest)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            if(init)
            {
                temperature = forestBaseTemp;
                init = false;
            }
            
        }
            
        if (this.state == State.Iceberg)
        {
            GetComponent<SpriteRenderer>().color = Color.cyan;
            if (init)
            {
                temperature = icebergBaseTemp;
                init = false;
            }
        }
            
        if (this.state == State.City)
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
            if(this.temperature > 80)
            {
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (this.temperature < 0)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (init)
            {
                temperature = cityBaseTemp;
                airPolotion = cityBasePolotion;
                init = false;
            }
            
        }
            
        if (this.state == State.Land)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            if (init)
            {
                temperature = landBaseTemp;
                init = false;
            }
        }
            
        if (this.state == State.Sea)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            if (init)
            {
                temperature = seaBaseTemp;
                init = false;
            }
        }
               

    }
}
