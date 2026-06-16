using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Collections.Generic;

namespace LekeNet
{


	public class NetHelper
	{
		public NetHelper ()
		{
		}


		/// <summary>
		/// 得到本机IP
		/// </summary>
		public static List<string> GetLocalIPList() 
		{
			IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
			List<string> ipStrs = new List<string> ();
			foreach(IPAddress ip in ips)
			{
				if(IsRightIP(ip.ToString())) {
					ipStrs.Add(ip.ToString());
				}
			}
			return ipStrs ;
		}

		/// <summary>
		/// 得到本机IP
		/// </summary>
		public static string GetLocalIP() 
		{
			IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
			foreach(IPAddress ip in ips)
			{
				if(IsRightIP(ip.ToString())) {
					return ip.ToString();
				}
			}
			return "" ;
		}

		//得到网关地址
		public static string GetGateway() 
		{ 
			//网关地址
			string strGateway = "";
			//获取所有网卡
			NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
			//遍历数组
			foreach (var netWork in nics)
			{
				//单个网卡的IP对象
				IPInterfaceProperties ip = netWork.GetIPProperties();
				//获取该IP对象的网关
				GatewayIPAddressInformationCollection gateways = ip.GatewayAddresses;
				foreach(var gateWay in gateways)
				{
					//如果能够Ping通网关
					if(IsPingIP(gateWay.Address.ToString()))
					{
						//得到网关地址
						strGateway = gateWay.Address.ToString();
						//跳出循环
						break;
					}
				}
				//如果已经得到网关地址
				if (strGateway.Length > 0) 
				{
					//跳出循环
					break;
				}
			}
			//返回网关地址
			return strGateway;
		}
		/// <summary>
		/// 判断是否为正确的IP地址
		/// </summary>
		/// <param name="strIPadd">需要判断的字符串</param>
		/// <returns>true = 是 false = 否</returns>
		public static bool IsRightIP(string strIPadd) 
		{
			//利用正则表达式判断字符串是否符合IPv4格式
			if (Regex.IsMatch(strIPadd, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
			{
				//根据小数点分拆字符串
				string[] ips = strIPadd.Split('.');
				if (ips.Length == 4 || ips.Length == 6)
				{
					//如果符合IPv4规则
					if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256)
						//正确
						return true;
					//如果不符合
					else
						//错误
						return false;
				}
				else
					//错误
					return false;
			}
			else
				//错误
				return false;
		}
		/// <summary>
		/// 尝试Ping指定IP是否能够Ping通
		/// </summary>
		/// <param name="strIP">指定IP</param>
		/// <returns>true 是 false 否</returns>
		public static bool IsPingIP(string strIP) 
		{
			try
			{
				//创建Ping对象
				Ping ping = new Ping();
				//接受Ping返回值
				ping.Send(strIP, 1000);
				//Ping通
				return true;
			}
			catch 
			{
				//Ping失败
				return false;
			}
		}

		#region 返回当前系统所启用的网络连接的信息
		public static NetworkInterface[] NetCardInfo()
		{
			return NetworkInterface.GetAllNetworkInterfaces();
		}
		#endregion



		public static string[] GetMacString()
		{
			string strMac = "";
			NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (NetworkInterface ni in interfaces)
			{
				if (ni.OperationalStatus == OperationalStatus.Up)
				{
					strMac += ni.GetPhysicalAddress().ToString() + "|";
				}
			}
			return strMac.Split('|');

		}

        public static string GetMac()
        {
            string[] macs = GetMacString();
            if(macs.Length > 0)
            {
                return macs[0];
            }
            return "";
        }

		public static string GetIpFromAddress(IPAddress address) {
			return address.ToString().Split(':')[0];
		}

	}




}

