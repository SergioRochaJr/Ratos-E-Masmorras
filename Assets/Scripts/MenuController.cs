using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI[] mainOptions;
    public TextMeshProUGUI[] skillOptions;
    public TextMeshProUGUI[] attackOptions;
    public TextMeshProUGUI[] attackDirectionOptions;
    public TextMeshProUGUI[] inventoryItems;
    public RectTransform indicator;
    public RectTransform inventoryPanel;
    public RectTransform inventoryIndicator;
    public float spacing = 30f;

    private int currentIndex = 0;
    private int currentMenu = 0;
    private bool isSelectingDirection = false;

    public bool IsInventoryOpen => currentMenu == 2;
    private string[] skillNames = { "Corte Duplo", "Investida" };
    private int selectedSkillIndex = 0;
    private bool isSelectingSkillDirection = false;

    public GameController gameController;

    public PlayerMovement playerMovement;

    void Start()
    {
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
        }

        gameController = FindObjectOfType<GameController>();
        ShowMainMenu();
    }

    void Update()
    {
        if (gameController.isPlayerTurn)
        {
            if (isSelectingDirection)
            {
                HandleDirectionSelection();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isSelectingDirection = false;
                    ShowMainMenu();
                }
            }
            else if (isSelectingSkillDirection)
            {
                HandleSkillDirectionSelection();
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    isSelectingSkillDirection = false;
                    ShowSkillsMenu();
                }
            }
            else
            {
                HandleMenuNavigation();
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                ExecuteOption();
            }

            if (Input.GetKeyDown(KeyCode.Escape) && !isSelectingDirection && !isSelectingSkillDirection)
            {
                GoBack();
            }
        }
    }

    private void HandleDirectionSelection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Atacou para cima!");
            playerMovement.PerformAttack(Vector2.up); // Agora a instância correta é chamada
            EndTurn();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Atacou para a esquerda!");
            playerMovement.PerformAttack(Vector2.left);
            EndTurn();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Atacou para baixo!");
            playerMovement.PerformAttack(Vector2.down);
            EndTurn();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Atacou para a direita!");
            playerMovement.PerformAttack(Vector2.right);
            EndTurn();
        }
    }


    private void HandleSkillDirectionSelection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log($"{skillNames[selectedSkillIndex]} para cima!");
            EndTurn();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log($"{skillNames[selectedSkillIndex]} para esquerda!");
            EndTurn();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log($"{skillNames[selectedSkillIndex]} para baixo!");
            EndTurn();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log($"{skillNames[selectedSkillIndex]} para direita!");
            EndTurn();
        }
    }

    private void HandleMenuNavigation()
    {
        if (currentMenu == 2)
        {
            if (Input.GetKeyDown(KeyCode.W)) { currentIndex = Mathf.Max(0, currentIndex - 1); UpdateIndicator(); }
            else if (Input.GetKeyDown(KeyCode.S)) { currentIndex = Mathf.Min(inventoryItems.Length - 1, currentIndex + 1); UpdateIndicator(); }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W)) { currentIndex = Mathf.Max(0, currentIndex - 1); UpdateIndicator(); }
            else if (Input.GetKeyDown(KeyCode.S)) { currentIndex = Mathf.Min(GetCurrentOptions().Length - 1, currentIndex + 1); UpdateIndicator(); }
        }
    }

    private void ExecuteOption()
    {
        if (!gameController.isPlayerTurn) return;

        switch (currentMenu)
        {
            case 0:
                HandleMainMenuSelection();
                break;
            case 1:
                HandleSkillSelection();
                break;
            case 2:
                UseItem();
                break;
        }
    }

    private void HandleMainMenuSelection()
    {
        switch (currentIndex)
        {
            case 0:
                ShowAttackDirectionMenu();
                break;
            case 1:
                ShowSkillsMenu();
                break;
            case 2:
                ShowInventory();
                break;
        }
    }

    private void HandleSkillSelection()
    {
        selectedSkillIndex = currentIndex;
        ShowSkillDirectionMenu();
    }

    private void ShowMainMenu()
    {
        currentMenu = 0;
        currentIndex = 0;
        ToggleOptions(mainOptions, true);
        ToggleOptions(skillOptions, false);
        ToggleOptions(attackOptions, false);
        ToggleOptions(attackDirectionOptions, false);
        inventoryPanel.gameObject.SetActive(false);
        UpdateIndicator();
    }

    private void ShowAttackDirectionMenu()
    {
        currentMenu = 0;
        isSelectingDirection = true;
        currentIndex = 0;
        ToggleOptions(mainOptions, false);
        ToggleOptions(skillOptions, false);
        ToggleOptions(attackOptions, false);
        ToggleOptions(attackDirectionOptions, true);
        inventoryPanel.gameObject.SetActive(false);
        UpdateIndicator();
    }

    private void ShowSkillsMenu()
    {
        currentMenu = 1;
        currentIndex = 0;
        ToggleOptions(mainOptions, false);
        ToggleOptions(skillOptions, true);
        inventoryPanel.gameObject.SetActive(false);
        UpdateIndicator();
    }

    private void ShowSkillDirectionMenu()
    {
        currentMenu = 1;
        isSelectingSkillDirection = true;
        currentIndex = 0;
        ToggleOptions(mainOptions, false);
        ToggleOptions(skillOptions, false);
        ToggleOptions(attackOptions, false);
        ToggleOptions(attackDirectionOptions, true);
        inventoryPanel.gameObject.SetActive(false);
        UpdateIndicator();
    }

    private void ShowInventory()
    {
        currentMenu = 2;
        currentIndex = 0;
        ToggleOptions(mainOptions, false);
        ToggleOptions(skillOptions, false);
        inventoryPanel.gameObject.SetActive(true);
        indicator.gameObject.SetActive(false);
        inventoryIndicator.gameObject.SetActive(true);
        UpdateIndicator();
    }

    private void GoBack()
    {
        switch (currentMenu)
        {
            case 1:
            case 2:
                ShowMainMenu();
                inventoryIndicator.gameObject.SetActive(false);
                indicator.gameObject.SetActive(true);
                break;
        }
    }

    private TextMeshProUGUI[] GetCurrentOptions()
    {
        switch (currentMenu)
        {
            case 0: return mainOptions;
            case 1: return skillOptions;
            case 2: return inventoryItems;
            default: return new TextMeshProUGUI[0];
        }
    }

    private void ToggleOptions(TextMeshProUGUI[] options, bool state)
    {
        foreach (var option in options)
        {
            option.gameObject.SetActive(state);
        }
    }

    private void UseItem()
    {
        string selectedItem = inventoryItems[currentIndex].text;

        if (selectedItem == "Poção de Vida")
        {
            Debug.Log($"Usando item: {selectedItem}");

            PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(1);
                Debug.Log("Corações recuperados!");
            }
        }
        else
        {
            Debug.Log($"Item {selectedItem} não tem efeito!");
        }

        gameController.EndPlayerTurn();
    }

    private void UpdateIndicator()
    {
        if (currentMenu == 2)
        {
            if (inventoryItems.Length > 0 && currentIndex >= 0 && currentIndex < inventoryItems.Length)
            {
                Vector3 newPosition = inventoryItems[currentIndex].transform.position;
                inventoryIndicator.position = new Vector3(inventoryIndicator.position.x, newPosition.y, newPosition.z);
            }
        }
        else
        {
            TextMeshProUGUI[] options = GetCurrentOptions();
            if (options.Length == 0) return;

            if (currentIndex >= 0 && currentIndex < options.Length)
            {
                Vector3 newPosition = options[currentIndex].transform.position;
                indicator.position = new Vector3(indicator.position.x, newPosition.y, newPosition.z);
            }
        }
    }

    private void EndTurn()
    {
        isSelectingDirection = false;
        isSelectingSkillDirection = false;
        gameController.EndPlayerTurn();
        ShowMainMenu();
    }
}
