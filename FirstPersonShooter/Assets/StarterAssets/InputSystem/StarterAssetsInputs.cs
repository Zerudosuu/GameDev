using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool shoot;

        public bool reload;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        private PlayerInput playerInput;

#if ENABLE_INPUT_SYSTEM
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            var playerActions = playerInput.actions.FindActionMap("Player");
            if (playerActions != null)
            {
                var shootAction = playerActions.FindAction("Shoot");
                if (shootAction != null)
                {
                    shootAction.performed += OnShootPerformed;
                    shootAction.canceled += OnShootCanceled;
                }
            }
        }

        private void OnDisable()
        {
            var playerActions = playerInput.actions.FindActionMap("Player");
            if (playerActions != null)
            {
                var shootAction = playerActions.FindAction("Shoot");
                if (shootAction != null)
                {
                    shootAction.performed -= OnShootPerformed;
                    shootAction.canceled -= OnShootCanceled;
                }
            }
        }

        private void OnShootPerformed(InputAction.CallbackContext context)
        {
            shoot = true;
        }

        private void OnShootCanceled(InputAction.CallbackContext context)
        {
            shoot = false;
        }

        public void OnReload(InputValue value)
        {
            if (value.isPressed)
            {
                ReloadInput(true);
            }
            else
            {
                ReloadInput(false);
            }
        }

        public void ReloadInput(bool newReloadState)
        {
            reload = newReloadState;
        }

        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
#endif
    }
}
