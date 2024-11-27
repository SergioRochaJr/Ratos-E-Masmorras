using UnityEngine;
using TMPro;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI[] mainOptions; // Opções principais do menu
    public TextMeshProUGUI[] skillOptions; // Opções de habilidades
    public RectTransform indicator; // Indicador para mostrar a seleção atual
    public RectTransform inventoryPanel; // Painel do inventário
    public float spacing = 30f; // Espaço entre as opções

    private int currentIndex = 0; // Índice da opção selecionada atualmente
    private int currentMenu = 0; // 0 = Menu principal, 1 = Habilidades, 2 = Inventário

    void Start()
    {
        ShowMainMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)) // Move para cima
        {
            currentIndex = Mathf.Max(0, currentIndex - 1);
            UpdateIndicator();
        }
        else if (Input.GetKeyDown(KeyCode.S)) // Move para baixo
        {
            int optionsLength = GetCurrentOptions().Length;
            currentIndex = Mathf.Min(optionsLength - 1, currentIndex + 1);
            UpdateIndicator();
        }

        if (Input.GetKeyDown(KeyCode.Return)) // Confirma a seleção (Enter)
        {
            ExecuteOption();
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // Volta ao menu anterior (ESC)
        {
            GoBack();
        }
    }

    // Atualiza a posição do indicador
    private void UpdateIndicator()
{
    if (currentMenu == 2) // Não atualiza o indicador no inventário
        return;

    TextMeshProUGUI[] options = GetCurrentOptions();
    if (options.Length == 0) return;

    Vector3 newPosition = options[currentIndex].transform.position;
    indicator.position = new Vector3(indicator.position.x, newPosition.y, newPosition.z);
}

    // Executa a ação correspondente à opção selecionada
    private void ExecuteOption()
    {
        switch (currentMenu)
        {
            case 0: // Menu principal
                switch (currentIndex)
                {
                    case 0:
                        Debug.Log("Atacar selecionado!");
                        break;
                    case 1:
                        ShowSkillsMenu();
                        break;
                    case 2:
                        ShowInventory();
                        break;
                }
                break;
            case 1: // Menu de habilidades
                Debug.Log($"Habilidade {currentIndex + 1} selecionada!");
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

    // Mostra o inventário
    private void ShowInventory()
{
    currentMenu = 2;
    currentIndex = 0;
    ToggleOptions(mainOptions, false); // Desativa opções do menu principal
    ToggleOptions(skillOptions, false); // Desativa opções de habilidades
    inventoryPanel.gameObject.SetActive(true); // Ativa o painel do inventário
    indicator.gameObject.SetActive(false); // Oculta o indicador
}

    // Volta ao menu anterior
    private void GoBack()
{
    switch (currentMenu)
    {
        case 1: // Se estiver no menu de habilidades
            ShowMainMenu();
            break;
        case 2: // Se estiver no inventário
            ShowMainMenu();
            indicator.gameObject.SetActive(true); // Reativa o indicador
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
}
