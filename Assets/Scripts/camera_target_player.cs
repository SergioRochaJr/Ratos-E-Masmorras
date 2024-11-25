    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class CameraTargetMovement : MonoBehaviour
    {
        public float moveSpeed = 5f; // Velocidade do movimento
        public float gridSize = 1f;  // Tamanho do tile (ex: 1 unidade)

        private Vector2 targetPosition; // Posi��o alvo para onde o personagem vai se mover
        private bool isMoving = false;  // Indica se o personagem est� se movendo

        // Refer�ncia ao Tilemap para verificar a colis�o com as paredes
        public Tilemap tilemap;
        public LayerMask paredeLayer; // Camada onde as paredes est�o (ex: camada "Parede")

        void Start()
        {
            targetPosition = transform.position; // Define a posi��o inicial como alvo
        }

        void Update()
        {
            // Somente permite o input se n�o estiver se movendo
            if (!isMoving)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) && CanMove(Vector2.up))
                    StartMovement(Vector2.up);
                else if (Input.GetKeyDown(KeyCode.DownArrow) && CanMove(Vector2.down))
                    StartMovement(Vector2.down);
                else if (Input.GetKeyDown(KeyCode.LeftArrow) && CanMove(Vector2.left))
                    StartMovement(Vector2.left);
                else if (Input.GetKeyDown(KeyCode.RightArrow) && CanMove(Vector2.right))
                    StartMovement(Vector2.right);
            }

            // Move o personagem em dire��o ao alvo
            if (isMoving)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // Checa se o personagem chegou ao destino
                if ((Vector2)transform.position == targetPosition)
                    isMoving = false;
            }
        }

        // Inicia o movimento em uma dire��o espec�fica
        private void StartMovement(Vector2 direction)
        {
            targetPosition = (Vector2)transform.position + direction * gridSize; // Define o pr�ximo tile como alvo
            isMoving = true;
        }

        // Checa se � poss�vel mover para a dire��o fornecida (verifica se h� uma parede)
        private bool CanMove(Vector2 direction)
        {
            // Calcula a posi��o de destino
            Vector2 newPos = (Vector2)transform.position + direction * gridSize;

            // Verifica se o tile de destino est� bloqueado (com a camada "Parede")
            Vector3Int tilePosition = tilemap.WorldToCell(newPos); // Converte a posi��o para o grid do Tilemap
            TileBase tileAtPosition = tilemap.GetTile(tilePosition);

            // Verifica se h� um tile de parede no destino
            if (tileAtPosition != null && tilemap.GetColliderType(tilePosition) != Tile.ColliderType.None)
            {
                // Se houver um tile de parede (ou outro objeto que bloqueia o movimento), o movimento n�o � permitido
                return false;
            }

            // Se n�o houver colis�o, o movimento � permitido
            return true;
        }
    }