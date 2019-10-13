using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FTD2XX_NET;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System.Collections;
using System.Diagnostics;
using Chart = System.Windows.Forms.DataVisualization.Charting.Chart;
using Charting = System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;

namespace Citiroc_serialTest
{
    public partial class Form_SerialTest : Form
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);
        FontFamily ffBryant = FontFamily.GenericSansSerif;
        private System.Drawing.Text.PrivateFontCollection pfcBryant = new System.Drawing.Text.PrivateFontCollection();
        private static Excel.Application xlApp;
        Excel.Workbook xlWorkBook;
        Excel.Worksheet xlWorkSheet;
        private static String savepath = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
        private static int i = 1;
        public static String date = "";

        public Form_SerialTest()
        {
            InitializeComponent();
            init_excel();
            
            try
            {
                byte[] fontData = Properties.Resources.Bryant_RegularCompressed;
                IntPtr fontPtr = Marshal.AllocCoTaskMem(fontData.Length);
                Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
                uint dummy = 0;
                pfcBryant.AddMemoryFont(fontPtr, Properties.Resources.Bryant_RegularCompressed.Length);
                AddFontMemResourceEx(fontPtr, (uint)Properties.Resources.Bryant_RegularCompressed.Length, IntPtr.Zero, ref dummy);
                Marshal.FreeCoTaskMem(fontPtr);

                ffBryant = pfcBryant.Families[0];
            }
            catch { MessageBox.Show("An error occured during the loading of the font. A generic font will be used which can impair the optimal display."); }
        }

        public void setTextBox(String s)
        {
            textBox_asicNumber.Text = s;
        }
        private void init_excel()
        {

            xlApp = new Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Excel is not properly installed!!");
                return;
            }

            object misValue = System.Reflection.Missing.Value;

            xlWorkBook = xlApp.Workbooks.Add(misValue);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);


            var chip = new object[1, 7]
            {
                { "Numéro Série", "Validité", "TestDC", "TestInDac", "TestThDac", "TestScurves", "TestHit" },
            };
            for (int j = 0; j < 7; j++)
            {
                xlWorkSheet.Cells[1, j+1] = chip[0, j];
            }
            xlWorkSheet.Columns[1].ColumnWidth = 30;
            xlApp.DisplayAlerts = false;

            xlWorkBook.SaveAs(savepath);

        }

        private void AjouterChip(object[,] chip)
        {
            int j;
            //int k;
            //int doub = 0;
            /*for (k = 2; k < i + 1; k++)
            {
                try
                {
                    if (chip[0, 0].ToString() == xlWorkSheet.Cells[k, 1].Value.ToString())
                    {
                        doub = 1;
                        break;
                    }
                }catch(Microsoft.CSharp.RuntimeBinder.RuntimeBinderException ex)
                {
                    doub = 0;
                }
            }*/
            /*if (doub == 1)
            {
                xlWorkSheet.Cells[i + 1, 1] = (String)chip[0, 0]+'-'+date;
                for (j = 1; j < 7; j++)
                {
                    xlWorkSheet.Cells[i + 1, j + 1] = chip[0, j];
                }
            }*/
            /*else
            {*/
                for (j = 0; j < 7; j++)
                {
                    xlWorkSheet.Cells[i + 1, j + 1] = chip[0, j];
                }
            //}
            i = i + 1;
            xlWorkSheet.Cells[1, 8] = i;

            xlWorkBook.SaveAs(savepath);
        }

       
        #region FTDI Public def.
        FTDI.FT_DEVICE ftdiDevice;
        uint ftdiDeviceCount;
        FTDI.FT_STATUS ftStatus;
        FTDI myFtdiDevice;
        FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList;
        FTDI.FT_DEVICE_INFO_NODE[] testBoardFtdiDevice = new FTDI.FT_DEVICE_INFO_NODE[1];
        FTDI.FT_DEVICE_INFO_NODE[] mezzBoardFtdiDevice = new FTDI.FT_DEVICE_INFO_NODE[1];

        public Int32 usbDevId;
        public Int32 usbMezzId;
        #endregion

        #region EPCS Operation Code
        private const byte AS_NOP = 0x00;
        private const byte AS_WRITE_ENABLE = 0x06;
        private const byte AS_WRITE_DISABLE = 0x04;
        private const byte AS_READ_STATUS = 0x05;
        private const byte AS_WRITE_STATUS = 0x01;
        private const byte AS_READ_BYTES = 0x03;
        private const byte AS_FAST_READ_BYTES = 0x0B;
        private const byte AS_PAGE_PROGRAM = 0x02;
        private const byte AS_ERASE_SECTOR = 0xD8;
        private const byte AS_ERASE_BULK = 0xC7;
        private const byte AS_READ_SILICON_ID = 0xAB;
        private const byte AS_CHECK_SILICON_ID = 0x9F;
        #endregion

        #region Pin Definition
        private const byte CONF_DONE = 0x80;
        private const byte ASDI = 0x40;
        private const byte DATAOUT = 0x10;
        private const byte NCE = 0x08;
        private const byte NCS = 0x04;
        private const byte NCONFIG = 0x02;
        private const byte DCLK = 0x01;
        private const byte CUR_DATA = 0x00;
        private const byte DEF_VALUE = 0x0E;
        #endregion

        #region Weeroc color
        static Color WeerocBlack = Color.FromArgb(255, 30, 30, 28);
        static Color WeerocDarkGreen = Color.FromArgb(255, 0, 83, 52);
        static Color WeerocGreen = Color.FromArgb(255, 29, 121, 104);
        static Color WeerocPaleBlue = Color.FromArgb(255, 0, 121, 144);
        static Color WeerocDarkBlue = Color.FromArgb(255, 34, 32, 70);
        static Color WeerocBlue = Color.FromArgb(255, 2, 65, 103);
        #endregion

        static int NbChannels = 32;

        #region measurement parameters

        int inputDacStep = 32; // step size for input DAC acquisition
        int thresholdDacStep = 4; // step size for dual threshold DAC acquisition
        int VthStep = -2; // step size for S-curves acquisition

        double Hconsumption = 100, Lconsumption = 70; // limits for power consumption
        double Hvbg = 3, Lvbg = 2; // limits for bandgap
        double HDCHG = 1.5, LDCHG = 1, HDCLG = 1.5, LDCLG = 1; // limits for shapers DC
        double LinDacSlope = -20e-3, HinDacSlope = -10e-3; // limits for input DAC slope
        double LinDacIntercept = 3.5; // limit for input DAC intercept
        double LDacSlope = 2e-3, HDacSlope = 2.5e-3; // limits for input DAC slope
        double LDacIntercept = 0.7, HDacIntercept = 1; // limit for input DAC intercept

        #endregion

        private void SerialTest_Load(object sender, EventArgs e)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;

            // Set FTDI device count to 0
            ftdiDeviceCount = 0;
            // Create new instance of the FTDI device class
            myFtdiDevice = new FTDI();
            // Initalize FTDI status
            ftStatus = FTDI.FT_STATUS.FT_OK;
            // Usb Devices ID
            usbDevId = 0;
            // Set default FTDI chip to FTDI2232H
            ftdiDevice = FTDI.FT_DEVICE.FT_DEVICE_2232H;

            #region Weeroc font
            controlList = GetControlHierarchy(this).ToList(); // Get the list of all the controls in the UI

            // Set font of all the controls to the Weeroc font.
            foreach (var control in controlList)
                if (control is Chart)
                {
                    if (((Chart)control).Titles.Count > 0) ((Chart)control).Titles[0].Font = new Font(ffBryant, ((Chart)control).Titles[0].Font.Size);
                    for (int i = 0; i < ((Chart)control).ChartAreas.Count; i++)
                    {
                        ((Chart)control).ChartAreas[i].AxisX.TitleFont = new Font(ffBryant, ((Chart)control).ChartAreas[i].AxisX.TitleFont.Size);
                        ((Chart)control).ChartAreas[i].AxisY.TitleFont = new Font(ffBryant, ((Chart)control).ChartAreas[i].AxisY.TitleFont.Size);
                        ((Chart)control).ChartAreas[i].AxisX.LabelStyle.Font = new Font(ffBryant, ((Chart)control).ChartAreas[i].AxisX.LabelStyle.Font.Size);
                        ((Chart)control).ChartAreas[i].AxisY.LabelStyle.Font = new Font(ffBryant, ((Chart)control).ChartAreas[i].AxisY.LabelStyle.Font.Size);
                    }
                }
                else control.Font = new Font(ffBryant, control.Font.Size);

            if (label_titleBar.Font.FontFamily.Name != "Bryant Regular Compressed") // Change font to generic if the weeroc font isn't loaded.
            {
                foreach (var control in controlList)
                    if (control is Chart)
                    {
                        if (((Chart)control).Titles.Count > 0) ((Chart)control).Titles[0].Font = new Font(ffBryant, ((Chart)control).Titles[0].Font.Size * 0.75F);
                        for (int i = 0; i < ((Chart)control).ChartAreas.Count; i++)
                        {
                            ((Chart)control).ChartAreas[0].AxisX.TitleFont = new Font(FontFamily.GenericSansSerif, ((Chart)control).ChartAreas[0].AxisX.TitleFont.Size * 0.75F);
                            ((Chart)control).ChartAreas[0].AxisY.TitleFont = new Font(FontFamily.GenericSansSerif, ((Chart)control).ChartAreas[0].AxisY.TitleFont.Size * 0.75F);
                            ((Chart)control).ChartAreas[0].AxisX.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, ((Chart)control).ChartAreas[0].AxisX.LabelStyle.Font.Size * 0.75F);
                            ((Chart)control).ChartAreas[0].AxisY.LabelStyle.Font = new Font(FontFamily.GenericSansSerif, ((Chart)control).ChartAreas[0].AxisY.LabelStyle.Font.Size * 0.75F);
                        }
                    }
                    else
                        control.Font = new Font(FontFamily.GenericSansSerif, control.Font.Size * 0.75F);
            }
            #endregion

            label_titleBar.Text += Application.ProductVersion;
            
            button_UIScale_Click(sender, e);
        }
        
        #region title bar

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void label_titleBar_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void label_titleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void label_titleBar_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            xlWorkBook.SaveAs(savepath);
            xlWorkBook.Close();
            xlApp.Quit();
            Application.Exit();
        }

        public void cl()
        {
            xlWorkBook.SaveAs(savepath);
            xlWorkBook.Close();
            xlApp.Quit();
           // this.Close();
           // return isGood;
        }
        private void btn_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        List<Control> controlList;
        private IEnumerable<Control> GetControlHierarchy(Control root)
        {
            var queue = new Queue<Control>();
            queue.Enqueue(root);
            do
            {
                var control = queue.Dequeue();
                yield return control;
                foreach (var child in control.Controls.OfType<Control>())
                    queue.Enqueue(child);
            } while (queue.Count > 0);
        }

        double scale = 1;
        private void button_UIScale_Click(object sender, EventArgs e)
        {
            float scaleF = 1;
            Rectangle resolution = Screen.PrimaryScreen.Bounds;
            int screenWidth = resolution.Width;
            int screenHeight = resolution.Height;

            scaleF = (float)screenWidth / 1280;
            if (scaleF > (float)screenHeight / 720) scaleF = (float)screenHeight / 720;

            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                SizeF Scale = new SizeF(scaleF, scaleF);
                ActiveForm.Scale(Scale);

                foreach (Control control in controlList)
                {
                    if (control is Chart)
                    {
                        if (((Chart)control).Titles.Count > 0) ((Chart)control).Titles[0].Font = new Font(((Chart)control).Titles[0].Font.FontFamily, ((Chart)control).Titles[0].Font.Size * scaleF);
                        for (int i = 0; i < ((Chart)control).ChartAreas.Count; i++)
                        {
                            ((Chart)control).ChartAreas[i].AxisX.TitleFont = new Font(((Chart)control).ChartAreas[i].AxisX.TitleFont.FontFamily, ((Chart)control).ChartAreas[i].AxisX.TitleFont.Size * scaleF);
                            ((Chart)control).ChartAreas[i].AxisY.TitleFont = new Font(((Chart)control).ChartAreas[i].AxisY.TitleFont.FontFamily, ((Chart)control).ChartAreas[i].AxisY.TitleFont.Size * scaleF);
                            ((Chart)control).ChartAreas[i].AxisX.LabelStyle.Font = new Font(((Chart)control).ChartAreas[i].AxisX.LabelStyle.Font.FontFamily, ((Chart)control).ChartAreas[i].AxisX.LabelStyle.Font.Size * scaleF);
                            ((Chart)control).ChartAreas[i].AxisY.LabelStyle.Font = new Font(((Chart)control).ChartAreas[i].AxisY.LabelStyle.Font.FontFamily, ((Chart)control).ChartAreas[i].AxisY.LabelStyle.Font.Size * scaleF);
                        }
                    }
                    else control.Font = new Font(control.Font.FontFamily, control.Font.Size * scaleF);
                }

                //tabControl1.ItemSize = new Size((int)(tabControl_top.ItemSize.Width * scaleF), (int)(tabControl_top.ItemSize.Height * scaleF));

                scale = scaleF;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                SizeF Scale = new SizeF(1.0F / scaleF, 1.0F / scaleF);
                ActiveForm.Scale(Scale);

                foreach (Control control in controlList)
                {
                    if (control is Chart)
                    {
                        if (((Chart)control).Titles.Count > 0) ((Chart)control).Titles[0].Font = new Font(((Chart)control).Titles[0].Font.FontFamily, ((Chart)control).Titles[0].Font.Size / scaleF);
                        for (int i = 0; i < ((Chart)control).ChartAreas.Count; i++)
                        {
                            ((Chart)control).ChartAreas[i].AxisX.TitleFont = new Font(((Chart)control).ChartAreas[i].AxisX.TitleFont.FontFamily, ((Chart)control).ChartAreas[i].AxisX.TitleFont.Size / scaleF);
                            ((Chart)control).ChartAreas[i].AxisY.TitleFont = new Font(((Chart)control).ChartAreas[i].AxisY.TitleFont.FontFamily, ((Chart)control).ChartAreas[i].AxisY.TitleFont.Size / scaleF);
                            ((Chart)control).ChartAreas[i].AxisX.LabelStyle.Font = new Font(((Chart)control).ChartAreas[i].AxisX.LabelStyle.Font.FontFamily, ((Chart)control).ChartAreas[i].AxisX.LabelStyle.Font.Size / scaleF);
                            ((Chart)control).ChartAreas[i].AxisY.LabelStyle.Font = new Font(((Chart)control).ChartAreas[i].AxisY.LabelStyle.Font.FontFamily, ((Chart)control).ChartAreas[i].AxisY.LabelStyle.Font.Size / scaleF);
                        }
                    }
                    else control.Font = new Font(control.Font.FontFamily, control.Font.Size / scaleF);
                }
                
                scale = 1;
            }
        }

        #endregion

        int connectStatus = -1; // -1 = not connected, 0 = board connected but most likely powering issue, 1 = board connected
        private void button_connect_Click(object sender, EventArgs e)
        {
            // If usb already opened, close it.
            if (usbDevId > 0)
            {
                USB.CloseUsbDevice(usbDevId);
                usbDevId = 0;
            }
            if (usbMezzId > 0)
            {
                USB.CloseUsbDevice(usbMezzId);
                usbMezzId = 0;
            }

            // if connectStatus was "connected", make it disconnected and return
            if (connectStatus == 1)
            {
                button_connect.BackColor = Color.Gainsboro;
                button_connect.ForeColor = Color.Black;
                connectStatus = -1;
                return;
            }

            // check for usb devices
            ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);
            
            if (ftdiDeviceCount > 0)
            {
                ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];
                ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);
                
                int index = 0;
                for (int i = 0; i < ftdiDeviceList.Length; i++)
                    if (ftdiDeviceList[i].Description == "PCB_Citiroc A") index = i;
                
                testBoardFtdiDevice[0] = ftdiDeviceList[index];

                int numUsbDev = USB.USB_GetNumberOfDevs();
                // Open the usb device
                usbDevId = USB.OpenUsbDevice(testBoardFtdiDevice[0].SerialNumber);

                bool retVerbose = false;
                bool retUsbOpen = USB.USB_Init(usbDevId, ref retVerbose);
                bool retSetLT = USB.USB_SetLatencyTimer(usbDevId, 2);

                // Read Latency Time from FPGA
                byte[] templtime = new byte[1];
                unsafe
                {
                    fixed (byte* array = templtime)
                    {
                        bool ret_value = USB.USB_GetLatencyTimer(usbDevId, array);
                    }
                }
                string strLatency = templtime[0].ToString();

                bool retSetBufSize = USB.USB_SetXferSize(usbDevId, 8192, 32768);
                bool retSetTimeOuts = USB.USB_SetTimeouts(usbDevId, 20, 20);
                
                if (retUsbOpen && retSetLT && retSetBufSize && retSetTimeOuts)
                {
                    byte[] tempRx = new byte[1];
                    string rdSubAdd100 = "00000000";
                    int testCon = 0;
                    // Try to connect to the device up to 10 times
                    while (rdSubAdd100 == "00000000" && testCon < 10)
                    {
                        testCon++;
                        // Sub address 100 contain the firware version. If rdSubAdd100 == 0 the board failed to connect
                        rdSubAdd100 = Firmware.readWord(100, usbDevId);
                        // Wait 20 ms
                        Thread.Sleep(20);
                    }
                    
                    // If rdSubAdd100 == 0, the board failed to connect 10 times in a row
                    if (rdSubAdd100 == "00000000")
                    {
                        MessageBox.Show("Looks like there is an issue with the testboard connection. Please verify the board is well plugged and powered and click again.");
                        connectStatus = 0;
                        button_connect.BackColor = Color.IndianRed;
                        button_connect.ForeColor = Color.White;
                        return;
                    }
                    else
                    {
                        Firmware.sendWord(0, "00110100", usbDevId);
                        Firmware.sendWord(1, "11110100", usbDevId);
                        Firmware.sendWord(2, "00000100", usbDevId);
                        Firmware.sendWord(3, "10011001", usbDevId);
                        Firmware.sendWord(5, "00000001", usbDevId);
                        Firmware.sendWord(30, "11111111", usbDevId);

                        sendSC(usbDevId, strDefSC);
                    }
                }
                
                // Connect the mezzanine board
                index = 0;
                for (int i = 0; i < ftdiDeviceList.Length; i++)
                    if (ftdiDeviceList[i].Description == "PCB_Test_Citiroc A") index = i;

                mezzBoardFtdiDevice[0] = ftdiDeviceList[index];

                numUsbDev = USB.USB_GetNumberOfDevs();
                // Open the usb device
                usbMezzId = USB.OpenUsbDevice(mezzBoardFtdiDevice[0].SerialNumber);

                retVerbose = false;
                retUsbOpen = USB.USB_Init(usbMezzId, ref retVerbose);
                retSetLT = USB.USB_SetLatencyTimer(usbMezzId, 2);

                // Read Latency Time from FPGA
                templtime = new byte[1];
                unsafe
                {
                    fixed (byte* array = templtime)
                    {
                        bool ret_value = USB.USB_GetLatencyTimer(usbMezzId, array);
                    }
                }
                strLatency = templtime[0].ToString();

                retSetBufSize = USB.USB_SetXferSize(usbMezzId, 8192, 32768);
                retSetTimeOuts = USB.USB_SetTimeouts(usbMezzId, 20, 20);

                if (retUsbOpen && retSetLT && retSetBufSize && retSetTimeOuts)
                {
                    byte[] tempRx = new byte[1];
                    string rdSubAdd100 = "00000000";
                    int testCon = 0;
                    // Try to connect to the device up to 10 times
                    while (rdSubAdd100 == "00000000" && testCon < 10)
                    {
                        testCon++;
                        // Sub address 100 contain the firware version. If rdSubAdd100 == 0 the board failed to connect
                        rdSubAdd100 = Firmware.readWord(100, usbMezzId);
                        // Wait 20 ms
                        Thread.Sleep(20);
                    }

                    // If rdSubAdd100 == 0, the board failed to connect 10 times in a row
                    if (rdSubAdd100 == "00000000")
                    {
                        MessageBox.Show("Looks like there is an issue with the mezzanine board.");
                        connectStatus = 0;
                        button_connect.BackColor = Color.IndianRed;
                        button_connect.ForeColor = Color.White;
                        return;
                    }
                    else
                    {
                        connectStatus = 1;
                        button_connect.BackColor = WeerocGreen;
                        button_connect.ForeColor = Color.White;
                    }
                }
            }
            else
            {
                MessageBox.Show("No USB Devices are connected.");
                connectStatus = -1;
                button_connect.BackColor = Color.IndianRed;
                button_connect.ForeColor = Color.White;
            }
        }

        public void ConnectUSB()
        {
            // If usb already opened, close it.
            if (usbDevId > 0)
            {
                USB.CloseUsbDevice(usbDevId);
                usbDevId = 0;
            }
            if (usbMezzId > 0)
            {
                USB.CloseUsbDevice(usbMezzId);
                usbMezzId = 0;
            }

            // if connectStatus was "connected", make it disconnected and return
            if (connectStatus == 1)
            {
                button_connect.BackColor = Color.Gainsboro;
                button_connect.ForeColor = Color.Black;
                connectStatus = -1;
                return;
            }

            // check for usb devices
            ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);

            if (ftdiDeviceCount > 0)
            {
                ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];
                ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);

                int index = 0;
                for (int i = 0; i < ftdiDeviceList.Length; i++)
                    if (ftdiDeviceList[i].Description == "PCB_Citiroc A") index = i;

                testBoardFtdiDevice[0] = ftdiDeviceList[index];

                int numUsbDev = USB.USB_GetNumberOfDevs();
                // Open the usb device
                usbDevId = USB.OpenUsbDevice(testBoardFtdiDevice[0].SerialNumber);

                bool retVerbose = false;
                bool retUsbOpen = USB.USB_Init(usbDevId, ref retVerbose);
                bool retSetLT = USB.USB_SetLatencyTimer(usbDevId, 2);

                // Read Latency Time from FPGA
                byte[] templtime = new byte[1];
                unsafe
                {
                    fixed (byte* array = templtime)
                    {
                        bool ret_value = USB.USB_GetLatencyTimer(usbDevId, array);
                    }
                }
                string strLatency = templtime[0].ToString();

                bool retSetBufSize = USB.USB_SetXferSize(usbDevId, 8192, 32768);
                bool retSetTimeOuts = USB.USB_SetTimeouts(usbDevId, 20, 20);

                if (retUsbOpen && retSetLT && retSetBufSize && retSetTimeOuts)
                {
                    byte[] tempRx = new byte[1];
                    string rdSubAdd100 = "00000000";
                    int testCon = 0;
                    // Try to connect to the device up to 10 times
                    while (rdSubAdd100 == "00000000" && testCon < 10)
                    {
                        testCon++;
                        // Sub address 100 contain the firware version. If rdSubAdd100 == 0 the board failed to connect
                        rdSubAdd100 = Firmware.readWord(100, usbDevId);
                        // Wait 20 ms
                        Thread.Sleep(20);
                    }

                    // If rdSubAdd100 == 0, the board failed to connect 10 times in a row
                    if (rdSubAdd100 == "00000000")
                    {
                        MessageBox.Show("Looks like there is an issue with the testboard connection. Please verify the board is well plugged and powered and click again.");
                        connectStatus = 0;
                        button_connect.BackColor = Color.IndianRed;
                        button_connect.ForeColor = Color.White;
                        return;
                    }
                    else
                    {
                        Firmware.sendWord(0, "00110100", usbDevId);
                        Firmware.sendWord(1, "11110100", usbDevId);
                        Firmware.sendWord(2, "00000100", usbDevId);
                        Firmware.sendWord(3, "10011001", usbDevId);
                        Firmware.sendWord(5, "00000001", usbDevId);
                        Firmware.sendWord(30, "11111111", usbDevId);

                        sendSC(usbDevId, strDefSC);
                    }
                }

                // Connect the mezzanine board
                index = 0;
                for (int i = 0; i < ftdiDeviceList.Length; i++)
                    if (ftdiDeviceList[i].Description == "PCB_Test_Citiroc A") index = i;

                mezzBoardFtdiDevice[0] = ftdiDeviceList[index];

                numUsbDev = USB.USB_GetNumberOfDevs();
                // Open the usb device
                usbMezzId = USB.OpenUsbDevice(mezzBoardFtdiDevice[0].SerialNumber);

                retVerbose = false;
                retUsbOpen = USB.USB_Init(usbMezzId, ref retVerbose);
                retSetLT = USB.USB_SetLatencyTimer(usbMezzId, 2);

                // Read Latency Time from FPGA
                templtime = new byte[1];
                unsafe
                {
                    fixed (byte* array = templtime)
                    {
                        bool ret_value = USB.USB_GetLatencyTimer(usbMezzId, array);
                    }
                }
                strLatency = templtime[0].ToString();

                retSetBufSize = USB.USB_SetXferSize(usbMezzId, 8192, 32768);
                retSetTimeOuts = USB.USB_SetTimeouts(usbMezzId, 20, 20);

                if (retUsbOpen && retSetLT && retSetBufSize && retSetTimeOuts)
                {
                    byte[] tempRx = new byte[1];
                    string rdSubAdd100 = "00000000";
                    int testCon = 0;
                    // Try to connect to the device up to 10 times
                    while (rdSubAdd100 == "00000000" && testCon < 10)
                    {
                        testCon++;
                        // Sub address 100 contain the firware version. If rdSubAdd100 == 0 the board failed to connect
                        rdSubAdd100 = Firmware.readWord(100, usbMezzId);
                        // Wait 20 ms
                        Thread.Sleep(20);
                    }

                    // If rdSubAdd100 == 0, the board failed to connect 10 times in a row
                    if (rdSubAdd100 == "00000000")
                    {
                        MessageBox.Show("Looks like there is an issue with the mezzanine board.");
                        connectStatus = 0;
                        button_connect.BackColor = Color.IndianRed;
                        button_connect.ForeColor = Color.White;
                        return;
                    }
                    else
                    {
                        connectStatus = 1;
                        button_connect.BackColor = WeerocGreen;
                        button_connect.ForeColor = Color.White;
                    }
                }
                MessageBox.Show("Connection successful");
            }
            else
            {
                MessageBox.Show("No USB Devices are connected.");
                connectStatus = -1;
                button_connect.BackColor = Color.IndianRed;
                button_connect.ForeColor = Color.White;
            }
        }


        public String Date
        {
            get { return date; }
            set { date = value; }
        }

        string strSaveName = "";
        private void button_start_Click(object sender, EventArgs e)
        {
            
            if (backgroundWorker_serialTest.IsBusy) { return; }
            ConnectUSB();
            if (Firmware.readWord(100, usbDevId) == "00000000")
            {
                button_connect.BackColor = Color.Gainsboro;
                button_connect.ForeColor = Color.Black;
                connectStatus = -1;
                MessageBox.Show("No USB Devices found.", "Warning", MessageBoxButtons.OKCancel);
                return;
            }

            sendSC(usbDevId, strDefSC);
            sendReadRegister();

            /*SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Save file";
            saveDialog.RestoreDirectory = true;
            saveDialog.FileName = "Citiroc1A_" + textBox_asicNumber.Text;

            if (saveDialog.ShowDialog() == DialogResult.OK) strSaveName = saveDialog.FileName;
            else return;*/
            //String solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            //strSaveName = solutionDirectory + @"\screens\\" + textBox_asicNumber;
            strSaveName = date + "+" + textBox_asicNumber.Text;
            label_results.Text = "";
            label_DcInfo.Text = "";
            chart_inputDac.Series.Clear();
            chart_dacCharge.Series.Clear();
            chart_dacTime.Series.Clear();
            chart_ScurvesSerialCharge.Series.Clear();
            chart_ScurvesSerialTime.Series.Clear();
            chart_chargeHGserial.Series.Clear();
            chart_chargeLGserial.Series.Clear();

            Firmware.sendWord(2, "00000000", usbMezzId);
            Firmware.sendWord(3, "00000000", usbMezzId);
            Firmware.sendWord(4, "00000000", usbMezzId);
            Firmware.sendWord(5, "00000000", usbMezzId);

            backgroundWorker_serialTest.RunWorkerAsync();
        }
        public static bool isFinished = true;
        public bool IsFinished
        {
            get { return isFinished; }
        }
        public void start()
        {
            isFinished = false;
            if (backgroundWorker_serialTest.IsBusy) { return; }
            if (Firmware.readWord(100, usbDevId) == "00000000")
            {
                button_connect.BackColor = Color.Gainsboro;
                button_connect.ForeColor = Color.Black;
                connectStatus = -1;
                MessageBox.Show("No USB Devices found.", "Warning", MessageBoxButtons.OKCancel);
                return;
            }

            sendSC(usbDevId, strDefSC);
            sendReadRegister();

            /*SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Save file";
            saveDialog.RestoreDirectory = true;
            saveDialog.FileName = "Citiroc1A_" + textBox_asicNumber.Text;

            if (saveDialog.ShowDialog() == DialogResult.OK) strSaveName = saveDialog.FileName;
            else return;*/
            //String solutionDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            //strSaveName = solutionDirectory + @"\screens\\" + textBox_asicNumber;
            strSaveName = date + "+" + textBox_asicNumber.Text;
            label_results.Text = "";
            label_DcInfo.Text = "";
            chart_inputDac.Series.Clear();
            chart_dacCharge.Series.Clear();
            chart_dacTime.Series.Clear();
            chart_ScurvesSerialCharge.Series.Clear();
            chart_ScurvesSerialTime.Series.Clear();
            chart_chargeHGserial.Series.Clear();
            chart_chargeLGserial.Series.Clear();

            Firmware.sendWord(2, "00000000", usbMezzId);
            Firmware.sendWord(3, "00000000", usbMezzId);
            Firmware.sendWord(4, "00000000", usbMezzId);
            Firmware.sendWord(5, "00000000", usbMezzId);

            backgroundWorker_serialTest.RunWorkerAsync();
        }

        bool TestHit_OK = true;
        bool TestDC_OK = true;
        bool TestInDac_OK = true;
        bool TestThDac_OK = true;
        bool TestScurves_OK = true;
        bool isGood = true;
        private void backgroundWorker_serialTest_DoWork(object sender, EventArgs e)
        {
            TestHit_OK = true;
            TestDC_OK = true;
            TestInDac_OK = true;
            TestThDac_OK = true;
            TestScurves_OK = true;

            Stopwatch sw = Stopwatch.StartNew();
            
            TextWriter tw = new StreamWriter(strSaveName);

            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = customCulture;

            ke2000 dmm;

            try { dmm = new ke2000("GPIB0::16::INSTR", true, true); }
            catch { MessageBox.Show("ke2000 not connected"); return; }

            uint instr = 0;
            uint defaultRM = 0;
            
            VISA2_2.VISA.viOpenDefaultRM(ref defaultRM);
            VISA2_2.VISA.viOpen(defaultRM, "GPIB0::16::INSTR", 0, 1000, ref instr);
            uint count = 0;
            double[] reading = new double[10];

            VISA2_2.VISA.viClear(instr);

            UpdatingLabel("Step 1/5 : DC measurements", label_results);

            #region DC

            string strSCPI = ":ROUT:OPEN:ALL";
            VISA2_2.VISA.viWrite(instr, strSCPI, (uint)strSCPI.Length, ref count);

            for (int i = 1; i < 11; i++)
            {
                strSCPI = ":ROUT:CLOS (@" + i.ToString() + ")";
                VISA2_2.VISA.viWrite(instr, strSCPI, (uint)strSCPI.Length, ref count);
                reading[i - 1] = dmm.Read(1000);

                strSCPI = ":ROUT:OPEN:ALL";
                VISA2_2.VISA.viWrite(instr, strSCPI, (uint)strSCPI.Length, ref count);
            }

            double consumption = Math.Round(reading[0] * 1000, 3);
            double vbg = Math.Round(reading[9], 3);
            double DCHG = Math.Round(reading[1], 3);
            double DCLG = Math.Round(reading[2], 3);

            UpdatingLabel("     Current consumption = " + Math.Round(reading[0] * 1000, 3) + " mA ; vbg = " + Math.Round(reading[9], 3) + " V ; DC HG = " + Math.Round(reading[1], 3) + " V ; DC LG = " + Math.Round(reading[2], 3) + " V ; Temp = " + Math.Round((2.75 - reading[5]) / 8e-3, 1) + " °C", label_DcInfo);

            tw.WriteLine("Current consumption (mA):\n" + Math.Round(reading[0] * 1000, 3) + "\nvbg (V):\n" + Math.Round(reading[9], 3) + "\nDC HG (V):\n" + Math.Round(reading[1], 3) + "\nDC LG (V):\n" + Math.Round(reading[2], 3) + "\nTemp (°C):\n" + Math.Round((2.75 - reading[5]) / 8e-3, 1));

            #endregion

            if (consumption > Hconsumption || consumption < Lconsumption || vbg < Lvbg || vbg > Hvbg || DCHG < LDCHG || DCHG > HDCHG || DCLG < LDCLG || DCLG > HDCLG) TestDC_OK = false;

            UpdatingLabel("Step 2/5 : Input DAC", label_results);

            #region input DAC

            string strSC = strDefSC;

            string strTempSC;
            string strTemp1;
            string strTemp2;
            int nbPts = 256 / inputDacStep + 1;

            double[,] inputDacArray = new double[nbPts, NbChannels + 1];

            for (int i = 0; i < nbPts - 1; i++)
                inputDacArray[i, 0] = i * 256 / nbPts;

            inputDacArray[nbPts - 1, 0] = 255;

            strTemp1 = strSC.Substring(0, 331);
            strTemp2 = strSC.Substring(619, 525);

            count = 0;
            strSCPI = ":ROUT:CLOS (@5)";
            VISA2_2.VISA.viWrite(instr, strSCPI, (uint)strSCPI.Length, ref count);

            for (int i = 0; i < nbPts; i++)
            {
                string dacCodes = "";
                for (int chn = 0; chn < NbChannels; chn++) dacCodes += IntToBin((int)inputDacArray[i, 0], 8) + "1";

                strTempSC = strTemp1 + dacCodes + strTemp2;

                sendSC(usbDevId, strTempSC);
                Thread.Sleep(1000);
                if (i == 1) Thread.Sleep(2000);
                if (i == 0) Thread.Sleep(10000);

                for (int chn = 0; chn < NbChannels; chn++)
                {
                    sendInputDacProbes(usbDevId, chn);
                    Thread.Sleep(100);
                    inputDacArray[i, chn + 1] = dmm.Read(1000);
                }
            }
            
            sendInputDacProbes(usbDevId, 33); // reset the probe register.

            tw.WriteLine(inputDacArray.GetLength(0) + " data for input Dac");
            for (int i = 0; i < inputDacArray.GetLength(0); i++)
            {
                for (int chn = 0; chn < NbChannels + 1; chn++)
                {
                    tw.Write(inputDacArray[i, chn] + " ");
                }
                tw.WriteLine();
            }

            #endregion
            
            UpdatingLabel("Step 3/5 : Threshold DAC", label_results);

            #region threshold DACs

            double[] X = new double[1024 / thresholdDacStep];
            double[] dacCharge = new double[1024 / thresholdDacStep];

            strTemp1 = strSC.Substring(0, 1117);
            strTemp2 = strSC.Substring(1127, 17);

            strSCPI = ":ROUT:CLOS (@8)";
            VISA2_2.VISA.viWrite(instr, strSCPI, (uint)strSCPI.Length, ref count);

            tw.WriteLine(X.Length + " data for linearity DAC charge");

            for (int dacCode = 0; dacCode < 1024; dacCode += thresholdDacStep)
            {
                strTempSC = strTemp1 + IntToBin(dacCode, 10) + strTemp2;
                sendSC(usbDevId, strTempSC);

                X[dacCode / thresholdDacStep] = dacCode;
                dacCharge[dacCode / thresholdDacStep] = dmm.Read(100);

                tw.WriteLine(X[dacCode / thresholdDacStep] + " " + dacCharge[dacCode / thresholdDacStep]);
            }

            strSCPI = ":ROUT:OPEN:ALL";
            VISA2_2.VISA.viWrite(instr, strSCPI, (uint)strSCPI.Length, ref count);

            double[] dacTime = new double[1024 / thresholdDacStep];

            strTemp1 = strSC.Substring(0, 1107);
            strTemp2 = strSC.Substring(1117, 27);

            strSCPI = ":ROUT:CLOS (@9)";
            VISA2_2.VISA.viWrite(instr, strSCPI, (uint)strSCPI.Length, ref count);

            tw.WriteLine(X.Length + " data for linearity DAC time");

            for (int dacCode = 0; dacCode < 1024; dacCode += thresholdDacStep)
            {
                strTempSC = strTemp1 + IntToBin(dacCode, 10) + strTemp2;
                sendSC(usbDevId, strTempSC);

                X[dacCode / thresholdDacStep] = dacCode;
                dacTime[dacCode / thresholdDacStep] = dmm.Read(100);

                tw.WriteLine(X[dacCode / thresholdDacStep] + " " + dacTime[dacCode / thresholdDacStep]);
            }

            tw.Flush();

            #endregion

            Firmware.sendWord(7, "00011000", usbDevId); // set Scurve clock to 100 kHz
            UpdatingLabel("Step 4/5 : S-curves", label_results);

            #region S-curves

            sendSC(usbDevId, strSC);

            int VthMin = 160;
            int VthMax = 220;

            double[,] ScurvesPedCharge;
            double[,] ScurvesSigCharge;

            Scurves(false, VthMin, VthMax, VthStep, chart_ScurvesSerialCharge, out ScurvesPedCharge, out ScurvesSigCharge);

            // Export series values into a DataSet object and write it in output file
            tw.WriteLine(ScurvesPedCharge.GetLength(0) + " data for S-curves pedestal charge");
            for (int i = 0; i < ScurvesPedCharge.GetLength(0); i++)
            {
                for (int chn = 0; chn < NbChannels + 1; chn++)
                {
                    tw.Write(ScurvesPedCharge[i, chn] + " ");
                }
                tw.WriteLine();
            }
            tw.WriteLine(ScurvesSigCharge.GetLength(0) + " data for S-curves signal charge");
            for (int i = 0; i < ScurvesSigCharge.GetLength(0); i++)
            {
                for (int chn = 0; chn < NbChannels + 1; chn++)
                {
                    tw.Write(ScurvesSigCharge[i, chn] + " ");
                }
                tw.WriteLine();
            }

            tw.Flush();

            sendSC(usbDevId, strSC);

            //VthMin = 160;
            //VthMax = 220;

            double[,] ScurvesPedTime;
            double[,] ScurvesSigTime;

            Scurves(true, VthMin, VthMax, VthStep, chart_ScurvesSerialTime, out ScurvesPedTime, out ScurvesSigTime);

            // Export series values into a DataSet object and write it in output file
            tw.WriteLine(ScurvesPedTime.GetLength(0) + " data for S-curves pedestal time");
            for (int i = 0; i < ScurvesPedTime.GetLength(0); i++)
            {
                for (int chn = 0; chn < NbChannels + 1; chn++)
                {
                    tw.Write(ScurvesPedTime[i, chn] + " ");
                }
                tw.WriteLine();
            }
            tw.WriteLine(ScurvesSigTime.GetLength(0) + " data for S-curves signal time");
            for (int i = 0; i < ScurvesSigTime.GetLength(0); i++)
            {
                for (int chn = 0; chn < NbChannels + 1; chn++)
                {
                    tw.Write(ScurvesSigTime[i, chn] + " ");
                }
                tw.WriteLine();
            }

            tw.Flush();

            #endregion

            //System.Media.SystemSounds.Beep.Play();
            //MessageBox.Show("Set charge injection ON.");
            
            UpdatingLabel("Step 5/5 : Charge measurements", label_results);

            #region charge measurement

            int nbAcq = 100;

            double[] dataHGped = new double[NbChannels + 1];
            double[] dataLGped = new double[NbChannels + 1];

            #region on pedestal

            strTemp1 = strSC.Substring(0, 1107);
            strTemp2 = strSC.Substring(1117, 27);

            sendSC(usbDevId, strTemp1 + IntToBin(100, 10) + strTemp2);

            // SubAdd 45 store the number of acquisitions to do and save in FIFO before reading it.
            string strNbAcq = IntToBin(nbAcq, 8);
            Firmware.sendWord(45, strNbAcq, usbDevId);

            Firmware.sendWord(2, "00000000", usbMezzId);
            Firmware.sendWord(3, "00000000", usbMezzId);
            Firmware.sendWord(4, "00000000", usbMezzId);
            Firmware.sendWord(5, "00000000", usbMezzId);

            Firmware.sendWord(43, "10000000", usbDevId);

            string rd4 = "00000000";

            // bit 7 of subAdd 4 is 1 when the acquisitions are done
            while (rd4.Substring(7, 1) == "0") { Thread.Sleep(10); rd4 = Firmware.readWord(4, usbDevId); }

            string subAdd22 = Firmware.readWord(22, usbDevId);

            int nbData = (NbChannels + 1) * nbAcq;

            byte[] FIFO20 = Firmware.readWord(20, nbData, usbDevId);
            byte[] FIFO21 = Firmware.readWord(21, nbData, usbDevId);
            byte[] FIFO23 = Firmware.readWord(23, nbData, usbDevId);
            byte[] FIFO24 = Firmware.readWord(24, nbData, usbDevId);

            byte[] FIFOHG = new byte[2 * nbData];
            byte[] FIFOLG = new byte[2 * nbData];

            for (int i = 0; i < nbData; i++)
            {
                FIFOHG[i * 2 + 1] = FIFO20[i];
                FIFOHG[i * 2 + 0] = FIFO21[i];
                FIFOLG[i * 2 + 1] = FIFO23[i];
                FIFOLG[i * 2 + 0] = FIFO24[i];
            }

            BitArray bitArrayHG = new BitArray(FIFOHG);
            BitArray bitArrayLG = new BitArray(FIFOLG);

            for (int i = 0; i < nbAcq; i++)
            {
                for (int chn = 0; chn < NbChannels + 1; chn++)
                {
                    bool[] boolArrayDataHG = { bitArrayHG[i * 528 + chn * 16 + 0], bitArrayHG[i * 528 + chn * 16 + 1], bitArrayHG[i * 528 + chn * 16 + 2], bitArrayHG[i * 528 + chn * 16 + 3], bitArrayHG[i * 528 + chn * 16 + 4], bitArrayHG[i * 528 + chn * 16 + 5],
                            bitArrayHG[i * 528 + chn * 16 + 6], bitArrayHG[i * 528 + chn * 16 + 7], bitArrayHG[i * 528 + chn * 16 + 8], bitArrayHG[i * 528 + chn * 16 + 9], bitArrayHG[i * 528 + chn * 16 + 10], bitArrayHG[i * 528 + chn * 16 + 11] };
                    BitArray bitArrayDataHG = new BitArray(boolArrayDataHG);
                    int[] array = new int[1];
                    bitArrayDataHG.CopyTo(array, 0);
                    dataHGped[chn] += array[0];

                    bool[] boolArrayDataLG = { bitArrayLG[i * 528 + chn * 16 + 0], bitArrayLG[i * 528 + chn * 16 + 1], bitArrayLG[i * 528 + chn * 16 + 2], bitArrayLG[i * 528 + chn * 16 + 3], bitArrayLG[i * 528 + chn * 16 + 4], bitArrayLG[i * 528 + chn * 16 + 5],
                            bitArrayLG[i * 528 + chn * 16 + 6], bitArrayLG[i * 528 + chn * 16 + 7], bitArrayLG[i * 528 + chn * 16 + 8], bitArrayLG[i * 528 + chn * 16 + 9], bitArrayLG[i * 528 + chn * 16 + 10], bitArrayLG[i * 528 + chn * 16 + 11] };
                    BitArray bitArrayDataLG = new BitArray(boolArrayDataLG);
                    bitArrayDataLG.CopyTo(array, 0);
                    dataLGped[chn] += array[0];
                }
            }

            Firmware.sendWord(43, "00000000", usbDevId);

            for (int chn = 0; chn < NbChannels + 1; chn++)
            {
                dataHGped[chn] /= nbAcq;
                dataLGped[chn] /= nbAcq;
            }

            tw.WriteLine("High gain charge measurement on pedestal");

            for (int chn = 0; chn < NbChannels + 1; chn++)
            {
                //chart_chargeHGserial.Series["pedestal"].Points.AddXY(chn, dataHGped[chn]);

                tw.Write(dataHGped[chn] + " ");
            }
            tw.WriteLine();

            tw.WriteLine("Low gain charge measurement on pedestal");

            for (int chn = 0; chn < NbChannels + 1; chn++)
            {
                //chart_chargeLGserial.Series["pedestal"].Points.AddXY(chn, dataLGped[chn]);

                tw.Write(dataLGped[chn] + " ");
            }
            tw.WriteLine();

            #endregion

            tw.Flush();

            #region on signal

            sendSC(usbDevId, strTemp1 + IntToBin(300, 10) + strTemp2);

            double[] dataHGsig = new double[NbChannels + 1];
            double[] dataLGsig = new double[NbChannels + 1];
            double[] hit = new double[NbChannels + 1];

            for (int chn = 0; chn < NbChannels; chn++)
            {
                Array.Clear(hit, 0, hit.Length);

                // SubAdd 45 store the number of acquisitions to do and save in FIFO before reading it.
                Firmware.sendWord(45, strNbAcq, usbDevId);

                int channel = (int)(Math.Pow(2, chn));
                string strChannel = IntToBin(channel, 32);

                string w2 = strChannel.Substring(24, 8);
                string w3 = strChannel.Substring(16, 8);
                string w4 = strChannel.Substring(8, 8);
                string w5 = strChannel.Substring(0, 8);
                
                Firmware.sendWord(2, w2, usbMezzId);
                Firmware.sendWord(3, w3, usbMezzId);
                Firmware.sendWord(4, w4, usbMezzId);
                Firmware.sendWord(5, w5, usbMezzId);

                Firmware.sendWord(43, "10000000", usbDevId);

                rd4 = "00000000";

                // bit 7 of subAdd 4 is 1 when the acquisitions are done
                Stopwatch swAcq = Stopwatch.StartNew();
                while (rd4.Substring(7, 1) == "0" && swAcq.Elapsed.Seconds < 5) { Thread.Sleep(10); rd4 = Firmware.readWord(4, usbDevId); }

                if (subAdd22 != "00000000") continue;

                FIFO20 = Firmware.readWord(20, nbData, usbDevId);
                FIFO21 = Firmware.readWord(21, nbData, usbDevId);
                FIFO23 = Firmware.readWord(23, nbData, usbDevId);
                FIFO24 = Firmware.readWord(24, nbData, usbDevId);

                FIFOHG = new byte[2 * nbData];
                FIFOLG = new byte[2 * nbData];

                for (int i = 0; i < nbData; i++)
                {
                    FIFOHG[i * 2 + 1] = FIFO20[i];
                    FIFOHG[i * 2 + 0] = FIFO21[i];
                    FIFOLG[i * 2 + 1] = FIFO23[i];
                    FIFOLG[i * 2 + 0] = FIFO24[i];
                }

                bitArrayHG = new BitArray(FIFOHG);
                bitArrayLG = new BitArray(FIFOLG);

                for (int i = 0; i < nbAcq; i++)
                {
                    for (int ii = 0; ii < NbChannels; ii++)
                    {
                        bool[] boolArrayDataHG = { bitArrayHG[i * 528 + ii * 16 + 0], bitArrayHG[i * 528 + ii * 16 + 1], bitArrayHG[i * 528 + ii * 16 + 2], bitArrayHG[i * 528 + ii * 16 + 3], bitArrayHG[i * 528 + ii * 16 + 4], bitArrayHG[i * 528 + ii * 16 + 5],
                            bitArrayHG[i * 528 + ii * 16 + 6], bitArrayHG[i * 528 + ii * 16 + 7], bitArrayHG[i * 528 + ii * 16 + 8], bitArrayHG[i * 528 + ii * 16 + 9], bitArrayHG[i * 528 + ii * 16 + 10], bitArrayHG[i * 528 + ii * 16 + 11] };
                        BitArray bitArrayDataHG = new BitArray(boolArrayDataHG);
                        int[] array = new int[1];
                        bitArrayDataHG.CopyTo(array, 0);
                        if (ii == chn) dataHGsig[chn] += array[0];

                        bool[] boolArrayDataLG = { bitArrayLG[i * 528 + ii * 16 + 0], bitArrayLG[i * 528 + ii * 16 + 1], bitArrayLG[i * 528 + ii * 16 + 2], bitArrayLG[i * 528 + ii * 16 + 3], bitArrayLG[i * 528 + ii * 16 + 4], bitArrayLG[i * 528 + ii * 16 + 5],
                            bitArrayLG[i * 528 + ii * 16 + 6], bitArrayLG[i * 528 + ii * 16 + 7], bitArrayLG[i * 528 + ii * 16 + 8], bitArrayLG[i * 528 + ii * 16 + 9], bitArrayLG[i * 528 + ii * 16 + 10], bitArrayLG[i * 528 + ii * 16 + 11] };
                        BitArray bitArrayDataLG = new BitArray(boolArrayDataLG);
                        bitArrayDataLG.CopyTo(array, 0);
                        if (ii == chn) dataLGsig[chn] += array[0];

                        hit[ii] += Convert.ToInt32(bitArrayHG[i * 528 + ii * 16 + 13]);
                    }                    
                }

                dataHGsig[chn] /= nbAcq;
                dataLGsig[chn] /= nbAcq;

                if (hit[chn] != nbAcq || hit.Sum() != nbAcq) TestHit_OK = false;
                
                Firmware.sendWord(2, "00000000", usbMezzId);
                Firmware.sendWord(3, "00000000", usbMezzId);
                Firmware.sendWord(4, "00000000", usbMezzId);
                Firmware.sendWord(5, "00000000", usbMezzId);
                Firmware.sendWord(43, "00000000", usbDevId);

            }

            tw.WriteLine("High gain charge measurement on signal");

            for (int chn = 0; chn < NbChannels; chn++)
            {
                //chart_chargeHGserial.Series["signal"].Points.AddXY(chn, dataHGsig[chn]);

                tw.Write(dataHGsig[chn] + " ");
            }
            tw.WriteLine();

            tw.WriteLine("Low gain charge measurement on signal");

            for (int chn = 0; chn < NbChannels; chn++)
            {
                //chart_chargeLGserial.Series["signal"].Points.AddXY(chn, dataLGsig[chn]);

                tw.Write(dataLGsig[chn] + " ");
            }
            tw.WriteLine();
            
            tw.WriteLine("Hit " + ((TestHit_OK) ? "OK" : "error"));
            #endregion

            #endregion

            tw.Close();
            
            sw.Stop();

            UpdatingLabel("", label_results);
            //MessageBox.Show("Elapsed time (s): " + Math.Round(sw.ElapsedMilliseconds / 1000D));
        }

        private void backgroundWorker_serialTest_RunWorkerCompleted(object sender, EventArgs e)
        {
            loadSerialData(strSaveName, true);

            Form frm = label_titleBar.FindForm();
                        
            frm.Update();

            try
            {
                using (Bitmap bitmap = new Bitmap(frm.Width, frm.Height))
                {
                    using (Graphics gb = Graphics.FromImage(bitmap))
                    using (Graphics gc = Graphics.FromHwnd(ActiveForm.Handle))
                    {

                        IntPtr hdcDest = IntPtr.Zero;
                        IntPtr hdcSrc = IntPtr.Zero;

                        try
                        {
                            hdcDest = gb.GetHdc();
                            hdcSrc = gc.GetHdc();

                            BitBlt(hdcDest, 0, 0, bitmap.Width, bitmap.Height, hdcSrc, 0, 0, SRC_COPY);
                        }
                        finally
                        {
                            if (hdcDest != IntPtr.Zero) gb.ReleaseHdc(hdcDest);
                            if (hdcSrc != IntPtr.Zero) gc.ReleaseHdc(hdcSrc);
                        }
                    }

                    bitmap.Save(strSaveName + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            catch { }
            
            sendSC(usbDevId, strDefSC);

            //System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer();

            if (TestDC_OK && TestInDac_OK && TestThDac_OK && TestScurves_OK && TestHit_OK)
            {
                /*soundPlayer.SoundLocation = "ZeldaTreasureChest.wav";
                soundPlayer.Play();*/

                MessageBox.Show("DC OK = " + TestDC_OK.ToString()
                    + "\nInput DAC OK = " + TestInDac_OK.ToString()
                    + "\nThreshold DAC OK = " + TestThDac_OK.ToString()
                    + "\nS-curves OK = " + TestScurves_OK.ToString()
                    + "\nHit OK = " + TestHit_OK.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.None);

                var chip1 = new object[1, 7]
                {
                     { date+"+"+textBox_asicNumber.Text, "YES", TestDC_OK,TestInDac_OK,TestThDac_OK,TestScurves_OK,TestHit_OK },
                };
                AjouterChip(chip1);
                isGood = true;

            }
            else
            {
                /*soundPlayer.SoundLocation = "MarioDeath.wav";
                soundPlayer.Play();*/

                /*MessageBox.Show("DC OK = " + TestDC_OK.ToString()
                    + "\nInput DAC OK = " + TestInDac_OK.ToString()
                    + "\nThreshold DAC OK = " + TestThDac_OK.ToString()
                    + "\nS-curves OK = " + TestScurves_OK.ToString()
                    + "\nHit OK = " + TestHit_OK.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);*/

                var chip0 = new object[1, 7]
                {
                    { date+"+"+textBox_asicNumber.Text, "NO", TestDC_OK,TestInDac_OK,TestThDac_OK,TestScurves_OK,TestHit_OK },
                };
                AjouterChip(chip0);
                isGood = false;
            }

            if (File.Exists(strSaveName + ".png") == false)
            {
                MessageBox.Show("Screenshot failed ! Click on \"Save image\".");
            }
            isFinished = true;
        }

        private bool sendInputDacProbes(int usbDevId, int chn)
        {
            bool result = false;
            char[] tmpProbeStream = new char[256];

            for (int i = 0; i < 256; i++) tmpProbeStream[i] = '0';

            if (chn < NbChannels) tmpProbeStream[224 + chn] = '1';

            string probeStream = new string(tmpProbeStream);

            int intLenProbeStream = probeStream.Length;
            byte[] bytProbe = new byte[intLenProbeStream / 8];

            probeStream = strRev(probeStream);

            for (int i = 0; i < (intLenProbeStream / 8); i++)
            {
                string strProbeCmdTmp = probeStream.Substring(i * 8, 8);
                strProbeCmdTmp = strRev(strProbeCmdTmp);
                UInt32 intCmdTmp = Convert.ToUInt32(strProbeCmdTmp, 2);
                bytProbe[i] = Convert.ToByte(intCmdTmp);
            }

            // Select probes parameters to FPGA
            Firmware.sendWord(1, "11010100", usbDevId);
            // Send probes parameters to FPGA
            int intLenBytProbes = bytProbe.Length;
            Firmware.sendWord(10, bytProbe, intLenBytProbes, usbDevId);

            // Start shift parameters to ASIC
            Firmware.sendWord(1, "11010110", usbDevId);
            // Stop shift parameters to ASIC
            Firmware.sendWord(1, "11010100", usbDevId);

            // probes test checksum test query
            Firmware.sendWord(0, "10110100", usbDevId);

            // Load probes
            Firmware.sendWord(1, "11010101", usbDevId);
            Firmware.sendWord(1, "11010100", usbDevId);

            // Send probes parameters to FPGA
            Firmware.sendWord(10, bytProbe, intLenBytProbes, usbDevId);

            // Start shift parameters to ASIC
            Firmware.sendWord(1, "11010110", usbDevId);
            // Stop shift parameters to ASIC
            Firmware.sendWord(1, "11010100", usbDevId);

            // Probes Correlation Test Result
            if (Firmware.readWord(4, usbDevId) == "00000000") result = true;

            // Reset probes test checksum test query
            Firmware.sendWord(0, "10110100", usbDevId);

            return result;
        }

        private bool sendReadRegister()
        {
            bool result = false;
            char[] tmpRrStream = new char[40];

            for (int i = 0; i < 40; i++) tmpRrStream[i] = '0';

            tmpRrStream[0] = '1';

            string rrStream = new string(tmpRrStream);

            byte[] bytRr = new byte[5];

            rrStream = strRev(rrStream);

            for (int i = 0; i < 5; i++)
            {
                string strRrCmdTmp = rrStream.Substring(i * 8, 8);
                strRrCmdTmp = strRev(strRrCmdTmp);
                UInt32 intCmdTmp = Convert.ToUInt32(strRrCmdTmp, 2);
                bytRr[i] = Convert.ToByte(intCmdTmp);
            }

            // Select probes parameters to FPGA
            Firmware.sendWord(1, "11010100", usbDevId);
            // Send probes parameters to FPGA
            Firmware.sendWord(12, bytRr, 8, usbDevId);

            // Start shift parameters to ASIC
            Firmware.sendWord(1, "11010110", usbDevId);
            // Stop shift parameters to ASIC
            Firmware.sendWord(1, "11010100", usbDevId);

            // probes test checksum test query
            Firmware.sendWord(0, "10110100", usbDevId);

            // Load probes
            Firmware.sendWord(1, "11010101", usbDevId);
            Firmware.sendWord(1, "11010100", usbDevId);

            // Send probes parameters to FPGA
            Firmware.sendWord(12, bytRr, 8, usbDevId);

            // Start shift parameters to ASIC
            Firmware.sendWord(1, "11010110", usbDevId);
            // Stop shift parameters to ASIC
            Firmware.sendWord(1, "11010100", usbDevId);

            // Probes Correlation Test Result
            if (Firmware.readWord(4, usbDevId) == "00000000") result = true;

            // Reset probes test checksum test query
            Firmware.sendWord(0, "10110100", usbDevId);
            
            return result;
        }

        private void linearityThresholdDac(Chart _chart, double[] X, double[] Y, double maxFit, string Name)
        {
            // Clear and initialyze charts
            _chart.Series.Clear();

            _chart.Series.Add("linearity" + Name);
            _chart.Series["linearity" + Name].ChartType = Charting.SeriesChartType.Line;
            _chart.Series["linearity" + Name].Color = Color.Blue;
            _chart.Series["linearity" + Name].ChartArea = "ChartArea_linearity";
            _chart.ChartAreas["ChartArea_linearity"].AxisY.Title = "threshold (V)";
            _chart.Series.Add("Fit" + Name);
            _chart.Series["Fit" + Name].ChartType = Charting.SeriesChartType.Line;
            _chart.Series["Fit" + Name].Color = Color.Black;
            _chart.Series.Add("residual" + Name);
            _chart.Series["residual" + Name].ChartArea = "ChartArea_residual";
            _chart.ChartAreas["ChartArea_residual"].AxisY.Title = "residual (mV)";
            _chart.Series["residual" + Name].ChartType = Charting.SeriesChartType.Line;
            _chart.Series["residual" + Name].Color = Color.Blue;
            //_chart.Series["residual"].MarkerStyle = Charting.MarkerStyle.Circle;
            _chart.Series.Add("INL" + Name);
            _chart.Series["INL" + Name].ChartArea = "ChartArea_INL";
            _chart.ChartAreas["ChartArea_INL"].AxisY.Title = "INL (%)";
            _chart.Series["INL" + Name].ChartType = Charting.SeriesChartType.Line;
            _chart.Series["INL" + Name].Color = Color.Blue;
            //_chart.Series["INL"].MarkerStyle = Charting.MarkerStyle.Circle;
            _chart.Series.Add("DNL" + Name);
            _chart.Series["DNL" + Name].ChartArea = "ChartArea_DNL";
            _chart.ChartAreas["ChartArea_DNL"].AxisY.Title = "DNL (LSB)";
            _chart.Series["DNL" + Name].ChartType = Charting.SeriesChartType.Line;
            _chart.Series["DNL" + Name].Color = Color.Blue;
            //_chart.Series["DNL"].MarkerStyle = Charting.MarkerStyle.Circle;
            _chart.Series.Add("posLimit");
            _chart.Series.Add("negLimit");

            int nbPts = Y.Length;

            Double[] Ylin_fit_temp = new Double[nbPts];
            Double[] Xlin_fit_temp = new Double[nbPts];

            int nbPtsFit = nbPts;

            // Trace
            for (int i = 0; i < nbPts; i++)
            {
                _chart.Series["linearity" + Name].Points.AddXY(X[i], Y[i]);
                _chart.Series["linearity" + Name].Points[i].ToolTip = string.Format("x = {0}, y = {1}", X[i], Y[i]);
                if (maxFit > X[i])
                {
                    Xlin_fit_temp[i] = X[i];
                    Ylin_fit_temp[i] = Y[i];
                }
                else
                {
                    if (nbPtsFit > i) nbPtsFit = i;
                }
            }

            //Fit
            Tuple<double, double> fit_tuple = new Tuple<double, double>(1, 0.1);
            Double[] Ylin_fit = new Double[nbPtsFit];
            Double[] Xlin_fit = new Double[nbPtsFit];

            for (int i = 0; i < nbPtsFit; i++)
            {
                Xlin_fit[i] = Xlin_fit_temp[i];
                Ylin_fit[i] = Ylin_fit_temp[i];
            }

            fit_tuple = MathNet.Numerics.Fit.Line(Xlin_fit, Ylin_fit);

            double intercept = fit_tuple.Item1;
            double slope = fit_tuple.Item2;
            
            if (slope < LDacSlope || slope > HDacSlope || intercept < LDacIntercept || intercept > HDacIntercept) TestThDac_OK = false;

            label_results.Text += _chart.Text + " intercept = " + Math.Round(intercept * 1000, 1).ToString() + " mV   –   Slope = " + Math.Round(slope * 1000, 3).ToString() + " mV";

            traceLinearity(intercept, slope, nbPts, _chart, X, Y, Name);
        }

        private void linearityInputDac(double[,] dataArray, double maxFit)
        {
            // Clear and initialyze charts
            chart_inputDac.Series.Clear();
            tableLayoutPanel_inputDac.Controls.Clear();

            tableLayoutPanel_inputDac.Controls.Add(new Label { Text = "chn", Anchor = AnchorStyles.Left, AutoSize = false }, 0, 0);
            tableLayoutPanel_inputDac.Controls.Add(new Label { Text = "slope (mV)", Anchor = AnchorStyles.Left, AutoSize = false }, 1, 0);
            tableLayoutPanel_inputDac.Controls.Add(new Label { Text = "intercept (V)", Anchor = AnchorStyles.Left, AutoSize = false }, 2, 0);

            for (int chn = 0; chn < NbChannels; chn++)
            {
                string linName = "linearity" + chn.ToString();
                string resName = "residual" + chn.ToString();
                string INLName = "INL" + chn.ToString();
                string DNLName = "DNL" + chn.ToString();

                chart_inputDac.Series.Add(linName);
                chart_inputDac.Series[linName].ChartType = Charting.SeriesChartType.Line;
                chart_inputDac.Series[linName].ChartArea = "ChartArea_linearity";
                chart_inputDac.ChartAreas["ChartArea_linearity"].AxisY.Title = "threshold (V)";
                chart_inputDac.Series.Add(resName);
                chart_inputDac.Series[resName].ChartArea = "ChartArea_residual";
                chart_inputDac.Series[resName].ChartType = Charting.SeriesChartType.Line;
                chart_inputDac.ChartAreas["ChartArea_residual"].AxisY.Title = "residual (mV)";
                chart_inputDac.Series.Add(INLName);
                chart_inputDac.Series[INLName].ChartArea = "ChartArea_INL";
                chart_inputDac.Series[INLName].ChartType = Charting.SeriesChartType.Line;
                chart_inputDac.ChartAreas["ChartArea_INL"].AxisY.Title = "INL (%)";
                chart_inputDac.Series.Add(DNLName);
                chart_inputDac.Series[DNLName].ChartArea = "ChartArea_DNL";
                chart_inputDac.Series[DNLName].ChartType = Charting.SeriesChartType.Line;
                chart_inputDac.ChartAreas["ChartArea_DNL"].AxisY.Title = "DNL (LSB)";
            }

            chart_inputDac.Series.Add("posLimit");
            chart_inputDac.Series.Add("negLimit");

            int nbPts = dataArray.GetLength(0);

            Double[] Ylinearity = new Double[nbPts];
            Double[] Xlinearity = new Double[nbPts];

            Double[] Ylin_fit_temp = new Double[nbPts];
            Double[] Xlin_fit_temp = new Double[nbPts];

            int nbPtsFit = nbPts;

            // Trace

            double avgIntercept = 0;
            double avgSlope = 0;
            double squareIntercept = 0;
            double squareSlope = 0;

            for (int chn = 0; chn < NbChannels; chn++)
            {
                for (int i = 0; i < nbPts; i++)
                {
                    Xlinearity[i] = dataArray[i, 0];
                    Ylinearity[i] = dataArray[i, chn + 1];
                    chart_inputDac.Series["linearity" + chn.ToString()].Points.AddXY(Xlinearity[i], Ylinearity[i]);
                    if (maxFit > Xlinearity[i])
                    {
                        Xlin_fit_temp[i] = Xlinearity[i];
                        Ylin_fit_temp[i] = Ylinearity[i];
                    }
                    else
                    {
                        if (nbPtsFit > i) nbPtsFit = i;
                    }
                }

                //Fit

                Tuple<double, double> fit_tuple = new Tuple<double, double>(90, 1);
                Double[] Ylin_fit = new Double[nbPtsFit];
                Double[] Xlin_fit = new Double[nbPtsFit];

                for (int i = 0; i < nbPtsFit; i++)
                {
                    Xlin_fit[i] = Xlin_fit_temp[i];
                    Ylin_fit[i] = Ylin_fit_temp[i];
                }

                fit_tuple = MathNet.Numerics.Fit.Line(Xlin_fit, Ylin_fit);
                double intercept = fit_tuple.Item1;
                double slope = fit_tuple.Item2;

                avgIntercept += intercept;
                avgSlope += slope;
                squareIntercept += intercept * intercept;
                squareSlope += slope * slope;

                if (slope < LinDacSlope || slope > HinDacSlope || intercept < LinDacIntercept) TestInDac_OK = false;

                tableLayoutPanel_inputDac.Controls.Add(new Label { Text = chn.ToString(), Anchor = AnchorStyles.Left, AutoSize = false }, 0, chn + 1);
                tableLayoutPanel_inputDac.Controls.Add(new Label { Text = Math.Round(slope * 1000, 3).ToString(), Anchor = AnchorStyles.Left, AutoSize = false }, 1, chn + 1);
                tableLayoutPanel_inputDac.Controls.Add(new Label { Text = Math.Round(intercept, 3).ToString(), Anchor = AnchorStyles.Left, AutoSize = false }, 2, chn + 1);
                
                traceLinearity(intercept, slope, nbPts, chart_inputDac, Xlinearity, Ylinearity, chn.ToString());
            }

            avgIntercept /= NbChannels;
            avgSlope /= NbChannels;
            squareIntercept /= NbChannels;
            squareSlope /= NbChannels;
            double sigmaIntercept = Math.Sqrt(squareIntercept - avgIntercept * avgIntercept);
            double sigmaSlope = Math.Sqrt(squareSlope - avgSlope * avgSlope);

            label_results.Text += "Input DAC intercept = " + Math.Round(avgIntercept, 3) + " ± " + Math.Round(sigmaIntercept, 3) + " V   –   Slope = " + Math.Round(avgSlope * 1000, 3) + " ± " + Math.Round(sigmaSlope * 1000, 3) + " mV";
        }

        double INLLimit = 1;
        double DNLAxisY = 1;
        double residualAxisY = 1;
        Charting.StripLine percentLinearity = new Charting.StripLine();
        private void traceLinearity(double intercept, double slope, int nbPts, Chart _chart, double[] Xlinearity, double[] Ylinearity, string Name)
        {
            INLLimit = 1;
            DNLAxisY = 1;
            residualAxisY = 1;
            double x, y;

            for (int i = 0; i < nbPts; i++)
            {
                x = Xlinearity[i];
                y = (Ylinearity[i] - (intercept + slope * Xlinearity[i])) * 1000;
                if (y > residualAxisY) residualAxisY = Math.Ceiling(y);
                _chart.Series["residual" + Name].Points.AddXY(x, y);
                _chart.Series["residual" + Name].Points[i].ToolTip = string.Format("x = {0}, y = {1}", x, y);
            }

            for (int i = 0; i < nbPts; i++)
            {
                x = Xlinearity[i];
                y = (Ylinearity[i] - (intercept + slope * Xlinearity[i])) / (Ylinearity[nbPts - 1] - Ylinearity[0]) * 100;
                _chart.Series["INL" + Name].Points.AddXY(x, y);
                _chart.Series["INL" + Name].Points[i].ToolTip = string.Format("x = {0}, y = {1}", x, y);
            }

            for (int i = 0; i < nbPts - 1; i++)
            {
                x = Xlinearity[i];
                y = (Xlinearity[i + 1] - Xlinearity[i]) * ((Ylinearity[i + 1] - Ylinearity[i]) / (slope * (Xlinearity[i + 1] - Xlinearity[i])) - 1);
                if (y > DNLAxisY) DNLAxisY = Math.Ceiling(y);
                _chart.Series["DNL" + Name].Points.AddXY(x, y);
                _chart.Series["DNL" + Name].Points[i].ToolTip = string.Format("x = {0}, y = {1}", x, y);
            }
            float titleHeight = _chart.Titles[0].Position.Height;
            titleHeight *= 1.5F;
            _chart.ChartAreas["ChartArea_linearity"].Position = new Charting.ElementPosition(2, titleHeight, 48, 50 - titleHeight);
            _chart.ChartAreas["ChartArea_residual"].Position = new Charting.ElementPosition(2, 50, 48, 50);
            _chart.ChartAreas["ChartArea_INL"].Position = new Charting.ElementPosition(50, 50, 48, 50);
            _chart.ChartAreas["ChartArea_DNL"].Position = new Charting.ElementPosition(50, titleHeight, 48, 50 - titleHeight);

            _chart.ChartAreas["ChartArea_linearity"].AxisX.Minimum = 0;
            _chart.ChartAreas["ChartArea_linearity"].AxisX.Maximum = 1024;
            _chart.ChartAreas["ChartArea_linearity"].AxisX.Interval = 256;
            _chart.ChartAreas["ChartArea_residual"].AxisX.Minimum = 0;
            _chart.ChartAreas["ChartArea_residual"].AxisX.Maximum = 1024;
            _chart.ChartAreas["ChartArea_residual"].AxisX.Interval = 256;
            _chart.ChartAreas["ChartArea_INL"].AxisX.Minimum = 0;
            _chart.ChartAreas["ChartArea_INL"].AxisX.Maximum = 1024;
            _chart.ChartAreas["ChartArea_INL"].AxisX.Interval = 256;
            _chart.ChartAreas["ChartArea_DNL"].AxisX.Minimum = 0;
            _chart.ChartAreas["ChartArea_DNL"].AxisX.Maximum = 1024;
            _chart.ChartAreas["ChartArea_DNL"].AxisX.Interval = 256;

            _chart.ChartAreas["ChartArea_linearity"].AxisX.LabelStyle.Enabled = false;
            _chart.ChartAreas["ChartArea_linearity"].AlignWithChartArea = "ChartArea_residual";
            _chart.ChartAreas["ChartArea_linearity"].AlignmentOrientation = Charting.AreaAlignmentOrientations.Vertical;
            _chart.ChartAreas["ChartArea_linearity"].AlignmentStyle = Charting.AreaAlignmentStyles.All;
            _chart.ChartAreas["ChartArea_DNL"].AxisX.LabelStyle.Enabled = false;
            _chart.ChartAreas["ChartArea_INL"].AlignWithChartArea = "ChartArea_DNL";
            _chart.ChartAreas["ChartArea_INL"].AlignmentOrientation = Charting.AreaAlignmentOrientations.Vertical;
            _chart.ChartAreas["ChartArea_INL"].AlignmentStyle = Charting.AreaAlignmentStyles.All;
            _chart.ChartAreas["ChartArea_INL"].AxisX.Title = "DAC code";
            _chart.ChartAreas["ChartArea_residual"].AxisX.Title = "DAC code";

            if (_chart == chart_inputDac)
            {
                _chart.ChartAreas["ChartArea_residual"].AxisY.Maximum = 50;
                _chart.ChartAreas["ChartArea_residual"].AxisY.Minimum = -50;
                _chart.ChartAreas["ChartArea_residual"].AxisY.Interval = 25;
                _chart.ChartAreas["ChartArea_DNL"].AxisY.Maximum = 5;
                _chart.ChartAreas["ChartArea_DNL"].AxisY.Minimum = -5;
                _chart.ChartAreas["ChartArea_DNL"].AxisY.Interval = 2.5;
                _chart.ChartAreas["ChartArea_linearity"].AxisX.Maximum /= 4;
                _chart.ChartAreas["ChartArea_linearity"].AxisX.Interval /= 4;
                _chart.ChartAreas["ChartArea_residual"].AxisX.Maximum /= 4;
                _chart.ChartAreas["ChartArea_residual"].AxisX.Interval /= 4;
                _chart.ChartAreas["ChartArea_INL"].AxisX.Maximum /= 4;
                _chart.ChartAreas["ChartArea_INL"].AxisX.Interval /= 4;
                _chart.ChartAreas["ChartArea_DNL"].AxisX.Maximum /= 4;
                _chart.ChartAreas["ChartArea_DNL"].AxisX.Interval /= 4;
                INLLimit = 2;
            }
            else
            {
                _chart.ChartAreas["ChartArea_residual"].AxisY.Maximum = residualAxisY;
                _chart.ChartAreas["ChartArea_residual"].AxisY.Minimum = -residualAxisY;
                _chart.ChartAreas["ChartArea_residual"].AxisY.Interval = residualAxisY / 2;
                _chart.ChartAreas["ChartArea_INL"].AxisY.Maximum = INLLimit;
                _chart.ChartAreas["ChartArea_INL"].AxisY.Minimum = -INLLimit;
                _chart.ChartAreas["ChartArea_INL"].AxisY.Interval = INLLimit / 2;
                _chart.ChartAreas["ChartArea_DNL"].AxisY.Maximum = DNLAxisY;
                _chart.ChartAreas["ChartArea_DNL"].AxisY.Minimum = -DNLAxisY;
                _chart.ChartAreas["ChartArea_DNL"].AxisY.Interval = DNLAxisY / 2;
            }

            _chart.Series["posLimit"].ChartArea = "ChartArea_INL";
            _chart.Series["negLimit"].ChartArea = "ChartArea_INL";
            _chart.Series["posLimit"].ChartType = Charting.SeriesChartType.Line;
            _chart.Series["posLimit"].Color = Color.IndianRed;
            _chart.Series["posLimit"].BorderWidth = 2;
            _chart.Series["negLimit"].ChartType = Charting.SeriesChartType.Line;
            _chart.Series["negLimit"].Color = Color.IndianRed;
            _chart.Series["posLimit"].BorderWidth = 2;
            _chart.Series["posLimit"].Points.AddXY(0, INLLimit / 2);
            _chart.Series["posLimit"].Points.AddXY(_chart.ChartAreas["ChartArea_INL"].AxisX.Maximum - 1, INLLimit / 2);
            _chart.Series["negLimit"].Points.AddXY(0, -INLLimit / 2);
            _chart.Series["negLimit"].Points.AddXY(_chart.ChartAreas["ChartArea_INL"].AxisX.Maximum - 1, -INLLimit / 2);

            string inlType = " %";
            _chart.Series["posLimit"].Points[0].SetValueXY(0, INLLimit / 2);
            _chart.Series["posLimit"].Points[1].SetValueXY(_chart.ChartAreas["ChartArea_INL"].AxisX.Maximum - 1, INLLimit / 2);
            _chart.Series["negLimit"].Points[0].SetValueXY(0, -INLLimit / 2);
            _chart.Series["negLimit"].Points[1].SetValueXY(_chart.ChartAreas["ChartArea_INL"].AxisX.Maximum - 1, -INLLimit / 2);
            _chart.Series["posLimit"].Font = new Font(_chart.ChartAreas[0].AxisX.TitleFont.FontFamily, 8F);
            _chart.Series["posLimit"].Points[1].LabelBackColor = Color.White;
            _chart.Series["posLimit"].Points[1].LabelBorderColor = Color.IndianRed;
            _chart.Series["posLimit"].Points[1].Label = "+ " + (INLLimit / 2).ToString() + inlType;
            _chart.Series["posLimit"].Font = new Font(_chart.ChartAreas[0].AxisX.TitleFont.FontFamily, 8F);
            _chart.Series["negLimit"].Points[1].LabelBackColor = Color.White;
            _chart.Series["negLimit"].Points[1].LabelBorderColor = Color.IndianRed;
            _chart.Series["negLimit"].Points[1].Label = "- " + (INLLimit / 2).ToString() + inlType;
            _chart.ChartAreas["ChartArea_INL"].AxisY.Maximum = INLLimit;
            _chart.ChartAreas["ChartArea_INL"].AxisY.Minimum = -INLLimit;
            _chart.ChartAreas["ChartArea_INL"].AxisY.Interval = INLLimit / 2;
        }

        private void Scurves(bool timeQb, int VthMin, int VthMax, int VthStep, Chart _chart, out double[,] ScurvesDataArrayPed, out double[,] ScurvesDataArraySig)
        {
            int nbPts = (VthMax - VthMin) / Math.Abs(VthStep) + 1;
            int sleepTicks = 20000;

            // Create two dimensional double array to store S-curves data
            ScurvesDataArrayPed = new double[nbPts, NbChannels + 1];
            ScurvesDataArraySig = new double[nbPts, NbChannels + 1];

            // parameters to pass to the ProgressChanged event of the background worker.
            double trigEff = 100;
            int DacCode = VthMin;

            int[] ScurveData = new int[NbChannels];
            int[] pulseData = new int[NbChannels];

            string strSC = strDefSC;
            string strTempSC;
            char[] chrTemp1;
            string strTemp2;

            if (timeQb)
            {
                chrTemp1 = strSC.Substring(0, 1117).ToCharArray();
                strTemp2 = strSC.Substring(1127, 17);
            }
            else
            {
                chrTemp1 = strSC.Substring(0, 1107).ToCharArray();
                strTemp2 = strSC.Substring(1117, 27);
            }

            #region on pedestal

            for (int i = 0; i < nbPts; i++)
            {
                if (VthStep > 0) DacCode = VthMin + i * VthStep;
                else DacCode = VthMax + i * VthStep;
                ScurvesDataArrayPed[i, 0] = DacCode;

                // mask all the channels
                for (int j = 0; j < NbChannels; j++) chrTemp1[j + 265] = '0';

                for (int chn = 0; chn < NbChannels; chn++)
                {
                    if (!timeQb)
                    {
                        // unmask the measured channel
                        chrTemp1[chn + 265] = '1';
                        // mask the previously measured channel
                        if (chn > 0) chrTemp1[chn + 264] = '0';
                    }
                    // disable all the preamps
                    for (int j = 0; j < NbChannels; j++) chrTemp1[633 + j * 15] = '1';
                    // enable measured channel preamp
                    //chrTemp1[633 + chn * 15] = '0';
                    
                    string strTemp1 = new string(chrTemp1);

                    // reconstruct the slow control with the unmasked measured channel
                    strTempSC = strTemp1 + IntToBin(DacCode, 10) + strTemp2;
                    sendSC(usbDevId, strTempSC);

                    // start data acquisition for channel chn
                    if (timeQb) Firmware.sendWord(31, IntToBin(chn, 8), usbDevId);
                    else Firmware.sendWord(31, IntToBin(34, 8), usbDevId);

                    pulseData[chn] = 0;
                    int tries = 0;
                    while (pulseData[chn] != 200 || chn == 0 && i == 0 && ScurveData[0] <= 50 && tries < 10) // redo the measurement until it's correct.
                    {
                        if (chn == 0 && i == 0)
                        {
                            sendSC(usbDevId, strTempSC);
                            if (timeQb) Firmware.sendWord(31, IntToBin(chn, 8), usbDevId);
                            else Firmware.sendWord(31, IntToBin(34, 8), usbDevId);
                            tries++;
                        }

                        // Reset and disable S-curves
                        Firmware.sendWord(3, "10011001", usbDevId);
                        // Stop reseting S-curves
                        Firmware.sendWord(3, "11011001", usbDevId);
                        // Enable S-curves
                        Firmware.sendWord(3, "11111001", usbDevId);
                        // wait enough time for the acquisition to perform
                        wait(sleepTicks);
                        // Disable S-curves
                        Firmware.sendWord(3, "11011001", usbDevId);

                        // read data in the FPGA
                        byte[] tmpFIFO8 = Firmware.readWord(8, NbChannels, usbDevId);
                        byte[] tmpFIFO18 = Firmware.readWord(18, NbChannels, usbDevId);
                        byte[] tmpFIFO9 = Firmware.readWord(9, NbChannels, usbDevId);
                        byte[] tmpFIFO19 = Firmware.readWord(19, NbChannels, usbDevId);
                        // pulseData is the measured number of acquisition windows
                        pulseData[chn] = Convert.ToInt32(tmpFIFO18[chn]) * 256 + Convert.ToInt32(tmpFIFO8[chn]);
                        // ScurveData is the number of trigger measured at DAC code i
                        ScurveData[chn] = Convert.ToInt32(tmpFIFO19[chn]) * 256 + Convert.ToInt32(tmpFIFO9[chn]);
                    }

                    // Store trigger efficiency of current channel in ScurvesDataArray
                    trigEff = ScurveData[chn] * 100.0 / pulseData[chn];
                    ScurvesDataArrayPed[i, chn + 1] = trigEff;
                }
            }

            #endregion
            
            if (timeQb)
            {
                chrTemp1 = strSC.Substring(0, 1117).ToCharArray();
                strTemp2 = strSC.Substring(1127, 17);
            }
            else
            {
                chrTemp1 = strSC.Substring(0, 1107).ToCharArray();
                strTemp2 = strSC.Substring(1117, 27);
            }

            #region on signal

            for (int chn = 0; chn < NbChannels; chn++)
            {
                // mask all the channels
                for (int i = 0; i < NbChannels; i++) chrTemp1[i + 265] = '0';

                if (!timeQb)
                {
                    // unmask the measured channel
                    chrTemp1[chn + 265] = '1';
                    // mask the previously measured channel
                    if (chn > 0) chrTemp1[chn + 264] = '0';
                }
                // disable all the preamps
                for (int j = 0; j < NbChannels; j++) chrTemp1[633 + j * 15] = '1';
                // enable measured channel preamp
                chrTemp1[633 + chn * 15] = '0';
                // Set the Ctest injection on the measured channel
                bool fshOnLg = false;
                if (fshOnLg)
                {
                    chrTemp1[chn * 15 + 632] = '1';
                    if (chn > 0) chrTemp1[(chn - 1) * 15 + 632] = '0';
                }
                else
                {
                    chrTemp1[chn * 15 + 631] = '1';
                    if (chn > 0) chrTemp1[(chn - 1) * 15 + 631] = '0';
                }

                string strTemp1 = new string(chrTemp1);
                for (int i = 0; i < nbPts; i++)
                {
                    if (VthStep > 0) DacCode = VthMin + 30 + i * VthStep;
                    else DacCode = VthMax + 30 + i * VthStep;
                    ScurvesDataArraySig[i, 0] = DacCode;

                    // reconstruct the slow control with the unmasked measured channel
                    strTempSC = strTemp1 + IntToBin(DacCode, 10) + strTemp2;
                    sendSC(usbDevId, strTempSC);

                    // start data acquisition for channel chn
                    if (timeQb) Firmware.sendWord(31, IntToBin(chn, 8), usbDevId);
                    else Firmware.sendWord(31, IntToBin(34, 8), usbDevId);

                    pulseData[chn] = 0;
                    int tries = 0;
                    while (pulseData[chn] != 200 || chn == 0 && i == 0 && ScurveData[0] <= 50 && tries < 10) // redo the measurement until it's correct.
                    {
                        if (chn == 0 && i == 0)
                        {
                            sendSC(usbDevId, strTempSC);
                            if (timeQb) Firmware.sendWord(31, IntToBin(chn, 8), usbDevId);
                            else Firmware.sendWord(31, IntToBin(33, 8), usbDevId);
                            tries++;
                        }

                        // Reset and disable S-curves
                        Firmware.sendWord(3, "10011001", usbDevId);
                        // Stop reseting S-curves
                        Firmware.sendWord(3, "11011001", usbDevId);
                        // Enable S-curves
                        Firmware.sendWord(3, "11111001", usbDevId);
                        // wait enough time for the acquisition to perform
                        wait(sleepTicks);
                        // Disable S-curves
                        Firmware.sendWord(3, "11011001", usbDevId);

                        // read data in the FPGA
                        byte[] tmpFIFO8 = Firmware.readWord(8, NbChannels, usbDevId);
                        byte[] tmpFIFO18 = Firmware.readWord(18, NbChannels, usbDevId);
                        byte[] tmpFIFO9 = Firmware.readWord(9, NbChannels, usbDevId);
                        byte[] tmpFIFO19 = Firmware.readWord(19, NbChannels, usbDevId);
                        // pulseData is the measured number of acquisition windows
                        pulseData[chn] = Convert.ToInt32(tmpFIFO18[chn]) * 256 + Convert.ToInt32(tmpFIFO8[chn]);
                        // ScurveData is the number of trigger measured at DAC code i
                        ScurveData[chn] = Convert.ToInt32(tmpFIFO19[chn]) * 256 + Convert.ToInt32(tmpFIFO9[chn]);
                    }

                    // Store trigger efficiency of current channel in ScurvesDataArray
                    trigEff = ScurveData[chn] * 100.0 / pulseData[chn];
                    ScurvesDataArraySig[i, chn + 1] = trigEff;
                }
            }

            #endregion

            // Send the slow control specified by the user to the ASIC (bits were forced during acquisition)
            sendSC(usbDevId, strSC);
        }

        private void fitScurves(double[,] ScurvesArray, Chart _chart, string seriesName)
        {
            int nbPts = ScurvesArray.GetLength(0);
            double sigmaGuess = 1;
            double sigmaMin = 0;
            double sigmaMax = 10;
            double fitMax = ScurvesArray[0, 0];
            double fitMin = ScurvesArray[nbPts - 1, 0];
            double[] sigma = new double[NbChannels];
            double[] position = new double[NbChannels];

            for (int chn = 0; chn < NbChannels; chn++)
            {
                double[] Xdata = new double[nbPts];
                double[] Ydata = new double[nbPts];

                for (int i = 0; i < nbPts; i++)
                {
                    Xdata[i] = ScurvesArray[i, 0];
                    Ydata[i] = ScurvesArray[i, chn + 1];
                }

                double[] p = { sigmaGuess, (fitMax + fitMin) / 2 }; // Initial conditions
                int[] sigmaLimited = { 1, 1 };
                double[] sigmaLimits = { sigmaMin, sigmaMax };
                int[] meanLimited = { 1, 1 };
                double[] meanLimits = { fitMin, fitMax };

                for (int i = 0; i < Xdata.Length; i++)
                    if (Xdata[i] < fitMin || Xdata[i] > fitMax) Ydata[i] = 0;

                mp_par[] pars = new mp_par[2] { new mp_par() { limited = sigmaLimited, limits = sigmaLimits }, new mp_par() { limited = meanLimited, limits = meanLimits } }; // Parameter constraints
                int status;

                mp_result result = new mp_result(2);

                CustomUserVariable v = new CustomUserVariable() { X = Xdata, Y = Ydata };

                // Call fitting function
                status = MPFit.Solve(fitFunction, nbPts, 2, p, pars, null, v, ref result);

                sigma[chn] = p[0];
                position[chn] = p[1];

                if (position[chn] == fitMin || position[chn] == fitMax) TestScurves_OK = false;
            }

            _chart.Series.Add(seriesName);
            _chart.Series[seriesName].ChartArea = "ChartArea_position";
            _chart.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
            _chart.ChartAreas["ChartArea_position"].AxisX.Interval = 4;
            _chart.ChartAreas["ChartArea_position"].AxisX.Maximum = NbChannels;

            double squarePosition = 0;

            for (int chn = 0; chn < NbChannels; chn++)
            {
                _chart.Series[seriesName].Points.AddXY(chn, position[chn]);
                squarePosition += position[chn] * position[chn];
            }

            double sigmaPosition = Math.Sqrt((squarePosition / NbChannels) - ((position.Sum() / NbChannels) * (position.Sum() / NbChannels)));
            label_results.Text += _chart.Titles[0].Text + " " + seriesName + " position = " + Math.Round(position.Sum() / NbChannels, 3) + " ± " + Math.Round(sigmaPosition, 3) + " DACu ";

        }

        double sqrt2 = Math.Sqrt(2);
        private int fitFunction(double[] p, double[] dy, IList<double>[] dvec, object vars)
        {
            double[] x, y;

            CustomUserVariable v = (CustomUserVariable)vars;

            x = v.X;
            y = v.Y;

            for (int i = 0; i < dy.Length; i++)
            {
                dy[i] = y[i] - 0.5 * (1 - MathNet.Numerics.SpecialFunctions.Erf((x[i] - p[1]) / (p[0] * sqrt2))) * 100;
            }

            return 0;
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            OpenFileDialog LoadDialog = new OpenFileDialog();
            LoadDialog.Title = "Specify Data file";
            LoadDialog.RestoreDirectory = true;

            string loadFileName = "";

            if (LoadDialog.ShowDialog() == DialogResult.OK)
            {
                if (LoadDialog.FileName == null) return;
                else loadFileName = LoadDialog.FileName;
            }
            else return;

            loadSerialData(loadFileName, false);
        }

        private void loadSerialData(string loadFileName, bool writeFile)
        {
            strSaveName = loadFileName;

            chart_inputDac.Series.Clear();
            chart_dacCharge.Series.Clear();
            chart_dacTime.Series.Clear();
            chart_ScurvesSerialCharge.Series.Clear();
            chart_ScurvesSerialTime.Series.Clear();
            chart_chargeHGserial.Series.Clear();
            chart_chargeLGserial.Series.Clear();
            label_results.Text = "";

            ArrayList DataArrayList;

            if (loadFileName == null) return;
            else DataArrayList = ReadFileLine(loadFileName);

            int DataArrayListCount = DataArrayList.Count;

            if (DataArrayListCount <= 1) return;

            string[] DataArray = new string[DataArrayListCount];
            DataArrayList.CopyTo(DataArray);

            int line = 1;

            if (!writeFile) line += 8;

            string[] DataSplit = DataArray[line].Split(' ');
            double current = Convert.ToDouble(DataSplit[0]);
            line += 2;
            DataSplit = DataArray[line].Split(' ');
            double vbg = Convert.ToDouble(DataSplit[0]);
            line += 2;
            DataSplit = DataArray[line].Split(' ');
            double DcHg = Convert.ToDouble(DataSplit[0]);
            line += 2;
            DataSplit = DataArray[line].Split(' ');
            double DcLg = Convert.ToDouble(DataSplit[0]);
            line += 2;
            DataSplit = DataArray[line].Split(' ');
            double Temp = Convert.ToDouble(DataSplit[0]);
            line++;

            label_DcInfo.Text = "     Current consumption = " + current + " mA   –   vbg = " + vbg + " V   –   DC HG = " + DcHg + " V   –   DC LG = " + DcLg + " V   –   Temp = " + Temp + " °C";

            #region input DAC

            DataSplit = DataArray[line].Split(' ');
            int nbPtsInputDac = Convert.ToInt32(DataSplit[0]);
            line++;

            string seriesName = "";

            for (int chn = 0; chn < NbChannels; chn++)
            {
                seriesName = "linearity" + chn.ToString();
                chart_inputDac.Series.Add(seriesName);
                chart_inputDac.Series[seriesName].ChartArea = "ChartArea_linearity";
                chart_inputDac.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
                seriesName = "residual" + chn.ToString();
                chart_inputDac.Series.Add(seriesName);
                chart_inputDac.Series[seriesName].ChartArea = "ChartArea_residual";
                chart_inputDac.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
                seriesName = "INL" + chn.ToString();
                chart_inputDac.Series.Add(seriesName);
                chart_inputDac.Series[seriesName].ChartArea = "ChartArea_INL";
                chart_inputDac.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
                seriesName = "DNL" + chn.ToString();
                chart_inputDac.Series.Add(seriesName);
                chart_inputDac.Series[seriesName].ChartArea = "ChartArea_DNL";
                chart_inputDac.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
            }

            double[,] inputDacDataArray = new double[nbPtsInputDac, NbChannels + 1];

            for (int i = 0; i < nbPtsInputDac; i++)
            {
                DataSplit = DataArray[line].Split(' ');
                inputDacDataArray[i, 0] = Convert.ToDouble(DataSplit[0]);
                for (int chn = 0; chn < NbChannels; chn++)
                    inputDacDataArray[i, chn + 1] = Convert.ToDouble(DataSplit[chn + 1]);
                line++;
            }

            linearityInputDac(inputDacDataArray, 200);

            label_results.Text += "\n";

            #endregion

            #region threshold DACs

            DataSplit = DataArray[line].Split(' ');
            int nbPtsDacCharge = Convert.ToInt32(DataSplit[0]);
            line++;

            double[] X = new double[nbPtsDacCharge];
            double[] dacCharge = new double[nbPtsDacCharge];

            seriesName = "linearity";
            chart_dacCharge.Series.Add(seriesName);
            chart_dacCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
            seriesName = "residual";
            chart_dacCharge.Series.Add(seriesName);
            chart_dacCharge.Series[seriesName].ChartArea = "ChartArea_residual";
            chart_dacCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
            seriesName = "INL";
            chart_dacCharge.Series.Add(seriesName);
            chart_dacCharge.Series[seriesName].ChartArea = "ChartArea_INL";
            chart_dacCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
            seriesName = "DNL";
            chart_dacCharge.Series.Add(seriesName);
            chart_dacCharge.Series[seriesName].ChartArea = "ChartArea_DNL";
            chart_dacCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;

            for (int i = 0; i < nbPtsDacCharge; i++)
            {
                DataSplit = DataArray[line].Split(' ');
                X[i] = Convert.ToDouble(DataSplit[0]);
                dacCharge[i] = Convert.ToDouble(DataSplit[1]);
                line++;
            }

            linearityThresholdDac(chart_dacCharge, X, dacCharge, 750, "DacCharge");

            label_results.Text += "   |   ";

            DataSplit = DataArray[line].Split(' ');
            int nbPtsDacTime = Convert.ToInt32(DataSplit[0]);
            line++;

            X = new double[nbPtsDacTime];
            double[] dacTime = new double[nbPtsDacTime];

            seriesName = "linearity";
            chart_dacCharge.Series.Add(seriesName);
            chart_dacCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
            seriesName = "residual";
            chart_dacCharge.Series.Add(seriesName);
            chart_dacCharge.Series[seriesName].ChartArea = "ChartArea_residual";
            chart_dacCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
            seriesName = "INL";
            chart_dacCharge.Series.Add(seriesName);
            chart_dacCharge.Series[seriesName].ChartArea = "ChartArea_INL";
            chart_dacCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
            seriesName = "DNL";
            chart_dacCharge.Series.Add(seriesName);
            chart_dacCharge.Series[seriesName].ChartArea = "ChartArea_DNL";
            chart_dacCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;

            for (int i = 0; i < nbPtsDacTime; i++)
            {
                DataSplit = DataArray[line].Split(' ');
                X[i] = Convert.ToDouble(DataSplit[0]);
                dacTime[i] = Convert.ToDouble(DataSplit[1]);
                line++;
            }

            linearityThresholdDac(chart_dacTime, X, dacTime, 750, "DacTime");

            label_results.Text += "\n";

            #endregion

            #region Scurves charge

            seriesName = "";

            for (int i = 0; i < NbChannels; i++)
            {
                seriesName = "chargeScurvePedestal_ch" + i.ToString();
                chart_ScurvesSerialCharge.Series.Add(seriesName);
                chart_ScurvesSerialCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
                chart_ScurvesSerialCharge.Series[seriesName].Color = Color.FromArgb(6 * i, 6 * i, 255);

                seriesName = "chargeScurveSignal_ch" + i.ToString();
                chart_ScurvesSerialCharge.Series.Add(seriesName);
                chart_ScurvesSerialCharge.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
                chart_ScurvesSerialCharge.Series[seriesName].Color = Color.FromArgb(255, 6 * i, 6 * i);
            }
            
            DataSplit = DataArray[line].Split(' ');
            int nbPtsScurves = Convert.ToInt32(DataSplit[0]);
            line++;

            double[,] ScurvesDataArrayPedQ = new double[nbPtsScurves, NbChannels + 1];
            double[,] ScurvesDataArraySigQ = new double[nbPtsScurves, NbChannels + 1];

            for (int i = 0; i < nbPtsScurves; i++)
            {
                DataSplit = DataArray[line].Split(' ');
                ScurvesDataArrayPedQ[i, 0] = Convert.ToDouble(DataSplit[0]);
                for (int chn = 0; chn < NbChannels; chn++)
                    ScurvesDataArrayPedQ[i, chn + 1] = Convert.ToDouble(DataSplit[chn + 1]);
                line++;
            }

            DataSplit = DataArray[line].Split(' ');
            nbPtsScurves = Convert.ToInt32(DataSplit[0]);
            line++;

            for (int i = 0; i < nbPtsScurves; i++)
            {
                DataSplit = DataArray[line].Split(' ');
                ScurvesDataArraySigQ[i, 0] = Convert.ToDouble(DataSplit[0]);
                for (int chn = 0; chn < NbChannels; chn++)
                    ScurvesDataArraySigQ[i, chn + 1] = Convert.ToDouble(DataSplit[chn + 1]);
                line++;
            }

            for (int chn = 0; chn < NbChannels; chn++)
            {
                for (int i = 0; i < nbPtsScurves; i++)
                {
                    chart_ScurvesSerialCharge.Series[chn * 2].Points.AddXY(ScurvesDataArrayPedQ[i, 0], ScurvesDataArrayPedQ[i, chn + 1]);
                    chart_ScurvesSerialCharge.Series[chn * 2 + 1].Points.AddXY(ScurvesDataArraySigQ[i, 0], ScurvesDataArraySigQ[i, chn + 1]);
                }
            }

            Charting.Axis ax = chart_ScurvesSerialCharge.ChartAreas[0].AxisX;
            Charting.Axis ay = chart_ScurvesSerialCharge.ChartAreas[0].AxisY;

            ax.Minimum = ScurvesDataArrayPedQ[ScurvesDataArraySigQ.GetLength(0) - 1, 0];
            ax.Maximum = ScurvesDataArraySigQ[0, 0];
            ax.Interval = 10;
            ax.LabelStyle.IntervalOffset = 0;
            ax.MajorGrid.IntervalOffset = 0;
            ax.MajorTickMark.IntervalOffset = 0;
            ax.MajorTickMark.LineColor = Color.White;
            ax.RoundAxisValues();
            ax.LineColor = Color.White;
            ay.Minimum = -5;
            ay.Maximum = 105;
            ay.Interval = 50;
            ay.LabelStyle.IntervalOffset = 5;
            ay.MajorGrid.IntervalOffset = 5;
            ay.MajorTickMark.IntervalOffset = 5;

            fitScurves(ScurvesDataArrayPedQ, chart_ScurvesSerialCharge, "pedestal");

            label_results.Text += "   |   ";

            fitScurves(ScurvesDataArraySigQ, chart_ScurvesSerialCharge, "signal");

            label_results.Text += "\n";

            chart_ScurvesSerialCharge.ChartAreas["ChartArea_position"].AxisY.IsStartedFromZero = false;
            chart_ScurvesSerialCharge.ChartAreas["ChartArea_position"].AxisX.Minimum = 0;
            chart_ScurvesSerialCharge.ChartAreas["ChartArea_position"].AxisX.Maximum = NbChannels;

            chart_ScurvesSerialCharge.Series["pedestal"].Color = Color.Blue;
            chart_ScurvesSerialCharge.Series["signal"].Color = Color.Red;

            #endregion

            #region Scurves time

            for (int i = 0; i < NbChannels; i++)
            {
                seriesName = "timeScurvePedestal_ch" + i.ToString();
                chart_ScurvesSerialTime.Series.Add(seriesName);
                chart_ScurvesSerialTime.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
                chart_ScurvesSerialTime.Series[seriesName].Color = Color.FromArgb(6 * i, 6 * i, 255);

                seriesName = "timeScurveSignal_ch" + i.ToString();
                chart_ScurvesSerialTime.Series.Add(seriesName);
                chart_ScurvesSerialTime.Series[seriesName].ChartType = Charting.SeriesChartType.Line;
                chart_ScurvesSerialTime.Series[seriesName].Color = Color.FromArgb(255, 6 * i, 6 * i);
            }
            
            DataSplit = DataArray[line].Split(' ');
            nbPtsScurves = Convert.ToInt32(DataSplit[0]);
            line++;

            double[,] ScurvesDataArrayPedT = new double[nbPtsScurves, NbChannels + 1];
            double[,] ScurvesDataArraySigT = new double[nbPtsScurves, NbChannels + 1];

            for (int i = 0; i < nbPtsScurves; i++)
            {
                DataSplit = DataArray[line].Split(' ');
                ScurvesDataArrayPedT[i, 0] = Convert.ToDouble(DataSplit[0]);
                for (int chn = 0; chn < NbChannels; chn++)
                    ScurvesDataArrayPedT[i, chn + 1] = Convert.ToDouble(DataSplit[chn + 1]);
                line++;
            }

            DataSplit = DataArray[line].Split(' ');
            nbPtsScurves = Convert.ToInt32(DataSplit[0]);
            line++;

            for (int i = 0; i < nbPtsScurves; i++)
            {
                DataSplit = DataArray[line].Split(' ');
                ScurvesDataArraySigT[i, 0] = Convert.ToDouble(DataSplit[0]);
                for (int chn = 0; chn < NbChannels; chn++)
                    ScurvesDataArraySigT[i, chn + 1] = Convert.ToDouble(DataSplit[chn + 1]);
                line++;
            }

            for (int chn = 0; chn < NbChannels; chn++)
            {
                for (int i = 0; i < nbPtsScurves; i++)
                {
                    chart_ScurvesSerialTime.Series[chn * 2].Points.AddXY(ScurvesDataArrayPedT[i, 0], ScurvesDataArrayPedT[i, chn + 1]);
                    chart_ScurvesSerialTime.Series[chn * 2 + 1].Points.AddXY(ScurvesDataArraySigT[i, 0], ScurvesDataArraySigT[i, chn + 1]);
                }
            }

            ax = chart_ScurvesSerialTime.ChartAreas[0].AxisX;
            ay = chart_ScurvesSerialTime.ChartAreas[0].AxisY;

            ax.Minimum = ScurvesDataArrayPedT[ScurvesDataArraySigQ.GetLength(0) - 1, 0];
            ax.Maximum = ScurvesDataArraySigT[0, 0];
            ax.Interval = 10;
            ax.LabelStyle.IntervalOffset = 0;
            ax.MajorGrid.IntervalOffset = 0;
            ax.MajorTickMark.IntervalOffset = 0;
            ax.MajorTickMark.LineColor = Color.White;
            ax.RoundAxisValues();
            ax.LineColor = Color.White;
            ay.Minimum = -5;
            ay.Maximum = 105;
            ay.Interval = 50;
            ay.LabelStyle.IntervalOffset = 5;
            ay.MajorGrid.IntervalOffset = 5;
            ay.MajorTickMark.IntervalOffset = 5;

            fitScurves(ScurvesDataArrayPedT, chart_ScurvesSerialTime, "pedestal");

            label_results.Text += "   |   ";

            fitScurves(ScurvesDataArraySigT, chart_ScurvesSerialTime, "signal");

            label_results.Text += "\n";

            chart_ScurvesSerialTime.ChartAreas["ChartArea_position"].AxisY.IsStartedFromZero = false;
            chart_ScurvesSerialTime.ChartAreas["ChartArea_position"].AxisX.Minimum = 0;
            chart_ScurvesSerialTime.ChartAreas["ChartArea_position"].AxisX.Maximum = NbChannels;


            chart_ScurvesSerialTime.Series["pedestal"].Color = Color.Blue;
            chart_ScurvesSerialTime.Series["signal"].Color = Color.Red;

            #endregion

            #region charge measurement

            chart_chargeHGserial.Series.Clear();
            chart_chargeHGserial.Series.Add("signal");
            chart_chargeHGserial.Series.Add("pedestal");
            chart_chargeHGserial.Series.Add("signal2");
            chart_chargeHGserial.Series["pedestal"].ChartArea = "ChartArea_pedestal";
            chart_chargeHGserial.Series["signal2"].ChartArea = "ChartArea_signal2";
            chart_chargeHGserial.Series["signal"].ChartType = Charting.SeriesChartType.FastLine;
            chart_chargeHGserial.Series["signal2"].ChartType = Charting.SeriesChartType.FastLine;
            chart_chargeHGserial.Series["pedestal"].ChartType = Charting.SeriesChartType.FastLine;
            chart_chargeHGserial.Series["signal"].Color = Color.Red;
            chart_chargeHGserial.Series["signal2"].Color = Color.Green;
            chart_chargeHGserial.Series["pedestal"].Color = Color.Blue;
            chart_chargeHGserial.ChartAreas[0].AxisX.Minimum = 0;
            chart_chargeHGserial.ChartAreas[0].AxisY.Title = "HG signal (ADCu)";
            chart_chargeHGserial.ChartAreas[1].AxisX.Minimum = 0;
            chart_chargeHGserial.ChartAreas[1].AxisY.Title = "HG pedestal (ADCu)";
            chart_chargeHGserial.ChartAreas[2].AxisX.Minimum = 0;
            chart_chargeHGserial.ChartAreas[2].AxisY.Title = "sig - ped (ADCu)";
            chart_chargeHGserial.ChartAreas[2].AxisX.Title = "Channel";

            chart_chargeLGserial.Series.Clear();
            chart_chargeLGserial.Series.Add("signal");
            chart_chargeLGserial.Series.Add("pedestal");
            chart_chargeLGserial.Series.Add("signal2");
            chart_chargeLGserial.Series["pedestal"].ChartArea = "ChartArea_pedestal";
            chart_chargeLGserial.Series["signal2"].ChartArea = "ChartArea_signal2";
            chart_chargeLGserial.Series["signal"].ChartType = Charting.SeriesChartType.FastLine;
            chart_chargeLGserial.Series["signal2"].ChartType = Charting.SeriesChartType.FastLine;
            chart_chargeLGserial.Series["pedestal"].ChartType = Charting.SeriesChartType.FastLine;
            chart_chargeLGserial.Series["signal"].Color = Color.Red;
            chart_chargeLGserial.Series["signal2"].Color = Color.Green;
            chart_chargeLGserial.Series["pedestal"].Color = Color.Blue;
            chart_chargeLGserial.ChartAreas[0].AxisX.Minimum = 0;
            chart_chargeLGserial.ChartAreas[0].AxisY.Title = "LG signal (ADCu)";
            chart_chargeLGserial.ChartAreas[1].AxisX.Minimum = 0;
            chart_chargeLGserial.ChartAreas[1].AxisY.Title = "LG pedestal (ADCu)";
            chart_chargeLGserial.ChartAreas[2].AxisX.Minimum = 0;
            chart_chargeLGserial.ChartAreas[2].AxisY.Title = "sig - ped (ADCu)";
            chart_chargeLGserial.ChartAreas[2].AxisX.Title = "Channel";

            chart_chargeHGserial.ChartAreas[0].AxisX.Interval = 4;
            chart_chargeHGserial.ChartAreas[0].AxisX.IntervalOffset = 1;
            chart_chargeHGserial.ChartAreas[0].AxisX.Maximum = 32;
            chart_chargeHGserial.ChartAreas[0].AxisX.Minimum = -1;
            chart_chargeHGserial.ChartAreas[0].AxisY.LineColor = Color.White;
            chart_chargeHGserial.ChartAreas[0].AxisY.MajorTickMark.LineColor = Color.White;
            chart_chargeHGserial.ChartAreas[0].AxisY.IsStartedFromZero = false;
            chart_chargeHGserial.ChartAreas[1].AxisX.Interval = 4;
            chart_chargeHGserial.ChartAreas[1].AxisX.IntervalOffset = 1;
            chart_chargeHGserial.ChartAreas[1].AxisX.Maximum = 32;
            chart_chargeHGserial.ChartAreas[1].AxisX.Minimum = -1;
            chart_chargeHGserial.ChartAreas[1].AxisY.LineColor = Color.White;
            chart_chargeHGserial.ChartAreas[1].AxisY.MajorTickMark.LineColor = Color.White;
            chart_chargeHGserial.ChartAreas[1].AxisY.IsStartedFromZero = false;
            chart_chargeHGserial.ChartAreas[2].AxisX.Interval = 4;
            chart_chargeHGserial.ChartAreas[2].AxisX.IntervalOffset = 1;
            chart_chargeHGserial.ChartAreas[2].AxisX.Maximum = 32;
            chart_chargeHGserial.ChartAreas[2].AxisX.Minimum = -1;
            chart_chargeHGserial.ChartAreas[2].AxisY.LineColor = Color.White;
            chart_chargeHGserial.ChartAreas[2].AxisY.MajorTickMark.LineColor = Color.White;
            chart_chargeHGserial.ChartAreas[2].AxisY.IsStartedFromZero = false;

            chart_chargeLGserial.ChartAreas[0].AxisX.Interval = 4;
            chart_chargeLGserial.ChartAreas[0].AxisX.IntervalOffset = 1;
            chart_chargeLGserial.ChartAreas[0].AxisX.Maximum = 32;
            chart_chargeLGserial.ChartAreas[0].AxisX.Minimum = -1;
            chart_chargeLGserial.ChartAreas[0].AxisY.LineColor = Color.White;
            chart_chargeLGserial.ChartAreas[0].AxisY.MajorTickMark.LineColor = Color.White;
            chart_chargeLGserial.ChartAreas[0].AxisY.IsStartedFromZero = false;
            chart_chargeLGserial.ChartAreas[1].AxisX.Interval = 4;
            chart_chargeLGserial.ChartAreas[1].AxisX.IntervalOffset = 1;
            chart_chargeLGserial.ChartAreas[1].AxisX.Maximum = 32;
            chart_chargeLGserial.ChartAreas[1].AxisX.Minimum = -1;
            chart_chargeLGserial.ChartAreas[1].AxisY.LineColor = Color.White;
            chart_chargeLGserial.ChartAreas[1].AxisY.MajorTickMark.LineColor = Color.White;
            chart_chargeLGserial.ChartAreas[1].AxisY.IsStartedFromZero = false;
            chart_chargeLGserial.ChartAreas[2].AxisX.Interval = 4;
            chart_chargeLGserial.ChartAreas[2].AxisX.IntervalOffset = 1;
            chart_chargeLGserial.ChartAreas[2].AxisX.Maximum = 32;
            chart_chargeLGserial.ChartAreas[2].AxisX.Minimum = -1;
            chart_chargeLGserial.ChartAreas[2].AxisY.LineColor = Color.White;
            chart_chargeLGserial.ChartAreas[2].AxisY.MajorTickMark.LineColor = Color.White;
            chart_chargeLGserial.ChartAreas[2].AxisY.IsStartedFromZero = false;

            chart_chargeHGserial.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chart_chargeHGserial.ChartAreas[0].AlignWithChartArea = "ChartArea_signal2";
            chart_chargeHGserial.ChartAreas[0].AlignmentOrientation = Charting.AreaAlignmentOrientations.Vertical;
            chart_chargeHGserial.ChartAreas[0].AlignmentStyle = Charting.AreaAlignmentStyles.All;
            chart_chargeHGserial.ChartAreas[1].AxisX.LabelStyle.Enabled = false;
            chart_chargeHGserial.ChartAreas[1].AlignWithChartArea = "ChartArea_signal2";
            chart_chargeHGserial.ChartAreas[1].AlignmentOrientation = Charting.AreaAlignmentOrientations.Vertical;
            chart_chargeHGserial.ChartAreas[1].AlignmentStyle = Charting.AreaAlignmentStyles.All;

            chart_chargeLGserial.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chart_chargeLGserial.ChartAreas[0].AlignWithChartArea = "ChartArea_signal2";
            chart_chargeLGserial.ChartAreas[0].AlignmentOrientation = Charting.AreaAlignmentOrientations.Vertical;
            chart_chargeLGserial.ChartAreas[0].AlignmentStyle = Charting.AreaAlignmentStyles.All;
            chart_chargeLGserial.ChartAreas[1].AxisX.LabelStyle.Enabled = false;
            chart_chargeLGserial.ChartAreas[1].AlignWithChartArea = "ChartArea_signal2";
            chart_chargeLGserial.ChartAreas[1].AlignmentOrientation = Charting.AreaAlignmentOrientations.Vertical;
            chart_chargeLGserial.ChartAreas[1].AlignmentStyle = Charting.AreaAlignmentStyles.All;

            line++;

            double[] HGped = new double[NbChannels];
            double[] HGsig = new double[NbChannels];
            double[] LGped = new double[NbChannels];
            double[] LGsig = new double[NbChannels];
            double square = 0;
            double sum = 0;
            double sigma = 0;

            DataSplit = DataArray[line].Split(' ');
            for (int chn = 0; chn < NbChannels; chn++)
            {
                HGped[chn] = Convert.ToDouble(DataSplit[chn]);
                square += HGped[chn] * HGped[chn];
                chart_chargeHGserial.Series["pedestal"].Points.AddXY(chn, HGped[chn]);
            }

            sigma = Math.Sqrt((square / NbChannels) - ((HGped.Sum() / NbChannels) * (HGped.Sum() / NbChannels)));
            label_results.Text += "High gain pedestal = " + Math.Round(HGped.Sum() / NbChannels, 3) + " ± " + Math.Round(sigma, 3) + " ADCu";
            square = 0;

            line += 2;
            label_results.Text += "   |   ";

            DataSplit = DataArray[line].Split(' ');
            for (int chn = 0; chn < NbChannels; chn++)
            {
                LGped[chn] = Convert.ToDouble(DataSplit[chn]);
                square += LGped[chn] * LGped[chn];
                chart_chargeLGserial.Series["pedestal"].Points.AddXY(chn, LGped[chn]);
            }

            sigma = Math.Sqrt((square / NbChannels) - ((LGped.Sum() / NbChannels) * (LGped.Sum() / NbChannels)));
            label_results.Text += "Low gain pedestal = " + Math.Round(LGped.Sum() / NbChannels, 3) + " ± " + Math.Round(sigma, 3) + " ADCu";
            square = 0;

            line += 2;
            label_results.Text += "\n";

            DataSplit = DataArray[line].Split(' ');
            for (int chn = 0; chn < NbChannels; chn++)
            {
                HGsig[chn] = Convert.ToDouble(DataSplit[chn]);
                double signal = HGsig[chn] - HGped[chn];
                sum += signal;
                square += signal * signal;
                chart_chargeHGserial.Series["signal"].Points.AddXY(chn, HGsig[chn]);
                chart_chargeHGserial.Series["signal2"].Points.AddXY(chn, signal);
            }

            sigma = Math.Sqrt((square / NbChannels) - ((sum / NbChannels) * (sum / NbChannels)));
            label_results.Text += "High gain signal - pedestal = " + Math.Round(sum / NbChannels, 3) + " ± " + Math.Round(sigma, 3) + " ADCu";
            square = 0;
            sum = 0;

            line += 2;
            label_results.Text += "   |   ";

            DataSplit = DataArray[line].Split(' ');
            for (int chn = 0; chn < NbChannels; chn++)
            {
                LGsig[chn] = Convert.ToDouble(DataSplit[chn]);
                double signal = LGsig[chn] - LGped[chn];
                sum += signal;
                square += signal * signal;
                chart_chargeLGserial.Series["signal"].Points.AddXY(chn, LGsig[chn]);
                chart_chargeLGserial.Series["signal2"].Points.AddXY(chn, signal);
            }

            sigma = Math.Sqrt((square / NbChannels) - ((sum / NbChannels) * (sum / NbChannels)));
            label_results.Text += "Low gain signal - pedestal = " + Math.Round(sum / NbChannels, 3) + " ± " + Math.Round(sigma, 3) + " ADCu";
            square = 0;

            line++;
            label_results.Text += "\n";

            label_results.Text += DataArray[line];

            #endregion
            
            if (writeFile)
            {
                StreamReader streamReader = new StreamReader(loadFileName);
                string data = streamReader.ReadToEnd();
                streamReader.Close();

                data = Path.GetFileNameWithoutExtension(loadFileName) + "\n" + label_results.Text + "\n" + data;

                TextWriter tw = new StreamWriter(loadFileName);
                tw.WriteLine(data);
                tw.Flush();
                tw.Close();
            }
        }

        string strDefSC = "1110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111011101110111111111111111111111111111111111111111111111011110000111111101111001111011000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000000001000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000000100000000000111111110100101100010010110011111111111111011";
        private bool sendSC(int usbDevId, string strSC)
        {
            // Initialize result as false
            bool result = false;
            // Get standard length of slow control bitstream
            int SCLenght = strDefSC.Length;
            // Get length of current slow control bitstream
            int intLenStrSC = strSC.Length;
            byte[] bytSC = new byte[SCLenght / 8];
            // reverse slow control string before loading
            strSC = strRev(strSC);

            // If the length of the current bitstream is not OK, return false
            // else store the slow control in byte array
            if (intLenStrSC == SCLenght)
            {
                for (int i = 0; i < (SCLenght / 8); i++)
                {
                    string strScCmdTmp = strSC.Substring(i * 8, 8);
                    strScCmdTmp = strRev(strScCmdTmp);
                    uint intCmdTmp = Convert.ToUInt32(strScCmdTmp, 2);
                    bytSC[i] = Convert.ToByte(intCmdTmp);
                }
            }
            else return result;

            // Select slow control parameters to FPGA
            Firmware.sendWord(1, "11110100", usbDevId);
            // Send slow control parameters to FPGA
            int intLenBytSC = bytSC.Length;
            Firmware.sendWord(10, bytSC, intLenBytSC, usbDevId);

            // Start shift parameters to ASIC
            Firmware.sendWord(1, "11110110", usbDevId);
            // Stop shift parameters to ASIC
            Firmware.sendWord(1, "11110100", usbDevId);

            // Slow control test checksum test query
            Firmware.sendWord(0, "10110100", usbDevId);

            // Load slow control
            Firmware.sendWord(1, "11110101", usbDevId);
            Firmware.sendWord(1, "11110100", usbDevId);

            // Send slow control parameters to FPGA
            Firmware.sendWord(10, bytSC, intLenBytSC, usbDevId);

            // Start shift parameters to ASIC
            Firmware.sendWord(1, "11110110", usbDevId);
            // Stop shift parameters to ASIC
            Firmware.sendWord(1, "11110100", usbDevId);

            // Slow Control Correlation Test Result
            if (Firmware.readWord(4, usbDevId) == "00000000") result = true;

            // Reset slow control test checksum test query
            Firmware.sendWord(0, "00110100", usbDevId);

            return result;
        }

        #region misc. methods

        private void wait(int nbTicks100ns)
        {
            // 1 tick = 100 ns
            var sw = Stopwatch.StartNew();
            while (sw.ElapsedTicks < nbTicks100ns) { }
        }

        public delegate void LabelDelegate(string message, Label label);
        private void UpdatingLabel(string msg, Label lbl)
        {
            if (lbl.InvokeRequired) lbl.BeginInvoke(new LabelDelegate(UpdatingLabel), new object[] { msg, lbl });
            else lbl.Text = msg;
        }

        private void TextBox_asicNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label_titleBar_Click(object sender, EventArgs e)
        {

        }

        public static int GrayToInt(BitArray gray, int length) // Gray code to integer conversion
        {
            // gray conversion
            BitArray binary = new BitArray(length);
            binary[length - 1] = gray[length - 1];
            for (int i = length - 2; i >= 0; i--)
                binary[i] = !gray[i] ^ !binary[i + 1];
            // cast bitarray to int
            int[] result = new int[1];
            binary.CopyTo(result, 0);
            return result[0];
        }

        public static ArrayList ReadFileLine(string FilePathforRead) // Read a file and return the data in an ArrayList
        {
            StreamReader FiletoRead = new StreamReader(FilePathforRead);
            string sLine = "";
            ArrayList LineList = new ArrayList();
            while (sLine != null)
            {
                sLine = FiletoRead.ReadLine();
                if (sLine != null && !sLine.Equals(""))
                    LineList.Add(sLine);
            }
            FiletoRead.Close();
            return LineList;
        }

        public static double[] lookUpRepeatElement(double[] arrayIn, out double[] elementCount)
        {
            Dictionary<double, double> list = new Dictionary<double, double>();
            for (int i = 0; i < arrayIn.Length; i++)
            {
                if (list.ContainsKey(arrayIn[i]))
                    list[arrayIn[i]]++;
                else
                    list.Add(arrayIn[i], 1);
            }
            list = list.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            double[] eleCnt = new double[list.Keys.Count];
            double[] eleNum = new double[list.Values.Count];
            list.Keys.CopyTo(eleNum, 0);
            list.Values.CopyTo(eleCnt, 0);

            elementCount = eleCnt;
            return eleNum;
        }

        private static string IntToBin(int value, int len) // To convert a value from integer to binary representation into a string
        {
            return (len > 1 ? IntToBin(value >> 1, len - 1) : null) + "01"[value & 1];
        }

        public static string strRev(string s) // To reverse a string
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        #endregion

        [DllImport("gdi32.dll", EntryPoint = "BitBlt")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(
            [In()] System.IntPtr hdc, int x, int y, int cx, int cy,
            [In()] System.IntPtr hdcSrc, int x1, int y1, uint rop);

        private const int SRC_COPY = 0xCC0020;
        private void button_saveImage_Click(object sender, EventArgs e)
        {
            try
            {
                Control[] lbl = Controls.Find("screenshot", true);
                Controls.Remove(lbl[0]);
                lbl[0].Dispose();
            }
            catch { }

            string imageSaveFile = "";

            if (strSaveName == "") return;

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Title = "Save file";
            saveDialog.RestoreDirectory = true;
            FileInfo fi = new FileInfo(strSaveName);
            saveDialog.Filter = "(*.png)|*.png";
            saveDialog.FileName = fi.Name + ".png";

            if (saveDialog.ShowDialog() == DialogResult.OK) imageSaveFile = saveDialog.FileName;
            else return;

            var frm = label_titleBar.FindForm();

            using (Bitmap bitmap = new Bitmap(frm.Width, frm.Height))
            {
                using (Graphics gb = Graphics.FromImage(bitmap))
                using (Graphics gc = Graphics.FromHwnd(ActiveForm.Handle))
                {

                    IntPtr hdcDest = IntPtr.Zero;
                    IntPtr hdcSrc = IntPtr.Zero;

                    try
                    {
                        hdcDest = gb.GetHdc();
                        hdcSrc = gc.GetHdc();

                        BitBlt(hdcDest, 0, 0, bitmap.Width, bitmap.Height, hdcSrc, 0, 0, SRC_COPY);
                    }
                    finally
                    {
                        if (hdcDest != IntPtr.Zero) gb.ReleaseHdc(hdcDest);
                        if (hdcSrc != IntPtr.Zero) gc.ReleaseHdc(hdcSrc);
                    }
                }

                bitmap.Save(imageSaveFile, System.Drawing.Imaging.ImageFormat.Png);
            }
            /*var chip1 = new object[1, 7]
               {
                     { textBox_asicNumber.Text, "YES", 1,1,1,1,1 },
               };
            AjouterChip(chip1);*/
        }
    }
}
