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
    
    
}
