using Microcomm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.File
{
    public class FileInfoQueryCondition : QueryConditionWithTimeRange
    {
        public string Category { get; set; }
 
        public override int GetHashCode()
        {
            return this.Category==null?base.GetHashCode(): base.GetHashCode() * 13 + this.Category.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is FileInfoQueryCondition))
                return false;
            var other = obj as FileInfoQueryCondition;

            if (this.Category != other.Category)
                return false;
      
            return base.Equals(obj);
        }
    }
}
