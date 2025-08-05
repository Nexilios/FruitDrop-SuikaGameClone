using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
    public static FruitManager instance;
    public int maxFruitQueueSize = 3;
    
    [SerializeField] private List<WeightedFruitsData> weightedFruits;
    [SerializeField] private List<int> nextFruitList;
    [SerializeField] private int currentGameStage;
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {
        InitializeFruitList();
    }
    private int? GetRandomFruitIndex(WeightedFruitsData array)
    {
        float totalWeight = array.fruits.Sum(fruit => fruit.weight);
        
        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        
        for (int i = 0; i < array.fruits.Length; i++)
        {
            currentWeight += array.fruits[i].weight;
            if (randomValue <= currentWeight)
                return i;
        }

        return null;
    }

    private void InitializeFruitList()
    {
        if (weightedFruits.Count <= 0) return;
        
        nextFruitList.Clear();

        while (nextFruitList.Count < maxFruitQueueSize)
        {
            int nextIndex = GetRandomFruitIndex(weightedFruits[0]) ?? -1;
            
            if (nextIndex >= 0)
            {
                nextFruitList.Add(nextIndex);
            }
        }
    }

    private void PopFruitQueue()
    {
        nextFruitList.RemoveAt(0);
        
        int nextIndex = GetRandomFruitIndex(weightedFruits[0]) ?? -1;
            
        if (nextIndex >= 0)
        {
            nextFruitList.Add(nextIndex);
        }
    }
    
    public FruitData GetNextFruitData()
    {
        if (nextFruitList.Count <= 0 || weightedFruits.Count <= 0) return null;
        
        FruitData nextFruitData = weightedFruits[currentGameStage].fruits[nextFruitList.First()].fruitData;
        PopFruitQueue();
        
        return nextFruitData;
    }

    public void MergeFruit(GameObject fruit1, GameObject fruit2)
    {
        
    }
}
