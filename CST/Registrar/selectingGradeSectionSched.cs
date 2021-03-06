﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CST.Data;
using CST.Models;
using CST.Reports;

namespace CST.Registrar
{
    public partial class selectingGradeSectionSched : Form
    {

        string[] grades = { };
        string sno = "";
        string fn = "";
        int[] sectionIds = { };
        int countTotalStuds = 0;
        int caps = 0;
        int selectedSectIds = 0;
        SectionController sectionController = new SectionController();
        SchedSectionController schedSectionController = new SchedSectionController();
        StudentGradesController StudentGradesController = new StudentGradesController();
        AuditTrailControl auditTrail = new AuditTrailControl();
        StudentEnrolledController studentEnrolledController = new StudentEnrolledController();
        public selectingGradeSectionSched(string studno, string name, string studentType)
        {
            InitializeComponent();

          
            sno = studno;
            fn = name;
            label4.Text = "Student No: " + sno;
            label5.Text = "Student Name: " + fn;


            if (studentType == "New Student" || studentType == "Transferee Student")
            {
                label7.Visible = false;
                foreach (string grade in DataClass.getAllGrade())
                {
                    comboBox1.Items.Add(grade);
                }
            }
            else
            {
                label7.Visible = true;
                string gradeLast = studentEnrolledController.getLastGraDe(studno);
                int lastSyid = studentEnrolledController.getLastSyidEnrolled(studno);
                label7.Text = label7.Text+ " "+  gradeLast;
                int index = Array.IndexOf(DataClass.getAllGrade(), gradeLast);
                int totalFailed = StudentGradesController.getTotalFailed(studno, lastSyid);
             
                if(totalFailed >= 3)
                {
                    label8.Visible = true;
                    label8.Text = "Number of Failed Subjects :" + totalFailed;

                    comboBox1.Items.Add(DataClass.getAllGrade()[index]);
                   
                }
                else
                {
                    label8.Visible = false;

                    if (gradeLast == "Grade 10")
                        return;

                    for (int i = index + 1; i < DataClass.getAllGrade().Length; i++)
                    {
                        comboBox1.Items.Add(DataClass.getAllGrade()[i]);
                    }
                }
            }
        }

        private void selectingGradeSectionSched_Load(object sender, EventArgs e)
        {
            label44.Hide();
            timer1.Start();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            listView1.Items.Clear();
            sectionIds = sectionController.fillComboSect3(ref comboBox2, comboBox1.Text);
            comboBox2.Enabled = true;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSectIds = sectionIds[comboBox2.SelectedIndex];
            listView1.Items.Clear();
            schedSectionController.fillListSched(ref listView1, selectedSectIds);
            countTotalStuds = sectionController.totalStudentInSections(selectedSectIds);
            caps = sectionController.getCapacity(selectedSectIds);
            //caps--;
            if(caps> countTotalStuds)
            {
                button1.Enabled = true;
                button5.Enabled = true;
                
                label6.ForeColor = Color.ForestGreen;
                label6.Text = "Selected Section Is Available";

            }
            else if(caps == countTotalStuds)
            {
                button1.Enabled = false;
                button5.Enabled = false;
                label6.ForeColor = Color.IndianRed;
                label6.Text = "Selected Section Is Currently full";
            }
            label6.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (isValid())
            {
             
                studentEnrolledController.addEnrolledStudents(sno, comboBox1.Text, selectedSectIds);
                this.Hide();
                reqattachment frm = new reqattachment();
                frm.Show();
                /*RegistrarForm frm = new RegistrarForm();
                frm.Show();*/
                MessageBox.Show("Succesfully Added Student section and grade");
                auditTrail.addAudit(label44.Text, fn + " Register");
            }
            else
            {
                MessageBox.Show("Please Select Grade And Section", "validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool isValid()
        {
            bool isvalid = true;

            isvalid = comboBox1.SelectedIndex > -1 && isvalid;

            isvalid = comboBox2.SelectedIndex > -1 && isvalid;

            return isvalid;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex> -1)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listView1.Items.Count> 0)
            {
                DataSet ds = new DataSet();

                DataTable dt = new DataTable();
                dt.Columns.Add("Time Start", typeof(string));
                dt.Columns.Add("Time End", typeof(string));
                dt.Columns.Add("Subjects", typeof(string));
                dt.Columns.Add("Teacher", typeof(string));

                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    dt.Rows.Add(listView1.Items[i].SubItems[0].Text,
                        listView1.Items[i].SubItems[1].Text,
                        listView1.Items[i].SubItems[2].Text,
                        listView1.Items[i].SubItems[3].Text);
                }

                ds.Tables.Add(dt);
                //  ds.WriteXmlSchema("StudSched.xml");

                StudentSchedRep form = new StudentSchedRep(ds, comboBox2.SelectedItem.ToString(), comboBox1.SelectedItem.ToString());
                form.ShowDialog();
            }
           

        }

        private void selectingGradeSectionSched_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void selectingGradeSectionSched_FormClosing(object sender, FormClosingEventArgs e)
        {
        
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime my = DateTimeOffset.Now.DateTime.ToLocalTime().ToUniversalTime();
            DateTime mys = DateTimeOffset.Now.UtcDateTime.ToLocalTime();
            label44.Text = my.ToString("MM/dd/yyyy  hh:mm:ss tt");
            timer1.Enabled = true;

        }
    }
}
