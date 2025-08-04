using UnityEngine;
using UnityEngine.InputSystem;

public class DropperController : MonoBehaviour
{
    [Header("Prerequisites")]
    public GameObject fruitPrefab;
    public InputActionAsset inputActions;
    
    [Header("Movement Settings")]
    public float leftLimit = -2.6f;
    public float rightLimit = 2.6f;
    public float moveSpeed = 2f;
    
    [Header("Fruit")]
    public float fruitPosOffset;
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
        if (FruitManager.Instance != null && fruitPrefab != null)
        {
            InstantiateFruit();
        }
    }

    private void InstantiateFruit()
    {
        FruitData newFruitData = FruitManager.Instance.GetNextFruitData();
        
        GameObject newFruit = Instantiate(fruitPrefab, transform);
        newFruit.GetComponent<FruitScript>().SetFruitData(newFruitData);
        
        currentFruit = newFruit;
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
