using UnityEngine;
using Zenject;

public class PlayerMove : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 direction;
    private float speed = 10f;

    private IInputService input;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    [Inject]
    public void Construct(IInputService _input)
    {
        input = _input;
    }

    public void Update()
    {
        float hor = input.GetHorizontal();
        float vert = input.GetVertical();

        direction = new Vector3(hor, 0, vert);
    }

    public void FixedUpdate()
    {
        controller.Move(direction * speed * Time.fixedDeltaTime);
    }
}