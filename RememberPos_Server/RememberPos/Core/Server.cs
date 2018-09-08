using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net;
using log4net.Config;
using Photon.SocketServer;
using System.IO;
using RememberPos; 

namespace RememberPos
{
    public class Server : ApplicationBase
    {
		//创建连接
		protected override PeerBase CreatePeer(InitRequest initRequest)
		{
			return new Client(initRequest);
		}

		//服务器启动时调用
		protected override void Setup()
		{
			Singleton.Init(); 
			InitLogging();
            Singleton._log = ExitGames.Logging.LogManager.GetCurrentClassLogger();
            Singleton._log.Info("服务器启动时调用 Setup ok.");
		}

		//服务器停止时调用
		protected override void TearDown()
		{
			Singleton._log.Info("服务器停止时调用 TearDown ok.");
		}

		protected virtual void InitLogging()
		{
			ExitGames.Logging.LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
			GlobalContext.Properties["Photon:ApplicationLogPath"] =
			Path.Combine(this.ApplicationRootPath, "log");
			GlobalContext.Properties["LogFileName"] = this.ApplicationName;
			XmlConfigurator.ConfigureAndWatch(new
			FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
		}

	}
}
