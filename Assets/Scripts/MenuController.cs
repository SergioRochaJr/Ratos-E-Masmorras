using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI[] mainOptions; // Opções principais do menu
    public TextMeshProUGUI[] skillOptions; // Opções de habilidades
    public TextMeshProUGUI[] attackOptions; // Opções de ataque
    public TextMeshProUGUI[] attackDirectionOptions; // Direções de ataque (W, A, S, D)
    public TextMeshProUGUI[] inventoryItems; // Opções de itens no inventário
    public RectTransform indicator; // Indicador para mostrar a seleção atual
    public RectTransform inventoryPanel; // Painel do inventário
    public RectTransform inventoryIndicator; // Indicador para o inventário
    public float spacing = 30f; // Espaço entre as opções

    private int currentIndex = 0; // Índice da opção selecionada atualmente
    private int currentMenu = 0; // 0 = Menu principal, 1 = Habilidades, 2 = Inventário
    private bool isSelectingDirection = false; // Se estamos selecionando uma direção para atacar

    public bool IsInventoryOpen => currentMenu == 2; // Retorna true se o inventário estiver aberto
    private string[] skillNames = { "Corte Duplo", "Investida" }; // Habilidades disponíveis
    private int selectedSkillIndex = 0; // Armazena o índice da habilidade selecionada
    private bool isSelectingSkillDirection = false; // Para habilidades


    void Start()
    {
        ShowMainMenu();
    }

    void Update()
{
    if (isSelectingDirection) // Se estiver escolhendo direção de ataque
    {
        HandleDirectionSelection();
        
        // Voltar ao menu principal (ESC) quando estiver na tela de direção
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSelectingDirection = false;
            ShowMainMenu(); // Volta ao menu principal
        }
    }
    else if (isSelectingSkillDirection) // Se estiver escolhendo direção para habilidade
    {
        HandleSkillDirectionSelection();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isSelectingSkillDirection = false;
            ShowSkillsMenu(); // Volta ao menu de habilidades
        }
    }
    else // Navegação nos menus
    {
        HandleMenuNavigation();
    }

    // Confirmar seleção (Enter)
    if (Input.GetKeyDown(KeyCode.Return))
    {
        ExecuteOption();
    }

    // Voltar ao menu anterior (ESC)
    if (Input.GetKeyDown(KeyCode.Escape) && !isSelectingDirection && !isSelectingSkillDirection)
    {
        GoBack();
    }
}


    // Lida com a seleção de direção para ataque
    private void HandleDirectionSelection()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Atacou para cima!");
            isSelectingDirection = false;
            ShowMainMenu(); // Volta ao menu principal após atacar
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("Atacou para a esquerda!");
            isSelectingDirection = false;
            ShowMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Atacou para baixo!");
            isSelectingDirection = false;
            ShowMainMenu();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("Atacou para a direita!");
            isSelectingDirection = false;
            ShowMainMenu();
        }
    }

    private void HandleSkillDirectionSelection()
{
    if (Input.GetKeyDown(KeyCode.W))
    {
        Debug.Log($"{skillNames[selectedSkillIndex]} para cima!"); // Usando o nome da habilidade
        isSelectingSkillDirection = false;
        ShowMainMenu(); // Volta ao menu de habilidades após selecionar a direção
    }
    else if (Input.GetKeyDown(KeyCode.A))
    {
        Debug.Log($"{skillNames[selectedSkillIndex]} para esquerda!"); // Usando o nome da habilidade
        isSelectingSkillDirection = false;
        ShowMainMenu();
    }
    else if (Input.GetKeyDown(KeyCode.S))
    {
        Debug.Log($"{skillNames[selectedSkillIndex]} para baixo!"); // Usando o nome da habilidade
        isSelectingSkillDirection = false;
        ShowMainMenu();
    }
    else if (Input.GetKeyDown(KeyCode.D))
    {
        Debug.Log($"{skillNames[selectedSkillIndex]} para direita!"); // Usando o nome da habilidade
        isSelectingSkillDirection = false;
        ShowMainMenu();
    }
}


    // Lida com a navegação nos menus
    private void HandleMenuNavigation()
    {
        if (currentMenu == 2) // Inventário
        {
            if (Input.GetKeyDown(KeyCode.W)) { currentIndex = Mathf.Max(0, currentIndex - 1); UpdateIndicator(); }
            else if (Input.GetKeyDown(KeyCode.S)) { currentIndex = Mathf.Min(inventoryItems.Length - 1, currentIndex + 1); UpdateIndicator(); }
        }
        else // Menus principais ou de habilidades
        {
            if (Input.GetKeyDown(KeyCode.W)) { currentIndex = Mathf.Max(0, currentIndex - 1); UpdateIndicator(); }
            else if (Input.GetKeyDown(KeyCode.S)) { currentIndex = Mathf.Min(GetCurrentOptions().Length - 1, currentIndex + 1); UpdateIndicator(); }
        }
    }

    // Função para selecionar habilidades
private void ExecuteOption()
{
    switch (currentMenu)
    {
        case 0: // Menu principal
            switch (currentIndex)
            {
                case 0: // Atacar
                    ShowAttackDirectionMenu();
                    break;
                case 1: // Habilidades
                    ShowSkillsMenu();
                    break;
                case 2: // Inventário
                    ShowInventory();
                    break;
            }
            break;
        case 1: // Menu de habilidades
            selectedSkillIndex = currentIndex; // Salva a habilidade selecionada
            ShowSkillDirectionMenu();
            break;
        case 2: // Inventário
            UseItem();
            break;
    }
}



    // Mostra o menu principal
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

    // Mostra o menu de direção de ataque
    private void ShowAttackDirectionMenu()
    {
        currentMenu = 0; // Continua no menu principal
        isSelectingDirection = true;
        currentIndex = 0;
        ToggleOptions(mainOptions, false);
        ToggleOptions(skillOptions, false);
        ToggleOptions(attackOptions, false);
        ToggleOptions(attackDirectionOptions, true); // Exibe as opções de direção
        inventoryPanel.gameObject.SetActive(false);
        UpdateIndicator();
    }

    // Mostra o menu de habilidades
    private void ShowSkillsMenu()
    {
        currentMenu = 1;
        currentIndex = 0;
        ToggleOptions(mainOptions, false);
        ToggleOptions(skillOptions, true);
        inventoryPanel.gameObject.SetActive(false);
        UpdateIndicator();
    }

    // Mostra o menu de direção para habilidades
private void ShowSkillDirectionMenu()
{
    currentMenu = 1; // Continua no menu de habilidades
    isSelectingSkillDirection = true;
    currentIndex = 0;
    ToggleOptions(mainOptions, false);
    ToggleOptions(skillOptions, false);
    ToggleOptions(attackOptions, false);
    ToggleOptions(attackDirectionOptions, true); // Exibe as opções de direção
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

    // Volta ao menu anterior
    private void GoBack()
    {
        switch (currentMenu)
        {
            case 1:
            case 2:
                ShowMainMenu(); // Volta ao menu principal em qualquer situação
                inventoryIndicator.gameObject.SetActive(false);
                indicator.gameObject.SetActive(true);
                break;
        }
    }

    // Retorna as opções do menu atual
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

    // Liga ou desliga opções visuais
    private void ToggleOptions(TextMeshProUGUI[] options, bool state)
    {
        foreach (var option in options)
        {
            option.gameObject.SetActive(state);
        }
    }

    // Função para usar o item no inventário
    private void UseItem()
    {
        Debug.Log($"Usando item: {inventoryItems[currentIndex].text}");
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
}
