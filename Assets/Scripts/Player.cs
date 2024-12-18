using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Action
    {
        Idle,
        Run,
        Finish,
        Lose
    }

    [SerializeField] private Action _playerAction;
    public enum MovementDirection
    {
        GlobalX,
        GlobalZ
    }

    [SerializeField] private MovementDirection _currentDirection = MovementDirection.GlobalZ;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _horizontalMoveSpeed = 5f;
    [SerializeField] private float _horizontalLimit = 2f;
    private float _currentPos;

    [Header("Rotation")]
    [SerializeField] private float _turnDuration = 0.3f;
    private bool _isTurning;

    [Header("Player")]
    [SerializeField] private GameObject[] _allPlayerSkins;

    [Header("Collection")]
    [SerializeField] private int[] _update;
    private int _currentMoney = 0;
    private int _currentUpdateIndex = 0;
    private int _previewUpdateIndex = 0;

    [Header("SFX")]
    [SerializeField] private AudioClip _death;
    [SerializeField] private AudioClip _spin;

    private Vector2 _startInputPosition;
    private Vector2 _previousTouchPosition;
    private Animator _animator;

    public delegate void OnCollectItem(int currentCount, int currentIndex);
    public event OnCollectItem onCollectItem;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    void Update()
    {
        switch (_playerAction)
        {
            case Action.Idle:
                break;
            case Action.Run:
                Movement();
                HandleHorizontalMovement();
                break;
            case Action.Finish:
                break;
        }
    }

    private void Movement()
    {
        transform.position += transform.forward * _moveSpeed * Time.deltaTime;
    }

    private void HandleHorizontalMovement()
    {
#if UNITY_WEBGL || UNITY_ANDROID
        HandleTouchInput();
#else
        HandleMouseInput();
#endif
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startInputPosition = touch.position;
                    _previousTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    float deltaX = touch.position.x - _previousTouchPosition.x;
                    _previousTouchPosition = touch.position;
                    float horizontalMove = (deltaX / Screen.width) * _horizontalMoveSpeed;
                    transform.position += transform.right * horizontalMove;
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _startInputPosition = Input.mousePosition;
            _previousTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPos = Input.mousePosition;
            float deltaX = currentPos.x - _previousTouchPosition.x;
            _previousTouchPosition = currentPos;

            float horizontalMove = (deltaX / Screen.width) * _horizontalMoveSpeed;

            Vector3 horizontalOffset = transform.right * horizontalMove;

            Vector3 newPosition = transform.position + horizontalOffset;

            if (_currentDirection == MovementDirection.GlobalZ)
            {
                newPosition.x = Mathf.Clamp(newPosition.x, -_horizontalLimit + _currentPos, _horizontalLimit + _currentPos);
            }
            else if (_currentDirection == MovementDirection.GlobalX)
            {
                newPosition.z = Mathf.Clamp(newPosition.z, -_horizontalLimit + _currentPos, _horizontalLimit + _currentPos);
            }

            transform.position = newPosition;
        }
    }



    public void CollectItem(int value)
    {
        _currentMoney += value;
        CheckForUpgrade();
        onCollectItem?.Invoke(_currentMoney,_currentUpdateIndex);
    }

    private void CheckForUpgrade()
    {
        if (_currentMoney <= 0)
        {
            _playerAction = Action.Lose;
            FindObjectOfType<GameCanvas>().WinOrLose(false);
            PlayAnim("Die");
            AudioManager.Instance.PlaySFX(_spin);
            return;
        }
        for (int i = 0; i < _update.Length; i++)
        {
            if (_currentMoney >= _update[i])
            {
                _currentUpdateIndex = i;
            }
        }

        for (int i = _update.Length - 1; i >= 0; i--)
        {
            if (_currentMoney < _update[i])
            {
                _currentUpdateIndex = i;
            }
        }
        if (_previewUpdateIndex != _currentUpdateIndex)
        {
            _previewUpdateIndex = _currentUpdateIndex;
            PlayAnim("Spin");
            AudioManager.Instance.PlaySFX(_spin);
        }
        ApplySkin();
    }

    private void ApplySkin()
    {
        for (int i = 0; i < _allPlayerSkins.Length; i++)
        {
            _allPlayerSkins[i].SetActive(false);
            if (i == _currentUpdateIndex)
            {
                _allPlayerSkins[_currentUpdateIndex].SetActive(true);
            }
        }
    }


    public IEnumerator TurnCharacter(float turnAngle)
    {
        _isTurning = true;

        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, turnAngle, 0);

        float elapsedTime = 0;

        while (elapsedTime < _turnDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, elapsedTime / _turnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
        _isTurning = false;
    }
   
    private void PlayAnim(string name)
    {
        _animator.Play(name);
    }

    public void CompleteLevel()
    {
        _playerAction = Action.Finish;
        PlayAnim("Finish");
    }

    public void StartGame()
    {
        _playerAction = Action.Run;
        PlayAnim("Run");
    }
    public void SetMovementDirection(MovementDirection newDirection, float currentPos)
    {
        _currentPos = currentPos;
        _currentDirection = newDirection;
    }
}
