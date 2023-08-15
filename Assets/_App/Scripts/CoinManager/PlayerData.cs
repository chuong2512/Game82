using System;
using UnityEngine;

public class Constant
{
    public static string DataKey_PlayerData = "player_data";
    public static int countSong = 19;
    public static int priceUnlockSong = 1000;
}

public class PlayerData : BaseData
{
    public int intDiamond;
    public int currentSong;
    public bool[] listSongs;

    public Action<int> onChangeDiamond;

    public override void Init()
    {
        prefString = Constant.DataKey_PlayerData;
        if (PlayerPrefs.GetString(prefString).Equals(""))
        {
            ResetData();
        }

        base.Init();
    }

    public long time;
    public string timeRegister;

    public void SetTimeRegister(long timeSet)
    {
        timeRegister = DateTime.Now.ToBinary().ToString();
        time = timeSet;
        Save();
    }

    public void ResetTime()
    {
        time = 0;
        Save();
    }

    public override void ResetData()
    {
        intDiamond = 0;
        currentSong = 0;
        timeRegister = DateTime.Now.ToBinary().ToString();
        listSongs = new bool[Constant.countSong];

        for (int i = 0; i < 8; i++)
        {
            listSongs[i] = true;
        }

        Save();
    }

    public bool CheckLock(int id)
    {
        return this.listSongs[id];
    }

    public void Unlock(int id)
    {
        if (!listSongs[id])
        {
            listSongs[id] = true;
        }

        Save();
    }

    public void AddDiamond(int a)
    {
        intDiamond += a;

        onChangeDiamond?.Invoke(intDiamond);
        
        Save();
    }

    public bool CheckCanUnlock()
    {
        return intDiamond >= Constant.priceUnlockSong;
    }

    public void SubDiamond(int a)
    {
        intDiamond -= a;

        if (intDiamond < 0)
        {
            intDiamond = 0;
        }

        onChangeDiamond?.Invoke(intDiamond);
        
        Save();
    }

    public void ChooseSong(int i)
    {
        currentSong = i;
        Save();
    }
}