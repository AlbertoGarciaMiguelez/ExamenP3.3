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
        public NetworkVariable<int> Equipo = new NetworkVariable<int>();
        public static NetworkList<int> campeones= new NetworkList<int>();
        public static List<Color> coloresGuardados= new List<Color>();
        
        int numeroMaximo=3;

        //public static List<int> equipo1 = new List<int>();
        
        private Renderer enlace;

        public void Start(){
            Position.OnValueChanged += OnPositionChange;
            colorPlayer.OnValueChanged += OnColorChange;
            enlace= GetComponent<Renderer>();
            // Por si en el futuro hay mas equipos
            if(IsServer && IsOwner){
                coloresGuardados.Add(Color.blue);
                coloresGuardados.Add(Color.red);
                coloresGuardados.Add(Color.white);
            }
            if(IsServer && IsOwner){
                campeones.Add(0);
                campeones.Add(0);
                campeones.Add(0);
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
                PlayerEquipoSpawn(2);
                Equipo.Value=-1;
            }
        }
        public void PlayerEquipoSpawn(int x)
        {
            SubmitLlenarEquipoRequestServerRpc(x);
            SubmitPositionEquipoRequestServerRpc(x);
            SubmitColorEquipoRequestServerRpc(x);
        }
        public void PlayerEquipo(int x)
        {
            if(x==0){
                SubmitEquipo1(x);
            }else if(x==1){
                SubmitEquipo2(x);
            }else{
                SubmitLlenarEquipoRequestServerRpc(x);
                SubmitPositionEquipoRequestServerRpc(x);
                SubmitColorEquipoRequestServerRpc(x);
            }
        }
        public void SubmitEquipo1(int x){
            Debug.Log("Llamada");
            if (campeones[1] < numeroMaximo)
            {

                Debug.Log("Se llama equipo"+x);
                SubmitExpulsarEquipoRequestServerRpc();
                SubmitPositionEquipoRequestServerRpc(x);
                SubmitColorEquipoRequestServerRpc(x);
                SubmitEquipoRequestServerRpc(x);
                SubmitLlenarEquipoRequestServerRpc(x);
            }
            else
            {
                Debug.Log("Lleno equipo1");
            }
        }
        public void SubmitEquipo2(int x){
            if (campeones[2] < numeroMaximo)
            {
                SubmitExpulsarEquipoRequestServerRpc();
                SubmitPositionEquipoRequestServerRpc(x);
                SubmitColorEquipoRequestServerRpc(x);
                SubmitEquipoRequestServerRpc(x);
                SubmitLlenarEquipoRequestServerRpc(x);
            }
            else
            {
                Debug.Log("Lleno equipo2");
            }
        }
        [ServerRpc]
        void SubmitEquipoRequestServerRpc(int x, ServerRpcParams rpcParams = default)
        {
            Equipo.Value = x;
        }
        [ServerRpc]
        void SubmitExpulsarEquipoRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            if (Equipo.Value == 0)
            {
                campeones[0]--;
            }else if (Equipo.Value == 1)
            {
                campeones[1]--;
            }else
            {
                campeones[2]--;
            }
        }
        [ServerRpc]
        void SubmitLlenarEquipoRequestServerRpc(int x,ServerRpcParams rpcParams = default)
        {
            if (x == 0)
            {
                campeones[0]++;
            }else if (x == 1)
            {
                campeones[1]++;
            }else
            {
                campeones[2]++;
            }
        }
        [ServerRpc]
        void SubmitColorEquipoRequestServerRpc(int x,ServerRpcParams rpcParams = default)
        {
            if(x==0){
                Color newColor =coloresGuardados[x];
                colorPlayer.Value=newColor;
            }else if(x==1){
                Color newColor =coloresGuardados[x];
                colorPlayer.Value=newColor;
            }else{
                Color newColor =coloresGuardados[x];
                colorPlayer.Value=newColor;
            }
        }
        [ServerRpc]
        void SubmitPositionEquipoRequestServerRpc(int x,ServerRpcParams rpcParams = default)
        {
            if(x==0){
                Position.Value = GetRandomPositionEquipoOnPlane(x);
            }else if(x==1){
                Position.Value = GetRandomPositionEquipoOnPlane(x);
            }else{
                Position.Value = GetRandomPositionEquipoOnPlane(x);
            }
            
        }
        static Vector3 GetRandomPositionEquipoOnPlane(int x)
        {
            if(x==0){
                return new Vector3(Random.Range(-4f, -2f), 1f, Random.Range(-3f, 3f));
            }else if(x==1){
                return new Vector3(Random.Range(4f, 2f), 1f, Random.Range(-3f, 3f));
            }else{
                return new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-3f, 3f));
            }
        }
        
        void Update()
        {
            transform.position = Position.Value;
        }
    }
}