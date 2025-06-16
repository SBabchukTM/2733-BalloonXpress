using System;
using Core.UI;

namespace Runtime.UI
{
    public class SimpleDecisionPopupData : BasePopupData
    {
        public Action PressOkEvent;
        public string Message;
    }
}