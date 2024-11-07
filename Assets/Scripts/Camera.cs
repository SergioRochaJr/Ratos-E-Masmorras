using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Referência ao personagem
    public Vector3 offset; // Offset opcional para ajustar a posição da câmera

    void Update()
    {
        // Atualiza a posição da câmera para seguir o personagem com o offset
        transform.position = target.position + offset;
    }
}