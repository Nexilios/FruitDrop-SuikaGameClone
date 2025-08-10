using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitManager : MonoBehaviour
{
    public static FruitManager instance;
    public int maxFruitQueueSize = 3;
    public GameObject fruitPrefab;
    public GameObject fruitFolder;
    
    private int _maxGameStage;
    
    [SerializeField] private List<WeightedFruitsData> weightedFruits;
    [SerializeField] private int currentGameStage;
    
    [Header("Debug")]
    [SerializeField] private List<int> nextFruitList;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        if (fruitFolder == null) fruitFolder = GameObject.FindGameObjectWithTag("FruitFolder");
    }

    private void Start()
    {
        InitializeFruitList();

        _maxGameStage = weightedFruits.Count - 1;
    }
    
    private int? GetRandomFruitIndex(WeightedFruitsData array)
    {
        float totalWeight = array.totalWeight;
        
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
    
    private FruitData GetNextFruitDataFromQueue()
    {
        if (nextFruitList.Count <= 0 || weightedFruits.Count <= 0) return null;
        
        FruitData nextFruitData = weightedFruits[currentGameStage].fruits[nextFruitList.First()].fruitData;
        PopFruitQueue();
        
        return nextFruitData;
    }

    private FruitData GetUpgradeFruitData(FruitData.FruitNames upgradeFruitName)
    {
        var newFruitData = weightedFruits[currentGameStage].fruits.FirstOrDefault(wf => wf.fruitData.fruitName == upgradeFruitName)?.fruitData;
        
        return newFruitData ? newFruitData : null;
    }

    private FruitData GetFruitData(bool isMergeSpawn, FruitData.FruitNames fruitName)
    {
        return isMergeSpawn ? GetUpgradeFruitData(fruitName): GetNextFruitDataFromQueue();
    }
    
    public GameObject InstantiateFruit(Transform parent, Vector3? pos = null, bool isMergeSpawn = false, FruitData.FruitNames mergingFruitName = default)
    {
        var newFruitData = GetFruitData(isMergeSpawn, mergingFruitName);
        
        var spawnPos = pos ?? parent.position;
        
        var newFruit = pos.Equals(Vector3.zero) ? Instantiate(fruitPrefab, parent) : Instantiate(fruitPrefab, spawnPos, Quaternion.identity, parent);
        
        newFruit.GetComponent<FruitScript>().SetFruitData(newFruitData);

        return newFruit;
    }
    
    public void MergeFruit(GameObject fruit1, GameObject fruit2)
    {
        var newPoint = Vector3.Lerp(fruit1.transform.position, fruit2.transform.position, 0.6f);
        var fruit1Script = fruit1.GetComponent<FruitScript>();
        
        var upgradedFruitName = fruit1Script.GetFruitData().nextFruitTierName;
        if (upgradedFruitName != FruitData.FruitNames.Watermelon)
        {
            InstantiateFruit(fruitFolder.transform, newPoint, true, upgradedFruitName);
        }

        if (GameManager.instance)
        {
            int currScore = GameManager.instance.AddScore(fruit1Script.GetFruitData().mergeScoreReward);
            if (currScore >= weightedFruits[currentGameStage].upgradeScoreRequirement && currentGameStage < _maxGameStage)
            {
                currentGameStage += System.Math.Clamp(currentGameStage, 0, _maxGameStage);
            }
        }
        
        Destroy(fruit1, 0.01f);
        Destroy(fruit2, 0.01f);
    }
}
