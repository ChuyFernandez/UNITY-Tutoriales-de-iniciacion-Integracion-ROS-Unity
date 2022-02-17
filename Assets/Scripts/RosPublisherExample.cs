// Se usan espacios de nombres
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.UnityRoboticsDemo; // De los scripts ubicados en "Assets/RosMessages/UnityRoboticsDemo"
using Unity.Robotics.ROSTCPConnector; // Del script "ROSConnection"

public class RosPublisherExample : MonoBehaviour
{
    // La clase ROSConnection nos permite hacer la conexion con ROS 
    ROSConnection ros;
    // Especificamos el nombre del tema al cual se publicara la posicion y rotacion del cubo
    public string topicName = "pos_rot";
    // Declaramos un objeto de tipo GameObject para asociar al cubo de nuestra escena
    public GameObject cube;
    // Especificamos la frecuencia con la que se estara publicando en el tema
    public float publishMessageFrequency = 2.0f;
    // Variable que usaremos para determinar cuanto tiempo ha transcurrido desde el ultimo mensaje
    private float timeElapsed;

    public void Start()
    {
        /*
        Creamos una instancia de conexion con ROS, ya que es lo que devuelve esta propiedad.
        */
        ros = ROSConnection.GetOrCreateInstance();
        // Registramos este mensaje para publicar
        ros.RegisterPublisher<PosRotMsg>(topicName);
    }

    private void Update()
    {   
        // Time.deltaTime devuelve el tiempo transcurrido desde el último fotograma hasta el actual
        timeElapsed += Time.deltaTime;
        /*
        Si el tiempo transcurrido es mayor a la frecuencia que habiamos establecido volvemos a 
        publicar en el tema.
        */
        if (timeElapsed > publishMessageFrequency)
        {
            // Le asignamos una rotacion aleatoria al cubo
            cube.transform.rotation = Random.rotation;
            // Creamos un objeto de tipo PosRotMsg (tipo de mensaje para el tema "pos_rot")
            PosRotMsg cubePos = new PosRotMsg(
                cube.transform.position.x,
                cube.transform.position.y,
                cube.transform.position.z,
                cube.transform.rotation.x,
                cube.transform.rotation.y,
                cube.transform.rotation.z,
                cube.transform.rotation.w
            );
            /*
            Finalmente enviamos el mensaje al server_endpoint que se encuentra corriendo en ROS
            para que publique en el tema "pos_rot".
            */
            ros.Publish(topicName, cubePos);
            // Reiniciamos la variable para el tiempo transcurrido 
            timeElapsed = 0;
        }
    }
}

