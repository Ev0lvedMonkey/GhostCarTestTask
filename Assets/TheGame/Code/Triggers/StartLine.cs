using Ashsvp;

namespace TheGame.Code
{
    public class StartLine : TriggerZone<SimcadeVehicleController>
    {
        protected override void Awake()
        {
            base.Awake();
            SetTriggerAction(() => _raceManager.StartRace());
        }
    }
}