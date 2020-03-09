﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CST.Models;

namespace CST.Enrollment_Admin.DialogsSched
{
    public partial class AssignRoom : Form
    {
        RoomController roomController = new RoomController();
        string[] r_ids = { };
        public string selectedRoomid = "";
        public string roomName = "";

        public AssignRoom(string te, string ts)
        {
            InitializeComponent();
            r_ids = roomController.fillClassRoomAvail(ref comboBox1, te, ts);
        }

        private void AssignRoom_Load(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex > -1)
            {
                roomName = comboBox1.Text;
                selectedRoomid = r_ids[comboBox1.SelectedIndex];
                this.Hide();
            }
            else
            {
                MessageBox.Show("Please Pick a Room");
            }
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selectedRoomid = 0 + "";
            this.Hide();
        }
    }
}