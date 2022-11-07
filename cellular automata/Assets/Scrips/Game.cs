using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    
    Cell[,] grid;
    int vertical, horizontal, cols, rows;
    public float updateRate = 0.1f;
    private float timer = 0;
    // Start is called before the first frame update
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
                grid[x, y].SetAlive(RandomAlive());
            }
        }
    }
    int GetLivingNeighbours(int x, int y)
    {
        int count = 0;

        //
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                
                int column = (x + i + cols) % cols;
                int row = (y + j + rows) % rows;

                //If the cell is alive add 1, if it's dead the state is 0 so nothing happens
                if(grid[column, row].isAlive)
                    count += 1;
            }
        }
        //Remove the current cell from the count
        if(grid[x, y].isAlive)
            count -= 1;

        return count;
    }

    bool RandomAlive()
    {
        int rnd = Random.Range(0, 100);
        
        if(rnd > 75)
        {
            return true;
        }
        return false;
    }
    void gameLogic()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                //Rules
                //Live cell
                if(grid[x, y].isAlive)
                {
                    if (grid[x, y].numNeighbors != 2 &&  grid[x, y].numNeighbors != 3)
                    {
                        grid[x, y].SetAlive(false);

                    }
                }
                //Dead cell
                else
                {
                    if (grid[x, y].numNeighbors == 3)
                    {
                        grid[x, y].SetAlive(true);

                    }
                }
                grid[x, y].numNeighbors = GetLivingNeighbours(x, y);


            }
        }
    }
    void CountNeighbours()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {

                grid[x, y].numNeighbors = GetLivingNeighbours(x, y);


            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(timer >= updateRate)
        {
            timer = 0;
            CountNeighbours();
            gameLogic();
        }
        else
        {
            timer += Time.deltaTime;
        }
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    gameLogic();
        //}

    }
    
}
