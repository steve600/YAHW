using YAHW.MVVMBase;

namespace YAHW.Model
{
    /// <summary>
    /// <para>
    /// Class for Mainboard-Information
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
    public class MainboardInformation : BindableBase
    {
        private string manufacturer;

        /// <summary>
        /// Manufacturer
        /// </summary>
        public string Manufacturer
        {
            get { return manufacturer; }
            set { this.SetProperty<string>(ref this.manufacturer, value); }
        }

        private string product;

        /// <summary>
        /// The product
        /// </summary>
        public string Product
        {
            get { return product; }
            set { this.SetProperty<string>(ref this.product, value); }
        }

        private string serialNumber;

        /// <summary>
        /// Serial number
        /// </summary>
        public string SerialNumber
        {
            get { return serialNumber; }
            set { this.SetProperty<string>(ref this.serialNumber, value); }
        }

        private string ioHardware;

        /// <summary>
        /// I/O-Hardware
        /// </summary>
        public string IOHardware
        {
            get { return ioHardware; }
            set { this.SetProperty<string>(ref this.ioHardware, value); }
        }        
    }
}