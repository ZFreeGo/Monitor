using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.ConstructionElement
{
    /// <summary>
    ///   EventTransmissionControlFunction 传输控制功能控制事件
    /// </summary>
    public class EventTCF
    {
         /// <summary>
        /// 事件列表
        /// </summary>
        private List<EventProperty<TransmissionCotrolFunction>> eventList;

        /// <summary>
        /// 查找是否包含指定typeID的类型EventProperty，若有则返回。
        /// </summary>
        /// <param name="typeID">类型ID</param>
        /// <returns>EventProperty</returns>
        public EventProperty<TransmissionCotrolFunction> GetEventProcess(TransmissionCotrolFunction typeID)
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
        public void RemoveEventProcess(TransmissionCotrolFunction typeID)
        {
            eventList.Remove(eventList.Find(x => x.TypeID == typeID));
        }

        /// <summary>
        /// 添加EventProperty，若包含已有TypeID则返回错误。
        /// </summary>
        /// <param name="eventPro"></param>
        /// <returns></returns>
        public bool AddEventProcess(EventProperty<TransmissionCotrolFunction> eventPro)
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
            eventList = new List<EventProperty<TransmissionCotrolFunction>>();
     
        }
    }
}
