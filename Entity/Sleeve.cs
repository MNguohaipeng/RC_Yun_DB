using System;
using System.Collections.Generic;
using System.Text;

namespace Entity //�޸����ֿռ�
{
	public class Sleeve
	{
		private int iD;
		public int ID
		{
			get { return iD; }
			set { iD = value; }
		}
	
		private string code;
		public string Code
		{
			get { return code; }
			set { code = value; }
		}
	
		private decimal length;
		public decimal Length
		{
			get { return length; }
			set { length = value; }
		}
	
		private int fK_CoatSize_ID;
		public int FK_CoatSize_ID
		{
			get { return fK_CoatSize_ID; }
			set { fK_CoatSize_ID = value; }
		}
	}
}