using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RememberPos
{
	public class Scene
    {
        public int _id { private set; get; }
#if SERVER
        public static int s_id;
        public Scene()
        {
            ++s_id;
            _id = s_id;
        }
#endif
        public string _ownerAccountName; 
        public int _mode = 0; // 计数模式，计时模式 
        public Dictionary<string, PlayerData> _players = new Dictionary<string, PlayerData>(); // accountName
        public Dictionary<string, VisitorData> _visitors = new Dictionary<string, VisitorData>(); 
        public float _gapTime = 0.5f; 
        public int _maxCount = 4;
        public DateTime _lastShowedBlockTime;
        public int _width = 8;
        public int _height = 8; 

        // 场景操作
        public Dictionary<int, BlockData> _blocks = new Dictionary<int, BlockData>();
        public List<BlockData> _showedBlocks = new List<BlockData>(); 
        public Dictionary<string, int> _scores = new Dictionary<string, int>(); // accountName
    }
}
