namespace Assets.CampingSimulator.Scripts
{
    public interface IEncounterManager
    {
        float PlayerChargeAmount();
        bool IsPlayerCharging();
        bool IsPlayerLightOn();
        void Completed();
    }
}