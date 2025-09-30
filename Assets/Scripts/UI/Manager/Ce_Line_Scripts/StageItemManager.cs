using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageItemManager : MonoBehaviour
{
    [SerializeField] private GameObject[] starActive;
    [SerializeField] private GameObject[] starInactive;
    [SerializeField] private TextMeshProUGUI stageText;
    [SerializeField] private GameObject starPrize;
    [SerializeField] private Button stageButton;
    [SerializeField] private Image stageColor;

    [SerializeField] private Sprite stageActiveColor;
    [SerializeField] private Sprite stageInactiveColor;
    [SerializeField] private Sprite stageNextColor;

    private Color textActiveColor = Color.white;
    private Color textInactiveColor = new Color(0.5568628f, 0.5568628f, 0.5568628f);
    private Color textNextColor = Color.white;

    private int stage = 0;

    public void Setup(int stage, bool isPrize, int starShown, bool isShown, bool isNext)
    {
        this.stage = stage;
        stageText.text = stage.ToString();

        if (isShown)
        {
            stageButton.onClick.RemoveAllListeners();
            stageButton.onClick.AddListener(OnClick_StageButton);

            stageColor.sprite = !isNext ? stageActiveColor : stageNextColor;
            stageText.color = !isNext ? textActiveColor : textNextColor;

            if (!isPrize)
            {
                starPrize.SetActive(false);
                stageText.gameObject.SetActive(true);
            }
            else
            {
                starPrize.SetActive(true);
                stageText.gameObject.SetActive(false);
            }

            foreach (GameObject item in starActive)
            {
                item.SetActive(false);
            }

            foreach (GameObject item in starInactive)
            {
                item.SetActive(false);
            }

            for (int i = 0; i < starShown; i++)
            {
                starActive[i].SetActive(true);
            }

            for (int i = 0; i < Player.TOTAL_STAR_PER_STAGE - starShown; i++)
            {
                starInactive[i].SetActive(true);
            }
        }
        else
        {
            stageColor.sprite = stageInactiveColor;
            stageText.color = textInactiveColor;
            foreach (GameObject item in starActive)
            {
                item.SetActive(false);
            }

            foreach (GameObject item in starInactive)
            {
                item.SetActive(false);
            }
        }
    }

    private void OnClick_StageButton()
    {
        if (SystemManager.excludeButton)
            return;
        
        Player.Data.currentStage = stage;
        HomeUIManager.OnClick_PlayButton();
        LevelUIManager.Out();
    }
}