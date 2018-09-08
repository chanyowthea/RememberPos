using System.Collections;
using ProtoBuf;

[ProtoContract]
public class BaseCell
{
    [ProtoMember(1)]
    public int _id;
    [ProtoMember(2)]
    public string _name;
    [ProtoMember(3)]
    public string _description;
    [ProtoMember(4)]
    public int _gold;
}

[ProtoContract]
public class BlockCell : BaseCell
{

}

[ProtoContract]
public class BlockBoardCell : BaseCell
{

}

[ProtoContract]
public class PersonCell : BaseCell
{

}

[ProtoContract]
public class BoardCell : BaseCell
{

}
