using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YAHW.MVVMBase;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for GPU-Information
    /// </para>
    /// 
    /// <para>
    /// Class history:
    /// <list type="bullet">
    ///     <item>
    ///         <description>1.0: First release, working (Steffen Steinbrecher).</description>
    ///     </item>
    /// </list>
    /// </para>
    /// 
    /// <para>Author: Steffen Steinbrecher</para>
    /// <para>Date: 29.07.2015</para>
    /// </summary>
    public class FanControllerTemplate : BindableBase
    {
         private string name;

        /// <summary>
        /// Template-Name
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.SetProperty<string>(ref this.name, value); }
        }

        private IList<DataPoint> dataPoints = new List<DataPoint>();

        /// <summary>
        /// List with data points
        /// </summary>
        public IList<DataPoint> DataPoints
        {
            get { return dataPoints; }
            set { this.SetProperty<IList<DataPoint>>(ref this.dataPoints, value); }
        }
    }
}