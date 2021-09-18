using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisNetCore.Models
{
    public class Field
    {
        public int RowCount { get; internal set; } = 24;
        public int ColumnCount { get; internal set; } = 10;


    }
}
