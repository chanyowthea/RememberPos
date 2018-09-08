using RememberPos;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
	public bool _permitInput = true;
	public int _serverLocalPlayerId;
	public bool _useCountMode; // 计数模式还是计时模式
	public int _maxShowedBlocksCount = 4; // 计数模式还是计时模式
    public int _gold;
    public PlayerData _playerData;
}
