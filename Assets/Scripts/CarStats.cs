using UnityEngine;


[CreateAssetMenu(fileName = "NewCarStats", menuName = "Car/Car Stats")]
public class CarStats : ScriptableObject
{
   [Header("Car Info")]
    public string carName;
    public float maxSpeed;
    public float handling;

    [Header("Performance")]
    public float maxTorque = 1500f;   // Motorun üretebileceği maksimum tork
    public float brakeForce = 3000f;  // Fren kuvveti
}
