using System.Data;
using System.Collections.Generic;

public class ExcelUtil
{
    public List<BlockCell> GetBlockTable()
    {
        DataRowCollection collect = ExcelAccess.GetCollection("Block.xls");
        List<BlockCell> list = new List<BlockCell>();

        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            BlockCell cell = new BlockCell();
            cell._id = int.Parse(collect[i][0].ToString());
            cell._name = collect[i][1].ToString();
            cell._description = collect[i][2].ToString();
            cell._gold = int.Parse(collect[i][3].ToString());
            list.Add(cell);
        }
        return list;
    }

    public BlockCell GetBlock(int id)
    {
        var list = GetBlockTable();
        var c = list.Find((BlockCell cell) => cell._id == id);
        return c; 
    }

    public List<BlockBoardCell> GetBlockBoardTable()
    {
        DataRowCollection collect = ExcelAccess.GetCollection("BlockBoard.xls");
        List<BlockBoardCell> list = new List<BlockBoardCell>();

        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            BlockBoardCell cell = new BlockBoardCell();
            cell._id = int.Parse(collect[i][0].ToString());
            cell._name = collect[i][1].ToString();
            cell._description = collect[i][2].ToString();
            cell._gold = int.Parse(collect[i][3].ToString());
            list.Add(cell);
        }
        return list;
    }

    public BlockBoardCell GetBlockBoard(int id)
    {
        var list = GetBlockBoardTable();
        var c = list.Find((BlockBoardCell cell) => cell._id == id);
        return c;
    }

    public List<BoardCell> GetBoardTable()
    {
        DataRowCollection collect = ExcelAccess.GetCollection("Board.xls");
        List<BoardCell> list = new List<BoardCell>();

        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            BoardCell cell = new BoardCell();
            cell._id = int.Parse(collect[i][0].ToString());
            cell._name = collect[i][1].ToString();
            cell._description = collect[i][2].ToString();
            cell._gold = int.Parse(collect[i][3].ToString());
            list.Add(cell);
        }
        return list;
    }

    public BoardCell GetBoard(int id)
    {
        var list = GetBoardTable();
        var c = list.Find((BoardCell cell) => cell._id == id);
        return c;
    }

    public List<PersonCell> GetPersonTable()
    {
        DataRowCollection collect = ExcelAccess.GetCollection("Person.xls");
        List<PersonCell> list = new List<PersonCell>();

        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            PersonCell cell = new PersonCell();
            cell._id = int.Parse(collect[i][0].ToString());
            cell._name = collect[i][1].ToString();
            cell._description = collect[i][2].ToString();
            cell._gold = int.Parse(collect[i][3].ToString());
            list.Add(cell);
        }
        return list;
    }

    public PersonCell GetPerson(int id)
    {
        var list = GetPersonTable();
        var c = list.Find((PersonCell cell) => cell._id == id);
        return c;
    }
}

