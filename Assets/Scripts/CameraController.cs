using UnityEngine;

public class CameraController : HCMonobehavior
{
    public static CameraController Instance;

    public float TransitionSpeed;

    public Transform TargetFollow;
    public Vector3 Offset;

    private void Awake()
    {
        Instance = this;
        Offset = Transform.position - TargetFollow.transform.position;
        originOffset = Offset;
    }

    private Vector3 originOffset;

    public void ResetOffset()
    {
        Offset = originOffset;
    }

    private void LateUpdate()
    {
        if (TargetFollow == null)
            return;

        Transform.position = Vector3.Lerp(Transform.position,
            TargetFollow.position + Offset, Time.deltaTime * TransitionSpeed);
    }
}