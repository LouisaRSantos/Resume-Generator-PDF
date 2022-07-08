using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace PDF_Resume_Creator
{
    public partial class ResumePDF : Form
    {
        public ResumePDF()
        {
            InitializeComponent();
        }

        private void btnsaveJSON_Click(object sender, EventArgs e)
        {
            RTboxResume.Text = "";
            string file_name = TSName.Text + "_" + TFName.Text;
            try
            {
                Resume resume = new Resume()
                {
                    FirstName = TFName.Text,
                    MiddleInitital = TMInitial.Text,
                    Surname = TSName.Text,
                    Address = TAddress.Text,
                    ZIP = TZIP.Text,
                    Email = TEmail.Text,
                    PhoneNumber = TNumber.Text,
                    Objectives = TObjectives.Text,
                    College = TCollege.Text,
                    SeniorHS = TSHS.Text,
                    JuniorHS = TJHS.Text,
                    Company1 = TC1.Text,
                    CompanyDes1 = TCDes1.Text,
                    Company2 = TC2.Text,
                    CompanyDes2 = TCDes2.Text,
                    Company3 = TC3.Text,
                    CompanyDes3 = TCDes3.Text,
                    Skills = new List<string>
                    {
                        "\n\t" + "*" + TS1.Text,
                        "\n\t" + "*" + TS2.Text,
                        "\n\t" + "*" + TS3.Text,
                        "\n\t" + "*" + TS4.Text,
                        "\n\t" + "*" + TS5.Text
                    }
                };
                string json = JsonConvert.SerializeObject(resume, Formatting.None);
                File.WriteAllText(@"C:\Users\Santos\Documents\json\" + file_name + ".json", json);
                MessageBox.Show("Saved");

                json = string.Empty;
                json = File.ReadAllText(@"C:\Users\Santos\Documents\json\" + file_name + ".json");
                Resume resultResume = JsonConvert.DeserializeObject<Resume>(json);
                RTboxResume.Text = resultResume.ToString();
            }
            catch 
            {
                MessageBox.Show("Incomplete");
            }
            
        }
        class Resume
        {
            public string FirstName { get; set; }
            public string MiddleInitital { get; set; }
            public string Surname { get; set; }
            public string Address { get; set; }
            public string ZIP { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Objectives { get; set; }
            public string College { get; set; }
            public string SeniorHS { get; set; }
            public string JuniorHS { get; set; }
            public string Company1 { get; set; }
            public string CompanyDes1 { get; set; }
            public string Company2 { get; set; }
            public string CompanyDes2 { get; set; }
            public string Company3 { get; set; }
            public string CompanyDes3 { get; set; }
            public List<string> Skills { get; set; }

            public override string ToString()
            {
                return string.Format("\nName: {0} {1} {2} \nAddress: {3} {4} \nEmail: {5} \nPhone Number: {6} \n\nObjectives: {7} \n\nEducational Attainment \nCollege: {8} \nSenior High School: {9} \nJunior High School: {10} \n\nExperiences \nCompany: {11} \n* {12} \nCompany: {13} \n* {14} \nCompany: {15} \n* {16} \n\nSkills: {17}",
                        FirstName, MiddleInitital, Surname, Address, ZIP, Email, PhoneNumber, Objectives, College, 
                        SeniorHS, JuniorHS, Company1, CompanyDes1, Company2, CompanyDes2, Company3, CompanyDes3, string.Join(" ", Skills.ToArray()));
            }
        }

        private void btnsavePDF_Click(object sender, EventArgs e)
        {
            string image_name = txtboxPic.Text;
            string file_name = TSName.Text + "_" + TFName.Text;
            using (SaveFileDialog save = new SaveFileDialog() { Filter = "PDF file|*.pdf", ValidateNames = true, FileName = file_name})
            {
                if (RTboxResume.Text == "")
                {
                    MessageBox.Show("No file to save.");
                }
                else if (save.ShowDialog() == DialogResult.OK)
                {
                    Document doc = new Document(PageSize.LETTER, 10f, 10f, 10f, 0f);
                    try
                    {
                        PdfWriter.GetInstance(doc, new FileStream(save.FileName, FileMode.Create));
                        doc.Open();
                        System.Drawing.Image pImage = System.Drawing.Image.FromFile(@"C:\Users\Santos\Documents\json\" + image_name);

                        iTextSharp.text.Image ItextImage = iTextSharp.text.Image.GetInstance(pImage, System.Drawing.Imaging.ImageFormat.Jpeg);
                        ItextImage.ScalePercent(15f);
                        ItextImage.SetAbsolutePosition(doc.PageSize.Width - 50f - 80f, doc.PageSize.Height - 30f - 100f);

                        doc.Add(ItextImage);

                        iTextSharp.text.Font pfont1 = FontFactory.GetFont(iTextSharp.text.Font.FontFamily.TIMES_ROMAN.ToString(), 20,
                            iTextSharp.text.Font.BOLD, iTextSharp.text.BaseColor.BLACK);

                        Paragraph pgraph1 = new Paragraph(TSName.Text + ", " + TFName.Text + " " + TMInitial.Text, pfont1);
                        pgraph1.Alignment = Element.ALIGN_LEFT;
                        doc.Add(pgraph1);

                        pfont1 = FontFactory.GetFont(iTextSharp.text.Font.FontFamily.TIMES_ROMAN.ToString(), 14,
                            iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        Paragraph pgraph2 = new iTextSharp.text.Paragraph(RTboxResume.Text, pfont1);
                        doc.Add(pgraph2);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        doc.Close();
                        MessageBox.Show("File Saved");
                    }
                }
            }
        }

        private void RTboxResume_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
