﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhoenixCI.FormUI.Prefix1
{
    public partial class W11010 : W1xxx
    {
        public W11010(string programID, string programName) : base(programID, programName)
        {
            _DB_TYPE = "ci";
        }
    }
}
