using TMPro;
using UnityEngine;

namespace TheGame.Code
{
    public class RaceManager : MonoBehaviour
    {
        [SerializeField] private GameObject _playerCarPrefab;
        [SerializeField] private GameObject _ghostCarPrefab;
        [SerializeField] private TextMeshProUGUI _raceNumberText;
        [SerializeField] private Transform _spawnPoint;

        private TrajectoryRecorder _trajectoryRecorder;
        private GhostCarController _ghostCarController;
        private GameObject _playerCar;
        private GameObject _ghostCar;
        private bool _isFirstLap = true;
        private bool _isRaceStarted;

        private void Awake()
        {
            _trajectoryRecorder = new TrajectoryRecorder();
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

            _playerCar = Instantiate(_playerCarPrefab, _spawnPoint.position, _spawnPoint.rotation);
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
            _raceNumberText.text = "Гонка завершена";

            if (_playerCar != null)
            {
                Destroy(_playerCar);
            }
            if (_ghostCar != null)
            {
                Destroy(_ghostCar);
            }

            Invoke(nameof(StartSecondLap), 3f);
        }

        private void StartSecondLap()
        {
            _isFirstLap = false;
            _isRaceStarted = true;
            _raceNumberText.text = "2";

            _playerCar = Instantiate(_playerCarPrefab, _spawnPoint.position, _spawnPoint.rotation);

            _ghostCar = Instantiate(_ghostCarPrefab, _spawnPoint.position, _spawnPoint.rotation);
            _ghostCarController = _ghostCar.GetComponent<GhostCarController>();
            _ghostCarController.Initialize(_trajectoryRecorder.GetRecordedPositions());
        }
    }
}