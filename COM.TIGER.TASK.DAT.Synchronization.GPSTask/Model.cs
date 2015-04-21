using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COM.TIGER.TASK.DAT.Synchronization.GPSTask
{
    public class Model : COM.TIGER.TASK.DAT.Synchronization.DatBaseModel
    {
        public static int tick = 0;

        protected override string TABLENAME
        {
            get { return ""; }
        }
    }
}
