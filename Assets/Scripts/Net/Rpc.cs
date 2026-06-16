using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LekeNet
{
    public class Rpc
    {

        public delegate void RpcCallBack(byte[] data);

        public delegate Object RpcCall(int memId, byte[] data);

        private Dictionary<string, RpcCall> rpcCallDict = new Dictionary<string, RpcCall>();

        private Dictionary<string, RpcCallBack> rpcCallBackDict = new Dictionary<string, RpcCallBack>();

        public void AddRpcCall(string rpcName, RpcCall rpcCall)
        {
            this.rpcCallDict[rpcName] = rpcCall;
        }

        public RpcCall GetRpcCall(string rpcName)
        {
            if(this.rpcCallDict.ContainsKey(rpcName))
            {
                return this.rpcCallDict[rpcName];
            }
            return null;
        }

        

        public void AddRpcCallBack(string rpcName, RpcCallBack rpcCallBack)
        {
            this.rpcCallBackDict[rpcName] = rpcCallBack;
        }

        public RpcCallBack GetRpcCallBack(string rpcName)
        {
            if (this.rpcCallBackDict.ContainsKey(rpcName))
            {
                return this.rpcCallBackDict[rpcName];
            }
            return null;
        }

    }

    
}
