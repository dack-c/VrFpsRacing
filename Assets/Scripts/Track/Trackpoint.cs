using UnityEngine;

public class Trackpoint : MonoBehaviour
{
    public int index = -1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LapController Car))
        {
            Car.ProcessTrackpoint(this);
        }
    }
}
