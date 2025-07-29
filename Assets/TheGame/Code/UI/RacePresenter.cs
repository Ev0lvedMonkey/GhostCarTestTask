using System;

namespace TheGame.Code.UI
{
    public class RacePresenter : IDisposable
    {
        private RaceView _view;
        private RaceManager _raceManager;

        public RacePresenter(RaceView view, RaceManager raceManager)
        {
            _view = view;
            _raceManager = raceManager;
            _raceManager.OnRaceNumberChanged += UpdateUI;
            _view.OnStartRaceButtonPressed += StartRace;
        }

        public void Dispose()
        {
            _view.OnStartRaceButtonPressed -= StartRace;
        }

        private void UpdateUI(int raceNumber)
        {
            _view.SetRaceNumber($"{raceNumber}");
        }


        private void StartRace()
        {
            _raceManager.RestartRace();
        }
    }
}