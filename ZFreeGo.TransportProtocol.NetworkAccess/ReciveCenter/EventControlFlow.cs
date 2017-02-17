using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ReciveCenter
{
    /// <summary>
    /// 事件控制流处理tControlFlow
    /// </summary>
    public class EventControlFlow
    {
          /// <summary>
        /// 事件列表
        /// </summary>
        private List<EventProperty<FlowGather>> eventList;

        /// <summary>
        /// 查找是否包含指定typeID的类型EventProperty，若有则返回。
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <returns>EventProperty</returns>
        public EventProperty<FlowGather> GetEventProcess(FlowGather typeID)
        {
           
            return   eventList.Find(x => x.TypeID == typeID);       
        }



        /// <summary>
        /// 移除指定typeID的类型EventProperty
        /// </summary>
        /// <param name="typeID">类型ID</param>
        public void RemoveEventProcess(FlowGather typeID)
        {
            eventList.Remove(eventList.Find(x => x.TypeID == typeID));
        }

        /// <summary>
        /// 添加EventProperty，若包含已有TypeID则返回错误。
        /// </summary>
        /// <param name="eventPro"></param>
        /// <returns></returns>
        public bool AddEventProcess(EventProperty<FlowGather> eventPro)
        {

            //首先检测是否含有指定的TypID
            if (eventList.Find(x => x.TypeID == eventPro.TypeID) == null)
            {
                eventList.Add(eventPro);
                return true;
            }
            return false;
        }





        public EventControlFlow()
        {
            eventList = new List<EventProperty<FlowGather>>();
     
        }
    }
}
