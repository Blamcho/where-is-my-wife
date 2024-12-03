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
        [SerializeField] private LineRenderer _lineRenderer = default;
        [SerializeField] private SpriteRenderer _circleRenderer = default;
        [SerializeField] private float _hookLaunchTimeToReachHookpoint = 0f;
        private SpriteRenderer _arrowRenderer = default;
        private Vector3 _playerPosition = default;
        private Vector3 _playerLocalPosition = default;
        private Vector3 _arrowLocalPosition = default;
        private bool _playerInTriggerZone = false;
        private bool _hasSubscribedToObservables = false;
        private bool _executingHookLaunch, _renderingLineRendererAfterLaunch = false;
        private float _hookTimeElapsed = 0f;
        private IHookStateEvents _hookStateEvents;

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
                ChangeCircleColor(new Color32(255, 255, 255, 75));
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                UnsubscribeToObservables();
                FinishingHookInteraction();
                FinishingGizmoInteraction();
                ChangeCircleColor(new Color32(255, 255, 255, 50));
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

            if (_executingHookLaunch)
            {
                CalculateLocalPlayerPosition();
                _lineRenderer.SetPosition(0, _playerLocalPosition);
                UpdateHookLaunch();
            }

            if (_renderingLineRendererAfterLaunch)
            {
                CalculateLocalPlayerPosition();
                _lineRenderer.SetPosition(0, _playerLocalPosition);
                _lineRenderer.SetPosition(1, Vector3.zero);
                ComparePlayerPositionWithHookPoint();
            }
        }

        private void CalculateArrowPosition()
        {
            CalculateLocalPlayerPosition();
            _arrowLocalPosition = ((_playerLocalPosition * -1).normalized) * 3f;
        }

        private void CalculateLocalPlayerPosition()
        {
            _playerLocalPosition = _hookGameObject.transform.InverseTransformPoint(_playerPosition);
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

        private void SwitchLineRenderer(bool option)
        {
            _lineRenderer.enabled = option;
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

        private void ResetLineRendererPosition()
        {
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);
        }

        private void FinishingGizmoInteraction()
        {
            _playerInTriggerZone = false;
            SwitchArrowSpriteRenderer(false);
            ResetArrowPosition();
        }

        private void FinishingHookInteraction()
        {
            _renderingLineRendererAfterLaunch = false;
            SwitchLineRenderer(false);
            ResetLineRendererPosition();
        }

        private void StartHookInteraction()
        {
            CalculateLocalPlayerPosition();
            _lineRenderer.SetPosition(0,_playerLocalPosition);
            _lineRenderer.SetPosition(1, _playerLocalPosition);
            SwitchLineRenderer(true);
            _executingHookLaunch = true;
            ChangeCircleColor(new Color32(173, 255, 148, 50));
        }

        private void UpdateHookLaunch()
        {
            if (_hookTimeElapsed < _hookLaunchTimeToReachHookpoint)
            {
                _hookTimeElapsed += Time.fixedDeltaTime;
                _lineRenderer.SetPosition(1, Vector3.Lerp(_playerLocalPosition, Vector3.zero, _hookTimeElapsed));
            }
            else
            {
                _hookTimeElapsed = 0f;
                _executingHookLaunch = false;
                _renderingLineRendererAfterLaunch = true;
            }
        }

        private void ComparePlayerPositionWithHookPoint()
        {
            if (_playerLocalPosition.x <= 0.5f && _playerLocalPosition.x >= -0.5f && _playerLocalPosition.y <= 0.5f && _playerLocalPosition.y >= -0.5f)
            {
                FinishingHookInteraction();
            }
        }

        private void ChangeCircleColor(Color newColor)
        {
            _circleRenderer.color = newColor;
        }

        private void HookStart(Vector2 velocity)
        {
            if (!_playerInTriggerZone) return; 
            
            FinishingGizmoInteraction();
            UnsubscribeToObservables();
            StartHookInteraction();
        }

        private void SubscribeToObservables()
        {
            if (_hasSubscribedToObservables) return;
            _hasSubscribedToObservables = true;
            
            _hookStateEvents = PlayerManager.Instance.HookStateEvents;
            _hookStateEvents.HookStart += HookStart;
        }

        private void UnsubscribeToObservables()
        {
            if (!_hasSubscribedToObservables) return;
            _hasSubscribedToObservables = false;
            
            _hookStateEvents.HookStart -= HookStart;
        }
    }
}