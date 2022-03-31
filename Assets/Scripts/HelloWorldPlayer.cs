using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                MovePlayerSinEquipo();
            }
        }

        public void MovePlayerEquipo1()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionEquipo1OnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPosicionEquipo1RequestServerRpc();
            }
        }
        public void MovePlayerEquipo2()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionEquipo2OnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPosicionEquipo2RequestServerRpc();
            }
        }
        public void MovePlayerSinEquipo()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionSinEquipoOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionSinEquipoRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionSinEquipoRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionSinEquipoOnPlane();
        }
        static Vector3 GetRandomPositionSinEquipoOnPlane()
        {
            return new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-3f, 3f));
        }
        [ServerRpc]
        void SubmitPosicionEquipo1RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionEquipo1OnPlane();
        }
        static Vector3 GetRandomPositionEquipo1OnPlane()
        {
            return new Vector3(Random.Range(-4f, -2f), 1f, Random.Range(-3f, 3f));
        }
        [ServerRpc]
        void SubmitPosicionEquipo2RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionEquipo2OnPlane();
        }
        static Vector3 GetRandomPositionEquipo2OnPlane()
        {
            return new Vector3(Random.Range(4f, 2f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}