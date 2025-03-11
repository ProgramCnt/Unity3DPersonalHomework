using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    private Vector2 currentMovementInput;
    public LayerMask groundLayerMask;
    bool isMoving;
    bool _isSprinting;
    public bool IsSprint { get { return _isSprinting; } }

    [Header("Look")]
    public Transform CameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurrentXRotation;      //마우스의 delta값
    public float lookSensitivity;           //회전 민감도
    private Vector2 mouseDelta;

    [Header("Animator")]
    private Animator _animator;

    private Rigidbody _rigidbody;

    public Image interactUI;
    public TextMeshProUGUI interactUIText;

    LayerMask layerMask;
    RaycastHit hit;
    string text;

    public GameObject ItemSlotBox;

    [Header("ItemInfo")]
    public GameObject itemInfo;
    public Sprite icon;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        sprintSpeed = moveSpeed * 1.5f;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;           //마우스 커서를 고정
        layerMask = 1 << LayerMask.NameToLayer("Interactable");
        text = interactUIText.text;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
    }

    private void LateUpdate()
    {
        CameraLook();
    }

    private void Update()
    {
        RayCheck();
    }

    void Move()
    {
        Vector3 direction = transform.forward * currentMovementInput.y + transform.right * currentMovementInput.x;
        if (_animator.GetBool("IsSprint") && PlayerManager.Instance.Player.condition.Stamina.curValue > 0)
        {
            direction *= sprintSpeed;
            PlayerManager.Instance.Player.condition.Stamina.SubValue(0.5f);
            //Debug.Log("소모" + PlayerManager.Instance.Player.condition.Stamina.curValue);
        }
        else
        {
            _isSprinting = false;
            _animator.SetBool("IsMove", false);
            _animator.SetBool("IsSprint", false);
            direction *= moveSpeed;
        }
        direction.y = _rigidbody.velocity.y;

        _animator.SetBool("IsMove", isMoving);

        _rigidbody.velocity = direction;
    }

    void CameraLook()
    {
        camCurrentXRotation += mouseDelta.y * lookSensitivity;
        camCurrentXRotation = Mathf.Clamp(camCurrentXRotation, minXLook, maxXLook);
        CameraContainer.localEulerAngles = new Vector3(-camCurrentXRotation, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            currentMovementInput = context.ReadValue<Vector2>();
            _animator.SetBool("IsMove", true);
            _animator.SetFloat("PosX", currentMovementInput.x);
            _animator.SetFloat("PosY", currentMovementInput.y);
            isMoving = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            currentMovementInput = Vector2.zero;
            _animator.SetBool("IsMove", false);
            _animator.SetBool("IsSprint", false);
            _animator.SetFloat("PosX", 0f);
            _animator.SetFloat("PosY", 0f);
            isMoving = false;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && isGrounded())
        {
            if (PlayerManager.Instance.Player.condition.Stamina.curValue > 10)
            {
                PlayerManager.Instance.Player.condition.Stamina.SubValue(10);
                _animator.SetTrigger("IsJump");
            }
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (PlayerManager.Instance.Player.condition.Stamina.curValue > 0)
            {
                _isSprinting = true;
                _animator.SetBool("IsMove", true);
                _animator.SetBool("IsSprint", true);
            }
            else
            {
                _isSprinting = false;
                _animator.SetBool("IsMove", false);
                _animator.SetBool("IsSprint", false);
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isSprinting = false;
            _animator.SetBool("IsMove", false);
            _animator.SetBool("IsSprint", false);
        }
    }

    bool isGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 10f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (hit.collider != null)
            {
                hit.collider.GetComponent<IInteractable>()?.Interact();
            }
        }
    }

    void RayCheck()
    {
        Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 2f, layerMask);
        if (hit.collider == null)
        {
            icon = null;
            itemName.text = string.Empty;
            itemDescription.text = string.Empty;
            interactUI.gameObject.SetActive(false);
            itemInfo.SetActive(false);
        }
        else
        {
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(hit.point);

            interactUI.transform.position = screenPosition + Vector3.up * 0.2f;
            interactUIText.text = text;
            if (hit.collider.TryGetComponent<Door>(out var door))
            {
                interactUIText.text = "문열기(E)";
            }
            else if (hit.collider.TryGetComponent<PotionItem>(out var potionItem))
            {
                icon = potionItem.itemData.itemIcon;
                itemName.text = potionItem.itemData.itemName;
                itemDescription.text = potionItem.itemData.itemDescription;
                interactUIText.text = "물약획득(E)";
                itemInfo.SetActive(true);
            }
            
            interactUI.gameObject.SetActive(true);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * 2f);
        Gizmos.DrawRay(transform.position, Vector3.down * 5f);
    }

    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            int keyValue = 0;
            if (int.TryParse(context.control.name, out keyValue))
            {
                if (keyValue != 0)
                {
                    ItemSlotBox.transform.GetChild(keyValue - 1).GetComponent<ItemSlot>().UseItem();
                }
            }
        }
    }
}
