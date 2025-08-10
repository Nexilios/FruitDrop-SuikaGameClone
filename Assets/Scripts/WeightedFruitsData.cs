using System.Linq;
using UnityEngine;

[System.Serializable]
public class WeightedFruit
{
    public FruitData fruitData;
    public float weight;
}

[CreateAssetMenu(fileName = "WeightedFruitsData", menuName = "Scriptable Objects/WeightedFruitsData")]
public class WeightedFruitsData : ScriptableObject
{
    public WeightedFruit[] fruits;
    public float totalWeight;
    public int upgradeScoreRequirement;
    
    private void OnValidate()
    {
        totalWeight = fruits.Sum(f => f.weight);
    }
}