using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowGraph : MonoBehaviour
{
    [SerializeField] Game game;
    
    private RectTransform graphContainer;
    
    [SerializeField] private Sprite circleSprite;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    float graphHight;
    float graphWidth;
    float yMax;
    float yMin;
    float xSize;
    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        graphHight = graphContainer.sizeDelta.y;
        graphWidth = graphContainer.sizeDelta.x;
        yMax = 0f;
        yMin = 0f;
        xSize = 50f;
        
        
    }
    private GameObject CreateCircle(Vector2 position, Color color)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer,false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;
        gameObject.GetComponent<Image>().color = color;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }
    private void CreateLine(Vector2 posA, Vector2 posB, Color color)
    {
        GameObject gameObject = new GameObject("line", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = color;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (posB - posA).normalized;
        float distance = Vector2.Distance(posA, posB);
        
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        
        
        rectTransform.anchoredPosition = posA + dir * distance * 0.5f;
        float angle = Vector2.Angle(dir, transform.forward);
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * 180 / Mathf.PI);

    }
    private void GetMaxVal(List<float> values)
    {
        foreach(float val in values)
        {
            if(val > yMax)
            {
                yMax = val;
            }
            if(val < yMin)
            {
                yMin = val;
            }
        }
    }
    private List<float> SetupVals(List<float> values, float avg, float StandardDeviation )
    {
        float avgTemp = game.tempAvg;
        float tempStandardDeviation = game.tempStandardDeviation;
        for (int i = 0; i < values.Count; i++)
        {
            values[i] = (values[i] - avgTemp) / tempStandardDeviation;
        }
        return values;
    }
    public void ShowGraph(List<float> values, Color color)
    {
        yMin = values[0];
        GetMaxVal(values);
        yMax = yMax + ((yMax - yMin) * 0.2f);
        yMin = yMin - ((yMax - yMin) * 0.2f);
        //yMin = 0;
        xSize = graphWidth / values.Count ;
        GameObject lastDot = null;
        for (int i = 0; i < values.Count; i++)
        {
            
            float xPos = xSize + i * xSize;
            float yPos = ((values[i] - yMin) / (yMax - yMin)) * graphHight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPos, yPos), color);
            if (lastDot != null)
            {
                CreateLine(lastDot.GetComponent<RectTransform>().anchoredPosition,
                    circleGameObject.GetComponent<RectTransform>().anchoredPosition,color);
            }
            lastDot = circleGameObject;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPos, -7f);
            labelX.GetComponent<Text>().text = i.ToString();

            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(graphContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(xPos, -7f);
            
        }
        int seperator = Mathf.RoundToInt(graphHight / values.Count);
        for (int i = 0; i <= seperator; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float noormalizedVal = i * 1f / seperator;
            labelY.anchoredPosition = new Vector2(-7f, noormalizedVal * graphHight);
            labelY.GetComponent<Text>().text = (noormalizedVal * yMax).ToString("F2");

            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(graphContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(-4f, noormalizedVal * graphHight);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(game.days == 360)
        {
            
            ShowGraph(SetupVals(game.tempList, game.tempAvg, game.tempStandardDeviation ),Color.red);
            ShowGraph(SetupVals(game.polotionList, game.polotionAvg, game.polotionStandardDeviation),Color.white);
        }
    }
}
