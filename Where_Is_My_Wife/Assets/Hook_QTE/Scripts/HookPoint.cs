using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.UI
{
    public class HookPoint : MonoBehaviour
    {
        [SerializeField] private GameObject _hookGameObject;
        [SerializeField] private GameObject _arrowGizmo;
        private SpriteRenderer _arrowRenderer = default;
        private Vector3 _playerPosition = default;
        private Vector3 _playerLocalPosition = default;
        private Vector3 _arrowLocalPosition = default;
        private bool _playerInTriggerZone = false;
        private IPlayerInputEvent _playerInputEvent;

        private void Start()
        {
            _arrowRenderer = _arrowGizmo.GetComponent<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _playerInTriggerZone = true;
                SubscribeToObservables();
                AssignPlayerPosition(other);
                AssignArrowPositionAndRotation();
                SwitchArrowSpriteRenderer(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                FinishingGizmoInteraction();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                AssignPlayerPosition(other);
            }
        }

        private void FixedUpdate()
        {
            if (_playerInTriggerZone)
            {
                AssignArrowPositionAndRotation();
            }
        }

        private void CalculateArrowPosition()
        {
            _playerLocalPosition = _hookGameObject.transform.InverseTransformPoint(_playerPosition);
            _arrowLocalPosition = ((_playerLocalPosition * -1).normalized) * 3f;
        }

        private float GetArrowRotationAngle()
        {
            Vector3 _directionToTarget = _arrowLocalPosition - _playerLocalPosition;
            float _angle = Mathf.Atan2(_directionToTarget.y, _directionToTarget.x) * Mathf.Rad2Deg - 90f;
            return _angle;
        }

        private void SwitchArrowSpriteRenderer(bool option)
        {
            _arrowRenderer.enabled = option;
        }

        private void AssignPlayerPosition(Collider2D playerCollider)
        {
            _playerPosition = playerCollider.gameObject.transform.position;
        }

        private void AssignArrowPositionAndRotation()
        {
            CalculateArrowPosition();
            _arrowGizmo.transform.localPosition = _arrowLocalPosition;
            _arrowGizmo.transform.rotation = Quaternion.Euler(0f, 0f, GetArrowRotationAngle());
        }

        private void ResetArrowPosition()
        {
            _arrowGizmo.transform.position = _hookGameObject.transform.position;
        }

        private void FinishingGizmoInteraction()
        {
            _playerInTriggerZone = false;
            SwitchArrowSpriteRenderer(false);
            ResetArrowPosition();
            UnsubscribeToObservables();
        }

        private void SubscribeToObservables()
        {
            _playerInputEvent = InputEventManager.Instance.PlayerInputEvent;
            _playerInputEvent.HookStartAction += ExecuteHookStartEvent;
        }

        private void UnsubscribeToObservables()
        {
            _playerInputEvent.HookStartAction -= ExecuteHookStartEvent;
        }

        private void ExecuteHookStartEvent()
        {
            FinishingGizmoInteraction();
        }
    }
}