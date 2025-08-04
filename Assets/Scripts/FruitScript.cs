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

    private void SetFruitAnchorPosition()
    {
        var dropper = transform.parent.gameObject;
        SpriteRenderer dropperRenderer = dropper.GetComponent<SpriteRenderer>();

        if (dropperRenderer == null || fruitCollider == null) return;
        
        Bounds dropperBounds = dropperRenderer.bounds;
        
        // Get fruit radius in world space
        float fruitRadius = fruitCollider.radius * transform.lossyScale.y;
        
        // Position fruit center so the top of the circle touches dropper's bottom
        float targetY = dropperBounds.min.y - fruitRadius - dropper.GetComponent<DropperController>().fruitPosOffset;
        
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
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
        SetFruitAnchorPosition();
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
