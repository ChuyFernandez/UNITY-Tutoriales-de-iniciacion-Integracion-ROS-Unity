// Se importan esppacios de nombres
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.UnityRoboticsDemo; // De los scripts ubicados en "Assets/RosMessages/UnityRoboticsDemo"
using Unity.Robotics.ROSTCPConnector; // Del script "ROSConnection"

public class RosServiceExample : MonoBehaviour
{
    // La clase ROSConnection nos permite hacer la conexion con ROS 
    ROSConnection ros;
    // Especificamos el nombre del servicio al que llamaremos
    public string serviceName = "pos_srv";
    // Declaramos un objeto de tipo GameObject para asociar al cubo de nuestra escena
    public GameObject cube;
    // Vector que guarda la posicion de destino a donde tiene que viajar el cubo
    public Vector3 destination;
    // Velocidad del cubo
    public float speed=2.0f;
    // Distancia minima entre la posicion del cubo y del destino
    public float minDistance=2;
    public bool serviceAnsweredUs=false, isDone;
    public void Start()
    {
        // Creamos una instancia de conexion con ROS, ya que es lo que devuelve esta propiedad.
        ros = ROSConnection.GetOrCreateInstance();
        /*
        Registramos un servicio llamado "pos_srv" el cual se encuentra ejecutandose en ROS y
        estara capturando las solicitudes en su funcion de devolucion de llamada y devolvera 
        una respuesta.
        */
        ros.RegisterRosService<PositionServiceRequest, PositionServiceResponse>(serviceName);
        // Destino igual a la posicion actual del cubo
        destination=cube.transform.position;
        //destination=randomPosition(cube); 
    }

    private void Update()
    {   
        // "Time.time" devuelve la cantidad de segundos que han transcurrido desde que el proyecto comenzó a reproducirse
        isDone=Vector3.Distance(cube.transform.position, destination)<minDistance;
        float step=speed*Time.deltaTime;

        // Si el servicio no ha respondido no es necesario mover el cubo
        if(serviceAnsweredUs){
            cube.transform.position=Vector3.MoveTowards(cube.transform.position, destination, step);
        }

        if(isDone){
            // Creamos una instancia del tipo de mensaje de solicitud
            PosRotMsg cubePos=new PosRotMsg(
                cube.transform.position.x,
                cube.transform.position.y,
                cube.transform.position.x,
                cube.transform.rotation.x,
                cube.transform.rotation.y,
                cube.transform.rotation.z,
                cube.transform.rotation.w
            );
            // Creamos una instancia del tipo de objeto de solicitud
            PositionServiceRequest req=new PositionServiceRequest(cubePos);
            serviceAnsweredUs=false;
            ros.SendServiceMessage<PositionServiceResponse>(serviceName, req, CallbackDestination);
        }

        /*
        if(Vector3.Distance(cube.transform.position, destination)<minDistance){
            destination=randomPosition(cube);
        }
        */
    }

    // Metodo para capturar las devoluciones de llamada del servicio (la respuesta)
    private void CallbackDestination(PositionServiceResponse res){
        serviceAnsweredUs=true;
        destination=new Vector3(res.output.pos_x, res.output.pos_y, res.output.pos_z);
        Debug.Log("New destination: "+destination);
    }

    // Metodo de prueba
    // Posicion aleatoria dentro del plano (el plano esta colocado en el origen)
    private Vector3 randomPosition(GameObject gameObject){
        Vector3 gameObjectPos=gameObject.transform.position;
        float rangeX=4, rangeZ=4;
        float x=Random.Range(-rangeX, rangeX);
        float z=Random.Range(-rangeZ, rangeZ);
        return new Vector3(x, gameObjectPos.y, z);
    }
}
