using UnityEngine;
using System.Collections;

public class FormatUtil
{

	//float格式化保留7位有效位，防止小数位溢出坐标错乱
	public static float FloatFormat(float num)
	{
		return float.Parse(num.ToString("f7"));
	}
}
