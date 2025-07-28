using System.Collections.Generic;
using UnityEngine;

namespace TheGame.Code
{
    public class GhostCarController: MonoBehaviour
    {
        private const float Speed = 5f;

        private List<Vector3> _ghostPositions;
        private int _currentPositionIndex;
        private bool _isActive;

        public void Initialize(List<Vector3> positions)
        {
            _ghostPositions = positions;
            _currentPositionIndex = 0;
            _isActive = true;
            transform.position = _ghostPositions[0];
        }

        public void UpdateGhost(float deltaTime)
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

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                _currentPositionIndex++;
            }
        }
    }
}