// Se usan espacios de nombres
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.UnityRoboticsDemo; // De los scripts ubicados en "Assets/RosMessages/UnityRoboticsDemo"
using Unity.Robotics.ROSTCPConnector; // Del script "ROSConnection"

public class RosSubscriberExample : MonoBehaviour
{
    // La clase ROSConnection nos permite hacer la conexion con ROS 
    ROSConnection ros;
    // Especificamos el nombre del tema al cual se publicara la posicion y rotacion del cubo
    public string topicName = "color";
    // Declaramos un objeto de tipo GameObject para asociar al cubo de nuestra escena
    public GameObject cube;
    public void Start()
    {
        // Creamos una instancia de conexion con ROS, ya que es lo que devuelve esta propiedad.
        ros = ROSConnection.GetOrCreateInstance();
        /*
        Nos suscribimos al tema "color" del cual estaremos recibiendo un color aleatorio en 
        nuestra funcion de devolucion de llamada "ColorChange" para pintar nuestro cubo.
        */
        ros.Subscribe<UnityColorMsg>("color", ColorChange);
    }

    private void Update() {
        // Aqui no se hace nada
    }

    // El objeto de tipo UnityColorMsg es el tipo de mensaje para el tema "color"
    public void ColorChange(UnityColorMsg colorMessage)
    {
        /*
        Obtenemos el componente Renderer para acceder a la propiedad material para poder 
        cambiar el color del objeto.
        Color32.Color32(byte r, byte g, byte b, byte a)
        */
        cube.GetComponent<Renderer>().material.color = new Color32((byte)colorMessage.r, (byte)colorMessage.g, (byte)colorMessage.b, (byte)colorMessage.a);
    }
}
