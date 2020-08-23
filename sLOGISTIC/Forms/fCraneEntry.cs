//1. Create a Open File dialogbox for GTI, Analysis Condition and Member Shape files.
//2. Validate Section
//3. Validate Properties
//4. Validate file.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
//add-in
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace sLOGISTIC
{
    public partial class fCraneEntry : Form
    {
        //Form tMain;

        private string gsColStyle = string.Empty;

        private string gsColMin = string.Empty;
        private string gsColMax = string.Empty;

        private string gsR = string.Empty;
        private string gsG = string.Empty;
        private string gsB = string.Empty;

        private bool pbProcess = false;

        //public fCraneEntry(Form pMain)
        public fCraneEntry()
        {
            InitializeComponent();
            //tMain = pMain;
        }

#region main
        private void fCraneEntry_Load(object sender, EventArgs e)
        {
            //this.Icon = new Icon(this.Icon, this.Icon.Size);

            //cFP.gfBackColor(this);

            //subLoadLanguage();

            subSetObject();
            subSetOption();
            subSetSetting();

            gsStatusBar(stb1, "Ready...");
            stb2.Visible = false;
            stb3.Visible = false;

            this.WindowState = FormWindowState.Maximized;
        }
        private void fCraneEntry_Activated(object sender, EventArgs e)
        {
            if (btnGenerate.Enabled == true) { btnGenerate.Focus(); }
        }
        private void fCraneEntry_Shown(object sender, EventArgs e)
        {
            if (btnGenerate.Enabled == true) { btnGenerate.Focus(); }
        }
        private void fCraneEntry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (btnGenerate.Enabled == true) { btnGenerate.Focus(); }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.F2 || e.KeyCode == Keys.F3 || e.KeyCode == Keys.F4)
            {
                switch (e.KeyCode)
                {
                    case Keys.F2: btnGenerate.PerformClick(); break;
                    case Keys.F3: btnImage.PerformClick(); break;
                    case Keys.F4: btnExport.PerformClick(); break;
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                e.Handled = false;
            }
        }
        private void fCraneEntry_KeyPress(object sender, KeyPressEventArgs e)
        {
            //
        }
        private void fCraneEntry_Resize(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState != FormWindowState.Minimized)
                {
                    grpSection.SetBounds(grpSection.Left, grpSection.Top, grpSection.Width, grpSection.Height);
                    
                    grpMaterialMemberPropertiesDefaultValue.SetBounds(grpMaterialMemberPropertiesDefaultValue.Left, (grpSection.Top + grpSection.Height) + (1 * 10), grpMaterialMemberPropertiesDefaultValue.Width, (this.Height - (grpSection.Top + grpSection.Height + sst.Height)) - (1 * 50));

                    grpSelectAndLoad.SetBounds(grpSelectAndLoad.Left, grpSelectAndLoad.Top, (this.Width - (grpSelectAndLoad.Left)) - (1 * 24), grpSelectAndLoad.Height);

                    txtGTI.SetBounds(txtGTI.Left, txtGTI.Top, (grpSelectAndLoad.Width - (txtGTI.Left)) - (1 * 40), txtGTI.Height);
                    txtAnalysisCondition.SetBounds(txtAnalysisCondition.Left, txtAnalysisCondition.Top, (grpSelectAndLoad.Width - (txtAnalysisCondition.Left)) - (1 * 40), txtGTI.Height);
                    txtMemberShape.SetBounds(txtMemberShape.Left, txtMemberShape.Top, (grpSelectAndLoad.Width - (txtMemberShape.Left)) - (1 * 40), txtGTI.Height);

                    btnGTI.SetBounds((grpSelectAndLoad.Width - (btnGTI.Width)) - (1 * 8), btnGTI.Top, btnGTI.Width, btnGTI.Height);
                    btnAnalysisCondition.SetBounds((grpSelectAndLoad.Width - (btnAnalysisCondition.Width)) - (1 * 8), btnAnalysisCondition.Top, btnAnalysisCondition.Width, btnAnalysisCondition.Height);
                    btnMemberShape.SetBounds((grpSelectAndLoad.Width - (btnMemberShape.Width)) - (1 * 8), btnMemberShape.Top, btnMemberShape.Width, btnMemberShape.Height);

                    dgw.SetBounds(dgw.Left, (grpSelectAndLoad.Top + grpSelectAndLoad.Height) + 4, (this.Width - (dgw.Left)) - (1 * 24), (this.Height - (dgw.Top + btnClose.Height + sst.Height)) - (1 * 44));

                    btnClose.SetBounds(btnClose.Left, (this.Height - (btnClose.Height + sst.Height)) - (1 * 40), btnClose.Width, btnClose.Height);
                    btnGenerate.SetBounds(btnGenerate.Left, (this.Height - (btnGenerate.Height + sst.Height)) - (1 * 40), btnGenerate.Width, btnGenerate.Height);
                    btnImage.SetBounds(btnImage.Left, (this.Height - (btnImage.Height + sst.Height)) - (1 * 40), btnImage.Width, btnImage.Height);
                    btnExport.SetBounds(btnExport.Left, (this.Height - (btnExport.Height + sst.Height)) - (1 * 40), btnExport.Width, btnExport.Height);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Resize", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void fCraneEntry_SizeChanged(object sender, EventArgs e)
        {
            //
        }
        private void fCraneEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pbProcess == true)
            {
                MessageBox.Show("Please wait to finish the process before closing.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
            else e.Cancel = false;
            //if (tMain.mnuAromeInput.Checked == true) tMain.mnuAromeInput.Checked = false;
        }
#endregion

#region button
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            bool lbReturn = fncGenerate();

            fCraneImage newMDIChild = new fCraneImage();
            newMDIChild.Show();
            newMDIChild.Dispose();

        }

        private void btnGTI_Click(object sender, EventArgs e)
        {
            string lsPath = string.Empty;

            int liLength = 0;

            if (txtGTI.Text != string.Empty)
            {
                lsPath = txtGTI.Text;

                liLength = gsubGetLastChar(lsPath);

                lsPath = lsPath.Substring(0, liLength);
            }
            else
            {
                lsPath = Application.ExecutablePath;
            }

            string lsRetVal = cBrowser.gsBrowser(lsPath, "GTI Files (*.gti)", "(*.gti)");

            if (lsRetVal != string.Empty)
            {
                txtGTI.Text = lsRetVal;
            }
        }
        private void btnAnalysisCondition_Click(object sender, EventArgs e)
        {
            string lsPath = string.Empty;

            int liLength = 0;

            if (txtAnalysisCondition.Text != string.Empty)
            {
                lsPath = txtAnalysisCondition.Text;

                liLength = gsubGetLastChar(lsPath);

                lsPath = lsPath.Substring(0, liLength);
            }
            else
            {
                lsPath = Application.ExecutablePath;
            }

            string lsRetVal = cBrowser.gsBrowser(lsPath, "Analysis Condition Files (*.csv)", "(*.csv)");

            if (lsRetVal != string.Empty)
            {
                txtAnalysisCondition.Text = lsRetVal;
            }
        }
        private void btnMemberShape_Click(object sender, EventArgs e)
        {
            string lsPath = string.Empty;

            int liLength = 0;

            if (txtMemberShape.Text != string.Empty)
            {
                lsPath = txtMemberShape.Text;

                liLength = gsubGetLastChar(lsPath);

                lsPath = lsPath.Substring(0, liLength);
            }
            else
            {
                lsPath = Application.ExecutablePath;
            }

            string lsRetVal = cBrowser.gsBrowser(lsPath, "Member Shape Files (*.csv)", "(*.csv)");

            if (lsRetVal != string.Empty)
            {
                txtMemberShape.Text = lsRetVal;
            }
        }

        private void btnImage_Click(object sender, EventArgs e)
        {
            fCraneImage newMDIChild = new fCraneImage();
            newMDIChild.Show();
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            //..
        }        
#endregion

#region function/procedure
        private void subSetObject()
        {
            txtGTI.Text = string.Empty;
            txtAnalysisCondition.Text = string.Empty;
            txtMemberShape.Text = string.Empty;

            txtE.Text = string.Empty;
            txtFY.Text = string.Empty;
            txtKY.Text = string.Empty;
            txtKZ.Text = string.Empty;

            txtNumberOfSections.Text = string.Empty;
            txtNumberOfCases.Text = string.Empty;
        }
        private void subSetOption()
        {
            txtNumberOfSections.Text = string.Empty;
            txtNumberOfCases.Text = string.Empty;
            
            txtNumberOfSections.Enabled = false;
            txtNumberOfCases.Enabled = false; 

            if (rdbSection1.Checked == true)
            {
                txtNumberOfSections.Text = "100";
                txtNumberOfCases.Text = "100";
            }
            else if (rdbSection2.Checked == true)
            {
                txtNumberOfSections.Text = "100";
                txtNumberOfCases.Text = "50";
            }
            if (rdbSection3.Checked == true)
            {
                txtNumberOfSections.Enabled = true;
                txtNumberOfCases.Enabled = true;               
            }

            if (txtNumberOfSections.Enabled == true) { txtNumberOfSections.Focus(); }
        }
        private void subSetSetting()
        {
            gsColStyle = string.Empty;
        
            gsColMin = string.Empty;
            gsColMax = string.Empty;
           
            gsR = string.Empty;
            gsG = string.Empty;
            gsB = string.Empty;

            gsR = string.Empty;
            gsG = string.Empty;
            gsB = string.Empty;

            gsColStyle = "1";

            gsColMin = "255";
            gsColMax = "1120";

            gsR = "0";
            gsG = "86";
            gsB = "255";

            gsR = "0";
            gsG = "176";
            gsB = "255";

            gsR = "0";
            gsG = "255";
            gsB = "253";

            gsR = "0";
            gsG = "255";
            gsB = "166";

            gsR = "0";
            gsG = "255";
            gsB = "79";

            gsR = "6";
            gsG = "255";
            gsB = "0";

            gsR = "92";
            gsG = "255";
            gsB = "0";

            gsR = "179";
            gsG = "255";
            gsB = "0";

            gsR = "255";
            gsG = "192";
            gsB = "0";

            gsR = "255";
            gsG = "128";
            gsB = "128";

            gsR = "255";        //Ov
            gsG = "0";
            gsB = "0";

            gsR = "0";          //Od
            gsG = "0";
            gsB = "0";

            txtE.Text = Convert.ToInt64("206000").ToString("###,##0");
            txtFY.Text = Convert.ToInt64("235").ToString("###,##0"); ;
            txtKY.Text = Convert.ToInt64(Convert.ToInt64("75")/100).ToString("###,##0.00");
            txtKZ.Text = Convert.ToInt64(Convert.ToInt64("06")/100).ToString("###,##0.00");

            txtGTI.Text = "C:\\CASC - Copy\\DBX\\OR\\OR_Test.gti";
            txtAnalysisCondition.Text = "C:\\CASC - Copy\\MEMPROP and 解析条件\\GE0259_BD(OR)_MEMPROP_200113.csv";
            txtMemberShape.Text = "C:\\CASC - Copy\\MEMPROP and 解析条件\\GE0259_BD(OR)_解析条件_200113.csv";
        }

        private int gsubGetLastChar(string pTxt)
        {
            int liReturn = 0;

            for (int i = (pTxt.Length - 1); i > 0; i--)
            {
                if (pTxt.Substring(i, 1) == "\\")
                {
                    liReturn = i;
                    break;
                }
            }

            return liReturn;
        }

        //statusbar
        private void gsStatusBar(ToolStripStatusLabel pTool, string msg)
        {
            pTool.Text = msg;
        }
        private void gsProgressBar(ToolStripProgressBar pTool, int pValue)
        {
            pTool.Value = pValue;
        }
        private void gsRichTextBox(string msg)
        {
            //+limited number of multi-line
            //if (rtbLog.Text == string.Empty)
            //{
            //    rtbLog.Text = msg;
            //}
            //else
            //{
            //    rtbLog.Text = rtbLog.Text + Environment.NewLine + msg;
            //    rtbLog.SelectionStart = rtbLog.TextLength;                   
            //}
        }

        private bool fncGenerate()
        {
            bool lbRetVal = false;

            if (rdbSection1.Checked == false && rdbSection1.Checked == false && rdbSection1.Checked == false)
            {
                MessageBox.Show("Please select option.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return lbRetVal;
            }

            if (txtNumberOfSections.Text == string.Empty)
            {
                MessageBox.Show("Invalid data entry.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNumberOfSections.Focus();
                return lbRetVal;
            }
            if (txtNumberOfCases.Text == string.Empty)
            {
                MessageBox.Show("Invalid data entry.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtNumberOfCases.Focus();                
                return lbRetVal;
            }

            if (txtGTI.Text == string.Empty)
            {
                MessageBox.Show("Invalid file entry.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGTI.Focus();
                return lbRetVal;
            }
            if (txtAnalysisCondition.Text == string.Empty)
            {
                MessageBox.Show("Invalid file entry.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAnalysisCondition.Focus();
                return lbRetVal;
            }
            if (txtMemberShape.Text == string.Empty)
            {
                MessageBox.Show("Invalid file entry.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtMemberShape.Focus();
                return lbRetVal;
            }

            if (txtE.Text == string.Empty)
            {
                MessageBox.Show("Invalid data entry.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtE.Focus();
                return lbRetVal;
            }
            if (txtFY.Text == string.Empty)
            {
                MessageBox.Show("Invalid data entry.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFY.Focus();
                return lbRetVal;
            }
            if (txtKY.Text == string.Empty)
            {
                MessageBox.Show("Invalid data entry.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtKY.Focus();
                return lbRetVal;
            }
            if (txtKZ.Text == string.Empty)
            {
                MessageBox.Show("Invalid data entry.", "Crane Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtKZ.Focus();
                return lbRetVal;
            }

            string lsGTIFile = string.Empty;

            string lsJOIAttrFile = string.Empty;
            string lsMEMAttriFile = string.Empty;
            string lsMEMPropFile = string.Empty;
            string lsSECFORFile = string.Empty;

            string lsPath = string.Empty;

            Int32 liLength = 0;

            lsGTIFile = txtGTI.Text.Trim();

            if (fncCreateText("") == true)
            {
                MessageBox.Show("Failed to create file!", "Generate", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return lbRetVal;
            }

            lbRetVal = fncReadGTIFile(lsGTIFile, out lsJOIAttrFile, out lsMEMAttriFile, out lsMEMPropFile, out lsSECFORFile);

            if (txtGTI.Text != string.Empty)
            {
                lsPath = txtGTI.Text;

                liLength = gsubGetLastChar(lsPath);

                lsPath = lsPath.Substring(0, liLength);
            }
            else
            {
                return lbRetVal;
            }

            lbRetVal = fncReadJOIFile(lsPath  + "\\" + lsJOIAttrFile);

            lbRetVal = fncReadMEMAttrFile(lsPath + "\\" + lsMEMAttriFile);

            lbRetVal = fncReadMEMPropFile(lsPath + "\\" + lsMEMPropFile);

            lbRetVal = fncReadSECForFile(lsPath + "\\" + lsSECFORFile);

            lbRetVal = fncReadACFile(txtAnalysisCondition.Text.Trim());

            lbRetVal = fncReadMSFile(txtMemberShape.Text.Trim());

            // create log
            // create *.stg file

            return lbRetVal;
        }

        private bool fncReadGTIFile(string psGTIFile, out string psJOIAttrFile, out string psMEMAttrFile, out string psMEMPropFile, out string psSECForFile)
        {
            bool lbReturn = false;

            psJOIAttrFile = string.Empty;
            psMEMAttrFile = string.Empty;
            psMEMPropFile = string.Empty;
            psSECForFile = string.Empty;
            try
            {
                dgw.Rows.Clear(); dgw.ClearSelection(); dgw.CurrentCell = null;

                string lsLine = string.Empty;

                long llCount = 0;

                Int32 liCnt = 1;
                Int32 liStart = 0;

                llCount = File.ReadAllLines(psGTIFile).Length;
                if (llCount <= 0)
                {
                    MessageBox.Show("Data file " + psGTIFile + " is empty.", "Read GTI File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return lbReturn;
                }

                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                gsStatusBar(stb1, "Processing data please wait...");
                stb2.Visible = true;
                stb2.Minimum = 0;
                stb2.Maximum = 100;
                stb3.Visible = true;
                gsStatusBar(stb3, "Line: 0 Total: 0");

                dgw.Rows.Add("Read GTI File: " + psGTIFile + "...");

                using (StreamReader sr = File.OpenText(psGTIFile))
                {
                    while ((lsLine = sr.ReadLine()) != null)
                    {
                        if (lsLine.Length > 1)
                        {
                            liStart = lsLine.IndexOf("JOI_ATTR.TXT");
                            if (liStart != -1)
                            {
                                if (lsLine.Substring(liStart, 12).Trim() == "JOI_ATTR.TXT")
                                {
                                    psJOIAttrFile = lsLine.Substring(liStart, 12).Trim();

                                    dgw.Rows.Add("JOI Attribute File: " + psJOIAttrFile + "...");
                                    dgw.Rows.
                                }
                            }

                            liStart = lsLine.IndexOf("MEM_ATTR.TXT");
                            if (liStart != -1)
                            {
                                if (lsLine.Substring(liStart, 12).Trim() == "MEM_ATTR.TXT")
                                {
                                    psMEMAttrFile = lsLine.Substring(liStart, 12).Trim();

                                    dgw.Rows.Add("MEM Attribute File: " + psMEMAttrFile + "...");
                                }
                            }

                            liStart = lsLine.IndexOf("MEM_PROP.TXT");
                            if (liStart != -1)
                            {
                                if (lsLine.Substring(liStart, 12).Trim() == "MEM_PROP.TXT")
                                {
                                    psMEMPropFile = lsLine.Substring(liStart, 12).Trim();

                                    dgw.Rows.Add("MEM Properties File: " + psMEMPropFile + "...");
                                }
                            }

                            liStart = lsLine.IndexOf("SEC_FOR.TXT");
                            if (liStart != -1)
                            {
                                if (lsLine.Substring(liStart, 11).Trim() == "SEC_FOR.TXT")
                                {
                                    psSECForFile = lsLine.Substring(liStart, 11).Trim();

                                    dgw.Rows.Add("SEC Properties File: " + psSECForFile + "...");
                                }
                            }

                            gsStatusBar(stb1, "Processing data please wait " + (Int64)((float)(liCnt) / (float)(llCount) * 100) + "%...");
                            gsStatusBar(stb3, "Line: " + liCnt.ToString("###,##0") + "     Total: " + llCount.ToString("###,##0"));
                            gsProgressBar(stb2, (Int16)((float)(liCnt) / (float)(llCount) * 100));
                            liCnt++;
                            Application.DoEvents();
                        }
                    }
                    sr.Close();
                }

                dgw.Rows.Add("Successfully read file " + psGTIFile + "...");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read GTI File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                //dgw.Focus();
                //if (dgw.RowCount > 0)
                //{
                //    dgw.FirstDisplayedScrollingRowIndex = 0;
                //}

                gsStatusBar(stb1, "Ready...");
                gsProgressBar(stb2, 0);
                stb2.Visible = false;
                stb3.Visible = false;
            }
            return lbReturn;
        }

        private bool fncReadJOIFile(string psJOIAttrFile)
        {
            bool lbReturn = false;

            try
            {
                string lsLine = string.Empty;

                long llCount = 0;

                Int32 liCnt = 1;

                llCount = File.ReadAllLines(psJOIAttrFile).Length;
                if (llCount <= 0)
                {
                    MessageBox.Show("Data file " + psJOIAttrFile + " is empty.", "Read JOI File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return lbReturn;
                }

                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                gsStatusBar(stb1, "Processing data please wait...");
                stb2.Visible = true;
                stb2.Minimum = 0;
                stb2.Maximum = 100;
                stb3.Visible = true;
                gsStatusBar(stb3, "Line: 0 Total: 0");

                dgw.Rows.Add("Read JOI File: " + psJOIAttrFile + "...");

                using (StreamReader sr = File.OpenText(psJOIAttrFile))
                {
                    while ((lsLine = sr.ReadLine()) != null)
                    {
                        if (lsLine.Length > 1)
                        {

                            dgw.Rows.Add(lsLine);

                            gsStatusBar(stb1, "Processing data [" + psJOIAttrFile + "] please wait " + (Int64)((float)(liCnt) / (float)(llCount) * 100) + "%...");
                            gsStatusBar(stb3, "Line: " + liCnt.ToString("###,##0") + "     Total: " + llCount.ToString("###,##0"));
                            gsProgressBar(stb2, (Int16)((float)(liCnt) / (float)(llCount) * 100));
                            liCnt++;
                            Application.DoEvents();
                        }
                    }
                    sr.Close();
                }

                dgw.Rows.Add("Successfully read file " + psJOIAttrFile + "...");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read GTI File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                gsStatusBar(stb1, "Ready...");
                gsProgressBar(stb2, 0);
                stb2.Visible = false;
                stb3.Visible = false;
            }
            return lbReturn;
        }
        private bool fncReadMEMAttrFile(string psMEMAttrFile)
        {
            bool lbReturn = false;

            try
            {
                string lsLine = string.Empty;

                long llCount = 0;

                Int32 liCnt = 1;

                llCount = File.ReadAllLines(psMEMAttrFile).Length;
                if (llCount <= 0)
                {
                    MessageBox.Show("Data file " + psMEMAttrFile + " is empty.", "Read MEM Attribute File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return lbReturn;
                }

                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                gsStatusBar(stb1, "Processing data please wait...");
                stb2.Visible = true;
                stb2.Minimum = 0;
                stb2.Maximum = 100;
                stb3.Visible = true;
                gsStatusBar(stb3, "Line: 0 Total: 0");

                dgw.Rows.Add("Read JOI File: " + psMEMAttrFile + "...");

                using (StreamReader sr = File.OpenText(psMEMAttrFile))
                {
                    while ((lsLine = sr.ReadLine()) != null)
                    {
                        if (lsLine.Length > 1)
                        {

                            dgw.Rows.Add(lsLine);

                            gsStatusBar(stb1, "Processing data [" + psMEMAttrFile + "] please wait " + (Int64)((float)(liCnt) / (float)(llCount) * 100) + "%...");
                            gsStatusBar(stb3, "Line: " + liCnt.ToString("###,##0") + "     Total: " + llCount.ToString("###,##0"));
                            gsProgressBar(stb2, (Int16)((float)(liCnt) / (float)(llCount) * 100));
                            liCnt++;
                            Application.DoEvents();
                        }
                    }
                    sr.Close();
                }

                dgw.Rows.Add("Successfully read file " + psMEMAttrFile + "...");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read MEM Attribute File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                gsStatusBar(stb1, "Ready...");
                gsProgressBar(stb2, 0);
                stb2.Visible = false;
                stb3.Visible = false;
            }
            return lbReturn;
        }
        private bool fncReadMEMPropFile(string psMEMPropFile)
        {
            bool lbReturn = false;

            try
            {
                string lsLine = string.Empty;

                long llCount = 0;

                Int32 liCnt = 1;

                llCount = File.ReadAllLines(psMEMPropFile).Length;
                if (llCount <= 0)
                {
                    MessageBox.Show("Data file " + psMEMPropFile + " is empty.", "Read MEM Properties File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return lbReturn;
                }

                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                gsStatusBar(stb1, "Processing data please wait...");
                stb2.Visible = true;
                stb2.Minimum = 0;
                stb2.Maximum = 100;
                stb3.Visible = true;
                gsStatusBar(stb3, "Line: 0 Total: 0");

                dgw.Rows.Add("Read JOI File: " + psMEMPropFile + "...");

                using (StreamReader sr = File.OpenText(psMEMPropFile))
                {
                    while ((lsLine = sr.ReadLine()) != null)
                    {
                        if (lsLine.Length > 1)
                        {

                            dgw.Rows.Add(lsLine);

                            gsStatusBar(stb1, "Processing data [" + psMEMPropFile + "] please wait " + (Int64)((float)(liCnt) / (float)(llCount) * 100) + "%...");
                            gsStatusBar(stb3, "Line: " + liCnt.ToString("###,##0") + "     Total: " + llCount.ToString("###,##0"));
                            gsProgressBar(stb2, (Int16)((float)(liCnt) / (float)(llCount) * 100));
                            liCnt++;
                            Application.DoEvents();
                        }
                    }
                    sr.Close();
                }

                dgw.Rows.Add("Successfully read file " + psMEMPropFile + "...");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read MEM Properties File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                gsStatusBar(stb1, "Ready...");
                gsProgressBar(stb2, 0);
                stb2.Visible = false;
                stb3.Visible = false;
            }
            return lbReturn;
        }
        private bool fncReadSECForFile(string psSECForFile)
        {
            bool lbReturn = false;

            try
            {
                string lsLine = string.Empty;

                long llCount = 0;

                Int32 liCnt = 1;

                llCount = File.ReadAllLines(psSECForFile).Length;
                if (llCount <= 0)
                {
                    MessageBox.Show("Data file " + psSECForFile + " is empty.", "Read SEC For File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return lbReturn;
                }

                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                gsStatusBar(stb1, "Processing data please wait...");
                stb2.Visible = true;
                stb2.Minimum = 0;
                stb2.Maximum = 100;
                stb3.Visible = true;
                gsStatusBar(stb3, "Line: 0 Total: 0");

                dgw.Rows.Add("Read JOI File: " + psSECForFile + "...");

                using (StreamReader sr = File.OpenText(psSECForFile))
                {
                    while ((lsLine = sr.ReadLine()) != null)
                    {
                        if (lsLine.Length > 1)
                        {

                            dgw.Rows.Add(lsLine);

                            gsStatusBar(stb1, "Processing data [" + psSECForFile + "] please wait " + (Int64)((float)(liCnt) / (float)(llCount) * 100) + "%...");
                            gsStatusBar(stb3, "Line: " + liCnt.ToString("###,##0") + "     Total: " + llCount.ToString("###,##0"));
                            gsProgressBar(stb2, (Int16)((float)(liCnt) / (float)(llCount) * 100));
                            liCnt++;
                            Application.DoEvents();
                        }
                    }
                    sr.Close();
                }

                dgw.Rows.Add("Successfully read file " + psSECForFile + "...");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read SEC FOR File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                gsStatusBar(stb1, "Ready...");
                gsProgressBar(stb2, 0);
                stb2.Visible = false;
                stb3.Visible = false;
            }
            return lbReturn;
        }

        private bool fncReadACFile(string psACFile)
        {
            bool lbReturn = false;

            try
            {
                string lsLine = string.Empty;

                long llCount = 0;

                Int32 liCnt = 1;

                llCount = File.ReadAllLines(psACFile).Length;
                if (llCount <= 0)
                {
                    MessageBox.Show("Data file " + psACFile + " is empty.", "Read AC File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return lbReturn;
                }

                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                gsStatusBar(stb1, "Processing data please wait...");
                stb2.Visible = true;
                stb2.Minimum = 0;
                stb2.Maximum = 100;
                stb3.Visible = true;
                gsStatusBar(stb3, "Line: 0 Total: 0");

                dgw.Rows.Add("Read AC File: " + psACFile + "...");

                using (StreamReader sr = File.OpenText(psACFile))
                {
                    while ((lsLine = sr.ReadLine()) != null)
                    {
                        if (lsLine.Length > 1)
                        {

                            dgw.Rows.Add(lsLine);

                            gsStatusBar(stb1, "Processing data [" + psACFile + "] please wait " + (Int64)((float)(liCnt) / (float)(llCount) * 100) + "%...");
                            gsStatusBar(stb3, "Line: " + liCnt.ToString("###,##0") + "     Total: " + llCount.ToString("###,##0"));
                            gsProgressBar(stb2, (Int16)((float)(liCnt) / (float)(llCount) * 100));
                            liCnt++;
                            Application.DoEvents();
                        }
                    }
                    sr.Close();
                }

                dgw.Rows.Add("Successfully read file " + psACFile + "...");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read AC File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                gsStatusBar(stb1, "Ready...");
                gsProgressBar(stb2, 0);
                stb2.Visible = false;
                stb3.Visible = false;
            }
            return lbReturn;
        }
        private bool fncReadMSFile(string psMSFile)
        {
            bool lbReturn = false;

            try
            {
                string lsLine = string.Empty;

                long llCount = 0;

                Int32 liCnt = 1;

                llCount = File.ReadAllLines(psMSFile).Length;
                if (llCount <= 0)
                {
                    MessageBox.Show("Data file " + psMSFile + " is empty.", "Read MS File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return lbReturn;
                }

                Cursor.Current = Cursors.WaitCursor;
                Cursor.Show();

                gsStatusBar(stb1, "Processing data please wait...");
                stb2.Visible = true;
                stb2.Minimum = 0;
                stb2.Maximum = 100;
                stb3.Visible = true;
                gsStatusBar(stb3, "Line: 0 Total: 0");

                dgw.Rows.Add("Read MS File: " + psMSFile + "...");

                using (StreamReader sr = File.OpenText(psMSFile))
                {
                    while ((lsLine = sr.ReadLine()) != null)
                    {
                        if (lsLine.Length > 1)
                        {

                            dgw.Rows.Add(lsLine);

                            gsStatusBar(stb1, "Processing data [" + psMSFile + "] please wait " + (Int64)((float)(liCnt) / (float)(llCount) * 100) + "%...");
                            gsStatusBar(stb3, "Line: " + liCnt.ToString("###,##0") + "     Total: " + llCount.ToString("###,##0"));
                            gsProgressBar(stb2, (Int16)((float)(liCnt) / (float)(llCount) * 100));
                            liCnt++;
                            Application.DoEvents();
                        }
                    }
                    sr.Close();
                }

                dgw.Rows.Add("Successfully read file " + psMSFile + "...");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Read MS File", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                Cursor.Current = Cursors.Default;

                gsStatusBar(stb1, "Ready...");
                gsProgressBar(stb2, 0);
                stb2.Visible = false;
                stb3.Visible = false;
            }
            return lbReturn;
        }

        private bool fncCreateText(string psFile)
        {
            try
            {
                if (System.IO.File.Exists(psFile) == false) System.IO.File.Create(psFile);
                return true;
            }
            catch { return false; }
        }
        private bool fncWriteToFile(string pFilePath, string pMsg)
        {
            try
            {
                using (StreamWriter sws = new StreamWriter(pFilePath, true))
                {
                    sws.WriteLine(pMsg);
                    sws.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Write to File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
        }
#endregion

#region events
        private void rdbSection1_CheckedChanged(object sender, EventArgs e)
        {
            subSetOption();
        }
        private void rdbSection2_CheckedChanged(object sender, EventArgs e)
        {
            subSetOption();
        }
        private void rdbSection3_CheckedChanged(object sender, EventArgs e)
        {
            subSetOption();
        }
#endregion

    }
}

// SSS -> calamity loan