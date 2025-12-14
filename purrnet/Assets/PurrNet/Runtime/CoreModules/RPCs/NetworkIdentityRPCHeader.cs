using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using PurrNet.Packing;

namespace PurrNet
{
    public static class NetworkIdentityRPCHeaderPacker
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining), UsedImplicitly]
        public static void Write(BitPacker stream, NetworkIdentityRPCHeader value)
        {
            Packer<NetworkID>.Write(stream, value.networkId);
            Packer<SceneID>.Write(stream, value.sceneId);
            Packer<PlayerID>.Write(stream, value.senderId);
            Packer<PlayerID?>.Write(stream, value.targetId);
            Packer<Size>.Write(stream, value.rpcId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), UsedImplicitly]
        public static void Read(BitPacker stream, ref NetworkIdentityRPCHeader value)
        {
            Packer<NetworkID>.Read(stream, ref value.networkId);
            Packer<SceneID>.Read(stream, ref value.sceneId);
            Packer<PlayerID>.Read(stream, ref value.senderId);
            Packer<PlayerID?>.Read(stream, ref value.targetId);
            Packer<Size>.Read(stream, ref value.rpcId);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), UsedImplicitly]
        public static bool WriteDelta(BitPacker stream, NetworkIdentityRPCHeader oldValue, NetworkIdentityRPCHeader value)
        {
            int wasAnyChangedFlag = stream.AdvanceBits(1);
            bool wasIdChanged = DeltaPacker<NetworkID>.Write(stream, oldValue.networkId, value.networkId);
            int wasContextChangedFlag = stream.AdvanceBits(1);

            bool wasContextChanged = DeltaPacker<SceneID>.Write(stream, oldValue.sceneId, value.sceneId);
            wasContextChanged = DeltaPacker<PlayerID>.Write(stream, oldValue.senderId, value.senderId) || wasContextChanged;
            wasContextChanged = DeltaPacker<PlayerID?>.Write(stream, oldValue.targetId, value.targetId) || wasContextChanged;
            wasContextChanged = DeltaPacker<Size>.Write(stream, oldValue.rpcId, value.rpcId) || wasContextChanged;

            bool wasChanged = wasIdChanged || wasContextChanged;

            stream.WriteAt(wasAnyChangedFlag, wasChanged);
            stream.WriteAt(wasContextChangedFlag, wasContextChanged);

            if (!wasChanged)
                stream.SetBitPosition(wasAnyChangedFlag + 1);
            else if (!wasContextChanged)
                stream.SetBitPosition(wasContextChangedFlag + 1);

            return wasChanged;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining), UsedImplicitly]
        public static void ReadDelta(BitPacker stream, NetworkIdentityRPCHeader oldValue, ref NetworkIdentityRPCHeader value)
        {
            bool wasChanged = default(bool);
            Packer<bool>.Read(stream, ref wasChanged);
            if (wasChanged)
            {
                NetworkID nid = default;
                DeltaPacker<NetworkID>.Read(stream, oldValue.networkId, ref nid);

                bool wasContextChanged = default(bool);
                Packer<bool>.Read(stream, ref wasContextChanged);

                if (wasContextChanged)
                {
                    DeltaPacker<SceneID>.Read(stream, oldValue.sceneId, ref value.sceneId);
                    DeltaPacker<PlayerID>.Read(stream, oldValue.senderId, ref value.senderId);
                    DeltaPacker<PlayerID?>.Read(stream, oldValue.targetId, ref value.targetId);
                    DeltaPacker<Size>.Read(stream, oldValue.rpcId, ref value.rpcId);
                }
                else value = oldValue;

                value.networkId = nid;
            }
            else
            {
                value = oldValue;
            }
        }
    }

    public struct NetworkIdentityRPCHeader : IPackedAuto, IEquatable<NetworkIdentityRPCHeader>
    {
        public NetworkID networkId;
        public SceneID sceneId;
        public PlayerID senderId;
        public PlayerID? targetId;
        public Size rpcId;

        public override string ToString()
        {
            return $"NetworkIdentityRPCHeader: {{ sceneId: {sceneId}, networkId: {networkId}, senderId: {senderId}, targetId: {targetId}, rpcId: {rpcId} }}";
        }

        public bool Equals(NetworkIdentityRPCHeader other)
        {
            return networkId.Equals(other.networkId) &&
                   sceneId.Equals(other.sceneId) &&
                   senderId.Equals(other.senderId) &&
                   Nullable.Equals(targetId, other.targetId) &&
                   rpcId.Equals(other.rpcId);
        }

        public override bool Equals(object obj)
        {
            return obj is NetworkIdentityRPCHeader other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(networkId, sceneId, senderId, targetId, rpcId);
        }
    }
}
