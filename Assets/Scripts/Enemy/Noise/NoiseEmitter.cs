using UnityEngine;

public class NoiseEmitter : MonoBehaviour
{
    [Header("Runtime")]
    public float CurrentRadius = 0f;

    public System.Action<NoiseEventType, float> OnNoiseChanged;

    public void SetNoiseRadius(float r)
    {
        NoiseEventType type;

        if (CurrentRadius == 0 && r > 0) type = NoiseEventType.Started;
        else if (r == 0 && CurrentRadius > 0) type = NoiseEventType.Stopped;
        else if (r > CurrentRadius) type = NoiseEventType.Increased;
        else type = NoiseEventType.Decreased;

        CurrentRadius = r;

        OnNoiseChanged?.Invoke(type, r);
    }

}

public enum NoiseEventType
{
    Started,
    Increased,
    Decreased,
    Stopped
}