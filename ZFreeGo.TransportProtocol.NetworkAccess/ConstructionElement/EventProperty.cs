using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ZFreeGo.TransportProtocol.NetworkAccess.BasicElement;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ReciveCenter
{
    public class EventProperty<T>
    {
        /// <summary>
        /// 事件
        /// </summary>
        public ManualResetEvent Event
        {
            get;
            set;
        }
        /// <summary>
        /// 类型ID
        /// </summary>
        public T TypeID
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Descr
        {
            get;
            set;
        }

        public EventProperty(T typeID, string str)
        {
            Event = new ManualResetEvent(false);
            TypeID = typeID;
            Descr = str;     
            
        }
        public EventProperty(T typeID)
        {
            Event = new ManualResetEvent(false);
            TypeID = typeID;
            Descr = typeID.ToString();
        }

            
    
    }
    public class EventProperty
    {
        /// <summary>
        /// 事件
        /// </summary>
        public ManualResetEvent Event
        {
            get;
            set;
        }
        /// <summary>
        /// 类型ID
        /// </summary>
        public TypeIdentification TypeID
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string Descr
        {
            get;
            set;
        }

        public EventProperty(TypeIdentification typeID, string str)
        {
            Event = new ManualResetEvent(false);
            TypeID = typeID;
            Descr = str;

        }
        public EventProperty(TypeIdentification typeID)
        {
            Event = new ManualResetEvent(false);
            TypeID = typeID;
            Descr = typeID.ToString();
        }
    }

            
    
}
