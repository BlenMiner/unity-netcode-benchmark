using Fusion;
using TMPro;
using UnityEngine;

public class SendRPCBehabiour : NetworkBehaviour
{
    [SerializeField] TMP_Text _text;

    public override void FixedUpdateNetwork()
    {
        if (!Object.HasStateAuthority) return;

        var v = Random.Range(-10000, 10000);
        SomeDataClientRpc(v);
    }

    [Rpc(sources: RpcSources.StateAuthority, targets: RpcTargets.All)]
    private void SomeDataClientRpc(int data)
    {
        _text.SetText(data.ToString());
    }
}
