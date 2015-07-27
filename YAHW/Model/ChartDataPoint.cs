using YAHW.MVVMBase;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for a DataPoint in a chart
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
    /// <para>Date: 12.07.2015</para>
    /// </summary>
    public class ChartDataPoint : BindableBase
    {
        private string name;

        /// <summary>
        /// The name.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { this.SetProperty<string>(ref this.name, value); }
        }

        private double value;

        /// <summary>
        /// The value
        /// </summary>
        public double Value
        {
            get { return value; }
            set { this.SetProperty<double>(ref this.value, value); }
        }
    }
}