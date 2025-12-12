using FishNet.Object;
using TMPro;
using UnityEngine;

public class SendRPCBehaviour : NetworkBehaviour
{
    [SerializeField] TMP_Text _text;

    public override void OnStartNetwork()
    {
        if (!IsServer) return;

        TimeManager.OnTick += OnTick;
    }

    private void OnTick()
    {
        var v = Random.Range(-10000, 10000);
        SomeData(v);
        _text.SetText(v.ToString());
    }

    [ObserversRpc]
    private void SomeData(int data)
    {
        _text.SetText(data.ToString());
    }
}
