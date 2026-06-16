using System;
using System.Collections.Generic;

public class KEventArgs : EventArgs{

    //参数名常量
    public const string TARGET_PARAM = "KEventArgs.TARGET";  //事件源对象

    public const string DELTAPOSITION_PARAM = "KEventArgs.DELTAPOSITION_PARAM";//鼠标

    public const string GESTURE_PARAM = "KEventArgs.GESTURE_PARAM";

	public Dictionary<string, object> data;

	public KEventArgs(){
		data = new Dictionary<string, object>();
	}
	
	public void Set(string key, object value){
		data[key] = value;
	}

	public object Get(string key){
		return data[key];
	}
}
