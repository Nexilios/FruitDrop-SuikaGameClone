using System;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D),typeof(CircleCollider2D))]
public class FruitScript : MonoBehaviour
{
    [Header("Debugging")]
    [SerializeField] private FruitData fruitData;
    [SerializeField] private SpriteRenderer fruitRenderer;
    [SerializeField] private Rigidbody2D fruitRigidBody2D;
    [SerializeField] private CircleCollider2D fruitCollider;
    
    public void SetFruitData(FruitData data)
    {
        fruitData = data;
    }

    private void ApplyFruitData()
    {
        if (fruitData == null) return;
        
        fruitRenderer.sprite = fruitData.sprite;
        
        gameObject.transform.localScale = fruitData.scale;
        
        fruitCollider.offset = fruitData.colliderOffset;
        fruitCollider.radius = fruitData.colliderRadius;

        fruitRigidBody2D.sharedMaterial = fruitData.physicsMaterial;
    }

    private void Awake()
    {
        if (fruitRenderer == null) fruitRenderer = GetComponent<SpriteRenderer>();
        if (fruitRigidBody2D == null) fruitRigidBody2D = GetComponent<Rigidbody2D>();
        if (fruitCollider == null) fruitCollider = GetComponent<CircleCollider2D>();
        
        ApplyFruitData();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Fruit")) return;
        
        var otherFruit = other.gameObject.GetComponent<FruitScript>();

        if (otherFruit != null && otherFruit.fruitData.fruitName == fruitData.fruitName &&
            gameObject.GetInstanceID() < otherFruit.GetInstanceID())
        {
            FruitManager.Instance.MergeFruit(gameObject, other.gameObject);
        }
    }
}
