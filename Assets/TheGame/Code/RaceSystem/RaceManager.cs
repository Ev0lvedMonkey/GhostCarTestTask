using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Ashsvp;

namespace TheGame.Code
{
    public class RaceManager : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private GameObject _playerCarPrefab;
        [SerializeField] private GameObject _ghostCarPrefab;
        [SerializeField] private Transform _spawnPoint;
        
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI _raceNumberText;
        [SerializeField] private Button _startRaceButton;

        private TrajectoryRecorder _trajectoryRecorder;
        private GhostCarController _ghostCarController;
        private GameObject _playerCar;
        private GameObject _ghostCar;
        private bool _isFirstLap = true;
        private bool _isRaceStarted;

        private void Awake()
        {
            _startRaceButton.onClick.AddListener(StartRace);
        }

        private void Update()
        {
            if (!_isRaceStarted) return;

            if (_isFirstLap)
            {
                RecordPlayerPosition();
            }
            else
            {
                _ghostCarController?.UpdateGhost(Time.deltaTime);
            }
        }

        public void StartRace()
        {
            _trajectoryRecorder = new TrajectoryRecorder();
            if (_isRaceStarted) return;

            _isRaceStarted = true;

            _playerCar = Instantiate(_playerCarPrefab, _spawnPoint.position, _spawnPoint.rotation);
            _trajectoryRecorder.ClearTrajectory();
            _raceNumberText.text = "1";
        }

        public void RestartRace()
        {
            _isRaceStarted = false;
            _isFirstLap = true;
            _raceNumberText.text = "1";
            _trajectoryRecorder.ClearTrajectory();

            if (_playerCar != null)
            {
                Destroy(_playerCar);
            }
            if (_ghostCar != null)
            {
                Destroy(_ghostCar);
            }

            StartRace();
        }

        public void FinishLap()
        {
            if (_isFirstLap)
            {
                _isFirstLap = false;
                _raceNumberText.text = "2";

                _ghostCar = Instantiate(_ghostCarPrefab, _spawnPoint.position, _spawnPoint.rotation);
                _ghostCarController = _ghostCar.GetComponent<GhostCarController>();
                _ghostCarController.Initialize(_trajectoryRecorder.GetRecordedPositions());

                _playerCar.transform.position = _spawnPoint.position;
                _playerCar.transform.rotation = _spawnPoint.rotation;
                _trajectoryRecorder.ClearTrajectory();

                if (_playerCar.TryGetComponent(out SimcadeVehicleController vehicleController))
                {
                    var rb = _playerCar.GetComponent<Rigidbody>();
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    vehicleController.accelerationInput = 0f;
                    vehicleController.brakeInput = 0f;
                    vehicleController.steerInput = 0f;
                }
            }
            else
            {
                EndRace();
            }
        }

        private void RecordPlayerPosition()
        {
            if (_playerCar != null)
            {
                _trajectoryRecorder.RecordPosition(_playerCar.transform.position);
            }
        }

        private void EndRace()
        {
            _isRaceStarted = false;

            if (_playerCar != null)
            {
                Destroy(_playerCar);
            }
            if (_ghostCar != null)
            {
                Destroy(_ghostCar);
            }

            Invoke(nameof(RestartRace), 3f);
        }
    }
}