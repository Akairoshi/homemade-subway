    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class PlayerController : MonoBehaviour, IUpdatable
    {
        [Header("Настройки игрока")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private float _gravity = 9.81f;
        //[SerializeField] private float _startSpeed = 10f;
        //[SerializeField] private float _acceleration = 0.5f;
        //[SerializeField] private float _maxSpeed = 50f;

    // Состояние игрока для работы с положением на рельсах
    private enum PlayerState { Unknown = -1, Left, Center, Right }
        private Dictionary<PlayerState, float> _linePosition = new()
        {
            [PlayerState.Left] = 2f,
            [PlayerState.Center] = 0f,
            [PlayerState.Right] = -2f
        };
        
        //Переменные для работы физики игрока
        private float _lineChangeSpeed = 10f;
        private PlayerState _playerState;
        private float _targetLineX;
        private float _verticalVelocity;
        private Vector3 _moveDirection;

        public static PlayerController Instance;

        private void Awake()
        {
            Instance = this;
            //Назначение стартовых значений для полоэегия игрока
            _playerState = PlayerState.Center;
            _targetLineX = _linePosition[_playerState];
        }

        //Апдейт менеджер для оптимизации
        public void OnUpdate(float deltaTime)
        {
            _moveDirection = Vector3.zero;
            
            UpdateVelocity(deltaTime);

            //Применение позиции игрока к самому игроку
            CollisionFlags flags = _characterController.Move(_moveDirection * deltaTime);

            if ((flags & CollisionFlags.Below) != 0)
            {
                _verticalVelocity = -2f;
            }

        }
        //Обновление скорости игрока
        private void UpdateVelocity(float deltaTime)
        {
            UpdateVerticalVelocity(deltaTime);
            LineChange(deltaTime);
            _moveDirection.y = _verticalVelocity;
        }

        //Обновление вертикальной скорости игрока
        private void UpdateVerticalVelocity(float deltaTime)
            {
                if (_characterController.isGrounded)
                {
                    if (_verticalVelocity < 0) _verticalVelocity = -2f;
                }
                else
                {
                    _verticalVelocity -= _gravity * deltaTime;
                }
            }
        private void LineChange(float deltaTime)
        {
            float deltaX = _targetLineX - transform.position.x;
            _moveDirection.x = deltaX * _lineChangeSpeed;
        }

        //Получение кнопки прыжка из PlayerInput
        public void OnJump(InputValue value)
        {
            if (value.isPressed && _characterController.isGrounded)
            {
                _verticalVelocity = Mathf.Sqrt(2 * _gravity);
            }
        }


        //Получение вектора движения из PlayerInput
        public void OnMove(InputValue value)
        {
            PlayerState prevState = _playerState;
            Vector3 input = value.Get<Vector3>();
            int lineDirection  = 0;

            if (input.x > 0)
            {
                lineDirection = 1;
            }
            else if (input.x < 0)
            {
                lineDirection = -1;
            }
            switch (_playerState)
               {
                    case PlayerState.Left:
                        if (lineDirection < 0) _playerState = PlayerState.Center;
                        break;
                    case PlayerState.Center:
                        if (lineDirection > 0) _playerState = PlayerState.Left;
                        else if (lineDirection < 0) _playerState = PlayerState.Right;
                        break;
                    case PlayerState.Right:
                        if (lineDirection > 0) _playerState = PlayerState.Center;
                        break;
                }

            if (prevState != _playerState)
                _targetLineX = _linePosition[_playerState];
        }

    //рестарт уровня в самое начало <--- заменить на экран проигрыша
    public void OnRestart()
        {
            SceneController.Instance.RestartScene();
        }
        //Управление PlayerController для взаимодействия с UpdateManager
        private void OnEnable()
        {
            if (UpdateManager.Instance != null)
                UpdateManager.Instance.Register(this);
        }

        private void OnDisable()
        {
            UpdateManager.Instance.Unregister(this);
        }
    }
