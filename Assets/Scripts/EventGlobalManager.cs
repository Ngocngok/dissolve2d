using Sigtrap.Relays;
using UnityEngine;

public class EventGlobalManager : Singleton<EventGlobalManager>
{
    public Relay OnStartLoadScene = new Relay();
    public Relay OnFinishLoadScene = new Relay();

    public Relay OnUpdateSetting = new Relay();

    public Relay OnChangeName = new Relay();

    public Relay OnPurchaseNoAds = new Relay();
}