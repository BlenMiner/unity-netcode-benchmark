using Mirror;
using TMPro;
using UnityEngine;

public class SendRPCBehaviour : NetworkBehaviour
{
    [SerializeField] TMP_Text _text;

    private void FixedUpdate()
    {
        if (isClient) return;
        OnTick();
    }

    private void OnTick()
    {
        var v = Random.Range(-10000, 10000);
        SomeData(v);
        _text.SetText(v.ToString());
    }

    [ClientRpc]
    private void SomeData(int data)
    {
        _text.SetText(data.ToString());
    }
}
