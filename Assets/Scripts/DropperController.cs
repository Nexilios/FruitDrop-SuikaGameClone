using UnityEngine;
using UnityEngine.InputSystem;

public class DropperController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float leftLimit = -2.6f;
    public float rightLimit = 2.6f;
    public float moveSpeed = 2f;
    
    [Header("Prerequisites")]
    public InputActionAsset inputActions;
    
    [SerializeField] private float fruitPosOffset = 0.1f;
    [SerializeField] private GameObject currentFruit;
    
    private InputActionMap _inputMap;
    private InputAction _moveAction;
    private InputAction _interactAction;

    private float _horizontalInput;
    
    private void Awake()
    {
        _inputMap = inputActions.FindActionMap("Player");
        _moveAction = _inputMap.FindAction("Move");
        _interactAction = _inputMap.FindAction("Interact");
    }
    
    private void OnEnable()
    {
        _inputMap.Enable();
    }

    private void OnDisable()
    {
        _inputMap.Disable();
    }
    
    private void Update()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        _horizontalInput = moveInput.x;
        
        if (_interactAction.WasPressedThisFrame())
        {
            DropFruit();
        }
    }

    private void Start()
    {
        if (FruitManager.Instance != null)
        {
            
        }
    }

    private void FixedUpdate()
    {
        float moveAmount = _horizontalInput * moveSpeed * Time.fixedDeltaTime;
        
        Vector3 currentPos = transform.position;
        
        float newX = currentPos.x + moveAmount;
        
        newX = Mathf.Clamp(newX, leftLimit, rightLimit);
        
        transform.position = new Vector3(newX, currentPos.y, currentPos.z);
    }

    private void DropFruit()
    {
        Debug.Log("Dropping fruit at position: " + transform.position.x);
    }

    private Vector3 GetFruitAnchorPosition(GameObject fruit)
    {
        Collider2D fruitCollider = fruit.GetComponent<Collider2D>();
        Collider2D dropperCollider = GetComponent<Collider2D>();
        
        if (fruitCollider != null && dropperCollider != null)
        {
            // Get the bounds
            Bounds fruitBounds = fruitCollider.bounds;
            Bounds dropperBounds = dropperCollider.bounds;
            
            // Position fruit so its top edge is just below the dropper's bottom edge
            float targetY = dropperBounds.min.y - fruitPosOffset - (fruitBounds.size.y * 0.5f);
            
            return new Vector3(
                transform.position.x, 
                targetY, 
                fruit.transform.position.z
            );
        }

        return new Vector3(0, 0, 0);
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw movement limits as red lines
        Gizmos.color = Color.red;
        Vector3 leftPoint = new Vector3(leftLimit, transform.position.y, transform.position.z);
        Vector3 rightPoint = new Vector3(rightLimit, transform.position.y, transform.position.z);
        
        Gizmos.DrawLine(leftPoint + Vector3.up * 0.5f, leftPoint + Vector3.down * 0.5f);
        Gizmos.DrawLine(rightPoint + Vector3.up * 0.5f, rightPoint + Vector3.down * 0.5f);
        
        // Draw movement range
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(leftPoint, rightPoint);
    }
}
