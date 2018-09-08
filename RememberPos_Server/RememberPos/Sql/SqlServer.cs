using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using RememberPos; 

namespace RememberPos
{
    namespace Sql
    {
        public enum EAccountResult
        {
            Success,
            AccountNotExist,
            PasswordNotCorrect,
        }
    }

    public class SqlServer
    {
        SqlAccess _sql;
        const string _accountTableName = "Account";
        const string _assetTableName = "Asset";

        public SqlServer()
        {
            _sql = new SqlAccess();
        }

        public void CreateAccountTable(bool isDeleteTable = false)
        {
            if (isDeleteTable)
            {
                _sql.DeleteTable(_accountTableName); 
            }
            _sql.CreateTable(_accountTableName,
                new string[] { "id", "accountName", "password", "nickName", "blockIdsInUse",
                    "blockBoardIdInUse", "boardIdInUse", "personIdInUse", "lastLoginTime", "lastObtainGoldTime"} 
                , new string[] { "int", "text", "text", "text", "text",
                    "int", "int", "int", "BIGINT", "BIGINT" });
        }

        public Sql.EAccountResult CheckAccount(string accountName, string password)
        {
            CreateAccountTable();
            // 查询帐号是否存在
            int count = _sql.GetCount(_accountTableName, new MySqlParameter("?" + "accountName", accountName));
            if (count == 0)
            {
                return Sql.EAccountResult.AccountNotExist;
            }

            // 查询密码是否正确
            count = _sql.GetCount(_accountTableName, new MySqlParameter("?" + "accountName", accountName)
                    , new MySqlParameter("?" + "password", password));
            if (count == 0)
            {
                return Sql.EAccountResult.PasswordNotCorrect;
            }
            return Sql.EAccountResult.Success;
        }

        public LobbyPlayerData GetLobbyPlayerData(string accountName)
        {
            CreateAccountTable();
            // 查询帐号是否存在
            int count = _sql.GetCount(_accountTableName, new MySqlParameter("?" + "accountName", accountName));
            if (count == 0)
            {
                Singleton._log.Info("帐号不存在! accountName=" + accountName);
                return null;
            }

            // 注意，这里的读取数据的顺序必须和LobbyPlayerData里面的字段顺序一致
            var list = _sql.ReaderInfo(_accountTableName, new string[] { "id", "accountName", "nickName" }
                    , new MySqlParameter("?" + "accountName", accountName));
            if (list.Count == 0)
            {
                Singleton._log.Info("没有相关信息! accountName=" + accountName);
                return null;
            }
            var p = new LobbyPlayerData();
            p.SetValues(list[0]);
            Singleton._log.Info("GetLobbyPlayerData nickName=" + p._nickName); 
            return p;
        }

        public PlayerData GetPlayerData(string accountName)
        {
            Singleton._log.Info("GetPlayerData! accountName=" + accountName);
            CreateAccountTable();
            // 查询帐号是否存在
            int count = _sql.GetCount(_accountTableName, new MySqlParameter("?" + "accountName", accountName));
            if (count == 0)
            {
                Singleton._log.Info("帐号不存在! accountName=" + accountName);
                return null;
            }

            // 注意，这里的读取数据的顺序必须和Player里面的字段顺序一致
            var list = _sql.ReaderInfo(_accountTableName, new string[]
            { "id", "accountName", "nickName", "blockIdsInUse", "blockBoardIdInUse",
                "boardIdInUse", "personIdInUse", "lastLoginTime", "lastObtainGoldTime"}
                    , new MySqlParameter("?" + "accountName", accountName));
            if (list.Count == 0)
            {
                Singleton._log.Info("没有相关信息! accountName=" + accountName);
                return null;
            }
            var p = new PlayerData();
            p.SetValues(list[0]);
            return p;
        }

        public int Register(PlayerData data, string password)
        {
            if (data == null)
            {
                return 1;
            }

            if (string.IsNullOrEmpty(password))
            {
                return 2;
            }

            _sql.UpdateInto(_accountTableName, new MySqlParameter("?" + "accountName", data._accountName),
                new MySqlParameter("?" + "accountName", data._accountName),
                new MySqlParameter("?" + "password", password),
                new MySqlParameter("?" + "nickName", data._nickName),
                new MySqlParameter("?" + "blockIdsInUse", data._blockIdsInUse.PackToStringInt()),
                new MySqlParameter("?" + "blockBoardIdInUse", data._blockBoardIdInUse),
                new MySqlParameter("?" + "boardIdInUse", data._boardIdInUse),
                new MySqlParameter("?" + "personIdInUse", data._personIdInUse),
                new MySqlParameter("?" + "lastLoginTime", data._lastLoginTime),
                new MySqlParameter("?" + "lastObtainGoldTime", data._lastObtainGoldTime));
            return 0;
        }

