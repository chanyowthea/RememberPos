using ProtoBuf;
using RememberPos;
using System.Threading;

[ProtoContract]
public class BlockData
{
	[ProtoMember(1)]
	public int _itemId = 1;
	[ProtoMember(2)]
	public int _index;
	// 每个Block的消失时间可能不同
	[ProtoMember(3)]
	public int _vanishTime = 5;
	[ProtoMember(4)]
	public bool _isShow;

#if SERVER
	// 所属场景ID，便于找到场景
	public int _sceneId;
	Thread t;
	public bool isShow
	{
		set
		{
			_isShow = value;
			Scene s;
            Singleton._log.Info("before TryGetValue value=" + value + ", _sceneId=" + _sceneId);
            if (Singleton._sceneManager._scenes.TryGetValue(_sceneId, out s))
			{
                Singleton._log.Info("value=" + value + ", mode=" + s._mode); 
				// 计时模式
				if (value && s._mode == 1)
				{
					if (t != null)
					{
						t.Abort();
					}
					t = new Thread(OnCountDown);
					t.Start();
				}
			}
		}
	}

	void OnCountDown()
    {
        Singleton._log.Info("OnCountDown sceneId=" + _sceneId);
        Thread.Sleep(_vanishTime * 1000);
		Singleton._sceneManager.HideBlock(_sceneId, _index);
	}

	~BlockData()
	{
		if (t != null)
		{
			t.Abort();
		}
	}
#endif
}
