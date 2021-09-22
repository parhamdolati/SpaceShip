using System.Collections;
using UnityEngine;


public class SpaceShipHandler : MonoBehaviour
{
    public GameObject Spaceship;
    public float SpaceShipSpeed = 3;
    public float SpaceShipRotateSpeed = 3;
    

    public IEnumerator StartSpaceShipControl()
    {
        while (true)
        {
            //harakate safine ru be jolo
            Spaceship.transform.Translate(Vector3.forward*SpaceShipSpeed*Time.deltaTime);
        
            //charkhesh safine
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    SpaceShipForward();
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    SpaceShipBack();
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
    
    public void SpaceShipForward()
    {
        Spaceship.transform.Rotate(Vector3.forward * SpaceShipRotateSpeed);
    }

    public void SpaceShipBack()
    {
        Spaceship.transform.Rotate(Vector3.back * SpaceShipRotateSpeed);
    }
}
