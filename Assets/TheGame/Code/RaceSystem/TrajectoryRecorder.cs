using System.Collections.Generic;
using UnityEngine;

namespace TheGame.Code
{
    public class TrajectoryRecorder
    {
        private List<Vector3> _recordedPositions = new List<Vector3>();

        public void RecordPosition(Vector3 position)
        {
            _recordedPositions.Add(position);
        }

        public List<Vector3> GetRecordedPositions()
        {
            return new List<Vector3>(_recordedPositions);
        }

        public void ClearTrajectory()
        {
            _recordedPositions.Clear();
        }
    }
}