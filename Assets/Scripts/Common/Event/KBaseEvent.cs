using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class KBaseEvent<Ttype, Tdata>
{
	public Tdata data;
	public Ttype eventType;

	public KBaseEvent(Ttype eventType, Tdata data = default(Tdata))
	{
	    this.eventType = eventType;
	    this.data = data;
	}

	public KBaseEvent(Ttype eventType):this(eventType , default(Tdata))
	{
	}

}