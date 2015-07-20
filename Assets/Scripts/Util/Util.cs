using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Util {
	public static System.Random SRand=new System.Random();

	public static List<int> RandomIndices(int range,int count){
		var ret = new List<int> ();
		var tmp = new List<int> ();
		for (int i=0; i<range; i++) {
			tmp.Add(i);
		};
		while (ret.Count<count&&tmp.Count>0) {
			int idx=Util.SRand.Next(0,tmp.Count);
			ret.Add(tmp[idx]);
			tmp.RemoveAt(idx);
		}
		return ret;
	}
}
