using Unity.Netcode;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<Color> colorPlayer = new NetworkVariable<Color>();
        public static List<Color> coloresGuardados= new List<Color>();
        
        private Renderer enlace;

        public void Start()
            {
            Position.OnValueChanged += OnPositionChange;
            colorPlayer.OnValueChanged += OnColorChange;
            enlace= GetComponent<Renderer>();
            // Por si en el futuro hay mas equipos
            if(IsServer && IsOwner){
                coloresGuardados.Add(Color.blue);
                coloresGuardados.Add(Color.red);
                coloresGuardados.Add(Color.white);
            }
            }

            public void OnPositionChange(Vector3 previousValue, Vector3 newValue){
            transform.position=Position.Value;
        }
        public void OnColorChange(Color previousValue, Color newValue){
            enlace.material.color = newValue;
        }
        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                MovePlayerSinEquipo();
            }
        }

        public void MovePlayerEquipo1()
        {
            SubmitPosicionEquipo1RequestServerRpc();
            SubmitColorEquipo1RequestServerRpc();
        }
        public void MovePlayerEquipo2()
        {
                SubmitPosicionEquipo2RequestServerRpc();
                SubmitColorEquipo2RequestServerRpc();
        }
        public void MovePlayerSinEquipo()
        {
                SubmitPositionSinEquipoRequestServerRpc();
                SubmitColorSinEquipoRequestServerRpc();
        }

        [ServerRpc]
        void SubmitColorSinEquipoRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Color newColor =coloresGuardados[2];
            colorPlayer.Value=newColor;
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
        void SubmitColorEquipo1RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Color newColor =coloresGuardados[0];
            colorPlayer.Value=newColor;
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
        void SubmitColorEquipo2RequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Color newColor =coloresGuardados[1];
            colorPlayer.Value=newColor;
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