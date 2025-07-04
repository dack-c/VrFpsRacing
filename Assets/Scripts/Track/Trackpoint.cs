using UnityEngine;

public class Trackpoint : MonoBehaviour
{
    public int index = -1;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out LapController controller))
        {
            var exitPosition = other.transform.position;
            var targetPosition = transform.position;
            var exitDirection = (exitPosition - targetPosition).normalized;

            var angle = Vector3.Angle(transform.forward, exitDirection);
            var index = this.index;

            if (90 < angle || GameManager.I.CurrentTrack.trackpoints.Length == index)
                --index;

            if (index == -1)
                ++index;

            controller.ProcessTrackpoint(this);
        }
    }
}