        public int SavePlayer(PlayerData data)
        {
            if (data == null)
            {
                Singleton._log.Info("SavePlayer data is empty! "); 
                return 1; 
            }

            Singleton._log.Info("SavePlayer accountName=" + data._accountName);
            _sql.UpdateInto(_accountTableName, new MySqlParameter("?" + "accountName", data._accountName),
                new MySqlParameter("?" + "nickName", data._nickName),
                new MySqlParameter("?" + "blockIdsInUse", data._blockIdsInUse.PackToStringInt()),
                new MySqlParameter("?" + "blockBoardIdInUse", data._blockBoardIdInUse),
                new MySqlParameter("?" + "boardIdInUse", data._boardIdInUse),
                new MySqlParameter("?" + "personIdInUse", data._personIdInUse),
                new MySqlParameter("?" + "lastLoginTime", data._lastLoginTime),
                new MySqlParameter("?" + "lastObtainGoldTime", data._lastObtainGoldTime));
            return 0; 
        }

        // 这里装备之后，匹配玩家要重新
        public int DecorateItem(string accountName, int type, bool isDecorate, int id)
        {
            if (string.IsNullOrEmpty(accountName) || id < 0)
            {
                Singleton._log.Info("DecorateItem accountName is empty! ");
                return 1;
            }

            Singleton._log.Info("DecorateItem accountName=" + accountName);
            switch (type)
            {
                case 0:
                    // 卸下只能对block而言
                    var p = Singleton._sqlServer.GetPlayerData(accountName);
                    List<int> ids = new List<int>(p._blockIdsInUse);
                    if (isDecorate)
                    {
                        if (ids.Contains(id))
                        {
                            Singleton._log.Info("已经装备了该物品！id=" + id);
                        }
                        else
                        {
                            ids.Add(id); 
                        }
                    }
                    else
                    {
                        if (ids.Contains(id))
                        {
                            ids.Remove(id); 
                        }
                        else
                        {
                            Singleton._log.Info("已经卸下了该物品！id=" + id);
                        }
                    }
                    _sql.UpdateInto(_accountTableName, new MySqlParameter("?" + "accountName", accountName),
                        new MySqlParameter("?" + "blockIdsInUse", (ids.ToArray()).PackToStringInt()));
                    break;
                case 1: 
                    _sql.UpdateInto(_accountTableName, new MySqlParameter("?" + "accountName", accountName),
                        new MySqlParameter("?" + "blockBoardIdInUse", id));
                    break;
                case 2:
                    _sql.UpdateInto(_accountTableName, new MySqlParameter("?" + "accountName", accountName),
                        new MySqlParameter("?" + "boardIdInUse", id));
                    break;
                case 3:
                    _sql.UpdateInto(_accountTableName, new MySqlParameter("?" + "accountName", accountName),
                        new MySqlParameter("?" + "personIdInUse", id));
                    break; 
                default: 
                    Singleton._log.InfoFormat("DecorateItem 找不到对应类型 type={0}! ", type); 
                    return 2; 
            }
            return 0;
        }
        
        public void CreateAssetTable(bool isDeleteTable = false)
        {
            if (isDeleteTable)
            {
                _sql.DeleteTable(_assetTableName);
            }
            _sql.CreateTable(_assetTableName, 
                new string[] { "id", "accountName", "gold", "blocks", "blockBoards", "boards", "persons" }
                , new string[] { "int", "text", "int", "text", "text", "text", "text" }); 
        }

        public int ChangeAsset(AssetData data)
        {
            if (data == null)
            {
                Singleton._log.Info("ChangeAsset data is empty! ");
                return 1;
            }

            //Singleton._log.Info("ChangeAsset accountName=" + data._accountName);
            _sql.UpdateInto(_assetTableName, new MySqlParameter("?" + "accountName", data._accountName),
                new MySqlParameter("?" + "accountName", data._accountName), 
                new MySqlParameter("?" + "gold", data._gold),
                new MySqlParameter("?" + "blocks", data._blocks.PackToStringInt()),
                new MySqlParameter("?" + "blockBoards", data._blockBoards.PackToStringInt()),
                new MySqlParameter("?" + "boards", data._boards.PackToStringInt()),
                new MySqlParameter("?" + "persons", data._persons.PackToStringInt()));
            return 0;
        }

        public int ChangeGold(string accountName, int gold)
        {
            if (string.IsNullOrEmpty(accountName))
            {
                Singleton._log.Info("accountName is empty! ");
                return 1;
            }
            Singleton._log.Info("ChangeGold accountName=" + accountName);
            _sql.UpdateInto(_assetTableName, new MySqlParameter("?" + "accountName", accountName),
                new MySqlParameter("?" + "gold", gold));
            return 0;
        }

        public AssetData GetAssetData(string accountName)
        {
            CreateAssetTable(); 
            // 查询帐号是否存在
            int count = _sql.GetCount(_assetTableName, new MySqlParameter("?" + "accountName", accountName));
            if (count == 0)
            {
                Singleton._log.Info("帐号不存在! accountName=" + accountName);
                return null;
            }

            // 注意，这里的读取数据的顺序必须和Player里面的字段顺序一致
            var list = _sql.ReaderInfo(_assetTableName, 
                new string[]{ "accountName", "gold", "blocks", "blockBoards", "boards", "persons"}
                    , new MySqlParameter("?" + "accountName", accountName));
            if (list.Count == 0)
            {
                Singleton._log.Info("没有相关信息! accountName=" + accountName);
                return null;
            }
            var a = new AssetData();
            a.SetValues(list[0]);
            return a;
        }
    }
}
