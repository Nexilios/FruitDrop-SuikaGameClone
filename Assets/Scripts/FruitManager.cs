using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
    public static FruitManager Instance;
    public GameObject[] fruits;

    public int maxFruitQueueSize = 3;
    private Queue<int> _fruitQueue;
    
    private FruitData.FruitNames _fruitNames;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        InitializeFruitsQueue();
    }

    private void AddNewFruitToQueue()
    {
        if (_fruitQueue.Count >= maxFruitQueueSize) return;
        
        int newFruitIndex = Random.Range(0, fruits.Length);
        _fruitQueue.Enqueue(newFruitIndex);
    }
    
    private void InitializeFruitsQueue()
    {
        if (fruits.Length <= 0) return;
        
        _fruitQueue = new Queue<int>();

        for (int i = 0; i < maxFruitQueueSize; i++)
        {
            AddNewFruitToQueue();
        }
    }

    public int GetNextFruitIndex()
    {
        int nextFruitIndex = _fruitQueue.Dequeue();
        
        AddNewFruitToQueue();
        
        return nextFruitIndex;
    }
    
    public void MergeFruit(GameObject fruit1, GameObject fruit2) 
    {
	    Debug.Log("Merging "  + fruit1.name + " and " + fruit2.name);
    }
}
