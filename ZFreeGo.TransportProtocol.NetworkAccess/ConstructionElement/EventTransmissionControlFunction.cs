using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransportProtocol.NetworkAccess.TransmissionControl104;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ReciveCenter
{
    /// <summary>
    ///   EventTransmissionControlFunction 传输控制功能控制事件
    /// </summary>
    public class EventTCF
    {
         /// <summary>
        /// 事件列表
        /// </summary>
        private List<EventProperty<TransmissionControlFunction>> eventList;

        /// <summary>
        /// 查找是否包含指定typeID的类型EventProperty，若有则返回。
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <returns>EventProperty</returns>
        public EventProperty<TransmissionControlFunction> GetEventProcess(TransmissionControlFunction typeID)
        {
            try
            {
                return eventList.Find(x => x.TypeID == typeID); 
            }
            catch(ArgumentNullException)
            {
                return null;
            }
           
            
                    
        }



        /// <summary>
        /// 移除指定typeID的类型EventProperty
        /// </summary>
        /// <param name="typeID">类型ID</param>
        public void RemoveEventProcess(TransmissionControlFunction typeID)
        {
            eventList.Remove(eventList.Find(x => x.TypeID == typeID));
        }

        /// <summary>
        /// 添加EventProperty，若包含已有TypeID则返回错误。
        /// </summary>
        /// <param name="eventPro"></param>
        /// <returns></returns>
        public bool AddEventProcess(EventProperty<TransmissionControlFunction> eventPro)
        {

            //首先检测是否含有指定的TypID
            if (eventList.Find(x => x.TypeID == eventPro.TypeID) == null)
            {
                eventList.Add(eventPro);
                return true;
            }
            return false;
        }





        public EventTCF()
        {
            eventList = new List<EventProperty<TransmissionControlFunction>>();
     
        }
    }
}
