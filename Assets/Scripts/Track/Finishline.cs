using UnityEngine;

public class Finishline : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out LapController controller))
        {
            controller.ProcessFinishline();
        }
    }
}
