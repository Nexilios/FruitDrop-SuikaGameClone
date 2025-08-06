using UnityEngine;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;

public class DropperController : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionAsset inputActions;
    
    [Header("Movement Settings")]
    public float leftLimit = -2.7f;
    public float rightLimit = 2.7f;
    public float moveSpeed = 2f;
    
    [Header("Fruit")]
    public float fruitPosOffset;
    
    [Header("Debug")]
    [SerializeField] private FruitScript currentFruitComp;
    [SerializeField] private SpriteRenderer dropperRenderer;
    
    private InputActionMap _inputMap;
    private InputAction _moveAction;
    private InputAction _interactAction;

    private float _horizontalInput;
    private float _currentLeftLimit;
    private float _currentRightLimit;
    
    private void OnEnable()
    {
        _inputMap.Enable();
    }
    
    private void Awake()
    {
        _inputMap = inputActions.FindActionMap("Player");
        _moveAction = _inputMap.FindAction("Move");
        _interactAction = _inputMap.FindAction("Interact");
        
        if (!dropperRenderer) dropperRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void Start()
    {
        _currentLeftLimit = leftLimit;
        _currentRightLimit = rightLimit;
        
        if (FruitManager.instance != null)
        {
            SpawnNewFruit();
        }
    }
    private void Update()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        _horizontalInput = moveInput.x;
        
        if (_interactAction.WasPressedThisFrame())
        {
            DropFruit().Forget();
        }
    }

    private void OnDisable()
    {
        _inputMap.Disable();
    }
    
    private void FixedUpdate()
    {
        float moveAmount = _horizontalInput * moveSpeed * Time.fixedDeltaTime;
        
        Vector3 currentPos = transform.position;
        
        float newX = currentPos.x + moveAmount;
        
        newX = Mathf.Clamp(newX, _currentLeftLimit, _currentRightLimit);
        
        transform.position = new Vector3(newX, currentPos.y, currentPos.z);
    }

    public void AdjustDropperOffset(Bounds fruitBounds)
    {
        ResetDropperOffset();
        
        _currentLeftLimit += fruitBounds.extents.x;
        _currentRightLimit -= fruitBounds.extents.x;
        
        float newX = Mathf.Clamp(transform.position.x, _currentLeftLimit, _currentRightLimit);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void SpawnNewFruit()
    {
        GameObject newFruit = FruitManager.instance.InstantiateFruit(transform);
        currentFruitComp = newFruit.GetComponent<FruitScript>();
    }
    
    public void SetFruitAnchorPosition(GameObject fruit)
    {
        var fruitCollider = fruit.GetComponent<CircleCollider2D>();

        if (!dropperRenderer || !fruitCollider) return;
        
        var dropperBounds = dropperRenderer.bounds;
        
        // Get fruit radius in world space
        float fruitRadius = fruitCollider.radius * transform.lossyScale.y;
        
        // Position fruit center so the top of the circle touches dropper's bottom
        float targetY = dropperBounds.min.y - fruitRadius - fruitPosOffset;
        
        fruit.transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
    }
    

    private void ResetDropperOffset()
    {
        _currentLeftLimit = leftLimit;
        _currentRightLimit = rightLimit;
    }
    
    private async UniTask DropFruit()
    {
        if (!currentFruitComp) return;
        currentFruitComp.Drop();
        await UniTask.Delay(System.TimeSpan.FromSeconds(0.5f));
        SpawnNewFruit();
    }
    
    private void OnDrawGizmosSelected()
    {
        // Draw movement limits as red lines
        Gizmos.color = Color.red;
        Vector3 leftPoint = new Vector3(_currentLeftLimit, transform.position.y, transform.position.z);
        Vector3 rightPoint = new Vector3(_currentRightLimit, transform.position.y, transform.position.z);
        
        Gizmos.DrawLine(leftPoint + Vector3.up * 0.5f, leftPoint + Vector3.down * 0.5f);
        Gizmos.DrawLine(rightPoint + Vector3.up * 0.5f, rightPoint + Vector3.down * 0.5f);
        
        // Draw movement range
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(leftPoint, rightPoint);
    }
}
