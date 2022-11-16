
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

    private void Awake()
    {
        state = GetState();
        wind = GetWindState();
        temperature = 25;
        airPolotion = 0;
        cloud = false;
        
    }
    public bool GetRain()
    {
        int rnd = Random.Range(0, 10);
        if (rnd > 7)
        {
            return true;
        }
        return false;
    }
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
    public void SetAlive()
    {

        this.GetCloud();
        if (this.state == State.Forest)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
            if(init)
            {
                temperature = 15;
                init = false;
            }
            
        }
            
        if (this.state == State.Iceberg)
        {
            GetComponent<SpriteRenderer>().color = Color.cyan;
            if (init)
            {
                temperature = -20;
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
                temperature = 20;
                airPolotion = 31;
                init = false;
            }
            
        }
            
        if (this.state == State.Land)
        {
            GetComponent<SpriteRenderer>().color = Color.yellow;
            if (init)
            {
                temperature = 10;
                init = false;
            }
        }
            
        if (this.state == State.Sea)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
            if (init)
            {
                temperature = 40;
                init = false;
            }
        }
               

    }
}
