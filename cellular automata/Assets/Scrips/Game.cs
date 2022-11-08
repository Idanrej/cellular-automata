using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    
    Cell[,] grid;
    public float cityPolotionRate = 0.5f;
    public float forestPolotionCleanRate = 0.1f;
    public float rainPolotionCleanRate = 0.1f;
    public float rainTempDecRate = 0.1f;
    int vertical, horizontal, cols, rows;
    public float updateRate = 0.1f;
    private float timer = 0;
    public TextMeshProUGUI daysText;
    private int days;
    
    private void Awake()
    {
        vertical = (int)Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.width);
        rows = vertical * 2;
        cols = horizontal * 2;
        grid = new Cell[cols, rows];
    }
    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }
    public void Spawn()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                //Gameobject newplayer = Instantiate(Resources.Load("Player", typeof(GameObject))) as Gameobject;
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector3(x - (horizontal - 0.5f), y - (vertical - 0.5f)), Quaternion.identity) as Cell;
                grid[x, y] = cell;
                grid[x, y].SetAlive();
            }
        }
    }
    float GetNeighboursPolotion(int x, int y)
    {
        float count = 0;
        if(x > 0)
        {
            if (grid[x-1, y].wind == Cell.WindDirection.West && grid[x - 1, y].airPolotion > 0)
            {

                count += grid[x - 1, y].airPolotion / 1000;
            }
        }
        if (y > 0)
        {
            if (grid[x, y - 1].wind == Cell.WindDirection.North && grid[x, y - 1].airPolotion > 0)
            {
                count += grid[x, y - 1].airPolotion / 1000;
            }
        }
        if (x < cols - 1)
        {
            if (grid[x + 1, y ].wind == Cell.WindDirection.East && grid[x + 1, y].airPolotion > 0)
            {
                count += grid[x + 1, y].airPolotion / 1000;
            }
        }

        if (y < rows - 1)
        {
            if (grid[x, y + 1].wind == Cell.WindDirection.South && grid[x, y + 1].airPolotion > 0)
            {
                count += grid[x, y + 1].airPolotion / 1000;
            }
        }

        
        return count;
    }

    
    
    void gameLogic()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                //check if any clouds are on the spot, if there are, check for rain.
                if(grid[x, y].cloud)
                {
                    if(grid[x, y].GetRain())
                    {
                        grid[x, y].rainDays += 1;
                        grid[x, y].temperature -= rainTempDecRate;
                        grid[x, y].airPolotion -= rainPolotionCleanRate;
                    }
                    
                }
                //if (grid[x, y].wind == Cell.WindDirection.None && grid[x, y].state != Cell.State.City)
                //    grid[x, y].windEffect -= 1;
                grid[x, y].airPolotion += grid[x, y].windEffect;
                if(grid[x, y].airPolotion > 30)
                {
                    grid[x, y].temperature += (grid[x, y].airPolotion / 1000);
                }
                else
                {
                    grid[x, y].temperature -= 0.1f;
                }
                
                grid[x, y].wind = grid[x, y].GetWindState();
                
                if (grid[x, y].state == Cell.State.City)
                {
                    grid[x, y].airPolotion += cityPolotionRate;
                    //if (grid[x, y].temperature > 70)
                    //{
                    //    grid[x, y].state = Cell.State.Land;
                    //}
                }
                if (grid[x, y].state == Cell.State.Forest)
                {
                    grid[x, y].airPolotion -= forestPolotionCleanRate;
                    if (grid[x, y].rainDays > 300)
                    {
                        grid[x, y].state = Cell.State.Sea;

                    }
                    if (grid[x, y].temperature > 50)
                    {
                        grid[x, y].state = Cell.State.Land;
                    }
                }
                if (grid[x, y].state == Cell.State.Iceberg)
                {
                    if (grid[x, y].temperature > 5)
                    {
                        grid[x, y].state = Cell.State.Sea;
                    }
                }
                if (grid[x, y].state == Cell.State.Land)
                {
                    //if(grid[x, y].rainDays > 20)
                    //{
                    //    grid[x, y].state = Cell.State.Forest;
                    //}
                    if (grid[x, y].temperature < 0)
                    {
                        grid[x, y].state = Cell.State.Iceberg;
                    }
                }
                if (grid[x, y].state == Cell.State.Sea)
                {
                    if (grid[x, y].temperature > 130)
                    {
                        grid[x, y].state = Cell.State.Land;
                    }
                    if (grid[x, y].temperature < 0)
                    {
                        grid[x, y].state = Cell.State.Iceberg;
                    }
                }
                if (grid[x, y].airPolotion < 0)
                    grid[x, y].airPolotion = 0;
                if(grid[x, y].airPolotion > 1000)
                {
                    grid[x, y].airPolotion = 1000;
                }
                if (grid[x, y].temperature < -80)
                    grid[x, y].temperature = -80;
                if (grid[x, y].temperature > 120)
                {
                    grid[x, y].temperature = 120;
                }
                grid[x, y].SetAlive();
            }
        }
    }
    void GetWindNeighbors()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {

                grid[x, y].windEffect = GetNeighboursPolotion(x, y);


            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (timer >= updateRate)
        {
            timer = 0;
            GetWindNeighbors();
            gameLogic();
            daysText.text = "day: " + days;
            days += 1;
        }
        else
        {
            timer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            GetWindNeighbors();
            gameLogic();
        }

    }

}
