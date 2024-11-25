using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Refer�ncia ao personagem
    public Vector3 offset; // Offset opcional para ajustar a posi��o da c�mera

    void Update()
    {
        // Atualiza a posi��o da c�mera para seguir o personagem com o offset
        transform.position = target.position + offset;
    }
}