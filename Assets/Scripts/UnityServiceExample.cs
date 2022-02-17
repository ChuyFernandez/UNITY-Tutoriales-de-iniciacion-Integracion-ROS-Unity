// Se usan espacios de nombres
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosMessageTypes.UnityRoboticsDemo; // De los scripts ubicados en "Assets/RosMessages/UnityRoboticsDemo"
using Unity.Robotics.ROSTCPConnector; // Del script "ROSConnection"
using Unity.Robotics.ROSTCPConnector.ROSGeometry; // Se utiliza para las conversiones de sistemas de coordenadas

public class UnityServiceExample : MonoBehaviour
{
    // La clase ROSConnection nos permite hacer la conexion con ROS 
    ROSConnection ros;
    // Especificamos el nombre del servicio
    public string serviceName = "obj_pose_srv";
 
    public void Start()
    {
        // Creamos una instancia de conexion con ROS, ya que es lo que devuelve esta propiedad.
        ros = ROSConnection.GetOrCreateInstance();
        /*
        Implementamos un servicio llamado "obj_pose_srv" el cual estara capturando las solicitudes
        en la funcion de devolucion de llamada "GetObjectPose" y devolvera una respuesta.
        */
        ros.ImplementService<ObjectPoseServiceRequest, ObjectPoseServiceResponse>(serviceName, GetObjectPose);
    }

    private void Update()
    {
         // Aqui no se hace nada
    }

    private ObjectPoseServiceResponse GetObjectPose(ObjectPoseServiceRequest req){
        ObjectPoseServiceResponse res=new ObjectPoseServiceResponse();
        GameObject gameObject=GameObject.Find(req.object_name);
        if(gameObject!=null){
            // Se convierte del sistema de coordenadas Unity al sistema de coordenadas ROS.
            res.object_pose.position=gameObject.transform.position.To<FLU>();
            res.object_pose.orientation=gameObject.transform.rotation.To<FLU>();
        }
        return res;
    }
}
