using System.Collections.Generic;
using UnityEngine;

namespace TheGame.Code
{
    public class GhostCarController : MonoBehaviour
    {
        private const float Speed = 10f;
        private const float RotationSpeed = 5f;

        private List<Vector3> _ghostPositions;
        private int _currentPositionIndex;
        private bool _isActive;

        public void Initialize(List<Vector3> positions)
        {
            _ghostPositions = positions;
            _currentPositionIndex = 0;
            _isActive = true;
            transform.position = _ghostPositions[0];

            if (_ghostPositions.Count > 1)
            {
                Vector3 direction = (_ghostPositions[1] - _ghostPositions[0]).normalized;
                if (direction != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(direction);
                }
            }
        }

        public void MoveGhost(float deltaTime)
        {
            if (!_isActive || _currentPositionIndex >= _ghostPositions.Count - 1)
            {
                return;
            }

            Vector3 targetPosition = _ghostPositions[_currentPositionIndex + 1];

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                Speed * deltaTime
            );

            Vector3 direction = (targetPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    targetRotation,
                    RotationSpeed * deltaTime
                );
            }

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                _currentPositionIndex++;
            }
        }
    }
}