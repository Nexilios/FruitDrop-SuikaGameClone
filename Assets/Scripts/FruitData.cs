using UnityEngine;

[CreateAssetMenu(fileName = "FruitData", menuName = "Scriptable Objects/FruitData")]
public class FruitData : ScriptableObject
{
    public enum FruitNames
    {
        Cherry,
        Strawberry,
        Mandarin,
        Orange,
        Apple,
        Pear,
        Peach,
		Pineapple,
        Melon,
        Watermelon
    }
    
    [Header("General Property")]
    public Vector3 scale;
    public Sprite sprite;
    
    [Header("Gameplay Property")]
    public float mergeScoreReward;
    public FruitNames fruitName;
    public FruitNames nextFruitTierName;

    [Header("CircleCollider2D Property")]
    public Vector2 colliderOffset;
    public float colliderRadius;
    public PhysicsMaterial2D physicsMaterial;
}

