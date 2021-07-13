using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimalMotion2.Domain
{
    public interface ITable
    {
        void AddRow(TableRowData tableData);
        void RemoveRow(int id);
        void UpdateRow(int id, ITableRow newRow);
        void Reset();
    }
}
