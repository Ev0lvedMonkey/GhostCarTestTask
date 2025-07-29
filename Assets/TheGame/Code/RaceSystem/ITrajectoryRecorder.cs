using System.Collections.Generic;
using UnityEngine;

namespace TheGame.Code.RaceSystem
{
    public interface ITrajectoryRecorder
    {
        void RecordPosition(Vector3 position);
        List<Vector3> GetRecordedPositions();
        void ClearTrajectory();
    }
}