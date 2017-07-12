using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.ServiceModel.Configuration;
using System.Text;
using System.Threading.Tasks;

namespace CCSFacade.DomainServices
{
	public class SmokeTest
	{
		public SmokeTest()
		{
			_sb = new StringBuilder();
		}

		private readonly StringBuilder _sb;
		private void LogIt(string msg)
		{
			_sb.AppendLine($"{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")} {msg}");
		}

		public string Results()
		{
			return _sb.ToString();
		}
		
		public bool AmIAlive()
		{
			var result = true;

			LogIt($"# Am I Alive ? ");


			LogIt($"|--# ConnectionStrings ");

			for (var i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
			{
				var conn = ConfigurationManager.ConnectionStrings[i];
				if (conn.Name == "LocalSqlServer") continue;//ignore
				var resp = CheckSqlServer(conn.ConnectionString);
				result = result && resp;
				LogIt($"|    |-- # {conn.Name} --> {resp}");
			}


			LogIt($"|--# AppSettings ");
			foreach (string key in ConfigurationManager.AppSettings)
			{
				if (key == "aspnet:UseTaskFriendlySynchronizationContext") continue;//ignore

				LogIt($"|    |-- # {key}");
			}						

			LogIt($"|--# Endpoints ");

			var endpointNames = GetEndPoints();
			foreach (string key in endpointNames.AllKeys)
			{
				var resp = CheckEndpoint(endpointNames[key]);
				result = result && resp;
				LogIt($"|    |-- # {key} --> {resp}");
			}

			LogIt((result)? "|--> System Ok" : "|--> || ERROR ||");
			return result;

		}

		private NameValueCollection GetEndPoints()
		{
			var clientSection = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;
			var endpointCollection = clientSection.ElementInformation.Properties[string.Empty].Value as ChannelEndpointElementCollection;
			var endpointNames = new NameValueCollection();
			foreach (ChannelEndpointElement endpointElement in endpointCollection)
			{
				endpointNames.Add(endpointElement.Name, endpointElement.Address.ToString());
			}
			return endpointNames;
		}

		private bool CheckSqlServer(string connstr)
		{
			var db = new SqlConnection(connstr);
			try
			{
				db.Open();				
				db.Close();
				db.Dispose();
				return true;
			}
			catch
			{
				return false;
			}			
		}

		private bool CheckEndpoint(string address)
		{
			try
			{
				System.Net.WebClient client = new WebClient();
				client.DownloadData(address);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
