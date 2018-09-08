using RememberPos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<BlockCell> fishs = Singleton._excelUtil.GetBlockTable();
            //foreach (BlockCell fish in fishs)
            //{
            //    Console.WriteLine(fish._description);
            //}

            Test();
#if SERVER
            Console.ReadKey();
#endif
        }

        static long GenerateId()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }

        static void Test()
        {
            SqlServer sql = new SqlServer();
            sql.CreateAccountTable(true);
            PlayerData data = new PlayerData();
            data._accountName = "1";
            data._nickName = "11";
            data._blockIdsInUse = new int[] {1
                // , 2, 3
            };
            sql.Register(data, "1");

            data._accountName = "2";
            data._nickName = "22";
            data._blockIdsInUse = new int[] { 3
                // , 4, 5
            };
            sql.Register(data, "1");

            sql.CreateAssetTable(true);
            AssetData a = new AssetData();
            a._accountName = "1"; 
            a._gold = 10;
            a._blocks = new int[] { 1, 2, 3 };
            a._blockBoards = new int[] { 0, 1, 2 };
            a._boards = new int[] { 0, 1, 2 };
            a._persons = new int[] { 0, 1, 2 };
            sql.ChangeAsset(a);

            a._accountName = "2";
            a._gold = 10;
            a._blocks = new int[] { 3, 4, 5 };
            a._blockBoards = new int[] { 0 };
            a._boards = new int[] { 0 };
            a._persons = new int[] { 0, 2 };
            sql.ChangeAsset(a);

            //var s = sql.GetAssetData("1"); 
            //sql.CreateAccountTable();
        }
    }
}
