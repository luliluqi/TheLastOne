using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private void Start()
    {
        #region//组件获取
        gameViewAnimator = gameView.GetComponent<Animator>();

        expSlider = exp.GetComponent<Slider>();
        level = exp.transform.Find("Level").GetComponent<TextMeshProUGUI>();

        hpSlider = playerHp.GetComponent<Slider>();

        selectTipBorder = selectAbility.transform.Find("SelectTip");
        select1 = selectAbility.transform.Find("1").GetComponent<Button>();
        select2 = selectAbility.transform.Find("2").GetComponent<Button>();
        select3 = selectAbility.transform.Find("3").GetComponent<Button>();
        okBtn = selectAbility.transform.Find("OK").GetComponent<Button>();
        skipBtn = selectAbility.transform.Find("Skip").GetComponent<Button>();
        fpsText = gameView.transform.Find("Fps").GetComponent<TextMeshProUGUI>();

        contentPanel = buildPanel.transform.Find("ContentPanel");
        info = buildPanel.transform.Find("Info");
        build1 = contentPanel.Find("1").GetComponent<Button>();
        build2 = contentPanel.Find("2").GetComponent<Button>();
        build3 = contentPanel.Find("3").GetComponent<Button>();
        build4 = contentPanel.Find("4").GetComponent<Button>();
        build5 = contentPanel.Find("5").GetComponent<Button>();

        openTreasurePanel = gameView.transform.Find("OpenTreasure");
        weaponNameText = openTreasurePanel.Find("Name").GetComponent<TextMeshProUGUI>();
        icon = openTreasurePanel.Find("Icon").GetComponent<Image>();
        yesBtn = openTreasurePanel.Find("Yes").GetComponent<Button>();
        noBtn = openTreasurePanel.Find("No").GetComponent<Button>();

        propertyPanel = gameView.transform.Find("PropertyPanel");
        Transform valueText = propertyPanel.Find("BackGround").Find("Value");
        for (int i = 0; i < valueText.childCount; i++)
        {
            propertyValueTexts.Add(valueText.GetChild(i).GetComponent<TextMeshProUGUI>());
        }

        gameRunTime = gameView.transform.Find("GameRunTime").GetComponent<TextMeshProUGUI>();
        killCount = gameView.transform.Find("KillCount").GetComponent<TextMeshProUGUI>();
        #endregion

        #region//事件注册
        GameManager._Instance.GameTimeUpdateEvent += UpdateGameTime;
        GameManager._Instance.GameOverEvent += GameOver;
        GameManager._Instance.GameRestartEvent += Restart;

        LevelSystem._Instance.gameObject.GetComponent<PlayerController>().AddListenOpenBuildPanel(OpenOrCloseBuildPanel);
        LevelSystem._Instance.AddListenLevelUp(OpenOrCloseSelectPanel);
        LevelSystem._Instance.AddListenLevelUp(ShowLevel);
        LevelSystem._Instance.AddListenGetExp(ShowExp);
        LevelSystem._Instance.GetComponent<PlayerDataController>().PlayerHpChange += ShowPlayerHp;
        LevelSystem._Instance.GetComponent<PlayerDataController>().DataUpdateEvent += ShowPlayerProperty;

        SelectEvent += BuildController._Instance.GetSelectIndex;

        //绑定选择序号
        select1.onClick.AddListener(() => Select(select1));
        select2.onClick.AddListener(() => Select(select2));
        select3.onClick.AddListener(() => Select(select3));
        okBtn.onClick.AddListener(() =>
        {
            SelectEvent?.Invoke(selectIndex);
            OpenOrCloseSelectPanel();
        });
        skipBtn.onClick.AddListener(OpenOrCloseSelectPanel);

        build1.onClick.AddListener(() => ShowInfo(build1));
        build2.onClick.AddListener(() => ShowInfo(build2));
        build3.onClick.AddListener(() => ShowInfo(build3));
        build4.onClick.AddListener(() => ShowInfo(build4));
        build5.onClick.AddListener(() => ShowInfo(build5));

        yesBtn.onClick.AddListener(TakeTreasure);
        noBtn.onClick.AddListener(AbandonTreasure);

        MyEventSystem._Instance.GenerateDamageText += GenerateDamageText;
        MyEventSystem._Instance.NpcDead += UpdateKillCount;
        MyEventSystem._Instance.LoadTreasureToUI += LoadTreasure;
        #endregion
    }

    [SerializeField]
    GameObject gameView;
    Animator gameViewAnimator;

    //经验条显示
    [SerializeField]
    GameObject exp;
    Slider expSlider;
    TextMeshProUGUI level;
    public void ShowExp(float sliderValue)
    {
        expSlider.value = sliderValue;
    }

    public void ShowLevel()
    {
        level.text = "Level:" + LevelSystem._Instance.Level.ToString();
    }

    void ResetLevel()
    {
        level.text = "Level:" + "0";
    }

    //玩家血条
    [SerializeField]
    GameObject playerHp;
    Slider hpSlider;

    public void ShowPlayerHp(float value)
    {
        hpSlider.value = value;
    }

    //能力选择，只有按下OK按钮才会确认选择
    [SerializeField]
    GameObject selectAbility;
    Transform selectTipBorder;
    Button select1;
    Button select2;
    Button select3;
    Button okBtn;
    Button skipBtn;
    UnityAction<int> SelectEvent;
    int selectIndex;
    bool isInSelect = false;
    public void OpenOrCloseSelectPanel()
    {
        isInSelect = !isInSelect;
        GameManager._Instance.Pause(isInSelect);

        //重置选择框
        selectIndex = 0;
        selectTipBorder.position = select1.transform.position;

        if (isInSelect)
        {
            gameViewAnimator.Play("Base Layer.SelectPanel", 0);
            //展示可选择的被动能力的信息
            var abilityInfos = BuildController._Instance.CreateSelect();

            select1.transform.Find("Icon").GetComponent<Image>().sprite = abilityInfos[0].icon;
            if (abilityInfos[0].icon == null) select1.transform.Find("Icon").gameObject.SetActive(false);
            else select1.transform.Find("Icon").gameObject.SetActive(true);
            select1.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = abilityInfos[0].description;

            select2.transform.Find("Icon").GetComponent<Image>().sprite = abilityInfos[1].icon;
            if (abilityInfos[1].icon == null) select2.transform.Find("Icon").gameObject.SetActive(false);
            else select2.transform.Find("Icon").gameObject.SetActive(true);
            select2.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = abilityInfos[1].description;

            select3.transform.Find("Icon").GetComponent<Image>().sprite = abilityInfos[2].icon;
            if (abilityInfos[2].icon == null) select3.transform.Find("Icon").gameObject.SetActive(false);
            else select3.transform.Find("Icon").gameObject.SetActive(true);
            select3.transform.Find("Description").GetComponent<TextMeshProUGUI>().text = abilityInfos[2].description;
        }
        else
        {
            gameViewAnimator.Play("Base Layer.Empty", 0);
        }
    }

    public void Select(Button clickButton)
    {
        selectTipBorder.position = clickButton.transform.position;
        switch (clickButton.name)
        {
            case "1": selectIndex = 0; break;
            case "2": selectIndex = 1; break;
            case "3": selectIndex = 2; break;
        }
    }

    //宝箱开箱
    [SerializeField]
    Transform openTreasurePanel;
    TextMeshProUGUI weaponNameText;
    Image icon;
    Button yesBtn;
    Button noBtn;
    string weaponName;
    void LoadTreasure(TreasureInfo info)
    {
        GameManager._Instance.Pause(true);
        weaponNameText.text = info.name;
        icon.sprite = info.icon;
        weaponName = info.name;
        gameViewAnimator.Play("Base Layer.OpenTreasurePanel", 0);
    }

    void TakeTreasure()
    {
        GameManager._Instance.Pause(false);
        gameViewAnimator.Play("Base Layer.Empty", 0);
        MyEventSystem._Instance.ChangeWeapon?.Invoke(weaponName);
    }

    void AbandonTreasure()
    {
        GameManager._Instance.Pause(false);
        gameViewAnimator.Play("Base Layer.Empty", 0);
    }

    //能力构建面板
    [SerializeField]
    GameObject buildPanel;
    Transform contentPanel;
    Transform info;
    Button build1;
    Button build2;
    Button build3;
    Button build4;
    Button build5;
    bool isOpenBuildPanel;

    public void OpenOrCloseBuildPanel()
    {
        isOpenBuildPanel = !isOpenBuildPanel;
        if (isOpenBuildPanel)
        {
            gameViewAnimator.Play("Base Layer.BuildPanel", 0);
        }
        else
        {
            if (isInSelect)
            {
                gameViewAnimator.Play("Base Layer.SelectPanel", 0);
            }
            else
            {
                gameViewAnimator.Play("Base Layer.Empty", 0);
            }
        }

        if (!isInSelect) GameManager._Instance.Pause(isOpenBuildPanel);
    }

    public void ShowInfo(Button clickButton)
    {
        if (info.gameObject.activeInHierarchy)
        {
            info.gameObject.SetActive(false);
            return;
        }

        string dsc;
        switch (clickButton.name)
        {
            case "1": dsc = BuildController._Instance.GetBuildDescription(0); break;
            case "2": dsc = BuildController._Instance.GetBuildDescription(1); break;
            case "3": dsc = BuildController._Instance.GetBuildDescription(2); break;
            case "4": dsc = BuildController._Instance.GetBuildDescription(3); break;
            case "5": dsc = BuildController._Instance.GetBuildDescription(4); break;
            default: dsc = null; break;
        }
        info.GetChild(0).GetComponent<TextMeshProUGUI>().text = dsc;
        info.position = clickButton.transform.position;
        info.gameObject.SetActive(true);
    }

    //帧数显示
    TextMeshProUGUI fpsText;
    float second = 1f;
    int fps = 0;

    void GetFPS()
    {
        second -= Time.deltaTime;
        fps++;
        if (second <= 0)
        {
            fpsText.text = "FPS:" + fps.ToString();
            fps = 0;
            second = 1f;
        }
    }

    private void Update()
    {
        GetFPS();
    }

    //伤害飘字
    [SerializeField] Camera mainCam;
    [SerializeField] Transform damageTextParent;
    public void GenerateDamageText(DamageResoult resoult, Vector2 pos)
    {
        Vector2 pointInScreen = mainCam.WorldToScreenPoint(pos);
        if (pointInScreen.x > Screen.width || pointInScreen.y > Screen.height || pointInScreen.x < 0 || pointInScreen.y < 0)
        {
            return;
        }
        var text = MyObjectPool._Instance.GetObjectFromPool(ResourcesLoadManager._Instance.PickUpPath, "DamageText");
        text.transform.SetParent(damageTextParent);
        text.GetComponent<DamagerTextController>().BeCreate(resoult, pos);
    }

    //玩家属性展示
    Transform propertyPanel;
    List<TextMeshProUGUI> propertyValueTexts = new();
    public void ShowPlayerProperty()
    {
        PlayerDataController DC = LevelSystem._Instance.GetComponent<PlayerDataController>();
        propertyValueTexts[0].text = "+" + DC.Hp.ToString();
        propertyValueTexts[1].text = "+" + DC.ExtraHp.ToString();
        propertyValueTexts[2].text = "+" + DC.MoveSpeed.ToString();
        propertyValueTexts[3].text = "+" + (DC.CritRate * 100).ToString() + "%";
        propertyValueTexts[4].text = "+" + (DC.CritDamage * 100).ToString() + "%";
        propertyValueTexts[5].text = "+" + DC.Lucky.ToString();
        propertyValueTexts[6].text = "+" + DC.AutoTreatmentValue.ToString();
        propertyValueTexts[7].text = "+" + DC.TreatmentRate.ToString();
        propertyValueTexts[8].text = "+" + DC.KillTreatment.ToString();
        propertyValueTexts[9].text = "+" + DC.AttackPower.ToString();
        propertyValueTexts[10].text = "+" + DC.Resistance.ToString();
        propertyValueTexts[11].text = "+" + (DC.DodgeRate * 100).ToString() + "%";
        propertyValueTexts[12].text = "+" + DC.PickUpRange.ToString();
    }

    //更新游戏进行时间的显示
    TextMeshProUGUI gameRunTime;

    void UpdateGameTime(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;
        if (seconds < 10)
        {
            gameRunTime.text = minutes + ":" + "0" + seconds;
        }
        else
        {
            gameRunTime.text = minutes + ":" + seconds;
        }
    }

    //玩家击杀npc数量显示
    TextMeshProUGUI killCount;
    void UpdateKillCount()
    {
        killCount.text = "击杀:" + NpcManager._Instance.KillCount;
    }

    //游戏结束面板
    [SerializeField] GameObject gameOverPanel;
    public void GameOver()
    {
        gameView.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        gameView.SetActive(true);
        gameOverPanel.SetActive(false);
        killCount.text = "击杀:";
        ResetLevel();
    }
}
