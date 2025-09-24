using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelSystem : BaseSingleton<LevelSystem>
{
    [SerializeField]
    int level = 0;
    public int Level {  get { return level; } }

    [SerializeField]
    int levelGrowth;

    [SerializeField]
    int levelUpExp = 5;
    [SerializeField]
    int maxLevelUpExp;
    int currentExp;

    /// <summary>
    /// 升级事件
    /// </summary>
    protected UnityAction LevelUpEvent;
    public void AddListenLevelUp(UnityAction func)
    {
        LevelUpEvent += func;
    }
    public void RemoveListenLevelUp(UnityAction func)
    {
        LevelUpEvent -= func;
    }

    /// <summary>
    /// 获取经验事件
    /// </summary>
    protected UnityAction<float> GetExpEvent;
    public void AddListenGetExp(UnityAction<float> func)
    {
        GetExpEvent += func;
    }

    public void RemoveListenGetExp(UnityAction<float> func)
    {
        GetExpEvent -= func;
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        GameManager._Instance.GameRestartEvent += ResetLevel;
    }

    public void GetExp(int ExpValue)
    {
        currentExp += ExpValue;
        LevelUp();
        GetExpEvent?.Invoke(MathF.Round((float)currentExp / (float)levelUpExp, 2));
    }

    /// <summary>
    /// 升级
    /// </summary>
    protected void LevelUp()
    {
        if (currentExp < levelUpExp)
        {
            return;
        }

        level++;
        currentExp -= levelUpExp;

        LevelUpEvent?.Invoke();

        if (levelUpExp < maxLevelUpExp)
        {
            //升级所需经验递增
            levelUpExp = levelUpExp + levelGrowth;
        }
    }

    public void ResetLevel()
    {
        level = 0;
        currentExp = 0;
        levelUpExp = 5;
        GetExp(0);
        transform.position = Vector2.zero;
    }
}
