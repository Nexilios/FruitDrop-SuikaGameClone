using UnityEngine;

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
    }

    private void Start()
    {
        ApplyFruitData();
        var dropper = transform.parent.GetComponent<DropperController>();
        if (dropper)
        {
            dropper.SetFruitAnchorPosition(gameObject);
            return;
        }
        fruitRigidBody2D.simulated = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Fruit")) return;
        
        var otherFruit = other.gameObject.GetComponent<FruitScript>();

        if (otherFruit != null && otherFruit.fruitData.fruitName == fruitData.fruitName &&
            gameObject.GetInstanceID() < otherFruit.GetInstanceID())
        {
            FruitManager.instance.MergeFruit(gameObject, other.gameObject);
        }
    }

    public void Drop()
    {
        transform.SetParent(FruitManager.instance.fruitFolder ? FruitManager.instance.fruitFolder.transform : null);
        fruitRigidBody2D.simulated = true;
    }
}
