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
        public NetworkVariable<int> NumeroEquipo = new NetworkVariable<int>();
        public static List<Color> coloresGuardados= new List<Color>();
        public static List<int> ListaEquipo= new List<int>();
        
        public static int numeroMaximo=1;

        //public static List<int> equipo1 = new List<int>();
        
        private Renderer enlace;

        public void Start(){
            Position.OnValueChanged += OnPositionChange;
            colorPlayer.OnValueChanged += OnColorChange;
            enlace= GetComponent<Renderer>();
            // Por si en el futuro hay mas equipos
        }
        public void OnPositionChange(Vector3 previousValue, Vector3 newValue){
            transform.position=Position.Value;
        }
        public void OnColorChange(Color previousValue, Color newValue){
            enlace.material.color = newValue;
        }
        public override void OnNetworkSpawn()
        {
            if(IsServer && IsOwner){
                ListaEquipo.Add(0);
                ListaEquipo.Add(0);
                ListaEquipo.Add(0);
                coloresGuardados.Add(Color.white);
                coloresGuardados.Add(Color.blue);
                coloresGuardados.Add(Color.red);
            }
            if (IsOwner)
            {
                PlayerEquipo(-1);
            }
        }
        public void PlayerEquipo(int x){
            SubmitPlayerEquipoRequestServerRpc(x);
        }
        [ServerRpc]
        void SubmitPlayerEquipoRequestServerRpc(int NumeroEquipox, ServerRpcParams rpcParams = default)
        {
            Debug.Log("Primera "+ListaEquipo[0]);
            Debug.Log("Segunda "+ListaEquipo[1]);
            Debug.Log("Tercera "+ListaEquipo[2]);
            if(NumeroEquipox==-1){
                NumeroEquipo.Value = 0;
                Position.Value = GetRandomPositionEquipoOnPlane(0);
                Color newColor =coloresGuardados[0];
                colorPlayer.Value=newColor;
                ListaEquipo[0]++;
            }
            else if(ListaEquipo[NumeroEquipox]<= numeroMaximo || NumeroEquipox== 0){
                
                ListaEquipo[NumeroEquipo.Value]--;
                NumeroEquipo.Value = NumeroEquipox;
                Position.Value = GetRandomPositionEquipoOnPlane(NumeroEquipox);
                Color newColor =coloresGuardados[NumeroEquipo.Value];
                colorPlayer.Value=newColor;
                ListaEquipo[NumeroEquipo.Value]++;
                Debug.Log("Primera "+ListaEquipo[0]);
                Debug.Log("Segunda "+ListaEquipo[1]);
                Debug.Log("Tercera "+ListaEquipo[2]);
            }else{
                Debug.Log("Equipo "+ NumeroEquipox + "lleno");
            }
        }
        static Vector3 GetRandomPositionEquipoOnPlane(int x)
        {
            if(x==0){
                return new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-3f, 3f));
            }else if(x==1){
                return new Vector3(Random.Range(-4f, -2f), 1f, Random.Range(-3f, 3f));
            }else{
                return new Vector3(Random.Range(4f, 2f), 1f, Random.Range(-3f, 3f));
            }
        }
        
        void Update()
        {
            transform.position = Position.Value;
        }
    }
}