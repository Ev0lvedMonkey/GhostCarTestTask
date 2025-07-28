using Ashsvp;

namespace TheGame.Code
{
    public class FinishLine : TriggerZone<SimcadeVehicleController>
    {
        protected override void Awake()
        {
            base.Awake();
            SetTriggerAction(() => _raceManager.FinishLap());
        }
    }
}