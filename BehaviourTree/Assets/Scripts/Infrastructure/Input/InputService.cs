using UnityEngine;

public interface IInputService
{
    public float GetHorizontal();
    public float GetVertical();
}
public class InputService : IInputService
{
    public float GetHorizontal() =>
        Input.GetAxisRaw("Horizontal");

    public float GetVertical() =>
       Input.GetAxisRaw("Vertical");
}