using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game : MonoBehaviour
{
    //Grid data
    int vertical, horizontal, cols, rows;
    Cell[,] grid;
    //Avarages data
    public GameObject graph;
    WindowGraph window;
    public float tempAvg = 0;
    public float polotionAvg = 0;
    public float tempStandardDeviation = 0;
    public float polotionStandardDeviation = 0;

    //Graph Data
    public List<float> tempList = new List<float>();
    public List<float> polotionList = new List<float>();
    public int graphSamples = 10;

    //Start env values
    public float cityPolotionRate = 0.5f;
    public float forestPolotionCleanRate = 0.1f;
    public float rainPolotionCleanRate = 0.1f;
    public float rainTempDecRate = 0.1f;
    public float polotiondevider = 1000;
    
    public float updateRate = 0.1f;
    private float timer = 0;

    //Text data
    public TextMeshProUGUI daysText;
    public TextMeshProUGUI tempAvgText;
    public TextMeshProUGUI tempStandardDeviationText;
    public TextMeshProUGUI polotionAvgText;
    public TextMeshProUGUI polotionStandardDeviationText;
    public int days;
    


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
        window = gameObject.GetComponent<WindowGraph>();
        
        Spawn();

    }
    
    //Spawn the cells with a Random starting Env.
    public void Spawn()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                
                Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector3(x - (horizontal - 0.5f), y - (vertical - 0.5f)), Quaternion.identity) as Cell;
                grid[x, y] = cell;
                grid[x, y].SetAlive();
            }
        }
    }
    //Get some of the neighbours cell polotion if the wind is in your diriection.
    float GetNeighboursPolotion(int x, int y)
    {
        
        float count = 0;
        if(x > 0)
        {
            if (grid[x-1, y].wind == Cell.WindDirection.West && grid[x - 1, y].airPolotion > 0)
            {

                count += grid[x - 1, y].airPolotion / polotiondevider;
            }
        }
        if (y > 0)
        {
            if (grid[x, y - 1].wind == Cell.WindDirection.North && grid[x, y - 1].airPolotion > 0)
            {
                count += grid[x, y - 1].airPolotion / polotiondevider;
            }
        }
        if (x < cols - 1)
        {
            if (grid[x + 1, y ].wind == Cell.WindDirection.East && grid[x + 1, y].airPolotion > 0)
            {
                count += grid[x + 1, y].airPolotion / polotiondevider;
            }
        }

        if (y < rows - 1)
        {
            if (grid[x, y + 1].wind == Cell.WindDirection.South && grid[x, y + 1].airPolotion > 0)
            {
                count += grid[x, y + 1].airPolotion / polotiondevider;
            }
        }

        
        return count;
    }

    
    // Set up all the conditions for the env,
    // change temp if needed, change env if needed.
    // 


    void GameLogic()
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
                
                grid[x, y].airPolotion += grid[x, y].windEffect;
                if(grid[x, y].airPolotion > 30)
                {
                    grid[x, y].temperature += (grid[x, y].airPolotion / 1000);
                }
                else
                {
                    grid[x, y].temperature -= 0.001f;
                }
                
                grid[x, y].wind = grid[x, y].GetWindState();
                
                if (grid[x, y].state == Cell.State.City)
                {
                    grid[x, y].airPolotion += cityPolotionRate;
                    
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
                //temp and polotion top amount.
                if (grid[x, y].airPolotion < 0)
                    grid[x, y].airPolotion = 0;
                if(grid[x, y].airPolotion > 1000)
                {
                    grid[x, y].airPolotion = 1000;
                }
                if (grid[x, y].temperature < -80)
                    grid[x, y].temperature = -80;
                if (grid[x, y].temperature > 140)
                {
                    grid[x, y].temperature = 140;
                }
                tempAvg += grid[x, y].temperature;
                polotionAvg += grid[x, y].airPolotion;
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
    private float StandardDeviation(bool temp, float Avg)
    {

        float avgTemp = (Avg / grid.Length);
        float totalTempSum = 0;

        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                if (temp)
                {
                    totalTempSum += Mathf.Pow((grid[x, y].temperature - avgTemp), 2);
                }
                else
                {
                    totalTempSum += Mathf.Pow((grid[x, y].airPolotion - avgTemp), 2);
                }

            }
        }
        totalTempSum = totalTempSum / grid.Length;

        return Mathf.Sqrt(totalTempSum);
    }

    // Print the data to the screen.
    void DataPrinter()
    {
        daysText.text = "day: " + days;
        tempAvgText.text = "avg temp: " + (tempAvg / grid.Length);
        if(days % graphSamples == 0)
        {
            tempList.Add((tempAvg / grid.Length));
            polotionList.Add(polotionAvg / grid.Length);
        }
        tempStandardDeviation = StandardDeviation(true, tempAvg);
        tempStandardDeviationText.text = "Temp Standard Deviation = " + tempStandardDeviation;
        polotionAvgText.text = "avg polotion: " + (polotionAvg / grid.Length);
        polotionStandardDeviation = StandardDeviation(false, polotionAvg);
        polotionStandardDeviationText.text = "polotion Standard Deviation = " + polotionStandardDeviation;
        polotionAvg = 0;
        tempAvg = 0;
        if (days == 355)
        {
            graph.SetActive(true);
        }
        days += 1;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (timer >= updateRate && days < 366)
        {

            timer = 0;
            GetWindNeighbors();
            GameLogic();
            DataPrinter();
        }
        else
        {
            timer += Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {

            
            GetWindNeighbors();
            GameLogic();
        }

    }

}
