using System;
using UnityEngine;
using Ashsvp;
using TheGame.Code.RaceSystem;
using VContainer;

namespace TheGame.Code
{
    public class RaceManager : MonoBehaviour
    {
        [Header("Data")] [SerializeField] private GameObject _playerCarPrefab;
        [SerializeField] private GameObject _ghostCarPrefab;
        [SerializeField] private Transform _spawnPoint;

        private ITrajectoryRecorder _trajectoryRecorder;
        private GhostCarController _ghostCarController;
        private GameObject _playerCar;
        private GameObject _ghostCar;
        private bool _isFirstLap = true;
        private bool _isRaceStarted;

        private const int FirstRace = 1;
        private const int SecondRace = 2;

        public event Action<int> OnRaceNumberChanged;

        [Inject]
        public void Construct(ITrajectoryRecorder trajectoryRecorder)
        {
            _trajectoryRecorder = trajectoryRecorder;
            Debug.Log("Construct");
        }

        public void Start()
        {
            _playerCar = Instantiate(_playerCarPrefab, _spawnPoint.position, _spawnPoint.rotation);
            _ghostCar = Instantiate(_ghostCarPrefab, _spawnPoint.position, _spawnPoint.rotation);
            _playerCar.SetActive(false);
            _ghostCar.SetActive(false);
            _ghostCarController = _ghostCar.GetComponent<GhostCarController>();
        }

        private void FixedUpdate()
        {
            if (!_isRaceStarted) return;

            if (_isFirstLap)
                RecordPlayerPosition();
            else
                _ghostCarController?.MoveGhost(Time.fixedDeltaTime);
        }

        public void RestartRace()
        {
            _isRaceStarted = true;
            _isFirstLap = true;
            _playerCar.SetActive(true);
            _ghostCar.SetActive(false);
            _playerCar.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
            _trajectoryRecorder.ClearTrajectory();
            OnRaceNumberChanged?.Invoke(FirstRace);

            if (_playerCar.TryGetComponent(out SimcadeVehicleController vehicleController))
                vehicleController.Halt();
        }

        public void StartFinishLap()
        {
            if (_isFirstLap)
            {
                _isFirstLap = false;
                OnRaceNumberChanged?.Invoke(SecondRace);

                _ghostCar.SetActive(true);
                _ghostCar.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
                _ghostCarController.Initialize(_trajectoryRecorder.GetRecordedPositions());

                _playerCar.transform.SetPositionAndRotation(_spawnPoint.position, _spawnPoint.rotation);
                _trajectoryRecorder.ClearTrajectory();

                if (_playerCar.TryGetComponent(out SimcadeVehicleController vehicleController))
                    vehicleController.Halt();
            }
            else
            {
                EndRace();
            }
        }

        private void RecordPlayerPosition()
        {
            if (_playerCar != null)
                _trajectoryRecorder.RecordPosition(_playerCar.transform.position);
        }

        private void EndRace()
        {
            _isRaceStarted = false;
            _playerCar.SetActive(false);
            _ghostCar.SetActive(false);
            RestartRace();
        }
    }
}