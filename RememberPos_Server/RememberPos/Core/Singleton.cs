using ExitGames.Logging;
using System.Collections.Generic;

namespace RememberPos
{
	public static class Singleton
	{
		public static ILogger _log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
		public static _Serializer _serializer;
		public static SqlServer _sqlServer;
		public static ServerCallback _serverCallback;
		public static MessageManager _messageManager;
		public static Dictionary<string, Client> _clients = new Dictionary<string, Client>();
		public static RoomManager _roomManager;
		public static SceneManager _sceneManager;
        public static TimeUtil _timeUtil;
        public static RandomUtil _randomUtil;
        public static ExcelUtil _excelUtil;

        public static void Init()
		{
			_log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
			_serializer = new _Serializer();
			_sqlServer = new SqlServer();
			_serverCallback = new ServerCallback();
			_messageManager = new MessageManager();
			_roomManager = new RoomManager();
			_sceneManager = new SceneManager();
            _timeUtil = new TimeUtil();
            _randomUtil = new RandomUtil();
            _excelUtil = new ExcelUtil(); 
		}
	}
}
