using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheGame.Code.UI
{
    public class RaceView : MonoBehaviour
    {
        [SerializeField] private RaceManager _raceManager;
        
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI _raceNumberText;
        [SerializeField] private Button _startRaceButton;

        private RacePresenter _racePresenter;
        public event Action OnStartRaceButtonPressed;
        
        private void Awake()
        {
            _startRaceButton.onClick.AddListener(() =>
            {
                OnStartRaceButtonPressed?.Invoke();
                _startRaceButton.gameObject.SetActive(false);
            });
            _racePresenter = new RacePresenter(this, _raceManager);
        }

        private void OnDestroy()
        {
            _startRaceButton.onClick.RemoveAllListeners();
        }

        public void SetRaceNumber(string raceNumber)
        {
            _raceNumberText.text = raceNumber;
        }
    }
}